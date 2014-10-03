using System.Text.RegularExpressions;

namespace TsdLib
{
    /// <summary>
    /// Common base class for communicating with a database.
    /// </summary>
    public abstract class DatabaseConnection
    {
        /// <summary>
        /// Gets the name of the test system connecting to the database.
        /// </summary>
        public string TestSystemName { get; private set; }
        /// <summary>
        /// Gets the version of the test system connecting to the database.
        /// </summary>
        public string TestSystemVersion { get; private set; }
        /// <summary>
        /// Gets the filtered application version, which will be used to categorize settings.
        /// </summary>
        protected string AppVersion { get; private set; }

        /// <summary>
        /// Initialize a new DatabaseConnection for the specified test system.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="appVersionFilter">A RegEx used to mask the application version. Default is to maintain Major.Minor and mask off Build.Revision.</param>
        protected DatabaseConnection(string testSystemName, string testSystemVersion, string appVersionFilter = @"\d+\.\d+")
        {
            TestSystemName = testSystemName;
            TestSystemVersion = testSystemVersion;
            Match match = Regex.Match(TestSystemVersion, appVersionFilter);
            AppVersion = match.Success ? match.Value : TestSystemVersion;
        }

        /// <summary>
        /// Write a string to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="dataDescription">Description of the data</param>
        public abstract void WriteStringToDatabase(string data, string dataDescription);

        /// <summary>
        /// Read a string from the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="dataDescription">Description of the data</param>
        /// <returns>Data read from database in the specified indexes.</returns>
        public abstract string ReadStringFromDatabase(string dataDescription);

        /// <summary>
        /// Uploads a file to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="sourceFilePath">Absolute path to the file to upload.</param>
        /// <param name="fileDescription">Description of the file.</param>
        /// <param name="overWrite">True to overwrite existing file.</param>
        public abstract void UploadFileToDatabase(string sourceFilePath, string fileDescription, bool overWrite);
    }
}