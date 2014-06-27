using System;
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
using TsdLib.Instrument;

namespace TsdLib.InstrumentGenerator
{
    public enum Language
    {
        CSharp,
        VisualBasic
    }
    
    public class InstrumentGenerator
    {
        public static int Main(string[] args) //Console entry point
        {
            if (args.Length == 0)
            {
                const int width = 20;
                Console.WriteLine(string.Join(Environment.NewLine,
                    "TsdLib Dynamic Instrument Assembly Generator",
                    "Dynamically generates class library (*.dll) files",
                    "",
                    "Usage: TsdLib.InstrumentGenerator.exe -f c|<xmlFileOrFolder> [-vb]",
                    "",
                    "   -f <xmlFileOrFolder>".PadRight(width) + "XML source location. Can be a file or folder.",
                    "       <xmlFileOrFolder>".PadRight(width) + "absolute or relative path to the instrument file or folder",
                    "       -c".PadRight(width) + "use current execution directory",
                    "   -vb".PadRight(width) + "Visual Basic code (C# is default)",
                    "",
                    "Press Enter to exit..."
                    ));

                Console.ReadLine();
                return 1;
            }

            Trace.Listeners.Add(new ConsoleTraceListener());

            var location = args.ElementAt(Array.IndexOf(args, "-f") + 1);

            if (location == "c")
                location = Directory.GetCurrentDirectory();

            //string location = args.Contains("-f c") ? Directory.GetCurrentDirectory() : args.ElementAt(Array.IndexOf(args, "-f") + 1);

            try
            {
                GenerateInstrumentAssembly(location, args.Contains("-vb") ? Language.VisualBasic : Language.CSharp);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return 2;
            }
            

            return 0;
        }

        public static Assembly GenerateInstrumentAssembly(string xmlFileOrFolder, Language language, string schemaFile = "Instruments.xsd") //Library entry point
        {
            CodeNamespace ns = GenerateCodeNamespace(xmlFileOrFolder, schemaFile);

            Assembly asy = GenerateAssembly(ns, language, true);

            return asy;
        }

        static CodeNamespace GenerateCodeNamespace(string xmlFileOrFolder, string schema)
        {
            var files = (File.GetAttributes(xmlFileOrFolder).HasFlag(FileAttributes.Directory) ? (Directory.EnumerateFiles(xmlFileOrFolder)) : (new[] {xmlFileOrFolder}))
                .Where(file => Path.GetExtension(file) == ".xml")
                .Select(file => XDocument.Load(file, LoadOptions.SetBaseUri))
                .Where(doc => doc.Root != null && doc.Root.Name.LocalName == "Instrument")
                ;

            CodeNamespace ns = new CodeNamespace("TsdLib.Instrument.Dynamic");
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument"));

            foreach (XDocument doc in files)
            {
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, schema);

                string docName = doc.BaseUri.Replace("file:///", "");
                doc.Validate(schemas,
                    (o, e) => { throw new InstrumentGeneratorException("File: " + docName + " could not be validated against schema: " + schema, e.Exception); });

                XElement instrumentElement = doc.Root;
                Debug.Assert(instrumentElement != null, "File: " + docName + " does not have a valid root element.");

                //Generate instrument class
                CodeTypeDeclaration instrumentClass = new CodeTypeDeclaration(instrumentElement.Attribute("Name").Value);

                string connectionType = (string)instrumentElement.Attribute("ConnectionType");
                CodeTypeReference instrumentBaseReference = new CodeTypeReference(typeof(InstrumentBase<>));
                instrumentBaseReference.TypeArguments.Add(connectionType);
                instrumentClass.BaseTypes.Add(instrumentBaseReference);

                ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument." + connectionType.Split(new[] { "Connection" }, StringSplitOptions.RemoveEmptyEntries)[0]));

                //Add interface references
                InterfaceReferenceCollection interfaceReferences = new InterfaceReferenceCollection(instrumentElement);
                instrumentClass.BaseTypes.AddRange(interfaceReferences);

                ns.Types.Add(instrumentClass);

                //Add ID attributes
                string idCommand = (string)instrumentElement.Attribute("IdCommand");
                if (idCommand != null)
                    instrumentClass.CustomAttributes.Add(new IdCommandAttribute(idCommand));
                instrumentClass.CustomAttributes.Add(new IdResponseAttribute((string) instrumentElement.Attribute("IdResponse"), (string)instrumentElement.Attribute("IdResponseTermChar")));

                //Add VisaAttributes
                foreach (XElement visaAttributeElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "VisaAttribute"))
                    instrumentClass.CustomAttributes.Add(new VisaAttributeAttribute(visaAttributeElement));

                //TODO: Add initialization commands
                var initCommands = (string)instrumentElement.Attribute("InitCommands");
                if (initCommands != null)
                {
                    
                }

                //Add info property overloads
                foreach (XElement infoElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "Info"))
                {
                    instrumentClass.Members.Add(new InfoProperty(infoElement, "Message", idCommand));
                    if ((string)infoElement.Attribute("RegEx") != null)
                    instrumentClass.Members.Add(new InfoProperty(infoElement, "RegEx", ".*"));
                }

                instrumentClass.Members.Add(new InfoProperty(instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "ModelNumber"), "Message", idCommand));
                instrumentClass.Members.Add(new InfoProperty(instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "SerialNumber"), "Message", idCommand));
                instrumentClass.Members.Add(new InfoProperty(instrumentElement.Elements().FirstOrDefault(e => e.Name.LocalName == "FirmwareVersion"), "Message", idCommand));

                //Generate command methods
                foreach (XElement methodElement in instrumentElement.Elements().Where(e => e.Name.LocalName == "Command"))
                    instrumentClass.Members.Add(new CommandMethod(methodElement));

            }
            return ns;
        }

        static Assembly GenerateAssembly(CodeNamespace codeNamespace, Language language, bool generateCodeFile)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(language.ToString());

            if (generateCodeFile)
            {
                string fileName = Path.Combine(SpecialFolders.Assemblies, codeNamespace.Name + "." + provider.FileExtension);
                using (StreamWriter writer = new StreamWriter(fileName, false))
                    provider.GenerateCodeFromNamespace(codeNamespace, writer, new CodeGeneratorOptions { BracingStyle = "C" });
            }

            CodeAttributeDeclaration attribute = new CodeAttributeDeclaration(new CodeTypeReference(typeof (InternalsVisibleToAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression("TsdLib.Instrument")));

            string[] namespaces = codeNamespace.Imports
                .Cast<CodeNamespaceImport>()
                .Select((cnsi => cnsi.Namespace + ".dll"))
                .ToArray();

            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.AssemblyCustomAttributes.Add(attribute);

            ccu.ReferencedAssemblies.AddRange(namespaces);

            ccu.Namespaces.Add(codeNamespace);
            
            CompilerParameters cp = new CompilerParameters
            {
                OutputAssembly = Path.Combine(SpecialFolders.Assemblies, codeNamespace.Name + ".dll"),
                IncludeDebugInformation = true,
                GenerateExecutable = false,
            };

            CompilerResults cr = provider.CompileAssemblyFromDom(cp, ccu);

            if (cr.Errors.HasErrors)
                throw new CompilerException("Error compiling.", cr.Errors);

            return cr.CompiledAssembly;
        }
    }

    class IdCommandAttribute : CodeAttributeDeclaration
    {
        public IdCommandAttribute(string command)
        {
            Name = "IdCommand";
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(command)));
        }
    }

    class IdResponseAttribute : CodeAttributeDeclaration
    {
        public IdResponseAttribute(string command, string termChar = null)
        {
            Name = "IdResponse";
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(command)));
            if (termChar != null)
                Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(termChar[0])));
        }
    }

    class VisaAttributeAttribute : CodeAttributeDeclaration
    {
        public VisaAttributeAttribute(XElement visaAttributeElement)
        {
            Name = "VisaAttribute";
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression((string)visaAttributeElement.Attribute("Name"))));
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression((string)visaAttributeElement.Attribute("Type"))));
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression((string)visaAttributeElement.Attribute("Value"))));
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
    //new InfoProperty(instrumentElement.Element("ModelNumber"), "Message", idCommand)
    class InfoProperty : CodeMemberProperty
    {
        public InfoProperty(XElement infoElement, string attributeName, string defaultValue)
        {
            Name = infoElement.Name.LocalName + attributeName;
            Type = new CodeTypeReference(typeof (string));
// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            Attributes = MemberAttributes.Family | MemberAttributes.Override;

            GetStatements.Add(new CodeMethodReturnStatement(
                            new CodePrimitiveExpression((string)infoElement.Attribute(attributeName) ?? defaultValue)));
        }
    }

    class InitCommandMethod : CodeMemberMethod
    {
        public InitCommandMethod(string initCommand)
        {
            Name = "Init";
        }
    }

    class CommandMethod : CodeMemberMethod
    {
        public CommandMethod(XElement methodElement)
        {
            Name = methodElement.Attribute("Name").Value;
// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            Attributes = MemberAttributes.Public | MemberAttributes.Final;

            //Required for VB code generation
            string interfaceImplementation = (string) methodElement.Attribute("Implements");
            if (interfaceImplementation != null)
                ImplementationTypes.Add(interfaceImplementation);
            else
            {
                if (methodElement.Parent != null)
                    ImplementationTypes.Add((string)methodElement.Parent.Attribute("Interfaces"));
            }

            //Method parameters
            List<CodeExpression> innerMethodParameters = new List<CodeExpression> { new CodePrimitiveExpression(methodElement.Attribute("Message").Value) };
            foreach (XElement parameterElement in methodElement.Elements().Where(e => e.Name.LocalName == "Parameter"))
            {
                string type = parameterElement.Attribute("Type").Value;
                string name = parameterElement.Attribute("Name").Value;
                Parameters.Add(new CodeParameterDeclarationExpression(type, name));
                innerMethodParameters.Add(new CodeArgumentReferenceExpression(name));
            }

            CodeMethodReferenceExpression sendCommandMethodReference = new CodeMethodReferenceExpression(new CodeFieldReferenceExpression(null, "Connection"), "SendCommand");
            CodeMethodInvokeExpression sendCommandMethodInvoke = new CodeMethodInvokeExpression(sendCommandMethodReference, innerMethodParameters.ToArray());

            string returnType = (string)methodElement.Attribute("ReturnType");
            if (returnType == null || returnType == "Void")
                Statements.Add(sendCommandMethodInvoke);
            else
            {
                CodeTypeReference returnTypeReference = new CodeTypeReference(returnType);
                ReturnType = returnTypeReference;
                sendCommandMethodReference.TypeArguments.Add(returnTypeReference);
                CodeMethodReturnStatement returnStatement =
                    new CodeMethodReturnStatement(sendCommandMethodInvoke);
                string parser = (string)methodElement.Attribute("RegEx") ?? ".*";
                sendCommandMethodInvoke.Parameters.Insert(1, new CodePrimitiveExpression(parser));
                Statements.Add(returnStatement);
            }
        }
    }
}
