﻿using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TsdLib.Configuration
{
    /// <summary>
    /// A simulated RemiControl implementation that persists Remi settings to the local disk.
    /// </summary>
    public class RemiControlTest : IRemiControl
    {
        private const string AppVersionFilter = @"\d+\.\d+";
        private readonly string _settingsBasePath;

        /// <summary>
        /// Initialize a new RemiControlTest instance specifying a location on the local disk to persist settings.
        /// </summary>
        /// <param name="settingsBasePath">Absolute path on the local file system to use for persisting settings.</param>
        public RemiControlTest(string settingsBasePath)
        {
            _settingsBasePath = settingsBasePath;
        }

        /// <summary>
        /// Write a string of data to Remi.
        /// </summary>
        /// <param name="data">The raw data to write.</param>
        /// <param name="applicationName">Name of the currently executing application. Used to determine where to store the data.</param>
        /// <param name="applicationVersion">Version of the currently executing application. Used to determine where to store the data.</param>
        /// <param name="dataCategory">Category or type of the data being written. Used to determine where to store the data.</param>
        public void WriteStringToRemi(string data, string applicationName, string applicationVersion, string dataCategory)
        {
            Match match = Regex.Match(applicationVersion, AppVersionFilter);
            string appVersion = match.Success ? match.Value : applicationVersion;
            string directoryName = Path.Combine(_settingsBasePath, applicationName, appVersion);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            File.WriteAllText(Path.Combine(directoryName, dataCategory), data);
        }

        /// <summary>
        /// Read a string of data from Remi.
        /// </summary>
        /// <param name="applicationName">Name of the currently executing application. Used to determine where to retrieve the data from.</param>
        /// <param name="applicationVersion">Version of the currently executing application. Used to determine where to retrieve the data from.</param>
        /// <param name="dataCategory">Category or type of the data to be read. Used to determine where to store the data.</param>
        /// <returns>The data from the specified location.</returns>
        public string ReadStringFromRemi(string applicationName, string applicationVersion, string dataCategory)
        {
            Match match = Regex.Match(applicationVersion, AppVersionFilter);
            string appVersion = match.Success ? match.Value : applicationVersion;
            string filePath = Path.Combine(_settingsBasePath, applicationName, appVersion, dataCategory);

            if (!File.Exists(filePath))
                throw new ConfigDoesNotExistInRemiException(applicationName, applicationVersion, dataCategory);

            string data = File.ReadAllText(filePath);

            return data;
        }
    }

    /// <summary>
    /// Will be replaced with the exception generated by RemiControl
    /// </summary>
    [Serializable]
    class ConfigDoesNotExistInRemiException : Exception
    {
        public string ApplicationName { get; private set; }
        public string ApplicationVersion { get; private set; }
        public string ConfigType { get; private set; }

        public override string Message
        {
            get { return ConfigType + " does not exist for " + ApplicationName + " v." + ApplicationVersion + "."; }
        }

        public ConfigDoesNotExistInRemiException(string applicationName, string applicationVersion, string configType)
        {
            ApplicationName = applicationName;
            ApplicationVersion = applicationVersion;
            ConfigType = configType;
        }
    }
}