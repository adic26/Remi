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

        [Output]
        public string Result { get; set; }

        public override bool Execute()
        {
            try
            {
                string assemblyInfo = File.ReadAllText(AssemblyInfoFilePath);

                assemblyInfo = Regex.Replace(assemblyInfo, ExistingInstructions(), "");

                Match versionMatch = Regex.Match(assemblyInfo, AssemblyVersionReadRegEx());
                Match fileVersionMatch = Regex.Match(assemblyInfo, AssemblyFileVersionReadRegEx());
                bool infoVersionMatchSuccess = Regex.Match(assemblyInfo, AssemblyInformationalVersionRegEx()).Success;

                Version majorMinorPatchVersion = Version.Parse(fileVersionMatch.Success ? fileVersionMatch.Value : versionMatch.Success ? versionMatch.Value : "1.0.0");

                if (!versionMatch.Success)
                    assemblyInfo += AssemblyVersionLine(majorMinorPatchVersion);

                if (!fileVersionMatch.Success)
                    assemblyInfo += AssemblyFileVersionLine(majorMinorPatchVersion);

                if (!infoVersionMatchSuccess)
                    assemblyInfo += AssemblyInformationalVersionLine(majorMinorPatchVersion);

                Version majorMinorVersion = Version.Parse(versionMatch.Success ? versionMatch.Value : "1.0.0");

                Version newVersion;
                if (majorMinorPatchVersion.Major == majorMinorVersion.Major && majorMinorPatchVersion.Minor == majorMinorVersion.Minor) //major and minor are the same, just bump patch
                    newVersion = new Version(majorMinorVersion.Major, majorMinorVersion.Minor, majorMinorPatchVersion.Build + 1);
                else if (majorMinorPatchVersion.Major == majorMinorVersion.Major) //minor has been updated by the developer, reset patch
                    newVersion = new Version(majorMinorVersion.Major, majorMinorVersion.Minor, 0);
                else //major has been updated by the developer, reset minor and patch
                    newVersion = new Version(majorMinorVersion.Major, 0, 0);


                assemblyInfo = Regex.Replace(assemblyInfo, AssemblyVersionReplacementRegEx(), AssemblyVersionLine(newVersion));
                assemblyInfo = Regex.Replace(assemblyInfo, AssemblyFileVersionReplacementRegEx(), AssemblyFileVersionLine(newVersion));
                assemblyInfo = Regex.Replace(assemblyInfo, AssemblyInformationalVersionRegEx(), AssemblyInformationalVersionLine(newVersion));

                assemblyInfo = Regex.Replace(assemblyInfo, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);

                File.WriteAllText(AssemblyInfoFilePath, assemblyInfo);

                Result = "Successfully updated version to " + newVersion;

                return true;
            }

            catch (Exception ex)
            {
                Result = ex.ToString();
                return false;
            }
        }

        private string ExistingInstructions()
        {
            return
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
        }

        public string AssemblyVersionLine(Version version)
        {
            return string.Format(@"{0}[assembly: AssemblyVersion(""{1}.{2}"")]", Environment.NewLine, version.Major, version.Minor);
        }

        public string AssemblyVersionReadRegEx()
        {
            return @"(?<=\[assembly: AssemblyVersion\("")\d+\.\d+(?=""\)\])";
        }

        public string AssemblyVersionReplacementRegEx()
        {
            return @"\[assembly: AssemblyVersion\("".*""\)\]";
        }

        public string AssemblyFileVersionLine(Version version)
        {
            return string.Format(@"{0}[assembly: AssemblyFileVersion(""{1}.{2}.{3}"")]", Environment.NewLine, version.Major, version.Minor, version.Build);
        }

        public string AssemblyFileVersionReadRegEx()
        {
            return @"(?<=\[assembly: AssemblyFileVersion\("")\d+\.\d+\.\d+(?=""\)\])";
        }

        public string AssemblyFileVersionReplacementRegEx()
        {
            return @"\[assembly: AssemblyFileVersion\("".*""\)\]";
        }

        public string AssemblyInformationalVersionLine(Version version)
        {
            return string.Format(
@"
#if DEBUG
[assembly: AssemblyInformationalVersion(""{0}.{1}.{2}-debug"")]
#else
[assembly: AssemblyInformationalVersion(""{0}.{1}.{2}"")]
#endif",
                version.Major, version.Minor, version.Build);
        }

        //public string AssemblyInformationalVersionRegEx()
        //{
        //    return @"(?s)\#if DEBUG..\[assembly:\s?AssemblyInformationalVersion\(""\d+\.\d+\.\d+-debug""\)\]..\#else..\[assembly:\s?AssemblyInformationalVersion\(""\d+\.\d+\.\d+""\)\]..\#endif";
        //}

        public string AssemblyInformationalVersionRegEx()
        {
            return
@"(?x)(?s)
\#if\sDEBUG..
\[assembly:\s?AssemblyInformationalVersion\(""\d+\.\d+\.\d+-debug""\)\]..
\#else..
\[assembly:\s?AssemblyInformationalVersion\(""\d+\.\d+\.\d+""\)\]..
\#endif";
        }

//        public string AssemblyInformationalVersionRegEx()
//        {
//            return
//@"(?s)\#if DEBUG..
//\[assembly:AssemblyInformationalVersion\(""\d+\.\d+\.\d+-debug""\)\]..
//.*
//\#endif";
//        }
    }
}
