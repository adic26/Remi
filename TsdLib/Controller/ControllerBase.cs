using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private readonly bool _devMode;
        private readonly bool _localDomain;
        private readonly TestDetails _testDetails;
        private readonly ICodeParser _instrumentParser;

        //Public properties
        /// <summary>
        /// Gets a reference to the user interface.
        /// </summary>
        public TView View { get; private set; }


        /// <summary>
        /// Gets the configuration manager that can be used to programatically interact with configuration objects.
        /// </summary>
        protected ConfigManager ConfigManager { get; private set; }

        //Constructor
        /// <summary>
        /// Initialize a new system controller.
        /// </summary>
        /// <param name="testDetails">A <see cref="_testDetails"/> object containing metadata describing the test request.</param>
        /// <param name="databaseConnection">An <see cref="IDatabaseConnection"/> object to handle persistence with a database.</param>
        /// <param name="instrumentParser">An <see cref="System.CodeDom.Compiler.ICodeParser"/> object to generate source code from instrument xml definition files.</param>
        /// <param name="localDomain">True to execute the test sequence in the local application domain. Only available in Debug configuration.</param>
        protected ControllerBase(TestDetails testDetails, IDatabaseConnection databaseConnection, ICodeParser instrumentParser, bool localDomain)
        {

#if DEBUG
            Trace.WriteLine("Using TsdLib debug assembly. Test results will only be stored as Analysis.");
            _localDomain = localDomain;
#else
            if (localDomain)
                Trace.WriteLine("Operating in release mode - ignoring localDomain command-line switch. Test sequence will be executed in the remote application domain.");
            _localDomain = false;
#endif

            //TODO: currently using Debug/Release - update TsdLib to use Dev,Eng,Prod approach
            _devMode = testDetails.TestSystemMode == "Debug";
            _testDetails = testDetails;
            _instrumentParser = instrumentParser;

            ConfigManager = new ConfigManager<TStationConfig, TProductConfig, TTestConfig, Sequence>(testDetails, databaseConnection);

            //set up view
            View = new TView
            {
                Text = testDetails.TestSystemName + " v." + testDetails.TestSystemVersion,
                StationConfigList = ConfigManager.GetConfigGroup<TStationConfig>().GetList(),
                ProductConfigList = ConfigManager.GetConfigGroup<TProductConfig>().GetList(),
                TestConfigList = ConfigManager.GetConfigGroup<TTestConfig>().GetList(),
                SequenceConfigList = ConfigManager.GetConfigGroup<Sequence>().GetList()
            };

#if DEBUG
            View.Text += "         DEBUG MODE";
#endif

            //subscribe to view events
            View.ViewEditConfiguration += ViewEditConfiguration;
            View.ExecuteTestSequence += ExecuteTestSequence;
            View.AbortTestSequence += AbortTestSequence;
        }

        //Event Handlers
        /// <summary>
        /// Default handler for the ViewBase.ViewEditConfiguration event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the View/Edit Configuration button.</param>
        /// <param name="e">Empty EventArgs object.</param>
        protected virtual void ViewEditConfiguration(object sender, EventArgs e)
        {
            ConfigManager.Edit(_devMode);
        }

        /// <summary>
        /// Default handler for the ViewBase.ExecuteTestSequence event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the Execute Test Sequence button.</param>
        /// <param name="e">EventArgs containing the product, station, test and sequence configuration objects.</param>
        protected async virtual void ExecuteTestSequence(object sender, TestSequenceEventArgs e)
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

                SynchronizationContext uiContext = SynchronizationContext.Current;

                await Task.Run(() =>
                {
                    TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> sequence;
                    if (_localDomain)
                    {
                        Type sequenceType = Assembly.GetEntryAssembly().GetType(_testDetails.TestSystemName + ".Sequences" + "." + sequenceConfig.Name);
                        sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>) Activator.CreateInstance(sequenceType);
                    }
                    else
                    {
                        List<CodeCompileUnit> codeCompileUnits = new List<CodeCompileUnit>();

                        if (!Directory.Exists("Instruments"))
                            Directory.CreateDirectory("Instruments");

                        if (!(_instrumentParser is BasicCodeParser))
                            codeCompileUnits.AddRange(Directory.GetFiles("Instruments", "*.xml").Select(xmlFile => _instrumentParser.Parse(new StreamReader(xmlFile))));
                        codeCompileUnits.Add(new BasicCodeParser(sequenceConfig.AssemblyReferences.ToArray()).Parse(new StringReader(sequenceConfig.SourceCode)));

                        DynamicCompiler generator = new DynamicCompiler(Language.CSharp);
                        string sequenceAssembly = generator.Compile(codeCompileUnits.ToArray());

                        sequenceDomain = AppDomain.CreateDomain("SequenceDomain");

                        sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>) sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, _testDetails.TestSystemName + ".Sequences" + "." + sequenceConfig.Name);
                        sequence.AddTraceListener(View.Listener);
                    }

                    EventProxy<TestInfo> infoEventProxy = new EventProxy<TestInfo>(uiContext);
                    sequence.InfoEventProxy = infoEventProxy;
                    infoEventProxy.Event += AddInfo;

                    EventProxy<MeasurementBase> measurementEventProxy = new EventProxy<MeasurementBase>(uiContext);
                    sequence.MeasurementEventProxy = measurementEventProxy;
                    measurementEventProxy.Event += AddMeasurement;

                    EventProxy<Data> dataEventProxy = new EventProxy<Data>(uiContext);
                    sequence.DataEventProxy = dataEventProxy;
                    dataEventProxy.Event += (s, data) => View.AddData(data);
                    
                    _tokenSource = new CancellationTokenSource();
                    _tokenSource.Token.Register(sequence.Abort);

                    sequence.ExecuteSequence(stationConfig, productConfig, testConfig, _testDetails);
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

        /// <summary>
        /// Default handler for the ViewBase.AbortTestSequence event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the Abort Test Sequence button.</param>
        /// <param name="e">Empty EventArgs object.</param>
        protected virtual void AbortTestSequence(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }

        /// <summary>
        /// Default handler for the TestSequenceBase.MeasurementEvent.
        /// </summary>
        /// <param name="sender">The test sequence where the measurement was captutred.</param>
        /// <param name="measurement">The <see cref="MeasurementBase"/> object that was added.</param>
        protected virtual void AddMeasurement(object sender, MeasurementBase measurement)
        {
            View.AddMeasurement(measurement);
        }

        /// <summary>
        /// Default handler for the TestSequenceBase.InfoEvent.
        /// </summary>
        /// <param name="sender">The test sequence where the information was captutred.</param>
        /// <param name="testInfo">The <see cref="TestInfo"/> object that was added.</param>
        protected virtual void AddInfo(object sender, TestInfo testInfo)
        {
            View.AddInformation(testInfo);
        }
    }
}
