﻿using System;
using System.CodeDom.Compiler;
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
        public CodeGeneratorException(string message) : base(message) { }
        /// <summary>
        /// Initialize a new CodeGeneratorException with the specified message and inner exception.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">The Exception that is the cause of the CodeGeneratorException.</param>
        public CodeGeneratorException(string message, Exception inner) : base(message, inner) { }
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
        public CompilerException(CompilerErrorCollection errors)
            : base(errors.ToStringEx()) { }
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
