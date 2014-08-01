using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    public class Config<T>
        where T : ConfigItem, new()
    {
        private static readonly Config<T> instance = new Config<T>();
        public static Config<T> Manager { get { return instance; } }

        private ConfigGroup<T> _configGroup;

        private Config()
        {
            _configGroup = new ConfigGroup<T>();
        }

        public IConfigGroup<T> GetConfigGroup()
        {
            return _configGroup;
        }

        public T GetConfig(string name)
        {
            T configItem = _configGroup
                .FirstOrDefault(config => config.Name == name);
            if (configItem == null)
                throw new ConfigException(typeof(T).Name + " named: " + name + " could not be found.");
            return configItem;
        }

        public void Edit()
        {
            using (ConfigForm<T> form = new ConfigForm<T>(_configGroup))
            {
                form.ShowDialog();
                if (form.DialogResult == DialogResult.OK)
                    _configGroup.Save();
                else
                    _configGroup = new ConfigGroup<T>(); //reload from persisted settings
            }
    }
    }
}