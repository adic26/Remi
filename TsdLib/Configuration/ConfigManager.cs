using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    public static class Config<T>
    where T : ConfigItem, new()
    {
        private static ConfigGroup<T> _configGroup = new ConfigGroup<T>();

        public static IConfigGroup<T> GetConfigGroup()
        {
            return _configGroup;
        }

        public static T GetConfig(string name)
        {
            T configItem = _configGroup
                .FirstOrDefault(config => config.Name == name);
            if (configItem == null)
                throw new ConfigException(typeof(T).Name + " named: " + name + " could not be found.");
            return configItem;
        }

        public static void Edit(bool editable)
        {
            using (ConfigForm<T> form = new ConfigForm<T>(_configGroup, editable))
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