
using System.IO;
using System.Linq;
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

        [Required]
        public string AssemblyInfoFilePath { get; set; }

        /// <summary>
        /// Executes the task to create an <see cref="AssemblyInformationalVersionAttribute"/> with a value equal to the <see cref="AssemblyVersionAttribute"/>.
        /// </summary>
        /// <returns>True if the task successfully executed; otherwise, false.</returns>
        public override bool Execute()
        {
            base.Execute();

            ITaskItem asy = Assemblies.FirstOrDefault();
            if (asy == null)
                return false;
            if (!asy.MetadataNames.Cast<string>().Contains("Version"))
                return false;
            string version = asy.GetMetadata("Version");

//            using (StreamWriter writer = File.AppendText(AssemblyInfoFilePath))
//            {
//                writer.WriteLine(
//@"#if DEBUG
//[assembly: AssemblyInformationalVersion(""1.1.0-debug"")]
//#endif");
//            }

            return true;
        }
    }
}
