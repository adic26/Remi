//NOTE: this project is NOT set to build in Debug or Release configuration, due to errors caused by MSBuild.exe locking the TsdLib.Build.dll
//To make changes to this project, re-enable the build in Configuration Manager and manually close any MSBuild.exe processes in the Windows Task Manager

//http://stackoverflow.com/questions/3919892/msbuild-exe-staying-open-locking-files
//http://stackoverflow.com/questions/3371545/visual-studio-2008-locks-custom-msbuild-task-assemblies
//http://stackoverflow.com/questions/13510465/the-mystery-of-stuck-inactive-msbuild-exe-processes-locked-stylecop-dll-nuget

using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace TsdLib.Build
{
    public class ApplySemanticVersioning : AppDomainIsolatedTask 
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
                string assemblyInfo = File.ReadAllText(AssemblyInfoFilePath);

                assemblyInfo = Regex.Replace(assemblyInfo, 
@"// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '\*' as shown below:
// \[assembly: AssemblyVersion\(""1.0.*""\)\]", "");

                Match versionMatch = Regex.Match(assemblyInfo, @"(?<=AssemblyVersion\("")\d+\.\d+");
                Match fileVersionMatch = Regex.Match(assemblyInfo, @"(?<=AssemblyFileVersion\("")\d+\.\d+\.\d+");
                Match infoVersionMatch = Regex.Match(assemblyInfo, @"(?<=AssemblyInformationalVersion\("")\d+\.\d+\.\d+");


                Version infoVersion = Version.Parse(infoVersionMatch.Success ? infoVersionMatch.Value : fileVersionMatch.Success ? fileVersionMatch.Value : versionMatch.Value);

                if (!infoVersionMatch.Success)
                {
                    assemblyInfo += string.Format(
@"#if DEBUG
[assembly: AssemblyInformationalVersion(""{0}"")]
#endif", infoVersion);
                }

                Version version = Version.Parse(versionMatch.Success ? versionMatch.Value : "1.0.0");

                Version newVersion;
                if (infoVersion.Major == version.Major && infoVersion.Minor == version.Minor) //major and minor are the same, just bump patch
                    newVersion = new Version(version.Major, version.Minor, infoVersion.Build + 1);
                else if (infoVersion.Major == version.Major) //minor has been updated by the developer, reset patch
                    newVersion = new Version(version.Major, version.Minor, 0);
                else //major has been updated by the developer, reset minor and patch
                    newVersion = new Version(version.Major, 0, 0);


                assemblyInfo = Regex.Replace(assemblyInfo, @"\[assembly: AssemblyVersion.*\]", @"[assembly: AssemblyVersion(""" + newVersion.Major + "." + newVersion.Minor + @""")]");
                assemblyInfo = Regex.Replace(assemblyInfo, @"\[assembly: AssemblyFileVersion.*\]", @"[assembly: AssemblyFileVersion(""" + newVersion + @""")]");
                assemblyInfo = Regex.Replace(assemblyInfo, @"\[assembly: AssemblyInformationalVersion.*\]", @"[assembly: AssemblyInformationalVersion(""" + newVersion + @"-debug"")]");

                string newAsyInfo = Regex.Replace(assemblyInfo, @"(?<=Assembly.*Version\("")\d+\.\d+\.\d+", newVersion.ToString());

                File.WriteAllText(AssemblyInfoFilePath, newAsyInfo);

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}
