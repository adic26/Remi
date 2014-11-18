using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TsdLib.Configuration
{
    /// <summary>
    /// A database implementation that persists data settings to the local disk.
    /// </summary>
    public class DatabaseFolderConnection : IDatabaseFileConnection
    {
        private readonly string _settingsBasePath;
        private readonly string _appVersionFilter;

        /// <summary>
        /// Initialize a new DatabaseFolderConnection instance specifying a location on the local or network file system to persist settings.
        /// </summary>
        /// <param name="settingsBasePath">Absolute path on the local or network file system to use for persisting settings.</param>
        /// <param name="appVersionFilter">OPTIONAL: A RegEx used to mask the application version. Default is to maintain Major.Minor and mask off Build.Revision.</param>
        public DatabaseFolderConnection(string settingsBasePath, string appVersionFilter = @"\d+\.\d+")
        {
            _settingsBasePath = settingsBasePath;
            _appVersionFilter = appVersionFilter;


        }

        /// <summary>
        /// Write a string to a folder structure using the name/version/mode of the test system as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">Operating mode of the test system (ie. Debug, Release, Development, Engineering, Production.</param>
        /// <param name="dataDescription">Description of the data</param>
        /// <param name="data">Data to write.</param>
        public void WriteStringToDatabase(string testSystemName, string testSystemVersion, string testSystemMode, string dataDescription, string data)
        {
            string file = getFilePath(testSystemName, testSystemVersion, testSystemMode, dataDescription);

            File.WriteAllText(file, data);
        }

        /// <summary>
        /// Read a string from a folder structure using the name/version/mode of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">Operating mode of the test system (ie. Debug, Release, Development, Engineering, Production.</param>
        /// <param name="dataDescription">Description of the data</param>
        /// <returns>Data read from database in the specified indexes.</returns>
        public string ReadStringFromDatabase(string testSystemName, string testSystemVersion, string testSystemMode, string dataDescription)
        {
            string file = getFilePath(testSystemName, testSystemVersion, testSystemMode, dataDescription);

            if (!File.Exists(file))
                throw new DataDoesNotExistException(testSystemName, testSystemVersion, dataDescription);

            return File.ReadAllText(file);
        }

        private string getFilePath(string testSystemName, string testSystemVersion, string testSystemMode, string dataDescription)
        {
            Match m = Regex.Match(testSystemVersion, _appVersionFilter);
            string appVersion = m.Success ? m.Value : testSystemVersion;
            string directory = Path.Combine(_settingsBasePath, testSystemName, appVersion, testSystemMode);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return Path.Combine(directory, dataDescription);
        }

        /// <summary>
        /// Uploads a file to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">Operating mode of the test system (ie. Debug, Release, Development, Engineering, Production.</param>
        /// <param name="sourceFilePath">Absolute path to the file to upload.</param>
        /// <param name="overWrite">True to overwrite existing file.</param>
        public void UploadFileToDatabase(string testSystemName, string testSystemVersion, string testSystemMode, string sourceFilePath, bool overWrite)
        {
            string fileName = Path.GetFileName(sourceFilePath);
            if (fileName == null)
                throw new InvalidFilePathException(sourceFilePath);

            string destinationFilePath = getFilePath(testSystemName, testSystemVersion, testSystemMode, fileName);

            if (!overWrite && File.Exists(destinationFilePath))
                return;

            File.Copy(sourceFilePath, destinationFilePath, overWrite);
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