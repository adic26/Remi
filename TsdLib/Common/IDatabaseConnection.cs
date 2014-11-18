namespace TsdLib
{
    /// <summary>
    /// Defines methods to allow reading and writing string data to and from a database.
    /// </summary>
    public interface IDatabaseConnection
    {
        /// <summary>
        /// Write a string to the database using the name/version/mode of the test system as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">Operating mode of the test system (ie. Debug, Release, Development, Engineering, Production.</param>
        /// <param name="dataDescription">Description of the data</param>
        /// <param name="data">Data to write.</param>
        void WriteStringToDatabase(string testSystemName, string testSystemVersion, string testSystemMode, string dataDescription, string data);

        /// <summary>
        /// Read a string from the database using the name/version/mode of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">Operating mode of the test system (ie. Debug, Release, Development, Engineering, Production.</param>
        /// <param name="dataDescription">Description of the data</param>
        /// <returns>Data read from database in the specified indexes.</returns>
        string ReadStringFromDatabase(string testSystemName, string testSystemVersion, string testSystemMode, string dataDescription);
    }

    /// <summary>
    /// Extends <see cref="IDatabaseConnection"/> by adding file upload and download method definitions.
    /// </summary>
    public interface IDatabaseFileConnection : IDatabaseConnection
    {
        /// <summary>
        /// Uploads a file to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">Operating mode of the test system (ie. Debug, Release, Development, Engineering, Production.</param>
        /// <param name="sourceFilePath">Absolute path to the file to upload.</param>
        /// <param name="overWrite">True to overwrite existing file.</param>
        void UploadFileToDatabase(string testSystemName, string testSystemVersion, string testSystemMode, string sourceFilePath, bool overWrite);
    }
}