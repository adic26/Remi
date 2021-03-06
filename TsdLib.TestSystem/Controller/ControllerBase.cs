﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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



            //TODO: if local domain, config manager should just reflect the entry assembly to get baked-in sequences and make Sequence read-only in editor form

            //set up view
            UI = new TView();
            if (UI.TraceListenerControl != null)
                Trace.Listeners.Add(UI.TraceListenerControl.Listener);
            UI.SetTitle(Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode);

            configManagerProvider = new ConfigManagerProvider(Details, configConnection);
            UI.ConfigControl.StationConfigManager = configManagerProvider.GetConfigManager<TStationConfig>();
            UI.ConfigControl.ProductConfigManager = configManagerProvider.GetConfigManager<TProductConfig>();
            UI.ConfigControl.TestConfigManager = configManagerProvider.GetConfigManager<TTestConfig>();
            UI.ConfigControl.SequenceConfigManager = configManagerProvider.GetConfigManager<SequenceConfigCommon>();
#if DEBUG
            Trace.WriteLine("Using TsdLib debug assembly.");
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
            using (ConfigItemCreateForm form = new ConfigItemCreateForm(false))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    TestCase testCase = new TestCase(form.ConfigItemName, UI.ConfigControl.SelectedTestConfig, UI.ConfigControl.SelectedSequenceConfig);
                    _testCaseProvider.Save(testCase);
                    UI.TestCaseControl.DisplayTestCases(_testCaseProvider.Load());
                }
            }
        }

        /// <summary>
        /// Default handler for the ViewBase.ExecuteTestSequence event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the Execute Test Sequence button.</param>
        /// <param name="e">An emptry EventArgs object.</param>
        protected virtual async void TestSequenceControl_ExecuteTestSequence(object sender, EventArgs e)
        {
            AppDomain sequenceDomain = null;

            SequenceConfigCommon[] sequenceConfigs = UI.ConfigControl.SelectedSequenceConfig.Cast<SequenceConfigCommon>().ToArray();
            if (!sequenceConfigs.Any())
            {
                Trace.WriteLine("No test sequence selected.");
                return;
            }

            bool publishResults = UI.TestSequenceControl.PublishResults;

            UI.SetState(State.TestStarting);

            Trace.WriteLine(Details);

            Trace.WriteLine(string.Format("Using {0} application domain", _localDomain ? "local" : "remote"));

            foreach (SequenceConfigCommon sequenceConfig in sequenceConfigs)
            {
                try
                {
                    IStationConfig stationConfig = UI.ConfigControl.SelectedStationConfig.FirstOrDefault();
                    if (stationConfig == null)
                        throw new NoConfigSelectedException(typeof(TStationConfig).Name);
                    IProductConfig productConfig = UI.ConfigControl.SelectedProductConfig.FirstOrDefault();
                    if (productConfig == null)
                        throw new NoConfigSelectedException(typeof(TProductConfig).Name);
                    TTestConfig[] testConfigs = UI.ConfigControl.SelectedTestConfig.Cast<TTestConfig>().ToArray();
                    if (!testConfigs.Any())
                        throw new NoConfigSelectedException(typeof(TTestConfig).Name);

                    DateTime startTime = DateTime.Now;

                    SequenceConfigCommon sequence = sequenceConfig;
                    EventManager eventManager = CreateEventManager(UI);

                    if (_localDomain)
                    {
                        _activeSequence = CreateSequenceObject(sequence.FullTypeName);
                    }
                    else
                    {
                        List<CodeCompileUnit> codeCompileUnits = new List<CodeCompileUnit> { new BasicCodeParser(sequence.AssemblyReferences.ToArray()).Parse(new StringReader(sequence.SourceCode)) };

                        IEnumerable<CodeCompileUnit> additionalCodeCompileUnits = GenerateAdditionalCodeCompileUnits(sequence.Namespace.Replace(".Sequences", ""));

                        DynamicCompiler generator = new DynamicCompiler(Language.CSharp, AppDomain.CurrentDomain.BaseDirectory);
                        string sequenceAssembly = generator.Compile(codeCompileUnits.Concat(additionalCodeCompileUnits));

                        sequenceDomain = AppDomain.CreateDomain("Sequence Domain");

                        _activeSequence = CreateSequenceObject(sequence.FullTypeName, sequenceAssembly, sequenceDomain);

                        _activeSequence.TraceOutput += eventManager.TraceOutput;
                    }
                    


                    
                    _activeSequence.DataAdded += eventManager.AddData;
                    _activeSequence.MeasurementAdded += eventManager.AddMeasurement;
                    _activeSequence.TestInfoAdded += eventManager.AddTestInfo;
                    _activeSequence.ProgressUpdated += eventManager.UpdateProgress;

                    _activeSequence.Config = configManagerProvider;

                    //TODO: ExecuteSequenceAsync
                    await Task.Run(() => _activeSequence.ExecuteSequence((TStationConfig) stationConfig, (TProductConfig) productConfig, testConfigs));

                    IResultHandler resultHandler = CreateResultHandler(Details);
                    if (resultHandler != null)
                        resultHandler.SaveResults(_activeSequence.TestInfo.ToArray(), _activeSequence.Measurements.ToArray(), startTime, DateTime.Now, publishResults);
                }
                catch (OperationCanceledException)
                {
                    if (_activeSequence.Error == null)
                        Trace.WriteLine("Test sequence was cancelled by user.");
                    else
                        Trace.WriteLine("Test sequence was cancelled due to error: " + _activeSequence.Error);
                }
                catch (Exception ex)
                {
                    if (!CreateErrorHandler().TryHandleError(ex, sequenceConfig.Name))
                        throw;
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
                        //Can't write to trace, since the listener on the other side may be unloaded
                        //Trace.WriteLine("AppDomain could not be unloaded." + Environment.NewLine + ex);
                    }
                    catch (Exception ex)
                    {

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
                _activeSequence.Abort();
        }

        /// <summary>
        /// Generates a sequence of <see cref="CodeCompileUnit"/> objects to be dynamically compiled for the test sequence. Not used when the -localDomain command-line switch is used.
        /// </summary>
        /// <returns>A sequence of <see cref="CodeCompileUnit"/> objects.</returns>
        protected virtual IEnumerable<CodeCompileUnit> GenerateAdditionalCodeCompileUnits(string nameSpace)
        {
            return new CodeCompileUnit[0];
        }

        protected virtual EventManager CreateEventManager(IView view)
        {
            return new EventManager(view);
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
}
