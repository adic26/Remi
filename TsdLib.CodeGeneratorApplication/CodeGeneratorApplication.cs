// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.Build.Evaluation;
//TODO: Remove instrument generator (integrated into TsdLib.Controller)

/*
 * TestSequence source - no (integrated into TsdLib.Controller)
 * TestSequence assembly - no (integrated into TsdLib.Controller)
 * Instruments source - yes (need for pre-build event to transform from xml to source code)
 * Instruments assembly - no (yes for now, until we integrate assembly generation into TsdLib.Controller)
 * */

namespace TsdLib.CodeGeneratorApplication
{
    /// <summary>
    /// Describes the types of input that are supported.
    /// </summary>
    public enum CodeType
    {
        /// <summary>
        /// An *.xml instrument definition file.
        /// </summary>
        Instruments,
        /// <summary>
        /// A *.cs or *.vb test sequence class file.
        /// </summary>
        TestSequence
    }

    /// <summary>
    /// Describes the types of output that can be generated.
    /// </summary>
    [Flags]
    public enum OutputFormat
    {
        /// <summary>
        /// Source code.
        /// </summary>
        Source = 1,
        /// <summary>
        /// Compiled assembly.
        /// </summary>
        Assembly = 2,
        /// <summary>
        /// Source code and a compiled assembly.
        /// </summary>
        Both = Source | Assembly
    }

    /// <summary>
    /// Describes the programming languages that are supported for code input or output.
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// C# language.
        /// </summary>
        CSharp,
        /// <summary>
        /// Visual Basic language.
        /// </summary>
        VisualBasic
    }
    
    /// <summary>
    /// Contains functionality to dynamically generate .NET source code and/or assemblies.
    /// </summary>
    public static class CodeGeneratorApplication
    {
        /// <summary>
        /// Dynamically generates code file (*.cs or *.vb) or class library (*.dll) files.
        /// </summary>
        /// <param name="args">source|assembly|both input_path output_path CSharp|VisualBasic schema_filename</param>
        /// <returns>No error: 0, No arguments supplied: 1, Invalid arguments: 2, Compiler error: 3, Unknown error: -1</returns>
        static int Main(string[] args)
        {
            if (args.Length < 6)
            {
                const int width = 35;
                Console.WriteLine(string.Join(Environment.NewLine,"",
                    "                 TsdLib Dynamic Instrument Assembly Generator",
                    "Dynamically generates code file (*.cs or *.vb) or class library (*.dll) files",
                    "",
                    "Usage: TsdLib.CodeGeneratorApplication.exe <Source|Assembly|Both> <input path> <output path> <CSharp|VisualBasic> <schema filename>",
                    "",
                    "   Instruments|TestSequence".PadRight(width) + "Type of code to generate. Instrument classes from *.xml files or TestSequence class from a *.cs or *.vb file.",
                    "   Source|Assembly|Both".PadRight(width) + "Generate a source code file (*.cs or *.vb) or an assembly (*.dll).",
                    "   <input>".PadRight(width) + "Source file location:",
                    "   ".PadRight(width + 4) + "For instruments: Format is *.xml and can be an individual file or a folder/project containing multiple files).",
                    "   ".PadRight(width + 4) + "For TestSequence, Format is any text file and must be an individual file.",
                    "   <output path>".PadRight(width) + "Location for the output file.",
                    "   <client application name".PadRight(width) + "Name of the client application, used to generate the namespace root",
                    "   CSharp|VisualBasic".PadRight(width) + "Language to use.",
                    "   <schema filename>".PadRight(width) + "Name of the XML schema (*.xsd) used to validate the XML source files. Only required for Instrument code generation.",
                    "",
                    "Press Enter to exit..."
                    ));

                Console.ReadLine();
                return -1;
            }

            Trace.Listeners.Add(new ConsoleTraceListener());

            CodeType codeType;
            OutputFormat outputFormat;
            string[] inputFiles;
            string outputPath;
            string clientNamespace;
            Language language;
            string schemaFile;

            try //Validate arguments and get input files
            {
                codeType = (CodeType) Enum.Parse(typeof (CodeType), args[0]);
                outputFormat = (OutputFormat) Enum.Parse(typeof (OutputFormat), args[1]);
                string inputPath = args[2];
                outputPath = args[3];
                clientNamespace = args[4];
                language = (Language)Enum.Parse(typeof(Language), args[5]);
                schemaFile = args.Length > 6 ? args[6] : "TsdLib.Instrument.xsd";

                if (Path.GetExtension(inputPath) == ".csproj" || Path.GetExtension(inputPath) == ".vbproj")
                {
                    Debug.WriteLine("Input path is a project: " + inputPath);

                    string projectFolder = Path.GetDirectoryName(inputPath);

                    Debug.Assert(projectFolder != null);

                    inputFiles = new Project(inputPath)
                        .GetItems("Content")
                        .Select(p => Path.Combine(projectFolder, p.EvaluatedInclude))
                        .Where(file => Path.GetExtension(file) == ".xml")
                        .ToArray();
                }

                else if (File.GetAttributes(inputPath).HasFlag(FileAttributes.Directory))
                {
                    Debug.WriteLine("Input path is a directory: " + inputPath);
                    inputFiles = Directory.EnumerateFiles(inputPath)
                        .Where(file => Path.GetExtension(file) == ".xml")
                        .ToArray();
                }

                else
                    inputFiles = new[] { inputPath };

                Debug.WriteLine("Input files:");
                foreach (string xmlFile in inputFiles)
                    Debug.WriteLine("\t" + xmlFile);

            }
            catch(Exception ex)
            {
                //command-line argument error
                Trace.WriteLine(ex);
                return -2; 
            }

            try //Generate code and/or assembly
            {
                switch (codeType)
                {
                    case CodeType.Instruments:
                        GenerateInstruments(outputFormat, inputFiles, outputPath, clientNamespace, language, schemaFile);
                        break;
                    case CodeType.TestSequence:
                        GenerateTestSequence(outputFormat, inputFiles[0], outputPath, clientNamespace, language);
                        break;
                }
            }
            catch (TsdLibException ex)
            {
                Trace.WriteLine(ex);
                return -3;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return -4;
            }

            return 0;
        }

        /// <summary>
        /// Generate source code and/or assembly from one or more instrument xml definitions.
        /// </summary>
        /// <param name="outputFormat">Generate source code, assembly or both.</param>
        /// <param name="xmlInputFiles">A sequence of *.xml instrument definition files.</param>
        /// <param name="outputPath">Absolute directory path to store the output file. If the directory does not exist, it will be created.</param>
        /// <param name="clientNamespace">Namespace to apply to the generated code.</param>
        /// <param name="language">Generate C# or Visual Basic code.</param>
        /// <param name="schemaFile">XML schema (*.xsd) file used to validate the XML input files.</param>
        public static void GenerateInstruments(OutputFormat outputFormat, IEnumerable<string> xmlInputFiles, string outputPath, string clientNamespace, Language language = Language.CSharp, string schemaFile = "TsdLib.Instrument.xsd")
        {
            CodeNamespace ns = generateInstrumentCodeNamespace(clientNamespace, xmlInputFiles.ToArray(), schemaFile);

            if (outputFormat.HasFlag(OutputFormat.Source))
                generateSource(ns, outputPath, "Instruments", language);

            if (outputFormat.HasFlag(OutputFormat.Assembly))
            {
                string[] assemblies = ns.Imports
                    .Cast<CodeNamespaceImport>()
                    .Select((cnsi => cnsi.Namespace + ".dll"))
                    .ToArray();

                generateAssembly(ns, outputPath, clientNamespace + ".Instruments", language, assemblies);
            }
        }

        /// <summary>
        /// Generate source code and/or assembly from a test sequence source code file.
        /// </summary>
        /// <param name="outputFormat">Generate source code, assembly or both.</param>
        /// <param name="inputFile">A *.cs or *.vb source code file containing the test sequence class.</param>
        /// <param name="outputPath">Absolute directory path to store the output file. If the directory does not exist, it will be created.</param>
        /// <param name="clientNamespace">Namespace to apply to the generated code.</param>
        /// <param name="language">Generate C# or Visual Basic code.</param>
        public static void GenerateTestSequence(OutputFormat outputFormat, string inputFile, string outputPath, string clientNamespace, Language language = Language.CSharp)
        {
            CodeNamespace ns = generateTestSequenceCodeNamespace(clientNamespace, File.ReadAllLines(inputFile));

            if (outputFormat.HasFlag(OutputFormat.Source))
                generateSource(ns, outputPath, "TestSequence", language);

            if (outputFormat.HasFlag(OutputFormat.Assembly))
                generateAssembly(ns, outputPath, clientNamespace + ".TestSequences", language);
        }

        static CodeNamespace generateInstrumentCodeNamespace(string clientNamespace, string[] xmlFiles, string schemaFile)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            var s = schemas.Add(null, schemaFile);
            string tns = s.TargetNamespace;

            XDocument[] docs = xmlFiles
                .Select(file => XDocument.Load(file, LoadOptions.SetBaseUri))
                .Where(doc => doc.Root != null && doc.Root.Attribute("xmlns") != null && (string)doc.Root.Attribute("xmlns") == tns)
                .ToArray();

            if (docs.Length != xmlFiles.Count())
            {
                Trace.WriteLine("Warning: Some input files do not conform to the schema:" + Path.GetFileName(schemaFile));

                IEnumerable<string> badFiles = xmlFiles.Except(docs.Select(doc => Path.GetFileName(doc.BaseUri)));
                foreach (string badFile in badFiles)
                    Trace.WriteLine("\t" + badFile);
            }

            CodeNamespace ns = new CodeNamespace(clientNamespace);
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument"));

            foreach (XDocument doc in docs)
            {
                string docName = new Uri(doc.BaseUri).AbsolutePath;

                doc.Validate(schemas,
                    (o, e) => { throw new CodeGeneratorException("File: " + docName + " could not be validated against schema: " + schemaFile, e.Exception); });

                XElement instrumentElement = doc.Root;
                Debug.Assert(instrumentElement != null, "File: " + docName + " does not have a valid root element.");

                string connectionType = (string)instrumentElement.Attribute("ConnectionType");
                ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument." + connectionType));

                //Generate instrument class
                CodeTypeDeclaration instrumentClass = new CodeTypeDeclaration((string)instrumentElement.Attribute("Name"));
                ns.Types.Add(instrumentClass);

                //Add InstrumentBase reference
                CodeTypeReference instrumentBaseReference = new CodeTypeReference("InstrumentBase");
                instrumentBaseReference.TypeArguments.Add(connectionType + "Connection");
                instrumentClass.BaseTypes.Add(instrumentBaseReference);

                //Add interface references
                InterfaceReferenceCollection interfaceReferences = new InterfaceReferenceCollection(instrumentElement);
                instrumentClass.BaseTypes.AddRange(interfaceReferences);

                //Add ID attributes
                instrumentClass.CustomAttributes.Add(new IdQueryAttributeDeclaration(instrumentElement.Elements().First(e => e.Name.LocalName == "IdQuery")));

                //If connection is a dummy object, expose it through a public property
                if (connectionType == "Dummy")
                    instrumentClass.Members.Add(new ConnectionReferenceProperty());

                //Add initialization command attributes
                string initCommands = (string)instrumentElement.Attribute("InitCommands");
                if (initCommands != null)
                    instrumentClass.CustomAttributes.Add(new CustomAttributeDeclaration("InitCommands", initCommands));

                //Add command delay attribute
                string commandDelay = (string)instrumentElement.Attribute("CommandDelay");
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
                instrumentClass.Members.AddRange(new InfoPropertyCollection(instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "ModelNumber")));
                instrumentClass.Members.AddRange(new InfoPropertyCollection(instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "SerialNumber")));
                instrumentClass.Members.AddRange(new InfoPropertyCollection(instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "FirmwareVersion")));

                //Add command methods
                foreach (XElement methodElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "Command"))
                    instrumentClass.Members.Add(new CommandMethod(methodElement));

                //Add query methods
                foreach (XElement methodElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "Query"))
                    instrumentClass.Members.Add(new QueryMethod(methodElement));

                //Add byte query methods
                foreach (XElement methodElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "ByteQuery"))
                    instrumentClass.Members.Add(new ByteQueryMethod(methodElement));
            }
            return ns;
        }

        static CodeNamespace generateTestSequenceCodeNamespace(string clientNamespace, IEnumerable<string> statements)
        {
            CodeNamespace ns = new CodeNamespace(clientNamespace);

            ns.Imports.Add(new CodeNamespaceImport("TsdLib.TestSequence"));
            ns.Imports.Add(new CodeNamespaceImport("System.Threading"));

            CodeTypeDeclaration testSequenceClass = new CodeTypeDeclaration("TestSequence");
            ns.Types.Add(testSequenceClass);

            //Inherit from TsdLib.TestSequenceBase
            testSequenceClass.BaseTypes.Add("TestSequenceBase");

            //Generate Execute() method
            CodeMemberMethod executeMethod = new CodeMemberMethod
            {
                Name = "Execute",
                Attributes = MemberAttributes.Family | MemberAttributes.Override,
            };

            //Add method parameters
            executeMethod.Parameters.Add(new CodeParameterDeclarationExpression("CancellationToken", "token"));

            //Add method statements
            IEnumerable<CodeStatement> codeStatements = statements.Select(statement => new CodeSnippetStatement("\t\t\t" + statement));
            executeMethod.Statements.AddRange(codeStatements.ToArray());

            testSequenceClass.Members.Add(executeMethod);

            return ns;
        }

        static void generateSource(CodeNamespace codeNamespace, string outputPath, string fileName, Language language = Language.CSharp)
        {

            CodeDomProvider provider = CodeDomProvider.CreateProvider(language.ToString());
            string outputDirectoryName = Path.GetDirectoryName(outputPath);
            if (outputDirectoryName == null)
                throw new CodeGeneratorException("Invalid output path specified: " + outputPath);
            if (Directory.Exists(outputDirectoryName))
                Directory.CreateDirectory(outputDirectoryName);
            string fullFileName = Path.Combine(outputDirectoryName, fileName + "." + provider.FileExtension);
            using (StreamWriter writer = new StreamWriter(fullFileName, false))
                provider.GenerateCodeFromNamespace(codeNamespace, writer, new CodeGeneratorOptions { BracingStyle = "C" });
        }

        static void generateAssembly(CodeNamespace codeNamespace, string outputPath, string fileName, Language language = Language.CSharp, params string[] referencedAssemblies)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(language.ToString());

            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(codeNamespace);
            ccu.ReferencedAssemblies.AddRange(referencedAssemblies);
            
            CompilerParameters cp = new CompilerParameters
            {
                OutputAssembly = Path.Combine(outputPath, fileName + ".dll"),
                IncludeDebugInformation = true,
                GenerateExecutable = false,
            };

            CompilerResults cr = provider.CompileAssemblyFromDom(cp, ccu);

            if (cr.Errors.HasErrors)
                throw new CompilerException(cr.Errors);
        }
    }

#region Custom Instrument Code Member Classes

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

#endregion
}
// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
