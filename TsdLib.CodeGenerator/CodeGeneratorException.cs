using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Text;

namespace TsdLib.CodeGenerator
{
    [Serializable]
    public class CodeGeneratorException : TsdLibException
    {
        public CodeGeneratorException(string message) : base(message) { }
        public CodeGeneratorException(string message, Exception inner) : base(message, inner) { }
        protected CodeGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CompilerException : TsdLibException
    {
        public CompilerException(string message, CompilerErrorCollection errors)
            : base(message + Environment.NewLine + errors.ToStringEx()) { }
    }

    static class CompilerErrorCollectionExtension
    {
        public static string ToStringEx(this CompilerErrorCollection collection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (CompilerError compilerError in collection)
            {
                sb.AppendLine(compilerError.ToString());
                if (compilerError.ErrorNumber == "CS0006")
                    sb.Append(". Please make sure your client has a reference to the dll.");
            }
            return sb.ToString();
        }
    }
}
