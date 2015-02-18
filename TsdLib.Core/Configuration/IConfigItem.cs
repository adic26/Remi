namespace TsdLib.Configuration
{
    /// <summary>
    /// Defines properties and methods required to interact with a configuration instance.
    /// </summary>
    public interface IConfigItem
    {
        /// <summary>
        /// Gets or sets the name of the configuration item.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// True if the configuration is stored locally and on a database. False to store locally only.
        /// </summary>
        bool StoreInDatabase { get; set; }
        /// <summary>
        /// True if the config item is an auto-generated default item.
        /// </summary>
        bool IsDefault { get; set; }
        /// <summary>
        /// Returns true if the config item is valid in the current context.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Performs a deep clone of the IConfigItem object.
        /// </summary>
        /// <returns>A new IConfigItem object.</returns>
        IConfigItem Clone();

        /// <summary>
        /// Save the configuration item to persisted storage.
        /// </summary>
        void Save();
    }
}
