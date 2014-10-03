using System;
using System.IO;

namespace TsdLib.Configuration
{
    /// <summary>
    /// A database implementation that persists data settings to the local disk.
    /// </summary>
    public class DatabaseFolderConnection : DatabaseConnection
    {
        private readonly string _directoryName;

        /// <summary>
        /// Initialize a new DatabaseFolderConnection instance specifying a location on the local disk to persist settings.
        /// </summary>
        /// <param name="settingsBasePath">Absolute path on the local file system to use for persisting settings.</param>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="released">True if the test system application is a released/production version.</param>
        /// <param name="appVersionFilter">A RegEx used to mask the application version. Default is to maintain Major.Minor and mask off Build.Revision.</param>
        public DatabaseFolderConnection(string settingsBasePath, string testSystemName, string testSystemVersion, bool released, string appVersionFilter = @"\d+\.\d+")
            : base(testSystemName, testSystemVersion,  appVersionFilter)
        {
            _directoryName = Path.Combine(settingsBasePath, TestSystemName, AppVersion, released ? "Release" : "Debug");

            if (!Directory.Exists(_directoryName))
                Directory.CreateDirectory(_directoryName);
        }

        /// <summary>
        /// Write a string to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="dataDescription">Description of the data</param>
        public override void WriteStringToDatabase(string data, string dataDescription)
        {
            File.WriteAllText(Path.Combine(_directoryName, dataDescription), data);
        }

        /// <summary>
        /// Read a string from the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="dataDescription">Description of the data</param>
        /// <returns>Data read from database in the specified indexes.</returns>
        public override string ReadStringFromDatabase(string dataDescription)
        {
            string filePath = Path.Combine(_directoryName, dataDescription);

            if (!File.Exists(filePath))
                throw new DataDoesNotExistException(TestSystemName, TestSystemVersion, dataDescription);

            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Uploads a file to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="sourceFilePath">Absolute path to the file to upload.</param>
        /// <param name="fileDescription">Description of the file.</param>
        /// <param name="overWrite">True to overwrite existing file.</param>
        public override void UploadFileToDatabase(string sourceFilePath, string fileDescription, bool overWrite)
        {
            string fileName = Path.GetFileName(sourceFilePath);
            if (fileName == null)
                throw new InvalidFilePathException(sourceFilePath);

            string destinationFilePath = Path.Combine(_directoryName, fileName);

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