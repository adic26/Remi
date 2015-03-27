using System.Diagnostics;
using System.IO;
using TsdLib.Measurements;

namespace TsdLib.DataAccess
{
    public class DatabaseTestResults
    {
        public static void PublishResults(ITestResults results)
        {
            string xmlFile = Path.Combine(SpecialFolders.GetResultsFolder(results.Details.SafeTestSystemName).FullName, SpecialFolders.GetResultsFileName(results.Details, results.Summary, "xml"));
            string path = Path.GetDirectoryName(xmlFile);
            if (string.IsNullOrWhiteSpace(path))
                throw new DirectoryNotFoundException("The results folder does not exist on this machine.");
            Trace.WriteLine("Uploading results to database...");
            DBControl.DAL.Results.UploadXML(xmlFile, path, Path.Combine(path, "PublishFailed"), Path.Combine(path, "Published"), false, true);
            Trace.WriteLine("Upload complete. Results can be viewed at " + results.Details.RequestNumber);
        }
    }
}