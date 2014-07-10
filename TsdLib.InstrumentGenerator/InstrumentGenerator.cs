﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TsdLib.InstrumentGenerator
{
    public enum Language
    {
        CSharp,
        VisualBasic
    }
    
    public class InstrumentGenerator
    {
        /// <summary>
        /// Dynamically generates code file (*.cs or *.vb) or class library (*.dll) files.
        /// Console entry point. Runs in a separate process.
        /// </summary>
        /// <param name="args">TODO: describe args</param>
        /// <returns>No error: 0, No arguments: 1, Compiler error: 2</returns>
        static int Main(string[] args) //Console entry point
        {
            if (args.Length == 0)
            {
                const int width = 20;
                Console.WriteLine(string.Join(Environment.NewLine,"",
                    "TsdLib Dynamic Instrument Assembly Generator",
                    "Dynamically generates code file (*.cs or *.vb) or class library (*.dll) files",
                    "",
                    "Usage: TsdLib.InstrumentGenerator.exe <-source|-assembly|-both> <input path> <output path> [-vb] [<schema filename>]",
                    "",
                    "   -f <xmlFileOrFolder>".PadRight(width) + "XML source location. Can be a file or folder.",
                    "       <xmlFileOrFolder>".PadRight(width) + "absolute or relative path to the instrument file or folder",
                    "       c".PadRight(width) + "use current execution directory",
                    "   -vb".PadRight(width) + "Visual Basic code (C# is default)",
                    "",
                    "Press Enter to exit..."
                    ));

                Console.ReadLine();
                return 1;
            }

            Trace.Listeners.Add(new ConsoleTraceListener());

            Language language = args.Contains("-vb") ? Language.VisualBasic : Language.CSharp;

            try
            {
                if (args.Contains("-source") || args.Contains("-both"))
                    GenerateCodeFile(args[1], args[2], language, args[4]);
                if (args.Contains("-assembly") || args.Contains("-both"))
                    GenerateAssembly(args[1], args[2], language, args[4]);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return 2;
            }

            return 0;
        }

        public static void GenerateAssembly(string inputPath, string outputPath, Language language = Language.CSharp, string schemaFile = "Instruments.xsd") //Library entry point
        {
            CodeNamespace ns = generateCodeNamespace(inputPath, schemaFile);

            generateAssembly(ns, outputPath, language);
        }

        public static string GenerateCodeFile(string inputPath, string outputPath, Language language = Language.CSharp, string schemaFile = "Instruments.xsd")
        {
            CodeNamespace codeNamespace = generateCodeNamespace(inputPath, schemaFile);

            CodeDomProvider provider = CodeDomProvider.CreateProvider(language.ToString());

            string fileName = Path.Combine(outputPath, "Instrument." + provider.FileExtension);
            using (StreamWriter writer = new StreamWriter(fileName, false))
                provider.GenerateCodeFromNamespace(codeNamespace, writer, new CodeGeneratorOptions { BracingStyle = "C" });

            return fileName;
        }

        static CodeNamespace generateCodeNamespace(string xmlFileOrFolder, string schemaFile)
        {
            var files = (File.GetAttributes(xmlFileOrFolder).HasFlag(FileAttributes.Directory) ? (Directory.EnumerateFiles(xmlFileOrFolder)) : (new[] {xmlFileOrFolder}))
                .Where(file => Path.GetExtension(file) == ".xml")
                .Select(file => XDocument.Load(file, LoadOptions.SetBaseUri))
                .Where(doc => doc.Root != null && doc.Root.Name.LocalName == "Instrument")
                ;

            CodeNamespace ns = new CodeNamespace("TsdLib.Instrument");
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument"));

            foreach (XDocument doc in files)
            {
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, schemaFile);

                string docName = doc.BaseUri.TrimStart("file:///".ToCharArray());
                doc.Validate(schemas,
                    (o, e) => { throw new InstrumentGeneratorException("File: " + docName + " could not be validated against schema: " + schemaFile, e.Exception); });

                XElement instrumentElement = doc.Root;
                Debug.Assert(instrumentElement != null, "File: " + docName + " does not have a valid root element.");

                string connectionType = (string)instrumentElement.Attribute("ConnectionType");
                ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument." + connectionType.Split(new[] { "Connection" }, StringSplitOptions.RemoveEmptyEntries)[0]));

                //Generate instrument class
                CodeTypeDeclaration instrumentClass = new CodeTypeDeclaration((string)instrumentElement.Attribute("Name"));
                ns.Types.Add(instrumentClass);

                //Add InstrumentBase reference
                CodeTypeReference instrumentBaseReference = new CodeTypeReference("InstrumentBase");
                instrumentBaseReference.TypeArguments.Add(connectionType);
                instrumentClass.BaseTypes.Add(instrumentBaseReference);

                //Add interface references
                InterfaceReferenceCollection interfaceReferences = new InterfaceReferenceCollection(instrumentElement);
                instrumentClass.BaseTypes.AddRange(interfaceReferences);

                //Add ID attributes
                instrumentClass.CustomAttributes.Add(new IdQueryAttributeDeclaration(instrumentElement.Elements().First(e => e.Name.LocalName == "IdQuery")));

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
                ctor.Parameters.Add(new CodeParameterDeclarationExpression(connectionType, "connection"));
                ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("connection"));

                //Add factory field and methods
                instrumentClass.Members.Add(new FactoryReference(connectionType));
                instrumentClass.Members.Add(new GetInstanceMethod(instrumentClass.Name));
                instrumentClass.Members.Add(new GetInstanceMethod(instrumentClass.Name, "address"));
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

        static void generateAssembly(CodeNamespace codeNamespace, string outputPath, Language language = Language.CSharp)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(language.ToString());

            string[] namespaces = codeNamespace.Imports
                .Cast<CodeNamespaceImport>()
                .Select((cnsi => cnsi.Namespace + ".dll"))
                .ToArray();

            CodeCompileUnit ccu = new CodeCompileUnit();

            //CodeAttributeDeclaration friendAssemblyAttribute =
            //    new CodeAttributeDeclaration(
            //        new CodeTypeReference(typeof(InternalsVisibleToAttribute)),
            //        new CodeAttributeArgument(
            //            new CodePrimitiveExpression("TsdLib.Instrument")));
            //ccu.AssemblyCustomAttributes.Add(friendAssemblyAttribute);
            
            ccu.ReferencedAssemblies.AddRange(namespaces);
            ccu.Namespaces.Add(codeNamespace);
            
            CompilerParameters cp = new CompilerParameters
            {
                OutputAssembly = Path.Combine(outputPath, "TsdLib.Instrument.dll"),
                IncludeDebugInformation = true,
                GenerateExecutable = false,
            };

            CompilerResults cr = provider.CompileAssemblyFromDom(cp, ccu);

            if (cr.Errors.HasErrors)
                throw new CompilerException("Error compiling.", cr.Errors);
        }
    }

#region Custom Code Member Classes

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
            string typeName = connectionType.Split(new[] {"Connection"}, StringSplitOptions.RemoveEmptyEntries)[0];
            Type = new CodeTypeReference(typeName + "Factory");
            Name = "_factory";
            Attributes = MemberAttributes.Static;
            InitExpression = new CodeObjectCreateExpression(Type);
        }
    }

    class GetInstanceMethod : CodeMemberMethod
    {
         public GetInstanceMethod(string instrumentName, string argumentName = null)
         {
             Attributes = MemberAttributes.Public | MemberAttributes.Static;
             Name = "GetInstance";
             ReturnType = new CodeTypeReference(instrumentName);
             CodeMethodReferenceExpression factoryMethodReference =
                 new CodeMethodReferenceExpression(
                     new CodeVariableReferenceExpression("_factory"),
                     "GetInstrument");
             factoryMethodReference.TypeArguments.Add(new CodeTypeReference(instrumentName));

             CodeMethodInvokeExpression factoryMethodInvoke = new CodeMethodInvokeExpression(factoryMethodReference);

             if (argumentName != null)
             {
                 Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "address"));
                 factoryMethodInvoke.Parameters.Add(new CodePrimitiveExpression(argumentName));
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
