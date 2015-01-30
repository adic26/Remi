

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace TsdLib.Build
{
    public class NuGetPack : AppDomainIsolatedTask 
    {
        [Required]
        public string OutputFolder { get; set; }
        [Required]
        public string TargetFile { get; set; }
        [Required]
        public string Configuration { get; set; }
        public string OtherArguments { get; set; }

        [Output]
        public string OutputPackage { get; set; }

        public override bool Execute()
        {
            try
            {
                foreach (string package in Directory.EnumerateFiles(OutputFolder, "*.nupkg"))
                    File.Delete(package);

                Process nugetProcess = Process.Start("nuget.exe", string.Format("pack {0} -OutputDirectory {1} -Prop Configuration={2} -IncludeReferencedProjects {3}", TargetFile, OutputFolder, Configuration, OtherArguments ?? ""));
                if (nugetProcess == null)
                    return false;

                if (!nugetProcess.WaitForExit(5000))
                    return false;

                OutputPackage = "TODO: get name of generated package";

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
