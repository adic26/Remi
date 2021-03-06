﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TsdLib.Configuration.Management
{
    /// <summary>
    /// Encapsulates configuration data and provides methods to save configuration.
    /// </summary>
    public interface IConfigManager : IListSource
    {
        /// <summary>
        /// Gets the type of configuration for this <see cref="IConfigManager"/>. Useful when binding to a UI control.
        /// </summary>
        string ConfigTypeName { get; }

        /// <summary>
        /// Gets the type of configuration for this <see cref="IConfigManager"/>.
        /// </summary>
        Type ConfigType { get; }

        /// <summary>
        /// Saves the configuration data to persisted storage.
        /// </summary>
        void Save();

        /// <summary>
        /// Add a new <see cref="IConfigItem"/> to the configuration instances.
        /// </summary>
        /// <param name="item"><see cref="IConfigItem"/> to add.</param>
        void Add(IConfigItem item);

        

        /// <summary>
        /// Reload the configuration data from persisted storage.
        /// </summary>
        void Reload();
    }

    /// <summary>
    /// Encapsulates configuration data and provides methods to add and reload configuration.
    /// </summary>
    //public interface IConfigManager<out T> : IConfigManager
    public interface IConfigManager<out T> : IConfigManager
        where T : IConfigItem
    {
        /// <summary>
        /// Gets the configuration objects handled by the config manager.
        /// </summary>
        /// <returns>The configuration objects.</returns>
        IEnumerable<T> GetConfigGroup();

        /// <summary>
        /// Gets a specific configuration object by name.
        /// </summary>
        /// <param name="name">Name of the configuration object.</param>
        /// <returns>The configuration object with the specified name.</returns>
        T GetConfig(string name);

        /// <summary>
        /// Create a new <typeparamref name="T"/> and add it to the configuration instances.
        /// </summary>
        /// <param name="name">Name to assign to the new configuration item</param>
        /// <param name="storeInDatabase"></param>
        T Add(string name, bool storeInDatabase);
    }

}