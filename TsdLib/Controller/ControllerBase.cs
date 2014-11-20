using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    /// <typeparam name="TEventHandlers">System-specific type of event handler proxy.</typeparam>
    public abstract class ControllerBase<TView, TStationConfig, TProductConfig, TTestConfig, TEventHandlers>
        where TView : IView, new()
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
        where TTestConfig : TestConfigCommon, new()
        where TEventHandlers : EventHandlersBase
    {
        //Private fields
        private CancellationTokenSource _tokenSource;
        private readonly bool _devMode;
        private readonly bool _localDomain;
        private readonly ICodeParser _instrumentParser;

        //Public and protected properties
        /// <summary>
        /// Gets a reference to the user interface.
        /// </summary>
        public TView View { get; private set; }
        /// <summary>
        /// Gets the configuration manager that can be used to programatically interact with configuration objects.
        /// </summary>
        protected ConfigManager ConfigManager { get; private set; }
        /// <summary>
        /// Gets or sets the metadata describing the test request.
        /// </summary>
        protected TestDetails Details { get; set; }

        //Constructor
        /// <summary>
        /// Initialize a new system controller.
        /// </summary>
        /// <param name="testDetails">A <see cref="Details"/> object containing metadata describing the test request.</param>
        /// <param name="configConnection">An <see cref="IDatabaseConnection"/> object to handle configuration persistence with a database.</param>
        /// <param name="instrumentParser">An <see cref="System.CodeDom.Compiler.ICodeParser"/> object to generate source code from instrument xml definition files.</param>
        /// <param name="localDomain">True to execute the test sequence in the local application domain. Only available in Debug configuration.</param>
        protected ControllerBase(TestDetails testDetails, IDatabaseConnection configConnection, ICodeParser instrumentParser, bool localDomain)
        {
            Thread.CurrentThread.Name = "UI Thread";

            //TODO: currently using Debug/Release - update TsdLib to use Dev,Eng,Prod approach
            _devMode = testDetails.TestSystemMode == "Debug";
            Details = testDetails;
            _instrumentParser = instrumentParser;

            ConfigManager = new ConfigManager<TStationConfig, TProductConfig, TTestConfig, Sequence>(testDetails, configConnection);

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
            Trace.WriteLine("Using TsdLib debug assembly. Test results will only be stored as Analysis.");
            _localDomain = localDomain;
            
#else
            if (localDomain)
                Trace.WriteLine("Operating in release mode - ignoring localDomain command-line switch. Test sequence will be executed in the remote application domain.");
            _localDomain = false;
#endif

            //subscribe to view events
            View.ViewEditConfiguration += EditConfiguration;
            View.EditTestDetails += EditTestDetails;
            View.ExecuteTestSequence += ExecuteTestSequence;
            View.AbortTestSequence += AbortTestSequence;
        }

        //Event Handlers
        /// <summary>
        /// Default handler for the ViewBase.ViewEditConfiguration event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the View/Edit Configuration button.</param>
        /// <param name="e">Empty EventArgs object.</param>
        protected virtual void EditConfiguration(object sender, EventArgs e)
        {
            ConfigManager.Edit(_devMode);
        }

        /// <summary>
        /// Default handler for the ViewBase.ViewEditConfiguration event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the View/Edit Configuration button.</param>
        /// <param name="e">True if requesting to use database settings. False otherwise.</param>
        protected virtual void EditTestDetails(object sender, bool e)
        {
            Details.Edit();
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

                //TODO: should we remove the await?
                await Task.Run(() =>
                {
                    Thread.CurrentThread.Name = "Sequence Thread";
                    TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> sequence;
                    TEventHandlers controllerProxy;
                    if (_localDomain)
                    {
                        controllerProxy = (TEventHandlers)Activator.CreateInstance(typeof(TEventHandlers), BindingFlags.CreateInstance, null, new object[] { View }, CultureInfo.CurrentCulture);
                        Type sequenceType = Assembly.GetEntryAssembly().GetType(Assembly.GetEntryAssembly().GetName().Name + ".Sequences" + "." + sequenceConfig.Name);
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


                        sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>)sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, Assembly.GetEntryAssembly().GetName().Name + ".Sequences" + "." + sequenceConfig.Name);

                        controllerProxy = (TEventHandlers) sequenceDomain.CreateInstanceFromAndUnwrap(Assembly.GetEntryAssembly().CodeBase, typeof (TEventHandlers).FullName, false, BindingFlags.CreateInstance, null, new object[]{ View}, CultureInfo.CurrentCulture, null);
                        
                        sequence.AddTraceListener(View.Listener);
                    }

                    //Set up View-handled events
                    EventProxy<TestInfo> infoEventProxy = new EventProxy<TestInfo>(uiContext);
                    sequence.InfoEventProxy = infoEventProxy;
                    infoEventProxy.Event += controllerProxy.InfoAdded;

                    EventProxy<MeasurementBase> measurementEventProxy = new EventProxy<MeasurementBase>(uiContext);
                    sequence.MeasurementEventProxy = measurementEventProxy;
                    measurementEventProxy.Event += controllerProxy.MeasurementAdded;

                    //sequence.MeasurementPlainEvent += MeasurementAddedPlain;

                    EventProxy<Data> dataEventProxy = new EventProxy<Data>(uiContext);
                    sequence.DataEventProxy = dataEventProxy;
                    dataEventProxy.Event += controllerProxy.DataAdded;


                    //Set up Controller-handled events
                    EventProxy<TestResultCollection> testCompleteEventProxy = new EventProxy<TestResultCollection>();
                    sequence.TestCompleteEventProxy = testCompleteEventProxy;
                    testCompleteEventProxy.Event += controllerProxy.TestComplete;

                    _tokenSource = new CancellationTokenSource();
                    _tokenSource.Token.Register(sequence.Abort);

                    sequence.ExecuteSequence(stationConfig, productConfig, testConfig, Details);
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
                //TODO: dispose the sequence - but make sure all events on the UI thread are finished
            }
        }

        /// <summary>
        /// Default handler for the <see cref="IView.AbortTestSequence"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IView"/> that raised the event.</param>
        /// <param name="e">Empty EventArgs object.</param>
        protected virtual void AbortTestSequence(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }
    }
}
