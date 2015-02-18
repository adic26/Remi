using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TsdLib.Configuration.Common;
using TsdLib.Measurements;

namespace TsdLib.TestSystem.TestSequence
{
    /// <summary>
    /// Represents a test sequence that can access multiple test config objects in a single execution.
    /// </summary>
    /// <typeparam name="TStationConfig">Type of Station Config used in the derived class.</typeparam>
    /// <typeparam name="TProductConfig">Type of Product Config used in the derived class.</typeparam>
    /// <typeparam name="TTestConfig">Type of Test Config used in the derived class.</typeparam>
    public abstract class MultiConfigTestSequence<TStationConfig, TProductConfig, TTestConfig> : ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig>
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
        where TTestConfig : TestConfigCommon
    {

        /// <summary>
        /// Client application overrides this method to define test steps.
        /// </summary>
        /// <param name="token">A cancellation token used to support cooperative cancellation. Should periodically call <see cref="CancellationToken.ThrowIfCancellationRequested"/>.</param>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfigs">An array of test config objects containing test-specific configuration.</param>
        protected abstract void ExecuteTest(CancellationToken token, TStationConfig stationConfig, TProductConfig productConfig, TTestConfig[] testConfigs);

        /// <summary>
        /// Start execution of the test sequence with the specified configuration objects.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfigs">An array of test config objects containing test-specific configuration.</param>
        public override void ExecuteSequence(TStationConfig stationConfig, TProductConfig productConfig, TTestConfig[] testConfigs)
        {
            try
            {
                AddTestInfo(new TestInfo(stationConfig.CommonBaseTypeName, stationConfig.Name));
                AddTestInfo(new TestInfo(productConfig.CommonBaseTypeName, productConfig.Name));
                foreach (TTestConfig testConfig in testConfigs)
                    AddTestInfo(new TestInfo(testConfig.CommonBaseTypeName, testConfig.Name));
                AddTestInfo(new TestInfo(CommonBaseTypeName, GetType().Name));

                Trace.WriteLine("Starting pre-test at " + DateTime.Now);

                ExecutePreTest(CancellationManager.Token, stationConfig, productConfig);

                Trace.WriteLine(string.Format("Starting {0} at {1}.", string.Join(", ", testConfigs.Select(tc => tc.Name)), DateTime.Now));

                ExecuteTest(CancellationManager.Token, stationConfig, productConfig, testConfigs);

                Trace.WriteLine("Starting post-test at ." + DateTime.Now);

                ExecutePostTest(CancellationManager.Token, stationConfig, productConfig);

                Trace.WriteLine("Completed test sequence at " + DateTime.Now);
                UpdateProgress(1, 1);
            }
            catch (Exception ex)
            {
                CancellationManager.Error = ex;
                throw;
            }
        }
    }
}
