using System;

namespace TsdLib.TestSequence
{
    /// <summary>
    /// Custom attribute to add an assembly reference to a dynamic test sequence.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyReferenceAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the referenced assembly.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Initialize a new AssemblyReferenceAttribute with the specified assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly to reference.</param>
        public AssemblyReferenceAttribute(string assemblyName)
        {
            AssemblyName = assemblyName;
        }
    }
}
