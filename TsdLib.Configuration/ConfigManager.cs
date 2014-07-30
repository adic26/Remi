using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    public class Config<T>
        where T : ConfigItem, new()
    {
        private static readonly Config<T> instance = new Config<T>();
        public static Config<T> Manager { get { return instance; } }
        private Config() { }

        private IConfigGroup<T> _configGroup;
        internal IConfigGroup<T> ConfigGroup
        {
            get { return _configGroup ?? (_configGroup = new ConfigGroup<T>()); }
        }

        internal T GetConfig(string name)
        {
            T configItem = ConfigGroup.FirstOrDefault(config => config.Name == name);
            if (configItem == null)
                throw new ConfigException(typeof(T).Name + " named: " + name + " could not be found.");
            return configItem;
        }

        public void Edit()
        {
            ConfigForm<T> form = new ConfigForm<T>(ConfigGroup);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
                ConfigGroup.Save();
            else
                _configGroup = new ConfigGroup<T>();
        }
    }
}