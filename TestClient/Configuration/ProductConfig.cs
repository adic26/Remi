using System;
using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{
    [Serializable]
    public class ProductConfig : ProductConfigCommon
    {
        [Category("Timing")]
        public int SettlingTime { get; set; }

        /// <summary>
        /// Initialize a new ProductConfig configuration instance from persisted settings.
        /// </summary>
        public ProductConfig()  { }

        /// <summary>
        /// Initialize a new ProductConfig instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on a database. False to store locally only.</param>
        public ProductConfig(string name, bool storeInDatabase)
            : base(name, storeInDatabase)
        {
            SettlingTime = 500;
        }
    }
}
