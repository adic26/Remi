using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TsdLib.CodeGenerator
{
    /// <summary>
    /// Contains functionality to dynamically generate .NET source code and/or assemblies.
    /// </summary>
    public static class Generator
    {
        /// <summary>
        /// Generates an assembly from the specified XML instrument definition file(s) and test sequence source code file.
        /// </summary>
        /// <param name="applicationName">Name of the client application. Will be used to generate namespaces.</param>
        /// <param name="instrumentFiles">An array of absolute or relative paths to the XML instrument definition files to compile into the assembly.</param>
        /// <param name="schemaFile">XML schema (*.xsd) file used to validate the XML input files.</param>
        /// <param name="sequenceFile">Absolute or relative path to the test sequence source code file to compile into the assembly.</param>
        /// <param name="language">Generate C# or Visual Basic code.</param>
        /// <returns>Absolute path the the generated assembly.</returns>
        public static string GenerateDynamicAssembly(string applicationName, string[] instrumentFiles, string schemaFile, string sequenceFile, Language language)
        {
            Trace.WriteLine("Compiling test sequence from:");
            foreach (string instrumentsFile in instrumentFiles)
                Trace.WriteLine("\t" + Path.GetFileNameWithoutExtension(instrumentsFile));
            Trace.WriteLine("\t" + Path.GetFileNameWithoutExtension(sequenceFile));

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters cp = new CompilerParameters
            {
                IncludeDebugInformation = true,
                OutputAssembly = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".dll")
            };

#if DEBUG
            cp.CompilerOptions += " /d:DEBUG";
#endif
#if TRACE
            cp.CompilerOptions += " /d:TRACE";
#endif

            GenerateInstrumentsClass(applicationName, instrumentFiles, schemaFile, Environment.CurrentDirectory, "Instruments", language);

            MatchCollection sequenceAssemblyReferences = Regex.Matches(File.ReadAllText(sequenceFile), "(?<=assembly: AssemblyReference\\(\").*(?=\"\\))");
            foreach (Match sequenceAssemblyReference in sequenceAssemblyReferences)
            {
                string assemblyName = sequenceAssemblyReference.Value;
                if (!cp.ReferencedAssemblies.Contains(assemblyName))
                    cp.ReferencedAssemblies.Add(assemblyName);
            }

            CompilerResults cr = provider.CompileAssemblyFromFile(cp, "Instruments.cs", sequenceFile);
            if (cr.Errors.HasErrors)
                throw new CompilerException(cr.Errors);

            Trace.WriteLine("Compiled successfully.");
            return cr.PathToAssembly;
        }

        /// <summary>
        /// Generates a source code file from the specified XML instrument definition file(s).
        /// </summary>
        /// <param name="applicationName">Name of the client application. Will be used to generate namespaces.</param>
        /// <param name="instrumentFiles">An array of absolute or relative paths to the XML instrument definition files to compile into the assembly.</param>
        /// <param name="schemaFile">XML schema (*.xsd) file used to validate the XML input files.</param>
        /// <param name="outputDirectoryName">Absolute directory path to store the output file. If the directory does not exist, it will be created.</param>
        /// <param name="outputFilename">Name to assign to the output source code file. The file extension will automatically be assigned, based on the value of the language parameter.</param>
        /// <param name="language">Generate C# or Visual Basic code.</param>
        public static void GenerateInstrumentsClass(string applicationName, string[] instrumentFiles, string schemaFile, string outputDirectoryName, string outputFilename, Language language)
        {
            CodeCompileUnit ccu = generateInstrumentCodeCompileUnit(applicationName, instrumentFiles, schemaFile);
            if (Path.HasExtension(outputFilename))
                outputFilename = Path.ChangeExtension(outputFilename, null);
            generateSource(ccu, outputDirectoryName, outputFilename, language);
        }

        private static CodeCompileUnit generateInstrumentCodeCompileUnit(string applicationName, string[] instrumentFiles, string schemaFile)
        {
            CodeNamespace ns = new CodeNamespace(applicationName + ".Instruments");
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(ns);

            XmlSchemaSet schemas = new XmlSchemaSet();
            XmlSchema s = schemas.Add(null, schemaFile);
            string tns = s.TargetNamespace;

            XDocument[] docs = instrumentFiles
                .Select(file => XDocument.Load(file, LoadOptions.SetBaseUri))
                .Where(
                    doc =>
                        doc.Root != null &&
                        doc.Root.Attribute("xmlns") != null &&
                        (string) doc.Root.Attribute("xmlns") == tns)
                .ToArray();

            if (docs.Length != instrumentFiles.Count())
            {
                Trace.WriteLine("Warning: Some input files do not conform to the schema:" + Path.GetFileName(schemaFile));

                IEnumerable<string> badFiles = instrumentFiles.Except(docs.Select(doc => Path.GetFileName(doc.BaseUri)));
                foreach (string badFile in badFiles)
                    Trace.WriteLine("\t" + badFile);
            }

            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument"));

            foreach (XDocument doc in docs)
            {
                string docName = new Uri(doc.BaseUri).AbsolutePath;

                doc.Validate(schemas,
                    (o, e) => { throw new CodeGeneratorException( "File: " + docName + " could not be validated against schema: " + schemaFile, e.Exception); });

                XElement rootElement = doc.Root;

                if (rootElement == null)
                    throw new CodeGeneratorException("File: " + docName + " does not have a valid root element.");
                
                foreach (XElement instrumentElement in rootElement.Elements().Where(e => e.Name.LocalName == "Instrument"))
                {
                    string connectionType = (string) instrumentElement.Attribute("ConnectionType");
                    string connectionTypeAssembly = "TsdLib.Instrument." + connectionType;
                    if (!ccu.ReferencedAssemblies.Contains(connectionTypeAssembly))
                        ccu.ReferencedAssemblies.Add(connectionTypeAssembly);
                    ns.Imports.Add(new CodeNamespaceImport(connectionTypeAssembly));

                    //Generate instrument class
                    CodeTypeDeclaration instrumentClass = new CodeTypeDeclaration((string) instrumentElement.Attribute("Name"));
                    ns.Types.Add(instrumentClass);

                    //Add InstrumentBase reference
                    CodeTypeReference instrumentBaseReference = new CodeTypeReference("InstrumentBase");
                    instrumentBaseReference.TypeArguments.Add(connectionType + "Connection");
                    instrumentClass.BaseTypes.Add(instrumentBaseReference);

                    //Add interface references
                    InterfaceReferenceCollection interfaceReferences = new InterfaceReferenceCollection(instrumentElement);
                    instrumentClass.BaseTypes.AddRange(interfaceReferences);

                    //Add ID attributes
                    instrumentClass.CustomAttributes.Add(
                        new IdQueryAttributeDeclaration(
                            instrumentElement.Elements().First(e => e.Name.LocalName == "IdQuery")));

                    //If connection is a dummy object, expose it through a public property
                    if (connectionType == "Dummy")
                        instrumentClass.Members.Add(new ConnectionReferenceProperty());

                    //Add initialization command attributes
                    string initCommands = (string) instrumentElement.Attribute("InitCommands");
                    if (initCommands != null)
                        instrumentClass.CustomAttributes.Add(new CustomAttributeDeclaration("InitCommands", initCommands));

                    //Add command delay attribute
                    string commandDelay = (string) instrumentElement.Attribute("CommandDelay");
                    if (commandDelay != null)
                        instrumentClass.CustomAttributes.Add(new CustomAttributeDeclaration("CommandDelay", commandDelay));

                    //Add Connection Attributes
                    foreach (XElement connectionAttributeElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "ConnectionSetting"))
                        instrumentClass.CustomAttributes.Add(new ConnectionSettingAttributeDeclaration(connectionAttributeElement));

                    //Add constructor
                    CodeConstructor ctor = new CodeConstructor {Attributes = MemberAttributes.Assembly};
                    ctor.Parameters.Add(new CodeParameterDeclarationExpression(connectionType + "Connection", "connection"));
                    ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("connection"));

                    //Add factory field and methods
                    instrumentClass.Comments.Add(new CodeCommentStatement("ReSharper disable once FieldCanBeMadeReadOnly.Local"));
                    instrumentClass.Members.Add(new FactoryReference(connectionType));
                    instrumentClass.Members.Add(new ConnectMethod(instrumentClass.Name));
                    instrumentClass.Members.Add(new ConnectMethod(instrumentClass.Name, "address"));
                    instrumentClass.Members.Add(ctor);

                    //Add info property overloads
                    instrumentClass.Members.AddRange(
                        new InfoPropertyCollection(
                            instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "ModelNumber")));
                    instrumentClass.Members.AddRange(
                        new InfoPropertyCollection(
                            instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "SerialNumber")));
                    instrumentClass.Members.AddRange(
                        new InfoPropertyCollection(
                            instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "FirmwareVersion")));

                    //Add command methods
                    foreach (
                        XElement methodElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "Command"))
                        instrumentClass.Members.Add(new CommandMethod(methodElement));

                    //Add query methods
                    foreach (XElement methodElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "Query"))
                        instrumentClass.Members.Add(new QueryMethod(methodElement));

                    //Add byte query methods
                    foreach (XElement methodElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "ByteQuery"))
                        instrumentClass.Members.Add(new ByteQueryMethod(methodElement));
                }
            }

            //return ns;
            return ccu;
        }

        private static void generateSource(CodeCompileUnit codeCompileUnit, string outputDirectoryName, string outputFileName, Language language)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(language.ToString());
            if (Directory.Exists(outputDirectoryName))
                Directory.CreateDirectory(outputDirectoryName);
            string fullFileName = Path.Combine(outputDirectoryName, outputFileName + "." + provider.FileExtension);
            using (StreamWriter writer = new StreamWriter(fullFileName, false))
                provider.GenerateCodeFromCompileUnit(codeCompileUnit, writer, new CodeGeneratorOptions { BracingStyle = "C" });
        }
    }

    #region Custom Instrument Code Member Classes
    // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
    class CustomAttributeDeclaration : CodeAttributeDeclaration
    {
        public CustomAttributeDeclaration(string name, string argumentValue)
        {
            Name = name;
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(argumentValue)));
        }
    }

    class IdQueryAttributeDeclaration : CodeAttributeDeclaration
    {
        public IdQueryAttributeDeclaration(XElement xElement)
        {
            Name = xElement.Name.LocalName;

            string response = (string)xElement.Attribute("Response");
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(response)));

            string command = (string)xElement.Attribute("Command");
            if (command != null)
                Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(command)));

            string termchar = (string)xElement.Attribute("TermChar");
            if (termchar != null)
                Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(termchar)));
        }
    }

    class ConnectionSettingAttributeDeclaration : CodeAttributeDeclaration
    {
        public ConnectionSettingAttributeDeclaration(XElement xElement)
        {
            Name = xElement.Name.LocalName;

            string name = (string)xElement.Attribute("Name");
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(name)));

            string argType = (string)xElement.Attribute("ArgumentType");
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(argType)));

            string argValue = (string)xElement.Attribute("ArgumentValue");
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(argValue)));
        }
    }

    class ConnectionReferenceProperty : CodeMemberProperty
    {
        public ConnectionReferenceProperty()
        {
            Name = "DummyConnection";
            Type = new CodeTypeReference("DummyConnection");
            Attributes = MemberAttributes.Public;
            GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(null, "Connection")
                    ));
        }
    }

    class InterfaceReferenceCollection : CodeTypeReferenceCollection
    {
        public IEnumerable<string> InterfaceNames { get; private set; }

        public InterfaceReferenceCollection(XElement instrumentElement)
        {
            string interfaces = (string) instrumentElement.Attribute("Interfaces");
            if (interfaces != null)
            {
                InterfaceNames = interfaces
                    .Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);

                AddRange(InterfaceNames
                    .Select(s => new CodeTypeReference(s))
                    .ToArray());
            }
        }
    }

    class FactoryReference : CodeMemberField
    {
        public FactoryReference(string connectionType)
        {
            string typeName = connectionType;
            Type = new CodeTypeReference(typeName + "Factory");
            Name = "_factory";
            Attributes = MemberAttributes.Static;
            InitExpression = new CodeObjectCreateExpression(Type);
        }
    }

    class ConnectMethod : CodeMemberMethod
    {
         public ConnectMethod(string instrumentName, string argumentName = null)
         {
             Attributes = MemberAttributes.Public | MemberAttributes.Static;
             Name = "Connect";
             ReturnType = new CodeTypeReference(instrumentName);
             CodeMethodReferenceExpression factoryMethodReference =
                 new CodeMethodReferenceExpression(
                     new CodeVariableReferenceExpression("_factory"),
                     "GetInstrument");
             factoryMethodReference.TypeArguments.Add(new CodeTypeReference(instrumentName));

             CodeMethodInvokeExpression factoryMethodInvoke = new CodeMethodInvokeExpression(factoryMethodReference);

             if (argumentName != null)
             {
                 Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), argumentName));
                 factoryMethodInvoke.Parameters.Add(new CodeVariableReferenceExpression(argumentName));
             }
             Statements.Add(new CodeMethodReturnStatement(factoryMethodInvoke));

         }
    }

    class InfoPropertyCollection : CodeTypeMemberCollection
    {
        public InfoPropertyCollection(XElement infoElement)
        {
            CodeMemberProperty messageMember = new CodeMemberProperty
            {
                Name = infoElement.Name.LocalName + "Message",
                Type = new CodeTypeReference(typeof (string)),
                Attributes = MemberAttributes.Family | MemberAttributes.Override
            };
            messageMember.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression((string)infoElement.Attribute("Message"))));
            Add(messageMember);

            if ((string)infoElement.Attribute("RegEx") != null)
            {
                CodeMemberProperty regexMember = new CodeMemberProperty
                {
                    Name = infoElement.Name.LocalName + "RegEx",
                    Type = new CodeTypeReference(typeof(string)),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                regexMember.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression((string)infoElement.Attribute("RegEx"))));
                Add(regexMember);
            }

            if ((string)infoElement.Attribute("TermChar") != null)
            {
                CodeMemberProperty regexMember = new CodeMemberProperty
                {
                    Name = infoElement.Name.LocalName + "TermChar",
                    Type = new CodeTypeReference(typeof(char)),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                regexMember.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(((string)infoElement.Attribute("TermChar"))[0])));
                Add(regexMember);
            }
        }
    }

    class Method : CodeMemberMethod
    {
        public Method(XElement methodElement)
        {
            Name = (string)methodElement.Attribute("Name");
            Attributes = MemberAttributes.Public | MemberAttributes.Final;

            //Required for VB code generation
            string interfaceImplementation = (string)methodElement.Attribute("Implements");
            if (interfaceImplementation != null)
                ImplementationTypes.Add(interfaceImplementation);
            else
                if (methodElement.Parent != null)
                    ImplementationTypes.Add((string)methodElement.Parent.Attribute("Interfaces"));

            var parameters = methodElement.Elements().Where(e => e.Name.LocalName == "Parameter")
                .Select(e => new CodeParameterDeclarationExpression((string)e.Attribute("Type"), (string)e.Attribute("Name")));
            Parameters.AddRange(parameters.ToArray());

            Statements.Add(new SendCommandMethodInvoke(methodElement));
        }
    }

    class CommandMethod : Method
    {
        public CommandMethod(XElement methodElement)
            : base(methodElement)
        {
            
        }
    }

    class QueryMethod : Method
    {
        public QueryMethod(XElement queryMethodElement)
            : base(queryMethodElement)
        {
            string returnType = (string)queryMethodElement.Attribute("ReturnType");
            ReturnType = new CodeTypeReference(returnType);

            //CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(SendCommandMethodInvoke);
            //string parser = (string)queryMethodElement.Attribute("RegEx") ?? ".*";
            //SendCommandMethodInvoke.Parameters.Insert(1, new CodePrimitiveExpression(parser));
            //Statements.Add(returnStatement);

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(new GetResponseTypedMethodInvoke(queryMethodElement));
            Statements.Add(returnStatement);
        }
    }

    class ByteQueryMethod : Method
    {
        public ByteQueryMethod(XElement byteQueryMethodElement)
            : base(byteQueryMethodElement)
        {
            ReturnType = new CodeTypeReference(typeof(byte[]));

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(new GetResponseBytesMethodInvoke(byteQueryMethodElement));
            Statements.Add(returnStatement);
        }
    }

    class SendCommandMethodInvoke : CodeMethodInvokeExpression
    {
        public SendCommandMethodInvoke(XElement methodElement)
        {
            Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("Connection"), "SendCommand");
            string message = (string)methodElement.Attribute("Message");
            Parameters.Add(new CodePrimitiveExpression(message));
            string delay = (string) methodElement.Attribute("Delay");

            Parameters.Add(new CodePrimitiveExpression(delay != null ? int.Parse(delay) : -1));

            IEnumerable<CodeExpression> query = methodElement.Elements()
                .Where(e => e.Name.LocalName == "Parameter")
                .Select(e => new CodeArgumentReferenceExpression((string)e.Attribute("Name")));
            Parameters.AddRange(query.ToArray());
        }
    }

    class GetResponseMethodInvoke : CodeMethodInvokeExpression
    {
        public GetResponseMethodInvoke(XElement methodElement)
        {
            string termChar = (string)methodElement.Attribute("TermChar");
            if (termChar != null)
                Parameters.Add(new CodePrimitiveExpression(termChar[0]));

            string delay = (string) methodElement.Attribute("Delay");
            if (delay != null)
                Parameters.Add(new CodePrimitiveExpression(int.Parse(delay)));
        }
    }

    class GetResponseTypedMethodInvoke : GetResponseMethodInvoke
    {
        public GetResponseTypedMethodInvoke(XElement queryMethodElement)
            : base(queryMethodElement)
        {
            Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("Connection"), "GetResponse");

            string returnType = (string)queryMethodElement.Attribute("ReturnType");
            Method.TypeArguments.Add(returnType);

            string regEx = (string) queryMethodElement.Attribute("RegEx");
            if (regEx != null)
                Parameters.Insert(0, new CodePrimitiveExpression(regEx));
            
            
        }
    }

    class GetResponseBytesMethodInvoke : GetResponseMethodInvoke
    {
        public GetResponseBytesMethodInvoke(XElement byteQueryMethodElement)
            : base(byteQueryMethodElement)
        {
            Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("Connection"), "GetByteResponse");

            string byteCount = (string) byteQueryMethodElement.Attribute("ByteCount");
            if (byteCount != null)
                Parameters.Insert(0, new CodePrimitiveExpression(int.Parse(byteCount)));
        }
    }
    // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
#endregion
}
