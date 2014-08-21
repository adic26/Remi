using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Static class to manage configuration items.
    /// </summary>
    /// <typeparam name="T">Type of configuration item.</typeparam>
    public static class Config<T>
        where T : ConfigItem, new()
    {
        private static ConfigGroup<T> _configGroup = new ConfigGroup<T>();

        /// <summary>
        /// Gets the group of configuration instances.
        /// </summary>
        /// <returns>A configuration group containing all instances of the specified configuration type.</returns>
        public static IConfigGroup<T> GetConfigGroup()
        {
            return _configGroup;
        }

        /// <summary>
        /// Gets a named configuration instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <returns>The configuration instance matching the specified name.</returns>
        public static T GetConfig(string name)
        {
            T configItem = _configGroup
                .FirstOrDefault(config => config.Name == name);
            if (configItem == null)
                throw new ConfigException(typeof(T), name);
            return configItem;
        }

        /// <summary>
        /// Displays the configuration form to view configuration parameters.
        /// </summary>
        /// <param name="editable">True to make configuration parameters writable; False to make configuration parameters read-only.</param>
        public static void Edit(bool editable)
        {
            using (ConfigForm<T> form = new ConfigForm<T>(_configGroup, editable))
            {
                form.ShowDialog();
                if (editable)
                {
                    if (form.DialogResult == DialogResult.OK)
                        _configGroup.Save();
                    else
                        _configGroup = new ConfigGroup<T>(); //reload from persisted settings
                }
            }
        }
    }
}