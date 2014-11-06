using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TsdLib.CodeGenerator
{
    /// <summary>
    /// Contains functionality to dynamically generate .NET source code and/or assemblies.
    /// </summary>
    public class DynamicCompiler
    {
        private readonly Language _language;
        private readonly string _tempPath;

        /// <summary>
        /// Initialize a new code generator.
        /// </summary>
        /// <param name="language">Generate C# or Visual Basic code.</param>
        public DynamicCompiler(Language language)
        {
            _language = language;
            _tempPath = Path.Combine(Path.GetTempPath(), "TsdLib");
            if (!Directory.Exists(_tempPath))
                Directory.CreateDirectory(_tempPath);
        }

        /// <summary>
        /// Generates an assembly from the specified XML instrument definition file(s) and test sequence source code file.
        /// </summary>
        /// <param name="testSequenceSourceCode">String containing the test sequence source code.</param>
        /// <param name="referencedAssemblies">Names of assemblies to be referenced by the test sequence assembly.</param>
        /// <returns>Absolute path the the generated assembly.</returns>
        public string GenerateTestSequenceAssembly(string testSequenceSourceCode, IEnumerable<string> referencedAssemblies)
        {

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
            
            CodeSnippetCompileUnit sequenceCcu = new CodeSnippetCompileUnit(testSequenceSourceCode);
            sequenceCcu.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());

            CodeGeneratorOptions options = new CodeGeneratorOptions { BracingStyle = "C"};

            //TODO: set StreamWriter line endings to Environment.NewLine

            string sequenceCodePath = Path.Combine(_tempPath, "sequence." + provider.FileExtension);
            using (StreamWriter w = new StreamWriter(sequenceCodePath, false))
                provider.GenerateCodeFromCompileUnit(sequenceCcu, w, options);

            CompilerResults compilerResults = provider.CompileAssemblyFromFile(cp, sequenceCodePath);

            if (compilerResults.Errors.HasErrors)
                throw new CompilerException(compilerResults.Errors);

            Trace.WriteLine("Compiled successfully.");

            return compilerResults.PathToAssembly;
        }

        /// <summary>
        /// Generates an class library (*.dll) assembly from the specified sequence of <see cref="CodeCompileUnit"/>.
        /// </summary>
        /// <param name="codeCompileUnits">A sequence of <see cref="CodeCompileUnit"/> containing the source code and assembly references required for compilation.</param>
        /// <returns>An absolute path to the dynamically generated assembly.</returns>
        public string Compile(IEnumerable<CodeCompileUnit> codeCompileUnits)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(_language.ToString());

            foreach (string file in Directory.EnumerateFiles(_tempPath))
                File.Delete(file);

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

            //TODO: if debugging isn't possible, generate source code files and add references from ccu's to cp
            //foreach (string s in sourceCode)
            //{
            //    CodeSnippetCompileUnit ccu = new CodeSnippetCompileUnit(s);
            //    Match m = Regex.Match(s, @"(?<=class )\w+");
            //    string fileName = Path.ChangeExtension(m.Success ? m.Value + "." : Path.GetRandomFileName(), provider.FileExtension);
            //    string fullPath = Path.Combine(_tempPath, fileName);
            //    using (StreamWriter w = new StreamWriter(fullPath, false))
            //        provider.GenerateCodeFromCompileUnit(ccu, w, new CodeGeneratorOptions { BracingStyle = "C" });
            //    codeFiles.Add(fullPath);
            //}

            foreach (CodeCompileUnit codeCompileUnit in codeCompileUnits)
            {
                cp.ReferencedAssemblies.AddRange(codeCompileUnit.ReferencedAssemblies.Cast<string>().ToArray());

                string fileName = Path.ChangeExtension(Path.GetRandomFileName(), provider.FileExtension);
                string fullPath = Path.Combine(_tempPath, fileName);
                using (StreamWriter w = new StreamWriter(fullPath, false))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, w, new CodeGeneratorOptions { BracingStyle = "C" });
                }


            }

            CompilerResults compilerResults = provider.CompileAssemblyFromFile(cp, Directory.GetFiles(_tempPath, "*." + provider.FileExtension));

            if (compilerResults.Errors.HasErrors)
                throw new CompilerException(compilerResults.Errors);

            Trace.WriteLine("Compiled successfully.");

            return compilerResults.PathToAssembly;
        }

        /// <summary>
        /// Generates an assembly from the source code files.
        /// </summary>
        /// <param name="sourceCode">A sequence of strings containing the source code to compile.</param>
        /// <param name="referencedAssemblies">Names of assemblies to be referenced by the compiled assembly.</param>
        /// <returns>Absolute path the the generated assembly.</returns>
        public string GenerateTestSequenceAssembly(IEnumerable<string> sourceCode, IEnumerable<string> referencedAssemblies)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(_language.ToString());

            string dllPath = Path.Combine(_tempPath, "sequence.dll");

            CompilerParameters cp = new CompilerParameters
            {
                IncludeDebugInformation = true,
                OutputAssembly = dllPath
            };
            cp.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());

#if DEBUG
            cp.CompilerOptions += " /d:DEBUG";
#endif
#if TRACE
            cp.CompilerOptions += " /d:TRACE";
#endif

            List<string> codeFiles = new List<string>();
            foreach (string s in sourceCode)
            {
                CodeSnippetCompileUnit ccu = new CodeSnippetCompileUnit(s);
                Match m = Regex.Match(s, @"(?<=class )\w+");
                string fileName = Path.ChangeExtension(m.Success ? m.Value + "." : Path.GetRandomFileName(), provider.FileExtension);
                string fullPath = Path.Combine(_tempPath, fileName);
                using (StreamWriter w = new StreamWriter(fullPath, false))
                    provider.GenerateCodeFromCompileUnit(ccu, w, new CodeGeneratorOptions {BracingStyle = "C"});
                codeFiles.Add(fullPath);
            }

            //TODO: set StreamWriter line endings to Environment.NewLine

            CompilerResults compilerResults = provider.CompileAssemblyFromFile(cp, codeFiles.ToArray());

            if (compilerResults.Errors.HasErrors)
                throw new CompilerException(compilerResults.Errors);

            Trace.WriteLine("Compiled successfully.");

            return compilerResults.PathToAssembly;
        }
    }
}
