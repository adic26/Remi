using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;

namespace TsdLib.CodeGenerator
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
    public static class CodeGenerator
    {
        /// <summary>
        /// Generate an assembly from the specified source code file.
        /// </summary>
        /// <param name="sourceCodeFilePath">Absolute or relative path to the source code file to compile into the assembly.</param>
        /// <param name="referencedAssemblies">Zero or more assemblies to reference in the generated assembly.</param>
        /// <returns>Absolute path the the generated assembly.</returns>
        public static string GenerateTestSequenceFromFile(string sourceCodeFilePath, params string[] referencedAssemblies)
        {
            Trace.Write(string.Format("Compiling test sequence from {0}... ", Path.GetFileName(sourceCodeFilePath)));
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters cp = new CompilerParameters
            {
                IncludeDebugInformation = true,
                OutputAssembly = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".dll"),
            };

#if DEBUG
            cp.CompilerOptions += " /d:DEBUG";
#endif
#if TRACE
            cp.CompilerOptions += " /d:TRACE";
#endif
            
            cp.ReferencedAssemblies.AddRange(referencedAssemblies);

            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceCodeFilePath);

            if (cr.Errors.HasErrors)
                throw new CompilerException(cr.Errors);

            Trace.WriteLine("Complete.");
            return cr.PathToAssembly;
        }
    }

}
