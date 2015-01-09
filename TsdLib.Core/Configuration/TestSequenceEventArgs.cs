using System;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Provides config data to pass to Test Sequence upon execution.
    /// </summary>
    [Obsolete("Shouldn't worry about passing data between User Controls - just let the controller take what it needs")]
    public class TestSequenceEventArgs : EventArgs
    {
        /// <summary>
        /// Station Config object to pass to the Test Sequence.
        /// </summary>
        public IConfigItem[] StationConfig { get; private set; }
        /// <summary>
        /// Product Config object to pass to the Test Sequence.
        /// </summary>
        public IConfigItem[] ProductConfig { get; private set; }
        /// <summary>
        /// Test Config object to pass to the Test Sequence.
        /// </summary>
        public IConfigItem[] TestConfig { get; private set; }
        /// <summary>
        /// Sequence Config object to pass to the Test Sequence.
        /// </summary>
        public IConfigItem[] SequenceConfig { get; private set; }
        /// <summary>
        /// True if Test Sequence results will be published to a database.
        /// </summary>
        public bool PublishResults { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TestSequenceEventArgs class.
        /// </summary>
        /// <param name="stationConfig">Station Config object to pass to the Test Sequence.</param>
        /// <param name="productConfig">Product Config object to pass to the Test Sequence.</param>
        /// <param name="testConfig">Test Config object to pass to the Test Sequence.</param>
        /// <param name="sequenceConfig">Sequence Config object to pass to the Test Sequence.</param>
        /// <param name="publishResults">True to publish Test Sequence results to a database.</param>
        public TestSequenceEventArgs(IConfigItem[] stationConfig, IConfigItem[] productConfig, IConfigItem[] testConfig, IConfigItem[] sequenceConfig, bool publishResults)
        {
            StationConfig = stationConfig;
            ProductConfig = productConfig;
            TestConfig = testConfig;
            SequenceConfig = sequenceConfig;
            PublishResults = publishResults;
        }
    }

}