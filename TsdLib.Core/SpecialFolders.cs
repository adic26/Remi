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
        private static readonly string currentDate = DateTime.Now.ToString("MMM_dd_yyyy");
        private static readonly string currentTime = DateTime.Now.ToString("HH_mm_ss");

        //private static FileInfo _errorLogs;
        ///// <summary>
        ///// Gets the folder where error logs are stored.
        ///// </summary>
        //public static FileInfo GetErrorLogs(string testSystemName)
        //{
        //    if (_errorLogs == null)
        //    {
        //        DirectoryInfo directory = baseFolder
        //            .CreateSubdirectory("ErrorLogs")
        //            .CreateSubdirectory(currentDate);

        //        _errorLogs = new FileInfo(Path.Combine(directory.FullName, testSystemName + ".txt"));
        //    }
        //    return _errorLogs;
        //}

        /// <summary>
        /// Gets the folder where trace logs are stored.
        /// </summary>
        public static StreamWriter GetErrorLogs(string testSystemName)
        {
            StreamWriter errorLogs;
            DirectoryInfo directory = baseFolder
                .CreateSubdirectory("ErrorLogs")
                .CreateSubdirectory(currentDate);

            try
            {
                errorLogs = new StreamWriter(Path.Combine(directory.FullName, testSystemName + "_" + currentTime + ".txt"), true);
            }
            catch (Exception)
            {
                errorLogs = new StreamWriter(Path.Combine(directory.FullName, testSystemName + "_" + currentTime + "_" + Path.ChangeExtension(Path.GetRandomFileName(), "txt")), true);
            }

            return errorLogs;
        }

        /// <summary>
        /// Gets the folder where trace logs are stored.
        /// </summary>
        public static StreamWriter GetTraceLogs(string testSystemName)
        {
            StreamWriter traceLogs;
                DirectoryInfo directory = baseFolder
                    .CreateSubdirectory("TraceLogs")
                    .CreateSubdirectory(currentDate);

            try
            {
                traceLogs = new StreamWriter(Path.Combine(directory.FullName, testSystemName + "_" + currentTime + ".txt"), true);
            }
            catch (Exception)
            {
                traceLogs = new StreamWriter(Path.Combine(directory.FullName, testSystemName + "_" + currentTime + "_" + Path.ChangeExtension(Path.GetRandomFileName(), "txt")), true);
            }

            return traceLogs;
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
        /// Creates a new uniquely named file for writing test results.
        /// </summary>
        /// <returns>An open <see cref="FileStream"/> object used to write test results information.</returns>
        public static FileStream GetResultsFile(ITestDetails details, ITestSummary summary, string extension, DirectoryInfo directoryInfo = null)
        {
            DirectoryInfo directory = directoryInfo ?? baseFolder.CreateSubdirectory("TestResults").CreateSubdirectory(details.SafeTestSystemName);
            if (!directory.Exists)
                directory.Create();
            string fileName = GetResultsFileName(details, summary, extension);

            return File.Open(Path.Combine(directory.FullName, fileName), FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        /// <summary>
        /// Creates a unique file name based on the specified test details and summary.
        /// </summary>
        /// <param name="details">An <see cref="ITestDetails"/> object containing metadata information relevent to the test.</param>
        /// <param name="summary">An <see cref="ITestSummary"/> object containing information about the results of a test.</param>
        /// <param name="extension">The file extension to apply.</param>
        /// <returns>A unique string that can be used to generate a test results file.</returns>
        public static string GetResultsFileName(ITestDetails details, ITestSummary summary, string extension)
        {
            string jobNumber = string.IsNullOrWhiteSpace(details.RequestNumber) ? "" : details.RequestNumber + "-";

            string unitNumber = details.UnitNumber == 0 ? "" : details.UnitNumber.ToString("D3") + "-";
            string timeStamp = summary.DateStarted.ToString("yyyy-MM-dd_hh-mm-ss");

            return jobNumber + unitNumber + timeStamp + "." + extension;
        }
    }
}
