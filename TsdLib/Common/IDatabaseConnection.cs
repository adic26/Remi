namespace TsdLib
{
    /// <summary>
    /// Interface for communicating with the a database.
    /// </summary>
    public interface IDatabaseConnection
    {
        /// <summary>
        /// Write a string to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="dataDescription">Description of the data</param>
        void WriteStringToDatabase(string data, string testSystemName, string testSystemVersion, string dataDescription);

        /// <summary>
        /// Read a string from the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="dataDescription">Description of the data</param>
        /// <returns>Data read from the database in the specified indexes.</returns>
        string ReadStringFromDatabase(string testSystemName, string testSystemVersion, string dataDescription);

        /// <summary>
        /// Uploads a file to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="sourceFilePath">Absolute path to the file to upload.</param>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="fileDescription">Description of the file.</param>
        /// <param name="overWrite">True to overwrite existing file.</param>
        void UploadFileToDatabase(string sourceFilePath, string testSystemName, string testSystemVersion, string fileDescription, bool overWrite);
    }
}