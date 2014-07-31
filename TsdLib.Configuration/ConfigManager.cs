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

        public IConfigGroup<T> GetConfigGroup()
        {
            return new ConfigGroup<T>();
        }

        public T GetConfig(string name)
        {
            T configItem = GetConfigGroup()
                .FirstOrDefault(config => config.Name == name);
            if (configItem == null)
                throw new ConfigException(typeof(T).Name + " named: " + name + " could not be found.");
            return configItem;
        }

        public void Edit()
        {
            ConfigGroup<T> copy = new ConfigGroup<T>(); 
            ConfigForm<T> form = new ConfigForm<T>(copy);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
                copy.Save();
        }
    }
}