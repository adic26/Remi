using System;
using System.IO;

namespace TsdLib
{
    /// <summary>
    /// Defines special folders used by the TsdLib Framework.
    /// </summary>
    public static class SpecialFolders
    {
        /// <summary>
        /// Gets the folder where API assemblies are stored.
        /// </summary>
        public static string Assemblies
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TsdLib", "Assemblies");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>
        /// Gets the folder where Test Sequence logs are stored.
        /// </summary>
        public static string Logs
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TsdLib", "Logs", DateTime.Now.ToString("MMM_dd_yyyy"));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>
        /// Gets the folder where Test Sequence measurements are stored for the specified test system.
        /// </summary>
        /// <param name="testSystemName">Name of the test system for which to get the measurements folder.</param>
        /// <returns>The absolute path to the measurements folder.</returns>
        public static string GetResultsFolder(string testSystemName)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TsdLib", testSystemName, "Results");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}
