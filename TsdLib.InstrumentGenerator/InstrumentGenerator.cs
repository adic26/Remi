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
        static int Main(string[] args) //Console entry point
        {
            if (args.Length == 0)
            {
                const int width = 20;
                Console.WriteLine(string.Join(Environment.NewLine,
                    "TsdLib Dynamic Instrument Assembly Generator",
                    "Dynamically generates class library (*.dll) files",
                    "",
                    "Usage: TsdLib.InstrumentGenerator.exe <xmlFileOrFolder> -cs|-vb",
                    "",
                    "   <xmlFileOrFolder>".PadRight(width) + "absolute or relative path to the instrument file or folder",
                    "   -cs".PadRight(width) + "generate C# code",
                    "   -vb".PadRight(width) + "generate Visual Basic code",
                    "",
                    "Press Enter to exit..."
                    ));

                Console.ReadLine();
                return 1;
            }

            Trace.Listeners.Add(new ConsoleTraceListener());

            GenerateInstrumentAssembly(args[0], args.Contains("-vb") ? Language.VisualBasic : Language.CSharp);

            return 0;
        }

        public static Assembly GenerateInstrumentAssembly(string xmlFileOrFolder, Language language, string schemaFile = "Instruments.xsd") //Library entry point
        {
            CodeNamespace ns = GenerateCodeNamespace(xmlFileOrFolder, schemaFile);

            Assembly asy = GenerateAssembly(ns, language, true);

            return asy;
        }

        static CodeNamespace GenerateCodeNamespace(string xml, string schema)
        {
            IEnumerable<string> files = File.GetAttributes(xml).HasFlag(FileAttributes.Directory) ? Directory.EnumerateFiles(xml) : new[] { xml };

            CodeNamespace ns = new CodeNamespace("TsdLib.Instrument.Dynamic");
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument"));

            foreach (string file in files)
            {
                XDocument doc = XDocument.Load(file);

                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, schema);

                string file1 = file; //introduce local variable to avoid capturing foreach iteration variable in closure
                doc.Validate(schemas,
                    (o, e) => { throw new InstrumentGeneratorException("File: " + file1 + " could not be validated against schema: " + schema, e.Exception); });

                XElement instrumentElement = doc.Root;
                Debug.Assert(instrumentElement != null, "File: " + file + " does not have a valid root element.");

                //Generate instrument class
                CodeTypeDeclaration instrumentClass = new CodeTypeDeclaration(instrumentElement.Name.LocalName);

                string connectionType = (string)instrumentElement.Attribute("ConnectionType");
                CodeTypeReference instrumentBaseReference = new CodeTypeReference(typeof(InstrumentBase<>));
                instrumentBaseReference.TypeArguments.Add(connectionType);
                instrumentClass.BaseTypes.Add(instrumentBaseReference);

                ns.Imports.Add(new CodeNamespaceImport("TsdLib.Instrument." + connectionType.Split(new[] { "Connection" }, StringSplitOptions.RemoveEmptyEntries)[0]));

                IEnumerable<string> interfaceNames = instrumentElement
                    .Attribute("Interfaces")
                    .Value
                    .Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);

                foreach (string interfaceName in interfaceNames)
                    instrumentClass.BaseTypes.Add(interfaceName);

                ns.Types.Add(instrumentClass);
                
                string identifier = (string)instrumentElement.Attribute("Identifier");
                CodeAttributeDeclaration identifierAttribute = new CodeAttributeDeclaration("Identifier", new CodeAttributeArgument(new CodePrimitiveExpression(identifier)));
                instrumentClass.CustomAttributes.Add(identifierAttribute);

                //Generate methods
                foreach (XElement methodElement in instrumentElement.Elements("Command"))
                {
// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                    CodeMemberMethod methodMember = new CodeMemberMethod
                    {
                        Name = methodElement.Attribute("Name").Value,
                        Attributes = MemberAttributes.Public | MemberAttributes.Final
                    };

                    //Required for VB code generation
                    string interfaceName = interfaceNames.FirstOrDefault(); //should add VB support for defining interface at the method level
                    if (interfaceName != null)
                        methodMember.ImplementationTypes.Add(interfaceName);

                    //add method parameters
                    List<CodeExpression> innerMethodParameters = new List<CodeExpression> { new CodePrimitiveExpression(methodElement.Attribute("Message").Value) };
                    foreach (XElement parameterElement in methodElement.Elements("Parameter"))
                    {
                        string type = parameterElement.Attribute("Type").Value;
                        string name = parameterElement.Attribute("Name").Value;
                        methodMember.Parameters.Add(new CodeParameterDeclarationExpression(type, name));
                        innerMethodParameters.Add(new CodeArgumentReferenceExpression(name));
                    }

                    //add internal connection reference
                    CodeFieldReferenceExpression connectionReference = new CodeFieldReferenceExpression(null, "Connection");
                    CodeMethodReferenceExpression sendCommandMethodReference = new CodeMethodReferenceExpression(connectionReference, "SendCommand");
                    CodeMethodInvokeExpression sendCommandMethodInvoke = new CodeMethodInvokeExpression(sendCommandMethodReference, innerMethodParameters.ToArray());
                    
                    string returnType = (string) methodElement.Attribute("ReturnType");
                    if (returnType == null || returnType == "Void")
                        methodMember.Statements.Add(sendCommandMethodInvoke);
                    else
                    {
                        CodeTypeReference returnTypeReference = new CodeTypeReference(returnType);
                        methodMember.ReturnType = returnTypeReference;
                        sendCommandMethodReference.TypeArguments.Add(returnTypeReference);
                        CodeMethodReturnStatement returnStatement =
                            new CodeMethodReturnStatement(sendCommandMethodInvoke);
                        string parser = (string) methodElement.Attribute("ResponseParser") ?? ".*";
                        sendCommandMethodInvoke.Parameters.Insert(1, new CodePrimitiveExpression(parser));
                        methodMember.Statements.Add(returnStatement);
                    }

                    instrumentClass.Members.Add(methodMember);
                }
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
}
