using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using TsdLib.Configuration.Common;
using TsdLib.Configuration.Connections;
using TsdLib.Configuration.Managers;
using TsdLib.Configuration.Null;
using TsdLib.Configuration.TestCases;
using TsdLib.Forms;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;
using TsdLib.UI;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Contains base functionality for the system controller without station/product/test configuration.
    /// </summary>
    /// <typeparam name="TView">Type of the derived user interface.</typeparam>
    /// <typeparam name="TSequence">Type of the derived test sequence.</typeparam>
    public abstract class ControllerBase<TView, TSequence> : ControllerBase<TView, TSequence, NullStationConfig, NullProductConfig, NullTestConfig>
        where TView : IView, new()
        where TSequence : ConfigurableTestSequence<NullStationConfig, NullProductConfig, NullTestConfig>, new()
    {
        /// <summary>
        /// Initialize a new system controller.
        /// </summary>
        /// <param name="testDetails">An <see cref="ITestDetails"/> object containing metadata describing the test request.</param>
        /// <param name="configConnection">An <see cref="IConfigConnection"/> object to handle configuration persistence with a database.</param>
        /// <param name="localDomain">True to execute the test sequence in the local application domain. Disables dynamic sequence/instrument generation.</param>
        protected ControllerBase(ITestDetails testDetails, IConfigConnection configConnection, bool localDomain)
            : base(testDetails, configConnection, localDomain)
        {

        }
    }

    /// <summary>
    /// Contains base functionality for the system controller.
    /// </summary>
    /// <typeparam name="TView">Type of the derived user interface.</typeparam>
    /// <typeparam name="TSequence">Type of the derived test sequence.</typeparam>
    /// <typeparam name="TStationConfig">Type of the derived station config.</typeparam>
    /// <typeparam name="TProductConfig">Type of the derived product config.</typeparam>
    /// <typeparam name="TTestConfig">Type of the derived test config.</typeparam>
    public abstract class ControllerBase<TView, TSequence, TStationConfig, TProductConfig, TTestConfig> : MarshalByRefObject
        where TView : IView, new()
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
        where TTestConfig : TestConfigCommon, new()
        where TSequence : ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig>
    {
        private TSequence _activeSequence;
        private readonly bool _localDomain;
        private readonly List<Task> _loggingTasks;
        private readonly TextWriterTraceListener _textWriterTraceListener;
        private readonly TestCaseProvider _testCaseProvider;

        /// <summary>
        /// Gets a reference to the user interface.
        /// </summary>
        public TView UI { get; private set; }

        /// <summary>
        /// Gets or sets the metadata describing the test request.
        /// </summary>
        protected ITestDetails Details { get; private set; }

        /// <summary>
        /// Gets a list of active tasks responsible for logging test results.
        /// </summary>
        protected ReadOnlyCollection<Task> LoggingTasks { get; private set; }

        /// <summary>
        /// Gets a provider that can be used to retrieve configuration manager instances for modifying, storing and recalling configuration data.
        /// </summary>
        protected ConfigManagerProvider configManagerProvider { get; private set; }

        /// <summary>
        /// Initialize a new system controller.
        /// </summary>
        /// <param name="testDetails">An <see cref="ITestDetails"/> object containing metadata describing the test request.</param>
        /// <param name="configConnection">An <see cref="IConfigConnection"/> object to handle configuration persistence with a database.</param>
        /// <param name="localDomain">True to execute the test sequence in the local application domain. Disables dynamic sequence/instrument generation.</param>
        protected ControllerBase(ITestDetails testDetails, IConfigConnection configConnection, bool localDomain)
        {
            Details = testDetails;

            Thread.CurrentThread.Name = "UI Thread";
            Trace.AutoFlush = true;
            _textWriterTraceListener = new TextWriterTraceListener(SpecialFolders.GetTraceLogs(Details.SafeTestSystemName));
            Trace.Listeners.Add(_textWriterTraceListener);
            
            _testCaseProvider = new TestCaseProvider(Details.SafeTestSystemName);

            _loggingTasks = new List<Task>();
            LoggingTasks = new ReadOnlyCollection<Task>(_loggingTasks);

            configManagerProvider = new ConfigManagerProvider(Details, configConnection);
            //TODO: if local domain, config manager should just reflect the entry assembly to get baked-in sequences and make Sequence read-only in editor form

            //set up view
            UI = new TView();
            if (UI.TraceListenerControl != null)
                Trace.Listeners.Add(UI.TraceListenerControl.Listener);
            UI.SetTitle(Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode);

            Details.TestSystemIdentityChanged += configDetails_TestSystemIdentityChanged;
            updateConfigManagers();

#if DEBUG
            Trace.WriteLine("Using TsdLib debug assembly. Test results will only be stored as Analysis.");
            _localDomain = localDomain;
#else
            //should still leave the option of running in local domain in production/release - just need to make SequenceConfig readonly in config manager
            //if (localDomain)
            //    Trace.WriteLine("Operating in release mode - ignoring localDomain command-line switch. Test sequence will be executed in the remote application domain.");
            //_localDomain = false;
            _localDomain = localDomain;
#endif

            //subscribe to view events
            if (UI.ConfigControl != null)
                UI.ConfigControl.ViewEditConfiguration += EditConfiguration;
            if (UI.TestDetailsControl != null)
                UI.TestDetailsControl.EditTestDetails += EditTestDetails;
            if (UI.TestSequenceControl != null)
            {
                UI.TestSequenceControl.ExecuteTestSequence += ExecuteTestSequence;
                UI.TestSequenceControl.AbortTestSequence += AbortTestSequence;
            }
            if (UI.TestCaseControl != null)
            {
                UI.TestCaseControl.TestCaseSaved += TestCaseControl_TestCaseSaved;
                UI.TestCaseControl.TestCaseSelected += TestCaseControl_TestCaseSelected;
                UI.TestCaseControl.DisplayTestCases(_testCaseProvider.Load());
            }
            UI.UIClosing += UIClosing;

            UI.SetState(State.ReadyToTest);
        }

        void TestCaseControl_TestCaseSelected(object sender, string selectedTestCaseName)
        {
            IEnumerable<ITestCase> testCases = _testCaseProvider.Load();
            ITestCase selected = testCases.FirstOrDefault(tc => tc.Name == selectedTestCaseName);
            UI.ConfigControl.SelectedTestConfig = UI.ConfigControl.TestConfigManager.GetConfigGroup().Where(cfg => selected.TestConfigs.Contains(cfg.Name)).ToArray();
            UI.ConfigControl.SelectedSequenceConfig = UI.ConfigControl.SequenceConfigManager.GetConfigGroup().Where(cfg => selected.Sequences.Contains(cfg.Name)).ToArray();
        }

        void TestCaseControl_TestCaseSaved(object sender, EventArgs e)
        {
            string testCaseName;
            using (ConfigItemCreateForm form = new ConfigItemCreateForm(false))
                testCaseName = form.ShowDialog() == DialogResult.OK ? form.ConfigItemName : "N/A";

            TestCase testCase = new TestCase(testCaseName, UI.ConfigControl.SelectedTestConfig, UI.ConfigControl.SelectedSequenceConfig);
            _testCaseProvider.Save(testCase);
        }

        void configDetails_TestSystemIdentityChanged(object sender, string e)
        {
            UI.SetTitle(Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode);
            if (UI.ConfigControl != null)
                updateConfigManagers();
        }

        private void updateConfigManagers()
        {
            configManagerProvider.Reload();
            IConfigManager<TStationConfig> stn = configManagerProvider.GetConfigManager<TStationConfig>();
            UI.ConfigControl.StationConfigManager = stn;
            IConfigManager<TProductConfig> prod = configManagerProvider.GetConfigManager<TProductConfig>();
            UI.ConfigControl.ProductConfigManager = prod;
            IConfigManager<TTestConfig> test = configManagerProvider.GetConfigManager<TTestConfig>();
            UI.ConfigControl.TestConfigManager = test;
            IConfigManager<ISequenceConfig> seq = configManagerProvider.GetConfigManager<SequenceConfigCommon>();
            UI.ConfigControl.SequenceConfigManager = seq;
        }

        /// <summary>
        /// Default handler for the ViewBase.ExecuteTestSequence event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the Execute Test Sequence button.</param>
        /// <param name="e">An emptry EventArgs object.</param>
        protected async void ExecuteTestSequence(object sender, EventArgs e)
        {
            AppDomain sequenceDomain = null;

            UI.SetState(State.TestStarting);

            Trace.WriteLine(Details);

            TStationConfig stationConfig = (TStationConfig) (UI.ConfigControl.SelectedStationConfig.FirstOrDefault() ?? configManagerProvider.GetConfigManager<TStationConfig>().GetList()[0]);
            TProductConfig productConfig = (TProductConfig)(UI.ConfigControl.SelectedProductConfig.FirstOrDefault() ?? configManagerProvider.GetConfigManager<TProductConfig>().GetList()[0]);
            TTestConfig[] testConfigs = UI.ConfigControl.SelectedTestConfig.Cast<TTestConfig>().ToArray();
            if (!testConfigs.Any())
                testConfigs = configManagerProvider.GetConfigManager<TTestConfig>().GetConfigGroup().ToArray();

            var sequenceConfigs = UI.ConfigControl.SelectedSequenceConfig.Cast<SequenceConfigCommon>().ToArray();
            if (!sequenceConfigs.Any())
                sequenceConfigs = configManagerProvider.GetConfigManager<SequenceConfigCommon>().GetConfigGroup().ToArray();
            bool publishResults = UI.TestSequenceControl.PublishResults;

            Trace.WriteLine(string.Format("Using {0} application domain", _localDomain ? "local" : "remote"));

            SynchronizationContext uiContext = SynchronizationContext.Current;

            ControllerProxy controllerProxy;

            foreach (SequenceConfigCommon sequenceConfig in sequenceConfigs)
            {
                try
                {
                    DateTime startTime = DateTime.Now;

                    SequenceConfigCommon config = sequenceConfig;
                    await Task.Run(() =>
                    {
                        if (_localDomain)
                        {
                            _activeSequence = (TSequence)Activator.CreateInstance(Assembly.GetEntryAssembly().GetType(config.FullTypeName));

                            controllerProxy = (ControllerProxy)Activator.CreateInstance(typeof(ControllerProxy), BindingFlags.CreateInstance, null, new object[] { UI, _activeSequence.CancellationManager }, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            List<CodeCompileUnit> codeCompileUnits = new List<CodeCompileUnit> { new BasicCodeParser(config.AssemblyReferences.ToArray()).Parse(new StringReader(config.SourceCode)) };

                            IEnumerable<CodeCompileUnit> additionalCodeCompileUnits = GenerateAdditionalCodeCompileUnits(config.Namespace.Replace(".Sequences", ""));

                            DynamicCompiler generator = new DynamicCompiler(Language.CSharp, AppDomain.CurrentDomain.BaseDirectory);
                            string sequenceAssembly = generator.Compile(codeCompileUnits.Concat(additionalCodeCompileUnits));

                            sequenceDomain = AppDomain.CreateDomain("Sequence Domain");

                            _activeSequence = (TSequence)sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, config.FullTypeName);
                            controllerProxy = (ControllerProxy) sequenceDomain.CreateInstanceAndUnwrap(typeof (ControllerProxy).Assembly.FullName, typeof (ControllerProxy).FullName, false, BindingFlags.CreateInstance, null, new object[] {UI, _activeSequence.CancellationManager}, CultureInfo.CurrentCulture, null);

                            if (UI.TraceListenerControl != null)
                                _activeSequence.AddTraceListener(UI.TraceListenerControl.Listener);

                            _activeSequence.AddTraceListener(_textWriterTraceListener);
                        }

                        //TODO: can we create the event proxies on the test sequence and just attach to them here?
                        EventProxy<ITestInfo> infoEventHandler = new EventProxy<ITestInfo>();
                        _activeSequence.InfoEventProxy = infoEventHandler;
                        infoEventHandler.Attach(controllerProxy.InfoAdded, uiContext);

                        EventProxy<IMeasurement> measurementEventHandler = new EventProxy<IMeasurement>();
                        _activeSequence.MeasurementEventProxy = measurementEventHandler;
                        measurementEventHandler.Attach(controllerProxy.MeasurementAdded, uiContext);

                        EventProxy<Tuple<int, int>> progressEventHandler = new EventProxy<Tuple<int, int>>();
                        _activeSequence.ProgressEventProxy = progressEventHandler;
                        progressEventHandler.Attach(controllerProxy.ProgressUpdated, uiContext);

                        EventProxy<object> dataEventHandler = new EventProxy<object>();
                        _activeSequence.DataEventProxy = dataEventHandler;
                        dataEventHandler.Attach(controllerProxy.DataAdded, uiContext);

                        _activeSequence.Config = configManagerProvider;

                        _activeSequence.ExecuteSequence(stationConfig, productConfig, testConfigs);
                    });

                    DateTime endTime = DateTime.Now;

                    string overallResult;
                    if (!_activeSequence.Measurements.Any() || _activeSequence.Measurements.All(m => m.Result == MeasurementResult.Undefined))
                        overallResult = "Undefined";
                    else
                        overallResult = _activeSequence.Measurements.All(m => m.Result == MeasurementResult.Pass) ? "Pass" : "Fail";
                    ITestResults testResults = new TestResultCollection(Details, _activeSequence.Measurements, new TestSummary(overallResult, startTime, endTime), _activeSequence.TestInfo);


                    _loggingTasks.Add(Task.Run(() =>
                    {
                        ITestResults localTestResults = testResults; //Create a local reference in case they are overwritten by the next sequence before the logging is complete.
                        if (Thread.CurrentThread.Name != null)
                            Thread.CurrentThread.Name = "Result Handler Thread";
                        Thread.CurrentThread.IsBackground = false;
                        SaveResults(localTestResults);
                        if (publishResults)
                            PublishResults(localTestResults);
                    }).ContinueWith(t =>
                        {
                            _loggingTasks.Remove(t);
                            if (t.IsFaulted && t.Exception != null)
                                Trace.WriteLine("Failed to log test results!" + Environment.NewLine + string.Join(Environment.NewLine, t.Exception.Flatten().InnerExceptions));
                        })
                    );

                }
                //TODO: use AggregateException?
                catch (OperationCanceledException) //User cancellation and controller proxy errors are propagated as OperationCancelledException
                {
                    if (_activeSequence.CancellationManager.CancelledByUser)
                    {
                        Trace.WriteLine("Test sequence was cancelled by user.");
                        break;
                    }

                    else if (_activeSequence.CancellationManager.Error != null)
                    {
                        Trace.WriteLine(string.Format("Exception:{0} was thrown from client code in the primary AppDomain", _activeSequence.CancellationManager.Error.GetType().Name));
                        throw _activeSequence.CancellationManager.Error;
                    }

                    //Test test sequence did not set the Error property - this should never happen
                    else
                    {
                        Trace.WriteLine("Test test sequence did not set the Error property - this should never happen");
                        MessageBox.Show("An undescribed error has occurred but the test sequence Error prperty was not set. Please contact TSD for support.", "Unknown Error:");
                    }
                }
                catch (Exception ex)
                {
                    displayError(ex, sequenceConfig.Name);
                }
                finally
                {
                    try
                    {
                        if (_activeSequence != null)
                        {
                            _activeSequence.Dispose();
                            _activeSequence = null;
                        }
                        if (sequenceDomain != null)
                            AppDomain.Unload(sequenceDomain);
                    }
                    catch (CannotUnloadAppDomainException ex)
                    {
                        Trace.WriteLine("AppDomain could not be unloaded." + Environment.NewLine + ex);
                    }

                }
            }
            UI.SetState(State.ReadyToTest);
        }

        private void displayError(Exception ex, string sequenceName)
        {
            Trace.WriteLine(ex);
            Task.Run(() =>
            {
                bool helpLinkPresent = ex.HelpLink != null;
                DialogResult result = MessageBox.Show(string.Join(Environment.NewLine,
                        "Error: " + ex.GetType().Name,
                        "Message: " + ex.Message,
                        helpLinkPresent ? "Would you like to view help for this error?" : ""),
                    "Error occurred in " + sequenceName,
                    helpLinkPresent ? MessageBoxButtons.YesNo : MessageBoxButtons.OK);

                if (result == DialogResult.Yes)
                    Process.Start(ex.HelpLink);
            }).ContinueWith(task => 
            {
                if (task.IsFaulted)
                    Trace.WriteLine(task.Exception);
            });
        }

        /// <summary>
        /// Default handler for the ViewBase.ViewEditConfiguration event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the View/Edit Configuration button.</param>
        /// <param name="configManagers">An array of <see cref="IConfigManager"/> objects containing the configuration data.</param>
        protected virtual void EditConfiguration(object sender, IConfigManager[] configManagers)
        {
            using (ConfigManagerForm form = new ConfigManagerForm(Details.TestSystemName, Details.TestSystemVersion, true, configManagers))
                if (form.ShowDialog() == DialogResult.OK)
                    foreach (IConfigManager modifiedConfig in form.ModifiedConfigs)
                        modifiedConfig.Save();
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
        /// Saves the specified <see cref="ITestResults"/> as xml and csv to the TsdLib.SpecialFolders location.
        /// </summary>
        /// <param name="results">The <see cref="ITestResults"/> that was captured by the test sequence.</param>
        protected virtual void SaveResults(ITestResults results)
        {
            results.SaveXml();
            string csvResultsFile = results.SaveCsv();
            
            Trace.WriteLine("CSV results saved to " + csvResultsFile);
        }

        /// <summary>
        /// Override to published the specified <see cref="ITestResults"/> to a database or user-defined location.
        /// </summary>
        /// <param name="results">The <see cref="ITestResults"/> that was captured by the test sequence.</param>
        protected virtual void PublishResults(ITestResults results)
        {

        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.UI.ITestSequenceControl.AbortTestSequence"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IView"/> that raised the event.</param>
        /// <param name="e">Empty event args.</param>
        protected virtual void AbortTestSequence(object sender, EventArgs e)
        {
            if (_activeSequence != null)
                _activeSequence.CancellationManager.Abort();
        }

        /// <summary>
        /// Generates a sequence of <see cref="CodeCompileUnit"/> objects to be dynamically compiled for the test sequence. Not used when the -localDomain command-line switch is used.
        /// </summary>
        /// <returns>A sequence of <see cref="CodeCompileUnit"/> objects.</returns>
        protected virtual IEnumerable<CodeCompileUnit> GenerateAdditionalCodeCompileUnits(string nameSpace)
        {
            return new CodeCompileUnit[0];
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.UI.IView.UIClosing"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IView"/> that raised the event.</param>
        /// <param name="e">A <see cref="CancelEventArgs"/> object to provide an opportunity to cancel the closing operation.</param>
        protected virtual void UIClosing(object sender, CancelEventArgs e)
        {
            if (LoggingTasks.Count > 0)
            {
                Trace.WriteLine(string.Format("There are currently {0} test result logging operations in progress", LoggingTasks.Count));
                //Currently, the logging operations are run on background threads, so there is no need to cancel the close or wait for the tasks to complete

                //Option 1: This will cancel the close and keep the UI open
                //e.Cancel = true;

                //Option 2: This will keep the application open until all logging is complete.
                //Task.WaitAll(LoggingTasks.ToArray());
            }
        }
    }
}
