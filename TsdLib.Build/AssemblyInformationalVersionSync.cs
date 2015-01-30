
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;

namespace TsdLib.Build
{
    public class AssemblyInformationalVersionSync : GetAssemblyIdentity
    {
        //private const string RegExPattern = @"(?<=#if DEBUG\s+[assembly: AssemblyInformationalVersion\("").*(?="")]\s+#endif)";
        private const string RegExPattern = @"(?<=assembly: AssemblyInformationalVersion\("").*(?=""\))";

        private const string InfoVersionDeclaration =@"
#if DEBUG
[assembly: AssemblyInformationalVersion(""1.1.0-debug"")]
#endif";

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
            try
            {
                base.Execute();

                ITaskItem asy = Assemblies.FirstOrDefault();
                if (asy == null)
                    return false;
                if (!asy.MetadataNames.Cast<string>().Contains("Version"))
                    return false;
                string version = asy.GetMetadata("Version");

                string existing = File.ReadAllText(AssemblyInfoFilePath);
                string modified = Regex.IsMatch(existing, RegExPattern) ? Regex.Replace(existing, RegExPattern, version + "-debug") : existing + InfoVersionDeclaration;
                //TODO: InformationalVersion is lower than AssemblyVersion by a few seconds
                File.WriteAllText(AssemblyInfoFilePath, modified);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
