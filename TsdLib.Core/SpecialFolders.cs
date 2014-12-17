using System;
using System.IO;
using System.Reflection;

namespace TsdLib
{
    /// <summary>
    /// Defines special folders used by the TsdLib Framework.
    /// </summary>
    public static class SpecialFolders
    {
        private static StreamWriter _errorLogs;
        /// <summary>
        /// Gets the folder where error logs are stored.
        /// </summary>
        public static StreamWriter ErrorLogs
        {
            get
            {
                if (_errorLogs == null)
                {
                    DirectoryInfo directory = baseFolder
                        .CreateSubdirectory("ErrorLogs")
                        .CreateSubdirectory(DateTime.Now.ToString("MMM_dd_yyyy"));

                    _errorLogs = new StreamWriter(Path.Combine(directory.FullName, Assembly.GetEntryAssembly().GetName().Name + ".txt"), false);
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

                    _traceLogs = new StreamWriter(Path.Combine(directory.FullName, Assembly.GetEntryAssembly().GetName().Name + ".txt"), false);
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
    }
}
