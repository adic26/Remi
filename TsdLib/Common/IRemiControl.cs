namespace TsdLib
{
    /// <summary>
    /// Interface for communicating with the Remi database.
    /// </summary>
    public interface IRemiControl
    {
        /// <summary>
        /// Write a string to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="dataDescription">Description of the data</param>
        void WriteStringToRemi(string data, string testSystemName, string testSystemVersion, string dataDescription);

        /// <summary>
        /// Read a string from the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="dataDescription">Description of the data</param>
        /// <returns>Data read from Remi in the specified indexes.</returns>
        string ReadStringFromRemi(string testSystemName, string testSystemVersion, string dataDescription);
    }
}