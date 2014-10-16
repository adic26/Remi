using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TsdLib.CodeGenerator
{
    /// <summary>
    /// Contains functionality to dynamically generate .NET source code and/or assemblies.
    /// </summary>
    public class Generator : IDisposable
    {
        private readonly string _testSystemName;
        private readonly string[] _instrumentFiles;
        private readonly Language _language;
        private readonly string _tempPath;

        /// <summary>
        /// Initialize a new code generator.
        /// </summary>
        /// <param name="testSystemName">Name of the test system. Will be used to generate namespaces.</param>
        /// <param name="instrumentFiles">An array of absolute or relative paths to the XML instrument definition files to compile into the assembly.</param>
        /// <param name="language">Generate C# or Visual Basic code.</param>
        public Generator(string testSystemName, string[] instrumentFiles, Language language)
        {
            _testSystemName = testSystemName;
            _instrumentFiles = instrumentFiles;
            _language = language;
            _tempPath = Path.Combine(Path.GetTempPath(), "TsdLib");
            if (!Directory.Exists(_tempPath))
                Directory.CreateDirectory(_tempPath);
        }

        /// <summary>
        /// Generates an assembly from the specified XML instrument definition file(s) and test sequence source code file.
        /// </summary>
        /// <param name="testSequenceName">Name of the test sequence that will be compiled.</param>
        /// <param name="testSequenceSourceCode">Name of the test sequence to run.</param>
        /// <param name="testSequenceReferencedAssemblies">Names of assemblies to be referenced by the test sequence assembly.</param>
        /// <returns>Absolute path the the generated assembly.</returns>
        public string GenerateTestSequenceAssembly(string testSequenceName, string testSequenceSourceCode, string[] testSequenceReferencedAssemblies)
        {
            Trace.WriteLine("Compiling test sequence from:");
            foreach (string instrumentsFile in _instrumentFiles)
                Trace.WriteLine("\t" + Path.GetFileNameWithoutExtension(instrumentsFile));
            Trace.WriteLine("\t" + Path.GetFileNameWithoutExtension(testSequenceName));

            CodeDomProvider provider = CodeDomProvider.CreateProvider(_language.ToString());
            
            string dllPath = Path.Combine(_tempPath, "sequence.dll");

            CompilerParameters cp = new CompilerParameters
            {
                IncludeDebugInformation = true,
                OutputAssembly = dllPath
            };

#if DEBUG
            cp.CompilerOptions += " /d:DEBUG";
#endif
#if TRACE
            cp.CompilerOptions += " /d:TRACE";
#endif

            CodeCompileUnit instrumentCcu = generateInstrumentCodeCompileUnit();
            
            CodeSnippetCompileUnit sequenceCcu = new CodeSnippetCompileUnit(testSequenceSourceCode);
            sequenceCcu.ReferencedAssemblies.AddRange(testSequenceReferencedAssemblies);

            CodeGeneratorOptions options = new CodeGeneratorOptions { BracingStyle = "C"};

            string instrumentCodePath = Path.Combine(_tempPath, "instruments." + provider.FileExtension);
            using (StreamWriter w = new StreamWriter(instrumentCodePath, false))
                provider.GenerateCodeFromCompileUnit(instrumentCcu, w, options);

            //TODO: set StreamWriter line endings to Environment.NewLine

            string sequenceCodePath = Path.Combine(_tempPath, "sequence." + provider.FileExtension);
            using (StreamWriter w = new StreamWriter(sequenceCodePath, false))
                provider.GenerateCodeFromCompileUnit(sequenceCcu, w, options);


            cp.ReferencedAssemblies.AddRange(instrumentCcu.ReferencedAssemblies.Cast<string>().Union(sequenceCcu.ReferencedAssemblies.Cast<string>()).ToArray());
            CompilerResults compilerResults = provider.CompileAssemblyFromFile(cp, instrumentCodePath, sequenceCodePath);

            if (compilerResults.Errors.HasErrors)
                throw new CompilerException(compilerResults.Errors);

            Trace.WriteLine("Compiled successfully.");

            return compilerResults.PathToAssembly;
        }

        /// <summary>
        /// Generates a source code file from the specified XML instrument definition file(s).
        /// </summary>
        /// <param name="outputDirectoryName">Absolute directory path to store the output file. If the directory does not exist, it will be created.</param>
        /// <returns>The absolute path the the generated source code file.</returns>
        public string GenerateInstrumentsClassFile(string outputDirectoryName)
        {
            CodeCompileUnit ccu = generateInstrumentCodeCompileUnit();
            
            CodeDomProvider provider = CodeDomProvider.CreateProvider(_language.ToString());

            if (Directory.Exists(outputDirectoryName))
                Directory.CreateDirectory(outputDirectoryName);
            string fullFileName = Path.Combine(outputDirectoryName, "Instruments." + provider.FileExtension);
            using (StreamWriter writer = new StreamWriter(fullFileName, false))
                provider.GenerateCodeFromCompileUnit(ccu, writer, new CodeGeneratorOptions { BracingStyle = "C" });

            return fullFileName;
        }

        /// <summary>
        /// Generates source code from the specified XML instrument definition file(s).
        /// </summary>
        /// <returns>The source code as a string.</returns>
        public string GenerateInstrumentsClassSourceCode()
        {
            CodeCompileUnit ccu = generateInstrumentCodeCompileUnit();

            CodeDomProvider provider = CodeDomProvider.CreateProvider(_language.ToString());

            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
                provider.GenerateCodeFromCompileUnit(ccu, writer, new CodeGeneratorOptions { BracingStyle = "C" });

            return sb.ToString();
        }

        /// <summary>
        /// Cleans up any temporary files created by the code generator.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //try { Directory.Delete(_tempPath, true); }
                //catch (Exception) { Trace.WriteLine("Could not delete temp files from: " + _tempPath); }
            }
        }

        /// <summary>
        /// Cleans up any temporary files created by the code generator.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private CodeCompileUnit generateInstrumentCodeCompileUnit()
        {
            string namespaceDeclaration = _testSystemName.Replace(' ', '_') + ".Instruments";

            //CodeNamespace ns = new CodeNamespace(runTime ? namespaceDeclaration + ".Dynamic" : namespaceDeclaration);
            CodeNamespace ns = new CodeNamespace(namespaceDeclaration);
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(ns);

            XmlSchema schema;
            using (Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TsdLib.CodeGenerator.TsdLib.Instruments.xsd"))
            {
                Debug.Assert(schemaStream != null, "The XML schema: TsdLib.CodeGenerator.TsdLib.Instruments.xsd is missing from the TsdLib.dll");
                schema = XmlSchema.Read(schemaStream, null);
            }
            string tns = schema.TargetNamespace;
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(schema);
            XDocument[] docs = _instrumentFiles
                .Select(file => XDocument.Load(file, LoadOptions.SetBaseUri))
                .Where(
                    doc =>
                        doc.Root != null &&
                        doc.Root.Attribute("xmlns") != null &&
                        (string) doc.Root.Attribute("xmlns") == tns)
                .ToArray();

            if (docs.Length != _instrumentFiles.Count())
            {
                Trace.WriteLine("Warning: Some input files do not conform to the schema: TsdLib.Instruments.xsd");

                IEnumerable<string> badFiles = _instrumentFiles.Except(docs.Select(doc => Path.GetFileName(doc.BaseUri)));
                foreach (string badFile in badFiles)
                    Trace.WriteLine("\t" + badFile);
            }

            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument"));
            //ns.Imports.Add(new CodeNamespaceImport("TsdLib.InstrumentLibrary.Instruments"));
            //ns.Imports.Add(new CodeNamespaceImport("TsdLib.InstrumentLibrary.Helpers"));

            foreach (XDocument doc in docs)
            {
                string docName = new Uri(doc.BaseUri).AbsolutePath;

                doc.Validate(schemas,
                    (o, e) => { throw new CodeGeneratorException("File: " + docName + " could not be validated against schema: TsdLib.Instruments.xsd.", e.Exception); });

                XElement rootElement = doc.Root;

                if (rootElement == null)
                    throw new CodeGeneratorException("File: " + docName + " does not have a valid root element.");
                
                foreach (XElement instrumentElement in rootElement.Elements().Where(e => e.Name.LocalName == "Instrument"))
                {
                    string connectionType = (string) instrumentElement.Attribute("ConnectionType");
                    string connectionTypeNamespace = "TsdLib.Instrument." + connectionType;
                    if (connectionType != "Dummy")
                    {
                        string connectionTypeAssembly = connectionTypeNamespace + ".dll";
                        if (!ccu.ReferencedAssemblies.Contains(connectionTypeAssembly))
                            ccu.ReferencedAssemblies.Add(connectionTypeAssembly);
                    }
                    ns.Imports.Add(new CodeNamespaceImport(connectionTypeNamespace));

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
