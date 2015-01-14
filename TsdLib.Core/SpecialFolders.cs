using System;
using System.IO;
using TsdLib.Configuration;
using TsdLib.Measurements;

namespace TsdLib
{
    /// <summary>
    /// Defines special folders used by the TsdLib Framework.
    /// </summary>
    public static class SpecialFolders
    {
        private static FileInfo _errorLogs;
        /// <summary>
        /// Gets the folder where error logs are stored.
        /// </summary>
        public static FileInfo ErrorLogs
        {
            get
            {
                if (_errorLogs == null)
                {
                    DirectoryInfo directory = baseFolder
                        .CreateSubdirectory("ErrorLogs")
                        .CreateSubdirectory(DateTime.Now.ToString("MMM_dd_yyyy"));

                    //_errorLogs = new FileInfo(Path.Combine(directory.FullName, Assembly.GetEntryAssembly().GetName().Name + ".txt"));
                    _errorLogs = new FileInfo(Path.Combine(directory.FullName, "TestAutomation" + ".txt"));
                }
                return _errorLogs;
            }
        }

        private static StreamWriter _traceLogs;
        /// <summary>
        /// Gets the folder where trace logs are stored.
        /// </summary>
        public static StreamWriter TraceLogs
        {
            get
            {
                if (_traceLogs == null)
                {
                    DirectoryInfo directory = baseFolder
                        .CreateSubdirectory("TraceLogs")
                        .CreateSubdirectory(DateTime.Now.ToString("MMM_dd_yyyy"));

                    //_traceLogs = new StreamWriter(Path.Combine(directory.FullName, Assembly.GetEntryAssembly().GetName().Name + ".txt"), false);
                    //TODO: if in use, create new file
                    _traceLogs = new StreamWriter(Path.Combine(directory.FullName, "TestAutomation" + ".txt"), false);
                }
                return _traceLogs;
            }
        }

        /// <summary>
        /// Gets the folder where configuration data is stored.
        /// </summary>
        public static DirectoryInfo Configuration
        {
            get
            {
                return baseFolder
                    .CreateSubdirectory("Configuration");
            }
        }

        /// <summary>
        /// Gets the folder where Test Sequence measurements are stored for the specified test system.
        /// </summary>
        /// <param name="testSystemName">Name of the test system for which to get the measurements folder.</param>
        /// <returns>The absolute path to the measurements folder.</returns>
        public static DirectoryInfo GetResultsFolder(string testSystemName)
        {
            return baseFolder
                .CreateSubdirectory("TestResults")
                .CreateSubdirectory(testSystemName);
        }

        private static DirectoryInfo baseFolder
        {
            get
            {
                DirectoryInfo directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                return directory.CreateSubdirectory("TsdLib");
            }
        }

        /// <summary>
        /// Save the test results to an xml file in the specified directory. Useful for uploading to database.
        /// </summary>
        
        /// <returns>The absolute path to the xml file generated.</returns>
        public static FileStream GetResultsFile(ITestDetails details, ITestSummary summary, string extension)
        {
            var directory = baseFolder
                .CreateSubdirectory("TestResults")
                .CreateSubdirectory(details.SafeTestSystemName);

            string jobNumber = string.IsNullOrWhiteSpace(details.JobNumber) ? "" : details.JobNumber + "-";

            string unitNumber = details.UnitNumber == 0 ? "" : details.UnitNumber.ToString("D3") + "-";
            string timeStamp = summary.DateStarted.ToString("yyyy-MM-dd_hh-mm-ss");

            string fileName = Path.Combine(directory.FullName, jobNumber + unitNumber + timeStamp + "." + extension);

            return File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public static string GetResultsFileName(ITestDetails details, ITestSummary summary, string extension)
        {
            FileStream s = GetResultsFile(details, summary, extension);
            s.Close();
            return s.Name;
        }
    }
}
