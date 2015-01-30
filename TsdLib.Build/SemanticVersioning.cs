

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace TsdLib.Build
{
    public class ApplySemanticVersioning : Task
    {
        [Required]
        public string AssemblyInfoFilePath { get; set; }

        public string DefaultVersionLine { get; set; }
        public string DefaultFileVersionLine { get; set; }
        public string DefaultInformationalVersionLine { get; set; }

        public override bool Execute()
        {
            try
            {
                string origialAsyInfo = File.ReadAllText(AssemblyInfoFilePath);
                string validatedAsyInfo = ValidateVersions(origialAsyInfo);

                var maxVersion = Regex.Matches(validatedAsyInfo, @"(?<=Assembly.*Version\("")\d+\.\d+\.\d+").Cast<Match>().Select(m => new Version(m.Value)).Max();

                var newVersion = new Version(maxVersion.Major, maxVersion.Minor, maxVersion.Build + 1 );

                string newAsyInfo = Regex.Replace(validatedAsyInfo, @"(?<=Assembly.*Version\("")\d+\.\d+\.\d+", newVersion.ToString());

                File.WriteAllText(AssemblyInfoFilePath, newAsyInfo);

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        private string ValidateVersions(string existingAsyInfo)
        {
            StringBuilder sb = new StringBuilder(existingAsyInfo).AppendLine();

            validateVersionInner(sb, @"(?<=\[assembly: AssemblyVersion\().*(?=\)\])", @"""\d+\.\d+\.\d+""", DefaultVersionLine ?? @"[assembly: AssemblyVersion(""1.0.0"")]");

            validateVersionInner(sb, @"(?<=assembly: AssemblyFileVersion\("").*(?=""\))", @"\d+\.\d+\.\d+", DefaultFileVersionLine ?? @"[assembly: AssemblyFileVersion(""1.0.0"")]");

            validateVersionInner(sb, @"(?<=assembly: AssemblyInformationalVersion\("").*(?=""\))", @"\d+\.\d+\.\d+-.*", DefaultInformationalVersionLine ?? string.Join(Environment.NewLine, @"#if DEBUG", @"[assembly: AssemblyInformationalVersion(""1.0.0"")]", @"#endif"));

            return sb.ToString();
        }

        private void validateVersionInner(StringBuilder sb, string linePattern, string versionPattern, string defaultLine)
        {
            if (!Regex.IsMatch(sb.ToString(), linePattern))
            {
                sb.AppendLine(defaultLine); //no existing value, insert default
                Trace.WriteLine("No existing value, insert default of " + defaultLine);
            }

            foreach (Match lineMatch in Regex.Matches(sb.ToString(), linePattern))
            {
                string version = lineMatch.Value;
                if (!Regex.IsMatch(version, versionPattern))
                {
                    sb.Replace(lineMatch.Value, defaultLine);
                    Trace.WriteLine("Existing value " + lineMatch.Value + " is invalid, set it to default");
                }
            }
        }
    }
}
