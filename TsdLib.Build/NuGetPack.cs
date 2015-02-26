

using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace TsdLib.Build
{
    public class NuGetPack : Task 
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
        [Output]
        public string NuGetOutput { get; set; }
        [Output]
        public string Success { get; set; }

        public override bool Execute()
        {
            try
            {
                foreach (string package in Directory.EnumerateFiles(OutputFolder, "*.nupkg"))
                    File.Delete(package);

                ProcessStartInfo nugetProcessStartInfo = new ProcessStartInfo
                {
                    FileName = "nuget.exe",
                    Arguments = string.Format("pack {0} -OutputDirectory {1} -Prop Configuration={2} -IncludeReferencedProjects -Symbols {3}", TargetFile, OutputFolder, Configuration, OtherArguments ?? ""),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };
                Process nugetProcess = Process.Start(nugetProcessStartInfo);
                if (nugetProcess == null || !nugetProcess.WaitForExit(5000))
                    throw new Exception("NuGet process failed to terminate.");

                if (nugetProcess.ExitCode == 0)
                {
                    NuGetOutput = nugetProcess.StandardOutput.ReadToEnd();
                    Success = "true";
                }
                else
                {
                    NuGetOutput = nugetProcess.StandardError.ReadToEnd();
                    Success = "false";
                }

                Match outputPackageMatch = Regex.Match(NuGetOutput, @"(?<=Successfully created package ').*\.nupkg(?=')");
                OutputPackage = outputPackageMatch.Success ? outputPackageMatch.Value : "N/A";
                
                return true;

            }
            catch (Exception ex)
            {
                NuGetOutput = ex.Message;
                Success = "false";
                return true;
            }
        }
    }
}
