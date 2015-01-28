// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TsdLib.InstrumentLibraryTools
{
    /// <summary>
    /// Contains functionality to dynamically generate .NET source code and/or assemblies.
    /// </summary>
    public class InstrumentParser : ICodeParser
    {
        private readonly string _nameSpace;
        private readonly string _language;

        /// <summary>
        /// Initialize a new instrument code parser to transform xml files into code files.
        /// </summary>
        /// <param name="nameSpace">Name of the test system. Will be used to generate namespaces.</param>
        /// <param name="language">Generate C# or Visual Basic code.</param>
        public InstrumentParser(string nameSpace, string language)
        {
            _nameSpace = nameSpace;
            _language = language;
        }

        public string GenerateSourceCode(TextReader file)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(_language);

            CodeCompileUnit ccu = Parse(file);

            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
                provider.GenerateCodeFromCompileUnit(ccu, writer, new CodeGeneratorOptions { BracingStyle = "C" });
            return sb.ToString();
        }

        /// <summary>
        /// Generates a <see cref="System.CodeDom.CodeCompileUnit"/> from the specified XML instrument definition file(s).
        /// </summary>
        /// <param name="sourceFile">An xml source code file to be converted to code.</param>
        /// <returns>A <see cref="System.CodeDom.CodeCompileUnit"/> containing the source code and assembly references required to compile.</returns>
        public CodeCompileUnit Parse(TextReader sourceFile)
        {
            string namespaceDeclaration = _nameSpace.Replace(' ', '_');
            if (!namespaceDeclaration.Contains(".Instruments"))
                namespaceDeclaration += ".Instruments";

            CodeNamespace ns = new CodeNamespace(namespaceDeclaration);
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(ns);

            XmlSchema schema;
            using (Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".TsdLib.Instruments.xsd"))
            {
                Debug.Assert(schemaStream != null, "The XML schema: TsdLib.CodeGenerator.TsdLib.Instruments.xsd is missing from the TsdLib.dll");
                // ReSharper disable once AssignNullToNotNullAttribute
                schema = XmlSchema.Read(schemaStream, null);
            }
            string tns = schema.TargetNamespace;
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(schema);

            XDocument doc = XDocument.Load(sourceFile);

            string docName = "";
            StreamReader sr = sourceFile as StreamReader;
            if (sr != null)
            {
                FileStream fs = sr.BaseStream as FileStream;
                if (fs != null)
                    docName = Path.GetFileName(fs.Name);
            }


            if (doc.Root == null || doc.Root.Attribute("xmlns") == null || (string)doc.Root.Attribute("xmlns") != tns)
                throw new CodeGeneratorException("Xml file " + docName + " is not a valid instrument.xml file.");

            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument"));
            ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument.Dummy"));

            doc.Validate(schemas,
                (o, e) => { throw new CodeGeneratorException("Xml file: " + docName + " could not be validated against schema: TsdLib.Instruments.xsd.", e.Exception); });

            XElement rootElement = doc.Root;

            if (rootElement == null)
                throw new CodeGeneratorException("Xml file: " + docName + " does not have a valid root element.");

            foreach (XElement instrumentElement in rootElement.Elements().Where(e => e.Name.LocalName == "Instrument"))
            {
                string connectionType = (string)instrumentElement.Attribute("ConnectionType");
                string connectionTypeNamespace = "TsdLib.Instrument." + connectionType;
                if (connectionType != "Dummy")
                {
                    string connectionTypeAssembly = connectionTypeNamespace + ".dll";
                    if (!ccu.ReferencedAssemblies.Contains(connectionTypeAssembly))
                        ccu.ReferencedAssemblies.Add(connectionTypeAssembly);
                }
                ns.Imports.Add(new CodeNamespaceImport(connectionTypeNamespace));

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
                instrumentClass.CustomAttributes.Add(
                    new IdQueryAttributeDeclaration(
                        instrumentElement.Elements().First(e => e.Name.LocalName == "IdQuery")));

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

                //Add constructors
                CodeConstructor ctor = new CodeConstructor { Attributes = MemberAttributes.Assembly };
                ctor.Parameters.Add(new CodeParameterDeclarationExpression(connectionType + "Connection", "connection"));
                ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("connection"));
                instrumentClass.Members.Add(ctor);
                if (connectionType != "Dummy")
                {
                    CodeConstructor dummyCtor = new CodeConstructor {Attributes = MemberAttributes.Assembly};
                    dummyCtor.Parameters.Add(new CodeParameterDeclarationExpression("DummyConnection", "connection"));
                    dummyCtor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("connection"));
                    instrumentClass.Members.Add(dummyCtor);
                }

                //Add factory field and Connect methods
                instrumentClass.Comments.Add(new CodeCommentStatement("ReSharper disable once FieldCanBeMadeReadOnly.Local"));
                instrumentClass.Members.Add(new FactoryReference(connectionType));
                instrumentClass.Members.Add(new ConnectMethod(instrumentClass.Name));
                instrumentClass.Members.Add(new ConnectMethod(instrumentClass.Name, "System.String", "address"));
                instrumentClass.Members.Add(new ConnectMethod(instrumentClass.Name, "ConnectionBase", "connection", connectionType + "Connection"));


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


            return ccu;
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
            Name = "Connection";
            Type = new CodeTypeReference("ConnectionBase");
            Attributes = MemberAttributes.Family | MemberAttributes.Final;
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
            string interfaces = (string)instrumentElement.Attribute("Interfaces");
            if (interfaces != null)
            {
                InterfaceNames = interfaces
                    .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

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
        public ConnectMethod(string instrumentName, string argumentType = null, string argumentName = null, string argumentCastType = null)
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

            if (argumentType != null && argumentName != null)
            {
                Parameters.Add(new CodeParameterDeclarationExpression(argumentType, argumentName));
                if (argumentCastType == null)
                    factoryMethodInvoke.Parameters.Add(new CodeVariableReferenceExpression(argumentName));
                else
                    factoryMethodInvoke.Parameters.Add(new CodeCastExpression(argumentCastType,
                        new CodeVariableReferenceExpression(argumentName)));
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
                Type = new CodeTypeReference(typeof(string)),
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
                CodeMemberProperty termCharMember = new CodeMemberProperty
                {
                    Name = infoElement.Name.LocalName + "TermChar",
                    Type = new CodeTypeReference(typeof(char)),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                termCharMember.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(((string)infoElement.Attribute("TermChar"))[0])));
                Add(termCharMember);
            }

            if ((string)infoElement.Attribute("Descriptor") != null)
            {
                CodeMemberProperty descriptorMember = new CodeMemberProperty
                {
                    Name = infoElement.Name.LocalName + "Descriptor",
                    Type = new CodeTypeReference(typeof(string)),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override
                };
                descriptorMember.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(((string)infoElement.Attribute("Descriptor")))));
                Add(descriptorMember);
            }
        }
    }

    class Method : CodeMemberMethod
    {
        protected CodeTryCatchFinallyStatement TryCatchFinally;
        protected CodeTypeReferenceExpression MonitorReference;
        protected CodeFieldReferenceExpression LockerReference;

        public Method(XElement methodElement)
        {
            Name = (string)methodElement.Attribute("Name");
            Attributes = MemberAttributes.Public;

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

            //Statements.Add(new SendCommandMethodInvoke(methodElement));


            MonitorReference = new CodeTypeReferenceExpression(typeof(Monitor));
            LockerReference = new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("Connection"), "SyncRoot");
            Statements.Add(new CodeMethodInvokeExpression(MonitorReference, "Enter", LockerReference));

            TryCatchFinally = new CodeTryCatchFinallyStatement();


            TryCatchFinally.TryStatements.Add(new SendCommandMethodInvoke(methodElement));

            TryCatchFinally.FinallyStatements.Add(new CodeMethodInvokeExpression(MonitorReference, "Exit", LockerReference));
            Statements.Add(TryCatchFinally);

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

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(new GetResponseTypedMethodInvoke(queryMethodElement));
            TryCatchFinally.TryStatements.Add(returnStatement);
        }
    }

    class ByteQueryMethod : Method
    {
        public ByteQueryMethod(XElement byteQueryMethodElement)
            : base(byteQueryMethodElement)
        {
            ReturnType = new CodeTypeReference(typeof(byte[]));

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(new GetResponseBytesMethodInvoke(byteQueryMethodElement));
            TryCatchFinally.TryStatements.Add(returnStatement);
        }
    }

    class SendCommandMethodInvoke : CodeMethodInvokeExpression
    {
        public SendCommandMethodInvoke(XElement methodElement)
        {
            Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("Connection"), "SendCommand");
            string message = (string)methodElement.Attribute("Message");
            Parameters.Add(new CodePrimitiveExpression(message));
            string delay = (string)methodElement.Attribute("Delay");

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

            string delay = (string)methodElement.Attribute("Delay");
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

            string regEx = (string)queryMethodElement.Attribute("RegEx");
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

            string byteCount = (string)byteQueryMethodElement.Attribute("ByteCount");
            if (byteCount != null)
                Parameters.Insert(0, new CodePrimitiveExpression(int.Parse(byteCount)));
        }
    }

    #endregion

}