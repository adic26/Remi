//NOTE: this project is NOT set to build in Debug or Release configuration, due to errors caused by MSBuild.exe locking the TsdLib.Build.dll
//To make changes to this project, re-enable the build in Configuration Manager and manually close any MSBuild.exe processes in the Windows Task Manager

//http://stackoverflow.com/questions/3919892/msbuild-exe-staying-open-locking-files
//http://stackoverflow.com/questions/3371545/visual-studio-2008-locks-custom-msbuild-task-assemblies
//http://stackoverflow.com/questions/13510465/the-mystery-of-stuck-inactive-msbuild-exe-processes-locked-stylecop-dll-nuget

using System;
using System.IO;
using System.Linq;
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

                Version maxVersion = Regex.Matches(assemblyInfo, @"(?<=Assembly.*Version\("")\d+\.\d+\.\d+").Cast<Match>().Select(m => new Version(m.Value)).Max();

                var newVersion = new Version(maxVersion.Major, maxVersion.Minor, maxVersion.Build + 1 );

                assemblyInfo = Regex.Replace(assemblyInfo, @"\[assembly: AssemblyVersion.*\]", @"[assembly: AssemblyVersion(""" + newVersion + @""")]");
                assemblyInfo = Regex.Replace(assemblyInfo, @"\[assembly: AssemblyFileVersion.*\]", "");
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
