using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Text;

namespace TsdLib.InstrumentGenerator
{
    class InstrumentGeneratorException : TsdLibException
    {
        public InstrumentGeneratorException(string message) : base(message) { }
        public InstrumentGeneratorException(string message, Exception inner) : base(message, inner) { }
        protected InstrumentGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    class CompilerException : TsdLibException
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
                sb.Append(compilerError);
            return sb.ToString();
        }
    }
}
