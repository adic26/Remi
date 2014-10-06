using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TsdLib.CodeGenerator;
using TsdLib.Configuration;
using TsdLib.TestSequence;
using TsdLib.View;
using TsdLib.TestResults;

namespace TsdLib.Controller
{
    /// <summary>
    /// Contains base functionality for the system controller.
    /// </summary>
    /// <typeparam name="TView">System-specific type of user interface.</typeparam>
    /// <typeparam name="TStationConfig">System-specific type of station config.</typeparam>
    /// <typeparam name="TProductConfig">System-specific type of product config.</typeparam>
    /// <typeparam name="TTestConfig">System-specific type of test config.</typeparam>
    public abstract class ControllerBase<TView, TStationConfig, TProductConfig, TTestConfig>
        where TView : IView, new()
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
        where TTestConfig : TestConfigCommon, new()
    {
        //Private fields
        private CancellationTokenSource _tokenSource;

        //Public properties
        /// <summary>
        /// Gets a reference to the user interface.
        /// </summary>
        public IView View { get; private set; }
        /// <summary>
        /// Gets the name of the test system. Also used as the client's namespace.
        /// </summary>
        public string TestSystemName { get; private set; }
        /// <summary>
        /// Gets the test system version.
        /// </summary>
        public string TestSystemVersion { get; private set; }

        //Constructor
        /// <summary>
        /// Initialize a new system controller.
        /// </summary>
        /// <param name="devMode">True to enable Developer Mode - config can be modified but results are stored in the Analysis category.</param>
        /// <param name="databaseConnection">An <see cref="DatabaseConnection"/> object to handle persistence with a database.</param>
        protected ControllerBase(bool devMode, DatabaseConnection databaseConnection)
        {
            TestSystemName = databaseConnection.TestSystemName;
            TestSystemVersion = databaseConnection.TestSystemVersion;

            //TODO: if _devMode, do not pull from database??

            ConfigManager manager = new ConfigManager<TStationConfig, TProductConfig, TTestConfig, Sequence>(databaseConnection);

            //set up view
            View = new TView
            {
                Text = TestSystemName + " v." + TestSystemVersion,
                StationConfigList = manager.GetConfigGroup<TStationConfig>().GetList(),
                ProductConfigList = manager.GetConfigGroup<TProductConfig>().GetList(),
                TestConfigList = manager.GetConfigGroup<TTestConfig>().GetList(),
                SequenceConfigList = manager.GetConfigGroup<Sequence>().GetList()
            };

            //subscribe to view events
            View.ViewEditConfiguration += (s, o) => manager.Edit(devMode);
            View.ExecuteTestSequence += ExecuteTestSequence;
            View.AbortTestSequence += (s, o) => _tokenSource.Cancel();
        }

        //Methods
        async void ExecuteTestSequence(object sender, TestSequenceEventArgs e)
        {
            AppDomain sequenceDomain = null;

            try
            {
                View.SetState(State.TestInProgress);

                TStationConfig stationConfig = (TStationConfig) e.StationConfig;
                TProductConfig productConfig = (TProductConfig) e.ProductConfig;
                TTestConfig testConfig = (TTestConfig) e.TestConfig;
                Sequence sequenceConfig = (Sequence) e.SequenceConfig;

                await Task.Run(() =>
                {
                    _tokenSource = new CancellationTokenSource();

                    using (Generator generator = new Generator(TestSystemName, Directory.EnumerateFiles("Instruments", "*.xml").ToArray(), Language.CSharp))
                    {

                        string sequenceAssembly = generator.GenerateTestSequenceAssembly(
                            sequenceConfig.Name,
                            sequenceConfig.FullSourceCode,
                            sequenceConfig.AssemblyReferences.ToArray());

                        sequenceDomain = AppDomain.CreateDomain("SequenceDomain");

                        TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> sequence =
                            (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>)
                                sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, TestSystemName + ".Sequences" + "." + sequenceConfig.Name);

                        sequence.AddTraceListener(View.Listener);

                        EventProxy<MeasurementEventArgs> measurementEventProxy = new EventProxy<MeasurementEventArgs>();
                        sequence.MeasurementEventProxy = measurementEventProxy;

                        measurementEventProxy.Event += measurementEventHandler;

                        _tokenSource.Token.Register(sequence.Abort);

                        sequence.ExecuteSequence(stationConfig, productConfig, testConfig);
                    }
                });

            }
            catch (TsdLibException ex)
            {
                DialogResult result = MessageBox.Show("Error details:" + Environment.NewLine + ex.Message + Environment.NewLine + "Would you like to view help for this error?", ex.GetType().Name, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Process.Start(ex.HelpLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error details:" + Environment.NewLine + ex.Message + Environment.NewLine + "This error was unexpected and not handled by the TsdLib Application. Please contact TSD for support.", ex.GetType().Name);
            }
            finally
            {
                if (sequenceDomain != null)
                    AppDomain.Unload(sequenceDomain);
                View.SetState(State.ReadyToTest);
            }
        }

        void measurementEventHandler(object sender, MeasurementEventArgs e)
        {
            Measurement measurement = e.Measurement;
            View.AddMeasurement(measurement);
        }
    }
}
