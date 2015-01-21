using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace TsdLib
{
    /// <summary>
    /// A tool to generate a <see cref="CodeCompileUnit"/> from source code.
    /// </summary>
    public class BasicCodeParser : ICodeParser
    {
        private readonly string[] _assemblyReferences;

        /// <summary>
        /// Initialize a BasicCodeParser with zero or more assembly references.
        /// </summary>
        /// <param name="assemblyReferences">Zero or more library (*.dll) files to reference.</param>
        public BasicCodeParser(params string[] assemblyReferences)
        {
            _assemblyReferences = assemblyReferences;
        }

        /// <summary>
        /// Parse the source code contained in a <see cref="System.IO.TextReader"/> .
        /// </summary>
        /// <param name="sourceFile">A <see cref="System.IO.TextReader"/> object containing the source code as a string (<see cref="System.IO.StringReader"/>), a text file (<see cref="System.IO.StreamReader"/>) or another <see cref="System.IO.TextReader"/> implementation.</param>
        /// <returns>A <see cref="CodeCompileUnit"/> containing the source code and required assembly references (where specified in the constructor).</returns>
        public CodeCompileUnit Parse(TextReader sourceFile)
        {
            CodeSnippetCompileUnit ccu = new CodeSnippetCompileUnit(sourceFile.ReadToEnd());
            ccu.ReferencedAssemblies.AddRange(_assemblyReferences);
            return ccu;
        }
    }
}