using System;
using System.Diagnostics;
using System.Threading;
using TsdLib.Configuration.Common;
using TsdLib.Measurements;

namespace TsdLib.TestSystem.TestSequence
{
    /// <summary>
    /// Represents a test sequence that can execute repeatedly, once per test config object.
    /// </summary>
    /// <typeparam name="TStationConfig">Type of Station Config used in the derived class.</typeparam>
    /// <typeparam name="TProductConfig">Type of Product Config used in the derived class.</typeparam>
    /// <typeparam name="TTestConfig">Type of Test Config used in the derived class.</typeparam>
    public abstract class SequentialTestSequence<TStationConfig, TProductConfig, TTestConfig> : ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig>
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
        where TTestConfig : TestConfigCommon
    {

        /// <summary>
        /// Executes the test sequence.
        /// </summary>
        /// <param name="token">A cancellation token used to support cooperative cancellation. Should periodically call <see cref="CancellationToken.ThrowIfCancellationRequested"/>.</param>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfig">Test config instance containing test-specific configuration.</param>
        protected abstract void ExecuteTest(CancellationToken token, TStationConfig stationConfig, TProductConfig productConfig, TTestConfig testConfig);


        /// <summary>
        /// Start execution of the test sequence with the specified configuration objects.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfigs">Zero or more test config instances containing test-specific configuration.</param>
        /// <returns>A <see cref="TestResultCollection"/> containing the test results.</returns>
        public override void ExecuteSequence(TStationConfig stationConfig, TProductConfig productConfig, params TTestConfig[] testConfigs)
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

                int testNumber = 0;
                foreach (TTestConfig testConfig in testConfigs)
                {
                    Trace.WriteLine(string.Format("Starting {0} at {1}.", testConfig.Name, DateTime.Now));
                    UpdateProgress(testNumber++, testConfigs.Length);

                    ExecuteTest(CancellationManager.Token, stationConfig, productConfig, testConfig);
                }

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
