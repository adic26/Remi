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
using TsdLib.Configuration;
using TsdLib.Measurements;
using TsdLib.TestSystem.CodeGenerator;
using TsdLib.TestSystem.TestSequence;
using TsdLib.UI;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Contains base functionality for the system controller.
    /// </summary>
    /// <typeparam name="TView">Type of the derived user interface.</typeparam>
    /// <typeparam name="TStationConfig">Type of the derived station config.</typeparam>
    /// <typeparam name="TProductConfig">Type of the derived product config.</typeparam>
    /// <typeparam name="TTestConfig">Type of the derived test config.</typeparam>
    public abstract class ControllerBase<TView, TStationConfig, TProductConfig, TTestConfig> : MarshalByRefObject
        where TView : IView, new()
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
        where TTestConfig : TestConfigCommon, new()
    {
        #region Private fields

        private readonly bool _localDomain;
        TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> _sequence;
        private readonly List<Task> _loggingTasks; 

        #endregion

        #region Public and protected properties

        /// <summary>
        /// Gets a reference to the user interface.
        /// </summary>
        public TView UI { get; private set; }

        /// <summary>
        /// Gets or sets the metadata describing the test request.
        /// </summary>
        protected TestDetails Details { get; private set; }

        /// <summary>
        /// Gets a configuration manager for station config.
        /// </summary>
        protected IConfigManager StationConfigManager { get; private set; }
        /// <summary>
        /// Gets a configuration manager for product config.
        /// </summary>
        protected IConfigManager ProductConfigManager { get; private set; }
        /// <summary>
        /// Gets a configuration manager for test config.
        /// </summary>
        protected IConfigManager TestConfigManager { get; private set; }
        /// <summary>
        /// Gets a configuration manager for sequence config.
        /// </summary>
        protected IConfigManager SequenceConfigManager { get; private set; }
        /// <summary>
        /// Gets a list of active tasks responsible for logging test results.
        /// </summary>
        protected ReadOnlyCollection<Task> LoggingTasks { get; private set; }

        #endregion

        /// <summary>
        /// Initialize a new system controller.
        /// </summary>
        /// <param name="testDetails">A <see cref="Details"/> object containing metadata describing the test request.</param>
        /// <param name="configConnection">An <see cref="IConfigConnection"/> object to handle configuration persistence with a database.</param>
        /// <param name="localDomain">True to execute the test sequence in the local application domain. Only available in Debug configuration.</param>
        protected ControllerBase(TestDetails testDetails, IConfigConnection configConnection, bool localDomain)
        {
            Thread.CurrentThread.Name = "UI Thread";
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(SpecialFolders.TraceLogs));

            _loggingTasks = new List<Task>();
            LoggingTasks = new ReadOnlyCollection<Task>(_loggingTasks);

            Details = testDetails;

            StationConfigManager = ConfigManager<TStationConfig>.GetConfigManager(Details, configConnection);
            ProductConfigManager = ConfigManager<TProductConfig>.GetConfigManager(Details, configConnection);
            TestConfigManager = ConfigManager<TTestConfig>.GetConfigManager(Details, configConnection);
            SequenceConfigManager = ConfigManager<Sequence>.GetConfigManager(Details, configConnection);

            //set up view
            UI = new TView { Title = Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode };

            testDetails.TestSystemIdentityChanged += testDetails_TestSystemIdentityChanged;
            testDetails_TestSystemIdentityChanged(this, "Initialization");

#if DEBUG
            Trace.WriteLine("Using TsdLib debug assembly. Test results will only be stored as Analysis.");
            _localDomain = localDomain;
#else
            if (localDomain)
                Trace.WriteLine("Operating in release mode - ignoring localDomain command-line switch. Test sequence will be executed in the remote application domain.");
            _localDomain = false;
#endif

            //subscribe to view events
            UI.ConfigControl.ViewEditConfiguration += EditConfigurationNew;
            UI.TestDetailsControl.EditTestDetails += EditTestDetails;
            UI.TestSequenceControl.ExecuteTestSequence += ExecuteTestSequence;
            UI.TestSequenceControl.AbortTestSequence += AbortTestSequence;
            UI.UIClosing += UIClosing;
        }

        void testDetails_TestSystemIdentityChanged(object sender, string e)
        {
            UI.Title = Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode;
            UI.ConfigControl.StationConfigManager = StationConfigManager.Reload();
            UI.ConfigControl.ProductConfigManager = ProductConfigManager.Reload();
            UI.ConfigControl.TestConfigManager = TestConfigManager.Reload();
            UI.ConfigControl.SequenceConfigManager = SequenceConfigManager.Reload();
        }

        /// <summary>
        /// Default handler for the ViewBase.ExecuteTestSequence event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the Execute Test Sequence button.</param>
        /// <param name="e">EventArgs containing the product, station, test and sequence configuration objects.</param>
        protected async void ExecuteTestSequence(object sender, TestSequenceEventArgs e)
        {
            AppDomain sequenceDomain = null;

            try
            {
                UI.SetState(State.TestInProgress);

                TStationConfig stationConfig = (TStationConfig)e.StationConfig.FirstOrDefault();
                TProductConfig productConfig = (TProductConfig)e.ProductConfig.FirstOrDefault();
                IEnumerable<TTestConfig> testConfigs = e.TestConfig.Cast<TTestConfig>();
                IEnumerable<Sequence> sequenceConfigs = e.SequenceConfig.Cast<Sequence>();
                bool publishResults = e.PublishResults;

                Trace.WriteLine(string.Format("Using {0} application domain", _localDomain ? "local" : "remote"));

                SynchronizationContext uiContext = SynchronizationContext.Current;

                ControllerProxy controllerProxy;

                foreach (Sequence sequenceConfig in sequenceConfigs)
                {
                    DateTime startTime = DateTime.Now;

                    Sequence config = sequenceConfig;
                    await Task.Run(() =>
                    {
                        if (_localDomain)
                        {
                            _sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>) Activator.CreateInstance(Assembly.GetEntryAssembly().GetType(Assembly.GetEntryAssembly().GetName().Name + ".Sequences" + "." + config.Name));
                            controllerProxy = (ControllerProxy) Activator.CreateInstance(typeof (ControllerProxy), BindingFlags.CreateInstance, null, new object[] {UI, _sequence}, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            List<CodeCompileUnit> codeCompileUnits = new List<CodeCompileUnit> {new BasicCodeParser(config.AssemblyReferences.ToArray()).Parse(new StringReader(config.SourceCode))};

                            IEnumerable<CodeCompileUnit> additionalCodeCompileUnits = GenerateCodeCompileUnits();

                            DynamicCompiler generator = new DynamicCompiler(Language.CSharp);
                            string sequenceAssembly = generator.Compile(codeCompileUnits.Concat(additionalCodeCompileUnits));

                            sequenceDomain = AppDomain.CreateDomain("Sequence Domain");

                            _sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>) sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, Details.SafeTestSystemName + ".Sequences" + "." + config.Name);
                            controllerProxy = (ControllerProxy) sequenceDomain.CreateInstanceAndUnwrap(typeof (ControllerProxy).Assembly.FullName, typeof (ControllerProxy).FullName, false, BindingFlags.CreateInstance, null, new object[] {UI, _sequence}, CultureInfo.CurrentCulture, null);

                            _sequence.AddTraceListener(UI.TraceListenerControl.Listener);
                        }

                        EventProxy<TestInfo> infoEventHandler = new EventProxy<TestInfo>();
                        _sequence.InfoEventProxy = infoEventHandler;
                        infoEventHandler.Attach(controllerProxy.InfoAdded, uiContext);

                        EventProxy<MeasurementBase> measurementEventHandler = new EventProxy<MeasurementBase>();
                        _sequence.MeasurementEventProxy = measurementEventHandler;
                        measurementEventHandler.Attach(controllerProxy.MeasurementAdded, uiContext);

                        EventProxy<object> dataEventHandler = new EventProxy<object>();
                        _sequence.DataEventProxy = dataEventHandler;
                        dataEventHandler.Attach(controllerProxy.DataAdded, uiContext);

                        _sequence.ExecuteSequence(Details, stationConfig, productConfig, testConfigs.ToArray());
                    });
                    DateTime endTime = DateTime.Now;

                    bool overallPass = _sequence.Measurements.Any() && _sequence.Measurements.All(m => m.Result == MeasurementResult.Pass);

                    ITestResults testResults = MeasurementsFactory.CreateTestResults(Details, _sequence.Measurements, overallPass ? "Pass" : "Fail", startTime, endTime, _sequence.TestInfo);

                    _loggingTasks.Add(Task.Run(() =>
                    {
                        ITestResults localTestResults = testResults; //Capture the test results to a local variable in case they are overwritten by the next sequence before the logging is complete.
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
                    }));
                }
                UI.SetState(State.ReadyToTest);
            }
            catch (OperationCanceledException) //User cancellation and controller proxy errors are propagated as OperationCancelledException
            {
                UI.SetState(State.ReadyToTest);
                if (_sequence.CancelledByUser)
                    Trace.WriteLine("Test sequence was cancelled by user.");

                //TsdLibException came from controller proxy
                else if (_sequence.Error is TsdLibException)
                {
                    DialogResult result = MessageBox.Show(_sequence.Error.GetType().Name + Environment.NewLine + _sequence.Error.Message + Environment.NewLine + "Would you like to view help for this error?", "Error occurred in test sequence", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        Process.Start(_sequence.Error.HelpLink);
                }

                //Some other exception came from controller proxy
                else if (_sequence.Error != null)
                    MessageBox.Show(_sequence.Error.GetType().Name + Environment.NewLine + _sequence.Error + Environment.NewLine + "This error was unexpected and not handled by the TsdLib Application. Please contact TSD for support.", "Unexpected error occurred in test sequence");

                //Test test sequence did not set the Error property - this should never happen
                else
                    MessageBox.Show("An undescribed error has occurred and was not reported by the test sequence. Please contact TSD for support.", "Unknown Error:");
            }
            catch (TsdLibException ex)
            {
                UI.SetState(State.ReadyToTest);
                DialogResult result = MessageBox.Show(ex.GetType().Name + Environment.NewLine + ex.Message + Environment.NewLine + "Would you like to view help for this error?", "Error occurred in test sequence", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Process.Start(ex.HelpLink);
            }
            catch (Exception ex)
            {
                UI.SetState(State.ReadyToTest);
                MessageBox.Show(ex.GetType().Name + Environment.NewLine + ex + Environment.NewLine + "This error was unexpected and not handled by the TsdLib Application. Please contact TSD for support.", "Unexpected error occurred in test sequence");
            }
            finally
            {
                if (_sequence != null)
                {
                    _sequence.Dispose();
                    _sequence = null;
                }
                if (sequenceDomain != null)
                    AppDomain.Unload(sequenceDomain);
            }
        }
        
        #region Virtual Methods

        /// <summary>
        /// Default handler for the ViewBase.ViewEditConfiguration event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the View/Edit Configuration button.</param>
        /// <param name="e">Empty EventArgs object.</param>
        protected virtual void EditConfiguration(object sender, EventArgs e)
        {
            using (ConfigManagerForm form = new ConfigManagerForm(Details.TestSystemName, Details.TestSystemVersion, true, new []{ StationConfigManager, ProductConfigManager, TestConfigManager, SequenceConfigManager}))
                if (form.ShowDialog() == DialogResult.OK)
                    foreach (IConfigManager modifiedConfig in form.ModifiedConfigs)
                        modifiedConfig.Save();
        }

        /// <summary>
        /// Default handler for the ViewBase.ViewEditConfiguration event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the View/Edit Configuration button.</param>
        /// <param name="configManagers">An array of <see cref="IConfigManager"/> objects containing the configuration data.</param>
        protected virtual void EditConfigurationNew(object sender, IConfigManager[] configManagers)
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
            DirectoryInfo resultsDirectory = SpecialFolders.GetResultsFolder(results.Details.TestSystemName);

            string xmlResultsFile = results.SaveXml(resultsDirectory);
            string csvResultsFile = results.SaveCsv(resultsDirectory);
            
            Trace.WriteLine("XML results saved to " + xmlResultsFile);
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
            if (_sequence != null)
                _sequence.Abort();
        }

        /// <summary>
        /// Generates a sequence of <see cref="CodeCompileUnit"/> objects to be dynamically compiled for the test sequence. Not used when the -localDomain command-line switch is used.
        /// </summary>
        /// <returns>A sequence of <see cref="CodeCompileUnit"/> objects.</returns>
        protected virtual IEnumerable<CodeCompileUnit> GenerateCodeCompileUnits()
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
                Trace.WriteLine("There are currently {0} test result logging operations in progress");
                //Currently, the logging operations are run on background threads, so there is no need to cancel the close or wait for the tasks to complete

                //Option 1: This will cancel the close and keep the UI open
                //e.Cancel = true;

                //Option 2: This will keep the application open until all logging is complete.
                //Task.WaitAll(LoggingTasks.ToArray());
            }
        }

        #endregion

    }
}
