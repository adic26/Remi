﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Encapsulates configuration data of a specified type and provides methods to save and retieve.
    /// </summary>
    /// <typeparam name="T">Type of configuration derived from <see cref="ConfigItem"/></typeparam>
    public class ConfigManager<T> : IConfigManager
        where T : ConfigItem, new()
    {
        private static ConfigManager<T> _instance;
        public static IConfigManager GetConfigManager(ITestDetails details, IConfigConnection sharedConfigConnection)
        {
            return _instance ?? (_instance = new ConfigManager<T>(details, sharedConfigConnection));
        }

        /// <summary>
        /// Gets the type of configuration for this <see cref="ConfigManager{T}"/>. Useful when binding to a UI control.
        /// </summary>
        public string ConfigTypeName
        {
            get { return typeof (T).Name; }
        }

        private readonly ITestDetails _testDetails;
        private readonly FileSystemConnection _localConfigConnection;
        private readonly IConfigConnection _sharedConfigConnection;
        private readonly XmlSerializer _serializer;

        private readonly BindingSource _bindingSource;
        private BindingList<T> _configs;

        /// <summary>
        /// Initialize a new configuration manager instance.
        /// </summary>
        /// <param name="testDetails">The <see cref="ITestDetails"/> used for retriving the relevant configuration data.</param>
        /// <param name="sharedConfigConnection">An <see cref="IConfigConnection"/> object used to persist configuration to a shared location (ie. database, network share, local shared directory, etc.)</param>
        public ConfigManager(ITestDetails testDetails, IConfigConnection sharedConfigConnection)
        {
            _testDetails = testDetails;
            _localConfigConnection = new FileSystemConnection(SpecialFolders.Configuration);
            _sharedConfigConnection = sharedConfigConnection;
            _serializer = new XmlSerializer(typeof(BindingList<T>));
            _configs = new BindingList<T>();
            _bindingSource = new BindingSource{DataSource = _configs};
        }

        /// <summary>
        /// Add a new <see cref="ConfigItem"/> to the collection.
        /// </summary>
        /// <param name="configItem">Config instance to add.</param>
        public void Add(T configItem)
        {
            T existing = _configs.FirstOrDefault(cfg => cfg.Name == configItem.Name);

            if (existing == null)
                Trace.WriteLine("Adding " + configItem.Name);
            else
            {
                _configs.Remove(existing);
                Trace.WriteLine("Replacing " + configItem.Name);
            }
            _configs.Add(configItem);
        }

        /// <summary>
        /// Retrive the set of configuration instances.
        /// </summary>
        /// <param name="reload">OPTIONAL: Specify true to reload the configuration data from storage.</param>
        /// <returns>A collection of configuration instances.</returns>
        public BindingList<T> GetConfigGroup(bool reload = false)
        {
            if (_configs.Count == 0 || reload)
            {
                XElement sharedXml = null;

                if (_sharedConfigConnection != null)
                {
                    string sharedConfigString;
                    var sharedConfigExists = _sharedConfigConnection.TryReadString(_testDetails.SafeTestSystemName, _testDetails.TestSystemVersion, _testDetails.TestSystemMode, typeof(T), out sharedConfigString);
                    if (sharedConfigExists)
                    {
                        XDocument sharedXmlDoc = XDocument.Parse(sharedConfigString);
                        sharedXml = sharedXmlDoc.Root;
                        if (sharedXml == null)
                            throw new InvalidConfigFileException(sharedXmlDoc);
                    }
                }

                string localConfigString;
                bool localConfigexists = _localConfigConnection.TryReadString(_testDetails.SafeTestSystemName, _testDetails.TestSystemVersion, _testDetails.TestSystemMode, typeof(T), out localConfigString);
                if (localConfigexists)
                {
                    XDocument localXmlDoc = XDocument.Parse(localConfigString);
                    XElement localXml = localXmlDoc.Root;
                    if (localXml == null)
                        throw new InvalidConfigFileException(localXmlDoc);

                    //Merge the shared config into the local config
                    if (sharedXml != null)
                        localXml.ReplaceAll(sharedXml.Elements()
                            .Union(localXml.Elements(), new ConfigXmlEqualityComparer(localXml, sharedXml)));

                    //If there are non-default configs, remove the defaults - they are no longer needed.
                    if (localXml.Elements().Any(e => e.Attribute("IsDefault").Value == "false"))
                        localXml.Elements().Where(e => e.Attribute("IsDefault").Value == "true").Remove();

                    //Deserialize the merged config XML into the configs list
                    using (XmlReader reader = localXml.CreateReader())
                        _configs = (BindingList<T>)(_serializer.Deserialize(reader));
                }
                else if (sharedXml != null) //Deserialize the shared config XML into the configs list
                    using (XmlReader reader = sharedXml.CreateReader())
                        _configs = (BindingList<T>)(_serializer.Deserialize(reader));
                else
                {
                    T newCfg = new T
                    {
                        Name = "Default" + typeof(T).Name,
                        Details = _testDetails,
                        IsDefault = true,
                        StoreInDatabase = false
                    };
                    newCfg.InitializeDefaultValues();
                    _configs.Clear();
                    _configs.Add(newCfg);
                }

                Save();
            }
            return _configs;
        }

        /// <summary>
        /// Retrieves a configuration instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <returns>A <see cref="ConfigItem"/> matching the name specified. Returns null if there is no matching ConfigItem.</returns>
        public T GetConfig(string name)
        {
            return GetConfigGroup().FirstOrDefault(
                cfg => cfg.Name == name &&
                _testDetails.SafeTestSystemName == cfg.Details.SafeTestSystemName &&
                _testDetails.TestSystemVersion == cfg.Details.TestSystemVersion &&
                _testDetails.TestSystemMode == cfg.Details.TestSystemMode);
        }

        /// <summary>
        /// Saves the configuration data to persisted storage.
        /// </summary>
        public void Save()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true
            };

            StringBuilder sbLocal = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(sbLocal, xmlWriterSettings))
                _serializer.Serialize(xmlWriter, _configs);

            _localConfigConnection.WriteString(_testDetails.SafeTestSystemName, _testDetails.TestSystemVersion, _testDetails.TestSystemMode, typeof(T), sbLocal.ToString());

            List<T> sharedConfigGroups = _configs.Where(cfg => cfg.StoreInDatabase).ToList();

            if (sharedConfigGroups.Any())
            {
                StringBuilder sbDatabase = new StringBuilder();
                using (XmlWriter xmlWriter = XmlWriter.Create(sbDatabase, xmlWriterSettings))
                    _serializer.Serialize(xmlWriter, new BindingList<T>(sharedConfigGroups));
                _sharedConfigConnection.WriteString(_testDetails.SafeTestSystemName, _testDetails.TestSystemVersion, _testDetails.TestSystemMode, typeof(T), sbDatabase.ToString());
            }
        }

        /// <summary>
        /// Add a new <see cref="IConfigItem"/> to the collection.
        /// </summary>
        /// <param name="configItem">Config instance to add.</param>
        void IConfigManager.Add(IConfigItem configItem)
        {
            T cfg = configItem as T;
            if (cfg != null)
                Add(cfg);
        }

        public IConfigManager Reload()
        {
            _bindingSource.DataSource = GetConfigGroup(true);
            return this;
        }

        #region Equality Comparer

        private class ConfigXmlEqualityComparer : IEqualityComparer<XElement>
        {
            private readonly XElement _localRoot;
            private readonly XElement _sharedRoot;

            public ConfigXmlEqualityComparer(XElement localRoot, XElement sharedRoot)
            {
                _localRoot = localRoot;
                _sharedRoot = sharedRoot;
            }

            public bool Equals(XElement localElement, XElement sharedElement)
            {
                string xName = localElement.Attribute("Name").Value;
                if (xName == null)
                    throw new InvalidConfigFileException(null, _localRoot);
                string yName = sharedElement.Attribute("Name").Value;
                if (yName == null)
                    throw new InvalidConfigFileException(null, _sharedRoot);
                return xName == yName;
            }

            public int GetHashCode(XElement obj)
            {
                string name = obj.Attribute("Name").Value;
                if (name == null)
                    throw new InvalidConfigFileException(null, _localRoot, _sharedRoot);
                return name.GetHashCode();
            }
        }

        #endregion

        #region IListSource Implementation

        public IList GetList()
        {
            if (_bindingSource.Count == 0)
                Reload();
            return _bindingSource;
        }

        public bool ContainsListCollection
        {
            get { return false; }
        }

        #endregion
    }
}