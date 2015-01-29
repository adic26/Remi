
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

namespace TsdLib.Build
{
    public class AssemblyInformationalVersionSync : GetAssemblyIdentity
    {
        [Output]
        public string AssemblyInformationalVersionAttributeValue { get; set; }

        /// <summary>
        /// Executes the task to create an <see cref="AssemblyInformationalVersionAttribute"/> with a value equal to the <see cref="AssemblyVersionAttribute"/>.
        /// </summary>
        /// <returns>True if the task successfully executed; otherwise, false.</returns>
        public override bool Execute()
        {
            base.Execute();

            var str = Assemblies;


            return true;
        }
    }
}
