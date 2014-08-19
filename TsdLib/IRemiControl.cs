namespace TsdLib
{
    /// <summary>
    /// Interface for communicating with the Remi database.
    /// </summary>
    public interface IRemiControl
    {
        /// <summary>
        /// Write a string to the database using applicationName, applicationVersion and dataDescription as indexes.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="applicationName">Name of the currently running application.</param>
        /// <param name="applicationVersion">Version of the currently running application.</param>
        /// <param name="dataDescription">Description of the data</param>
        void WriteStringToRemi(string data, string applicationName, string applicationVersion, string dataDescription);
        /// <summary>
        /// Read a string from the database using applicationName, applicationVersion and dataDescription as indexes.
        /// </summary>
        /// <param name="applicationName">Name of the currently running application.</param>
        /// <param name="applicationVersion">Version of the currently running application.</param>
        /// <param name="dataDescription">Description of the data</param>
        /// <returns>Data read from Remi in the specified indexes.</returns>
        string ReadStringFromRemi(string applicationName, string applicationVersion, string dataDescription);
    }
}