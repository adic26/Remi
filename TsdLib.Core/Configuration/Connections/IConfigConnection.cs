using System;

namespace TsdLib.Configuration.Connections
{
    /// <summary>
    /// Defines methods to allow reading and writing string data to and from a storage location.
    /// </summary>
    public interface IConfigConnection
    {
        /// <summary>
        /// Read a string from the storage location using the name/version/mode of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">An <see cref="OperatingMode"/> describing the use-case of the test system.</param>
        /// <param name="configType">The type used to encapsulate the configuration data.</param>
        /// <param name="data">Data to write to the storage location.</param>
        /// <returns>True on success; False otherwise.</returns>
        bool WriteString(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, Type configType, string data);

        /// <summary>
        /// Read a string from the storage location using the name/version/mode of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">An <see cref="OperatingMode"/> describing the use-case of the test system.</param>
        /// <param name="configType">The type used to encapsulate the configuration data.</param>
        /// <param name="data">Data read from the storage location.</param>
        /// <returns>True if read was successful; false otherwise.</returns>
        bool TryReadString(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, Type configType, out string data);

        /// <summary>
        /// Clone the current configuration to a different operating mode.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">An <see cref="OperatingMode"/> describing the use-case of the test system.</param>
        /// <param name="configType">The type used to encapsulate the configuration data.</param>
        /// <param name="newMode">Operating mode to clone current configuration to.</param>
        /// <returns>True if read was successful; false otherwise.</returns>
        bool CloneMode(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, Type configType, OperatingMode newMode);

        /// <summary>
        /// Clone the current configuration to a different operating version.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">An <see cref="OperatingMode"/> describing the use-case of the test system.</param>
        /// <param name="configType">The type used to encapsulate the configuration data.</param>
        /// <param name="newVersion">New version to clone current configuration to.</param>
        /// <returns>True if read was successful; false otherwise.</returns>
        bool CloneVersion(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, Type configType, Version newVersion);
    }

    /// <summary>
    /// Extends <see cref="IConfigConnection"/> by adding file upload and download method definitions.
    /// </summary>
    public interface IFileConnection : IConfigConnection
    {
        /// <summary>
        /// Uploads a file using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">An <see cref="OperatingMode"/> describing the use-case of the test system.</param>
        /// <param name="sourceFilePath">Absolute path to the file to upload.</param>
        /// <param name="overWrite">True to overwrite existing file.</param>
        void UploadFile(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, string sourceFilePath, bool overWrite);
    }
}