using System.Threading;
using TsdLib.Configuration.Common;

namespace TsdLib.TestSystem.TestSequence
{
    /// <summary>
    /// Represents a test sequence that can be executed with configuration parameters.
    /// </summary>
    /// <typeparam name="TStationConfig">Type of Station Config used in the derived class.</typeparam>
    /// <typeparam name="TProductConfig">Type of Product Config used in the derived class.</typeparam>
    /// <typeparam name="TTestConfig">Type of Test Config used in the derived class.</typeparam>
    public abstract class ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig> : TestSequenceBase
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
        where TTestConfig : TestConfigCommon
    {
        /// <summary>
        /// Performs initialization or connection setup befoer the test begins.
        /// </summary>
        /// <param name="token">A cancellation token used to support cooperative cancellation. Should periodically call <see cref="CancellationToken.ThrowIfCancellationRequested"/>.</param>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        protected virtual void ExecutePreTest(CancellationToken token, TStationConfig stationConfig, TProductConfig productConfig)
        {

        }

        /// <summary>
        /// Perform teardown or disconnection after the test is complete, but make sure to call base.ExecutePostTest in the overriding method.
        /// </summary>
        /// <param name="token">A cancellation token used to support cooperative cancellation. Should periodically call <see cref="CancellationToken.ThrowIfCancellationRequested"/>.</param>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        protected virtual void ExecutePostTest(CancellationToken token, TStationConfig stationConfig, TProductConfig productConfig)
        {

        }

        /// <summary>
        /// Start execution of the test sequence with the specified configuration objects.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfigs">An array of test config objects containing test-specific configuration.</param>
        public abstract void ExecuteSequence(TStationConfig stationConfig, TProductConfig productConfig, TTestConfig[] testConfigs);
    }
}
