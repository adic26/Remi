using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TsdLib.Configuration
{
    /// <summary>
    /// A database implementation that persists data settings to the local disk.
    /// </summary>
    public class DatabaseFolderConnection : IDatabaseConnection
    {
        private const string AppVersionFilter = @"\d+\.\d+";
        private readonly string _settingsBasePath;

        /// <summary>
        /// Initialize a new DatabaseFolderConnection instance specifying a location on the local disk to persist settings.
        /// </summary>
        /// <param name="settingsBasePath">Absolute path on the local file system to use for persisting settings.</param>
        public DatabaseFolderConnection(string settingsBasePath)
        {
            _settingsBasePath = settingsBasePath;
        }

        /// <summary>
        /// Write a string to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="dataDescription">Description of the data</param>
        public void WriteStringToDatabase(string data, string testSystemName, string testSystemVersion, string dataDescription)
        {
            Match match = Regex.Match(testSystemVersion, AppVersionFilter);
            string appVersion = match.Success ? match.Value : testSystemVersion;
            string directoryName = Path.Combine(_settingsBasePath, testSystemName, appVersion);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            File.WriteAllText(Path.Combine(directoryName, dataDescription), data);
        }

        /// <summary>
        /// Read a string from the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="dataDescription">Description of the data</param>
        /// <returns>Data read from database in the specified indexes.</returns>
        public string ReadStringFromDatabase(string testSystemName, string testSystemVersion, string dataDescription)
        {
            Match match = Regex.Match(testSystemVersion, AppVersionFilter);
            string appVersion = match.Success ? match.Value : testSystemVersion;
            string filePath = Path.Combine(_settingsBasePath, testSystemName, appVersion, dataDescription);

            if (!File.Exists(filePath))
                throw new DataDoesNotExistException(testSystemName, testSystemVersion, dataDescription);

            string data = File.ReadAllText(filePath);

            return data;
        }
    }

    /// <summary>
    /// Exception generated due to an error locating data.
    /// </summary>
    [Serializable]
    class DataDoesNotExistException : Exception
    {
        public DataDoesNotExistException(string testSystemName, string testSystemVersion, string dataDescription)
            : base(dataDescription + " does not exist for " + testSystemName + " v." + testSystemVersion + ".") { }
    }
}