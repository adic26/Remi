using System;
using System.IO;
using System.Text.RegularExpressions;
using TsdLib.Configuration.Exceptions;

namespace TsdLib.Configuration.Connections
{
    /// <summary>
    /// A database implementation that persists data settings to the local file system.
    /// </summary>
    public class FileSystemConnection : IFileConnection
    {
        private readonly DirectoryInfo _settingsBaseDirectory;
        private readonly string _appVersionFilter;

        /// <summary>
        /// Initialize a new FileSystemConnection instance specifying a location on the local or network file system to persist settings.
        /// </summary>
        /// <param name="settingsBasePath">DirectoryInfo containing the local or network file system to use for persisting settings.</param>
        /// <param name="appVersionFilter">OPTIONAL: A RegEx used to mask the application version. Default is to maintain Major.Minor and mask off Build.Revision.</param>
        public FileSystemConnection(DirectoryInfo settingsBasePath, string appVersionFilter = @"\d+\.\d+")
        {
            _settingsBaseDirectory = settingsBasePath;
            _appVersionFilter = appVersionFilter;

            if (!_settingsBaseDirectory.Exists)
                _settingsBaseDirectory.Create();
        }

        private string getDirectoryInfo(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode)
        {
            if (!_settingsBaseDirectory.Exists)
                throw new InvalidDirectoryException(_settingsBaseDirectory.FullName);
            Match match = Regex.Match(testSystemVersion.ToString(), _appVersionFilter);
            string appVersion = match.Success ? match.Value : testSystemVersion.ToString();
            return _settingsBaseDirectory.CreateSubdirectory(Path.Combine(testSystemName, appVersion, testSystemMode.ToString())).FullName;
        }

        /// <summary>
        /// Read a string from the file system using the name/version/mode of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion"><see cref="Version"/> of the test system.</param>
        /// <param name="testSystemMode">An <see cref="OperatingMode"/> describing the use-case of the test system.</param>
        /// <param name="configType">The type used to encapsulate the configuration data.</param>
        /// <param name="data">Data to write to the file system.</param>
        /// <returns>True on success; False otherwise.</returns>
        public bool WriteString(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, Type configType, string data)
        {
            //string file = Path.Combine(getDirectory(testSystemName, testSystemVersion, testSystemMode), configType.Name + ".xml");
            string file = Path.Combine(getDirectoryInfo(testSystemName, testSystemVersion, testSystemMode), configType.Name + ".xml");
            
            File.WriteAllText(file, data);

            return true;
        }

        /// <summary>
        /// Read a string from the file system using the name/version/mode of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion"><see cref="Version"/> of the test system.</param>
        /// <param name="testSystemMode">An <see cref="OperatingMode"/> describing the use-case of the test system.</param>
        /// <param name="configType">The type used to encapsulate the configuration data.</param>
        /// <param name="data">Data read from the file system.</param>
        /// <returns>True if read was successful; false otherwise.</returns>
        public bool TryReadString(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, Type configType, out string data)
        {
            string file = Path.Combine(getDirectoryInfo(testSystemName, testSystemVersion, testSystemMode), configType.Name + ".xml");

            if (!File.Exists(file))
            {
                data = "";
                return false;
            }

            data = File.ReadAllText(file);
            return true;
        }

        /// <summary>
        /// Uploads a file to the database using the name/version of the test system and dataDescription as indexes.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion"><see cref="Version"/> of the test system.</param>
        /// <param name="testSystemMode">Operating mode of the test system (ie. Debug, Release, Development, Engineering, Production.</param>
        /// <param name="sourceFilePath">Absolute path to the file to upload.</param>
        /// <param name="overWrite">True to overwrite existing file.</param>
        public void UploadFile(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, string sourceFilePath, bool overWrite)
        {
            string fileName = Path.GetFileName(sourceFilePath);
            if (fileName == null)
                throw new FileUploadException(sourceFilePath);

            string destinationFilePath = Path.Combine(getDirectoryInfo(testSystemName, testSystemVersion, testSystemMode), fileName);

            if (!overWrite && File.Exists(destinationFilePath))
                return;

            File.Copy(sourceFilePath, destinationFilePath, overWrite);
        }
    }
}