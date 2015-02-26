
using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace TsdLib.Build
{
    public class ApplySemanticVersioning : Task
    {
        [Required]
        public string AssemblyInfoFilePath { get; set; }
        [Required]
        public string VersionFilePath { get; set; }
        [Required]
        public string FullVersionFilePath { get; set; }
        [Required]
        public string IntermediateVersionFilePath { get; set; }

        [Output]
        public string ResultMessage { get; set; }
        [Output]
        public string OutputVersionFilePath { get; set; }
        [Output]
        public string Success { get; set; }

        public override bool Execute()
        {
            try
            {
                if (!File.Exists(VersionFilePath))
                    File.WriteAllText(VersionFilePath, "1.0");

                Version assemblyInfoVersion = readVersionFromFile(AssemblyInfoFilePath);
                Version userVersion = readVersionFromFile(VersionFilePath);
                Version fullVersion = readVersionFromFile(FullVersionFilePath) ?? new Version(userVersion.Major, userVersion.Minor, 0);

                cleanAssemblyInfoFile(AssemblyInfoFilePath);

                if (assemblyInfoVersion != null && userVersion < assemblyInfoVersion)
                {
                    File.WriteAllText(VersionFilePath, assemblyInfoVersion.ToString(2));
                    userVersion = assemblyInfoVersion;
                }

                Version newVersion;

                if (userVersion.Major == fullVersion.Major && userVersion.Minor == fullVersion.Minor)
                    newVersion = new Version(userVersion.Major, userVersion.Minor, fullVersion.Build + 1);
                else if (userVersion.Major == fullVersion.Major)
                    newVersion = new Version(userVersion.Major, userVersion.Minor, 0);
                else
                    newVersion = new Version(userVersion.Major, 0, 0);

                File.WriteAllText(FullVersionFilePath, newVersion.ToString(3));

                writeOutputVersionFile(IntermediateVersionFilePath, newVersion);

                OutputVersionFilePath = IntermediateVersionFilePath;

                ResultMessage = "Successfully updated version to " + newVersion;

                Success = "true";
                return true;

            }
            catch (Exception ex)
            {
                ResultMessage = "Failed to apply semantic versioning: " + ex.Message;
                Success = "false";
                return true;
            }

        }

        private const string OldInstructions =
@"// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '\*' as shown below:
// \[assembly: AssemblyVersion\(""1.0.*""\)\]";

        private const string NewInstructions = "//Please use the Version.txt file to update assembly version";

        private const string AssemblyVersionRegEx = @"\[assembly: Assembly.*Version.*\("".*""\)\]";

        private Version readVersionFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string fileContents = File.ReadAllText(filePath);

            Match fileVersionMatch = Regex.Match(fileContents, @"(?<=\[assembly: AssemblyFileVersion\("")\d+\.\d+\.\d+(?=.*""\)\])");
            if (fileVersionMatch.Success)
                return new Version(fileVersionMatch.Value);

            Match versionMatch = Regex.Match(fileContents, @"(?<=\[assembly: AssemblyVersion\("")\d+\.\d+(?=.*""\)\])");
            if (versionMatch.Success)
                return new Version(versionMatch.Value);

            Match rawVersionMatch = Regex.Match(fileContents, @"\d+\.\d+\.\d+");
            if (rawVersionMatch.Success)
                return new Version(rawVersionMatch.Value);

            Match rawVersionMajorMinorMatch = Regex.Match(fileContents, @"\d+\.\d+");
            if (rawVersionMajorMinorMatch.Success)
                return new Version(rawVersionMajorMinorMatch.Value);

            return null;
        }

        private void cleanAssemblyInfoFile(string filePath)
        {
            string contents = File.ReadAllText(filePath);

            //contains new instructions
            //doesn't contain old instructions
            //doesn't contain any Assembly*Version Attributes
            //up to date, just return
            if (Regex.IsMatch(contents, NewInstructions) && !Regex.IsMatch(contents, OldInstructions) && !Regex.IsMatch(contents, AssemblyVersionRegEx))
                return;

            //doesn't contain new instructions - append them
            if (!Regex.IsMatch(contents, NewInstructions))
                contents += Environment.NewLine + NewInstructions;

            //contains old instructions - remove them
            contents = Regex.Replace(contents, OldInstructions, "");

            //contains assembly version attributes - remove them
            contents = Regex.Replace(contents, AssemblyVersionRegEx, "");

            //remove any empty preprocessor statements that could be left over from legacy build tools
            contents = Regex.Replace(contents, @"\#if DEBUG\s+\#else\s+#endif", "");

            File.WriteAllText(filePath, contents);
        }

        private void writeOutputVersionFile(string filePath, Version version)
        {
            Version v = version ?? new Version(1, 0, 0);

            File.WriteAllText(filePath,
                string.Format(
@"using System.Reflection;
[assembly: AssemblyVersion(""{0}"")]
[assembly: AssemblyFileVersion(""{1}"")]
#if DEBUG
[assembly: AssemblyInformationalVersion(""{1}-debug"")]
#else
[assembly: AssemblyInformationalVersion(""{1}"")]
#endif",v.ToString(2),v.ToString(3)
                )
            );
        }
    }
}
