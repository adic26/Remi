using System;
using System.CodeDom;
using System.Collections.Generic;
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
using TsdLib.Configuration.Details;
using TsdLib.Configuration.Management;
using TsdLib.Configuration.Null;
using TsdLib.Configuration.TestCases;
using TsdLib.Forms;
using TsdLib.Measurements;
using TsdLib.Observer;
using TsdLib.TestSystem.TestSequence;
using TsdLib.UI;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Contains base functionality for the system controller without station/product/test configuration.
    /// </summary>
    /// <typeparam name="TView">Type of the derived user interface.</typeparam>
    public abstract class ControllerBase<TView> : ControllerBase<TView, NullStationConfig, NullProductConfig, NullTestConfig>
        where TView : IView, new()
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
    /// <typeparam name="TStationConfig">Type of the derived station config.</typeparam>
    /// <typeparam name="TProductConfig">Type of the derived product config.</typeparam>
    /// <typeparam name="TTestConfig">Type of the derived test config.</typeparam>
    public abstract class ControllerBase<TView, TStationConfig, TProductConfig, TTestConfig> : MarshalByRefObject
        where TView : IView, new()
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
        where TTestConfig : TestConfigCommon, new()
    {
        private ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig> _activeSequence;
        private readonly bool _localDomain;
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
            var textWriterTraceListener = new TextWriterTraceListener(SpecialFolders.GetTraceLogs(Details.SafeTestSystemName));
            Trace.Listeners.Add(textWriterTraceListener);
            
            _testCaseProvider = new TestCaseProvider(Details.SafeTestSystemName);

            configManagerProvider = new ConfigManagerProvider(Details, configConnection);
            //TODO: if local domain, config manager should just reflect the entry assembly to get baked-in sequences and make Sequence read-only in editor form

            //set up view
            UI = new TView();
            if (UI.TraceListenerControl != null)
                Trace.Listeners.Add(UI.TraceListenerControl.Listener);
            UI.SetTitle(Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode);
            
            configManagerProvider.Reload();
            updateUIConfigControl();

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
                UI.ConfigControl.ViewEditConfiguration += ConfigControl_EditConfiguration;
            if (UI.TestDetailsControl != null)
                UI.TestDetailsControl.EditTestDetails += TestDetailsControl_EditTestDetails;
            if (UI.TestSequenceControl != null)
            {
                UI.TestSequenceControl.ExecuteTestSequence += TestSequenceControl_ExecuteTestSequence;
                UI.TestSequenceControl.AbortTestSequence += TestSequenceControl_AbortTestSequence;
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

        private ITestDetailsEditor _testDetailsEditor;
        private ITestDetailsEditor testDetailsEditor
        {
            get
            {
                if (_testDetailsEditor == null)
                {
                    _testDetailsEditor = CreateTestDetailsEditor();
                    _testDetailsEditor.IdentityManager.TestSystemIdentityChanged += TestSystemIdentityChanged;
                }
                return _testDetailsEditor;
            }
        }

        private IConfigEditor _configEditor;
        private IConfigEditor configEditor
        {
            get
            {
                if (_configEditor == null)
                {
                    _configEditor = CreateConfigEditor();
                    _configEditor.IdentityManager.TestSystemIdentityChanged += TestSystemIdentityChanged;
                }
                return _configEditor;
            }
        }

        protected virtual void TestSystemIdentityChanged(object sender, EventArgs e)
        {
            UI.SetTitle(Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode);
            configManagerProvider.Reload();
            if (UI.ConfigControl != null)
                updateUIConfigControl();
        }

        protected virtual void TestDetailsControl_EditTestDetails(object sender, bool detailsFromDatabase)
        {
            testDetailsEditor.Edit(Details, detailsFromDatabase);

        }

        void TestCaseControl_TestCaseSelected(object sender, string selectedTestCaseName)
        {
            IEnumerable<ITestCase> testCases = _testCaseProvider.Load();
            ITestCase selected = testCases.FirstOrDefault(tc => tc.Name == selectedTestCaseName);
            if (selected != null)
            {
                UI.ConfigControl.SelectedTestConfig = UI.ConfigControl.TestConfigManager.GetConfigGroup().Where(cfg => selected.TestConfigs.Contains(cfg.Name)).ToArray();
                UI.ConfigControl.SelectedSequenceConfig = UI.ConfigControl.SequenceConfigManager.GetConfigGroup().Where(cfg => selected.Sequences.Contains(cfg.Name)).ToArray();
            }
        }

        void TestCaseControl_TestCaseSaved(object sender, EventArgs e)
        {
            string testCaseName;
            using (ConfigItemCreateForm form = new ConfigItemCreateForm(false))
                testCaseName = form.ShowDialog() == DialogResult.OK ? form.ConfigItemName : "N/A";

            TestCase testCase = new TestCase(testCaseName, UI.ConfigControl.SelectedTestConfig, UI.ConfigControl.SelectedSequenceConfig);
            _testCaseProvider.Save(testCase);
        }

        private void updateUIConfigControl()
        {
            UI.ConfigControl.StationConfigManager = configManagerProvider.GetConfigManager<TStationConfig>();
            UI.ConfigControl.ProductConfigManager = configManagerProvider.GetConfigManager<TProductConfig>();
            UI.ConfigControl.TestConfigManager = configManagerProvider.GetConfigManager<TTestConfig>();
            UI.ConfigControl.SequenceConfigManager = configManagerProvider.GetConfigManager<SequenceConfigCommon>();
        }

        /// <summary>
        /// Default handler for the ViewBase.ExecuteTestSequence event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the Execute Test Sequence button.</param>
        /// <param name="e">An emptry EventArgs object.</param>
        protected virtual async void TestSequenceControl_ExecuteTestSequence(object sender, EventArgs e)
        {
            AppDomain sequenceDomain = null;

            UI.SetState(State.TestStarting);

            Trace.WriteLine(Details);

            var sequenceConfigs = UI.ConfigControl.SelectedSequenceConfig.Cast<SequenceConfigCommon>().ToArray();
            if (!sequenceConfigs.Any())
                sequenceConfigs = configManagerProvider.GetConfigManager<SequenceConfigCommon>().GetConfigGroup().ToArray();
            bool publishResults = UI.TestSequenceControl.PublishResults;

            Trace.WriteLine(string.Format("Using {0} application domain", _localDomain ? "local" : "remote"));

            foreach (SequenceConfigCommon sequenceConfig in sequenceConfigs)
            {
                try
                {
                    DateTime startTime = DateTime.Now;

                    SequenceConfigCommon config = sequenceConfig;

                    ControllerProxy controllerProxy;
                    if (_localDomain)
                    {
                        _activeSequence = CreateSequenceObject(config.FullTypeName);
                        controllerProxy = CreateControllerProxy(UI, _activeSequence.CancellationManager);
                    }
                    else
                    {
                        List<CodeCompileUnit> codeCompileUnits = new List<CodeCompileUnit> { new BasicCodeParser(config.AssemblyReferences.ToArray()).Parse(new StringReader(config.SourceCode)) };

                        IEnumerable<CodeCompileUnit> additionalCodeCompileUnits = GenerateAdditionalCodeCompileUnits(config.Namespace.Replace(".Sequences", ""));

                        DynamicCompiler generator = new DynamicCompiler(Language.CSharp, AppDomain.CurrentDomain.BaseDirectory);
                        string sequenceAssembly = generator.Compile(codeCompileUnits.Concat(additionalCodeCompileUnits));

                        sequenceDomain = AppDomain.CreateDomain("Sequence Domain");

                        _activeSequence = CreateSequenceObject(config.FullTypeName, sequenceAssembly, sequenceDomain);
                        controllerProxy = CreateControllerProxy(UI, _activeSequence.CancellationManager);

                        foreach (TraceListener listener in Trace.Listeners)
                            _activeSequence.AddTraceListener(listener);
                    }
                    
                    //TStationConfig stationConfig = (TStationConfig)(UI.ConfigControl.SelectedStationConfig.FirstOrDefault() ?? configManagerProvider.GetConfigManager<TStationConfig>().GetList()[0]);
                    TStationConfig stationConfig = (TStationConfig)(UI.ConfigControl.SelectedStationConfig.FirstOrDefault() ?? configManagerProvider.GetConfigManager<TStationConfig>().GetConfigGroup().First());
                    TProductConfig productConfig = (TProductConfig)(UI.ConfigControl.SelectedProductConfig.FirstOrDefault() ?? configManagerProvider.GetConfigManager<TProductConfig>().GetConfigGroup().First());
                    TTestConfig[] testConfigs = UI.ConfigControl.SelectedTestConfig.Cast<TTestConfig>().ToArray();
                    if (!testConfigs.Any())
                        testConfigs = configManagerProvider.GetConfigManager<TTestConfig>().GetConfigGroup().ToArray();

                    _activeSequence.Subscribe((IObserver<DataContainer>)controllerProxy);
                    _activeSequence.Subscribe((IObserver<IMeasurement>)controllerProxy);
                    _activeSequence.Subscribe((IObserver<ITestInfo>)controllerProxy);
                    _activeSequence.Subscribe((IObserver<Tuple<int, int>>)controllerProxy);

                    _activeSequence.Config = configManagerProvider;

                    //TODO: ExecuteSequenceAsync
                    await Task.Run(() => _activeSequence.ExecuteSequence(stationConfig, productConfig, testConfigs));

                    IResultHandler resultHandler = CreateResultHandler(Details);
                    if (resultHandler != null)
                        resultHandler.SaveResults(_activeSequence.TestInfo.ToArray(), _activeSequence.Measurements.ToArray(), startTime, DateTime.Now, publishResults);
                }
                //TODO: simplify by using AggregateException?
                catch (OperationCanceledException) //User cancellation and controller proxy errors are propagated as OperationCancelledException
                //TODO: controller proxy errors shouldn't be propagated as OperationCancelledException
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
                    CreateErrorHandler().HandleError(ex, sequenceConfig.Name);
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

        //TODO: abstract to ConfigEditor class - allow changing the config editor without changing the ControllerBase, expose a virtual CreateConfigEditor to allow client to redefine
        /// <summary>
        /// Default handler for the ViewBase.ViewEditConfiguration event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the View/Edit Configuration button.</param>
        /// <param name="configManagers">An array of <see cref="IConfigManager"/> objects containing the configuration data.</param>
        protected virtual void ConfigControl_EditConfiguration(object sender, IConfigManager[] configManagers)
        {
            configEditor.Edit(configManagerProvider);
            //if (UI.ConfigControl != null)
            //    updateUIConfigControl();
        }

        protected virtual IConfigEditor CreateConfigEditor()
        {
            return new ConfigEditor();
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.UI.ITestSequenceControl.AbortTestSequence"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IView"/> that raised the event.</param>
        /// <param name="e">Empty event args.</param>
        protected virtual void TestSequenceControl_AbortTestSequence(object sender, EventArgs e)
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



        protected virtual ControllerProxy CreateControllerProxy(IView view, ICancellationManager testSequenceCancellationManager)
        {
            return new ControllerProxy(view, testSequenceCancellationManager);
        }

        protected virtual ControllerProxy CreateControllerProxy(IView view, ICancellationManager testSequenceCancellationManager, AppDomain appDomain)
        {
            return (ControllerProxy)appDomain.CreateInstanceAndUnwrap(typeof(ControllerProxy).Assembly.FullName, typeof(ControllerProxy).FullName, false, BindingFlags.CreateInstance, null, new object[] { view, testSequenceCancellationManager }, CultureInfo.CurrentCulture, null);
        }

        protected virtual ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig> CreateSequenceObject(string sequenceName)
        {
            return (ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig>)Activator.CreateInstance(Assembly.GetEntryAssembly().GetType(sequenceName));
        }

        protected virtual ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig> CreateSequenceObject(string sequenceName, string sequenceAssembly, AppDomain appDomain)
        {
            return (ConfigurableTestSequence<TStationConfig, TProductConfig, TTestConfig>)appDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, sequenceName);
        }

        protected virtual IResultHandler CreateResultHandler(ITestDetails testDetails)
        {
            return new ResultHandler(testDetails);
        }
        /// <summary>
        /// Override to customize the mechanism to view/edit test details.
        /// </summary>
        /// <returns>An <see cref="ITestDetailsEditor"/> object used to view/modify the test details.</returns>
        protected virtual ITestDetailsEditor CreateTestDetailsEditor()
        {
            return new TestDetailsEditor();
        }

        /// <summary>
        /// Override to customize the mechanism to handle errors encountered during the test sequence.
        /// </summary>
        /// <returns>An <see cref="IErrorHandler"/> object used to catch, handle, display and/or log errors.</returns>
        protected virtual IErrorHandler CreateErrorHandler()
        {
            return new ErrorHandler();
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.UI.IView.UIClosing"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IView"/> that raised the event.</param>
        /// <param name="e">A <see cref="CancelEventArgs"/> object to provide an opportunity to cancel the closing operation.</param>
        protected virtual void UIClosing(object sender, CancelEventArgs e)
        {
            //if (LoggingTasks.Count > 0)
            //{
            //    Trace.WriteLine(string.Format("There are currently {0} test result logging operations in progress", LoggingTasks.Count));
            //    Currently, the logging operations are run on foreground worker threads, so there is no need to cancel the close or wait for the tasks to complete

            //    Option 1: This will cancel the close and keep the UI open
            //    e.Cancel = true;

            //    Option 2: This will keep the application open until all logging is complete.
            //    Task.WaitAll(LoggingTasks.ToArray());
            //}
        }
    }
}
