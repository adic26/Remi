using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TsdLib.Configuration.Common;
using TsdLib.TestSystem;

namespace TsdLib.Configuration.Management
{
    public class LocalSequenceConfigManager : MarshalByRefObject, IConfigManager<LocalSequenceConfig>
    {
        private readonly List<LocalSequenceConfig> _configs = new List<LocalSequenceConfig>();

        public LocalSequenceConfigManager()
        {
            var sequences = Assembly.GetEntryAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITestSequence)));
            foreach (Type sequence in sequences)
                _configs.Add(new LocalSequenceConfig(sequence.FullName));
        }

        public IEnumerable<LocalSequenceConfig> GetConfigGroup()
        {
            return _configs;
        }

        public LocalSequenceConfig GetConfig(string name)
        {
            return GetConfigGroup()
                .FirstOrDefault(cfg => cfg.Name == name);
        }

        public LocalSequenceConfig Add(string name, bool storeInDatabase)
        {
            Trace.WriteLine("Running in local domain, so test sequence " + name + " will not be added to sequence config");

            return GetConfig(name);
        }

        public string ConfigTypeName
        {
            get { return "SequenceConfigCommon"; }
        }

        public Type ConfigType
        {
            get { return typeof(SequenceConfigCommon); }
        }

        public void Save()
        {
            Trace.WriteLine("Running in local domain, so sequence config will not be saved as xml");
        }

        public void Add(IConfigItem item)
        {
            Trace.WriteLine("Running in local domain, so test sequence " + item.Name + " will not be added to sequence config");
        }

        public void Reload()
        {
            Trace.WriteLine("Running in local domain, so sequence config will not be reloaded from xml");
        }

        public bool ContainsListCollection
        {
            get { return false; }
        }

        public IList GetList()
        {
            return _configs;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
