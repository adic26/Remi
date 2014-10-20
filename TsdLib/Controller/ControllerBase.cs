using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
        private readonly bool _localDomain;

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
        /// <param name="localDomain">True to execute the test sequence in the local application domain. Only available in Debug configuration.</param>
        protected ControllerBase(bool devMode, DatabaseConnection databaseConnection, bool localDomain = false)
        {
#if DEBUG
            Trace.WriteLine("Using TsdLib debug assembly. Test results will only be stored as Analysis.");
            _localDomain = localDomain;
#else
            if (localDomain)
                Trace.WriteLine("Operating in release mode - ignoring localDomain flag. Test sequence will be executed in the remote application domain.");
            _localDomain = false;
#endif
            TestSystemName = databaseConnection.TestSystemName;
            TestSystemVersion = databaseConnection.TestSystemVersion;

            ConfigManager manager = new ConfigManager<TStationConfig, TProductConfig, TTestConfig, Sequence>(databaseConnection);

            //set up view
            View = new TView
            {
                
#if DEBUG
                Text = TestSystemName + " v." + TestSystemVersion + "         DEBUG MODE",
#else
                Text = TestSystemName + " v." + TestSystemVersion,
#endif
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

                _tokenSource = new CancellationTokenSource();

                Trace.WriteLine(string.Format("Using {0} application domain", _localDomain ? "local" : "remote"));

                await Task.Run(() =>
                {
                    TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> sequence;
                    if (_localDomain)
                    {
                        Type sequenceType = Assembly.GetEntryAssembly().GetType(TestSystemName + ".Sequences" + "." + sequenceConfig.Name);
                        sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>) Activator.CreateInstance(sequenceType);
                    }
                    else
                    {
                        string sequenceAssembly;
                        using (Generator generator = new Generator(TestSystemName, Directory.EnumerateFiles("Instruments", "*.xml"), Language.CSharp))
                            sequenceAssembly = generator.GenerateTestSequenceAssembly(sequenceConfig.Name, sequenceConfig.SourceCode, sequenceConfig.AssemblyReferences);

                        sequenceDomain = AppDomain.CreateDomain("SequenceDomain");

                        sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>) sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, TestSystemName + ".Sequences" + "." + sequenceConfig.Name);
                        sequence.AddTraceListener(View.Listener);
                        
                    }

                    EventProxy<MeasurementEventArgs> measurementEventProxy = new EventProxy<MeasurementEventArgs>();
                    sequence.MeasurementEventProxy = measurementEventProxy;

                    measurementEventProxy.Event += measurementEventHandler;

                    _tokenSource = new CancellationTokenSource();
                    _tokenSource.Token.Register(sequence.Abort);

                    sequence.ExecuteSequence(stationConfig, productConfig, testConfig);
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
                MessageBox.Show("Error details:" + Environment.NewLine + ex + Environment.NewLine + "This error was unexpected and not handled by the TsdLib Application. Please contact TSD for support.", ex.GetType().Name);
            }
            finally
            {
                View.SetState(State.ReadyToTest);
                if (sequenceDomain != null)
                    AppDomain.Unload(sequenceDomain);
            }
        }

        void measurementEventHandler(object sender, MeasurementEventArgs e)
        {
            Measurement measurement = e.Measurement;
            View.AddMeasurement(measurement);
        }
    }
}
