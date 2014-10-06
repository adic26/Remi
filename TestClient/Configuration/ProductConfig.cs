using System;
using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{
    [Serializable]
    public class ProductConfig : ProductConfigCommon
    {
        //TODO: Create a product configuration structure using public properties with get and set accessors.
        //The values for these properties will be configured by the application at run-time (in Development mode only) or in the database
        //The property values will be accessed by the TestSequence.Execute() method

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
        /// <param name="testSystemName">Name of the test system the config item is used for.</param>
        public ProductConfig(string name, bool storeInDatabase, string testSystemName)
            : base(name, storeInDatabase, testSystemName)
        {
            SettlingTime = 500;
        }
    }
}
