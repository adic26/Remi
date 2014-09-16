﻿using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Text;

namespace TsdLib.CodeGenerator
{
    /// <summary>
    /// Exception due to an error generating source code and/or assembly.
    /// </summary>
    [Serializable]
    public class CodeGeneratorException : TsdLibException
    {
        /// <summary>
        /// Initialize a new CodeGeneratorException with the specified message.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the CodeGeneratorException.</param>
        public CodeGeneratorException(string message, Exception inner = null)
            : base(message, inner) { }

        ///// <summary>
        ///// Deserialization constructor used by the .NET Framework to initialize an instance of the CodeGeneratorException class from serialized data.
        ///// </summary>
        ///// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        ///// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        //protected CodeGeneratorException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }
    }

    /// <summary>
    /// Exception due to an error compiling input source code.
    /// </summary>
    [Serializable]
    public class CompilerException : TsdLibException
    {
        /// <summary>
        /// Initialize a new CompilerException with the specified collection of complier errors.
        /// </summary>
        /// <param name="errors">A CompilerErrorCollection object containing all of the errors and warnings generated by the compiler.</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the CompilerException.</param>
        public CompilerException(CompilerErrorCollection errors, Exception inner = null)
            : base(errors.ToStringEx(), inner) { }

        ///// <summary>
        ///// Deserialization constructor used by the .NET Framework to initialize an instance of the CompilerException class from serialized data.
        ///// </summary>
        ///// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        ///// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        //protected CompilerException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }
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
