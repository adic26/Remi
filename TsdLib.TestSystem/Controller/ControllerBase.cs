using System;
using System.CodeDom;
using System.Collections.Generic;
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

        private CancellationTokenSource _tokenSource;
        private readonly bool _localDomain;



        #endregion

        #region Public and protected properties

        /// <summary>
        /// Gets a reference to the user interface.
        /// </summary>
        public TView View { get; private set; }

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

        #endregion

        #region Constructor
        
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

            Details = testDetails;
            testDetails.TestSystemIdentityChanged += testDetails_TestSystemIdentityChanged;

            //_StationConfigManager = new ConfigManager<TStationConfig>(Details, configConnection);
            //_ProductConfigManager = new ConfigManager<TProductConfig>(Details, configConnection);
            //_TestConfigManager = new ConfigManager<TTestConfig>(Details, configConnection);
            //_SequenceConfigManager = new ConfigManager<Sequence>(Details, configConnection);

            StationConfigManager = ConfigManager<TStationConfig>.GetConfigManager(Details, configConnection);
            ProductConfigManager = ConfigManager<TProductConfig>.GetConfigManager(Details, configConnection);
            TestConfigManager = ConfigManager<TTestConfig>.GetConfigManager(Details, configConnection);
            SequenceConfigManager = ConfigManager<Sequence>.GetConfigManager(Details, configConnection);

            //set up view
            View = new TView {Text = Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode};
            PushConfigListsToView();

#if DEBUG
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

        void testDetails_TestSystemIdentityChanged(object sender, string e)
        {
            View.Text = Details.TestSystemName + " v." + Details.TestSystemVersion + " " + Details.TestSystemMode;
            PushConfigListsToView();
        }

        #endregion

        /// <summary>
        /// Default handler for the ViewBase.ExecuteTestSequence event.
        /// </summary>
        /// <param name="sender">Object that raised the exception. Should be a reference to the Execute Test Sequence button.</param>
        /// <param name="e">EventArgs containing the product, station, test and sequence configuration objects.</param>
        protected virtual async void ExecuteTestSequence(object sender, TestSequenceEventArgs e)
        {
            AppDomain sequenceDomain = null;
            TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> sequence;

            try
            {
                View.SetState(State.TestInProgress);

                TStationConfig stationConfig = (TStationConfig)e.StationConfig;
                TProductConfig productConfig = (TProductConfig)e.ProductConfig;
                TTestConfig testConfig = (TTestConfig)e.TestConfig;
                Sequence sequenceConfig = (Sequence)e.SequenceConfig;
                bool publishResults = e.PublishResults;

                _tokenSource = new CancellationTokenSource();

                Trace.WriteLine(string.Format("Using {0} application domain", _localDomain ? "local" : "remote"));

                SynchronizationContext uiContext = SynchronizationContext.Current;

                ITestResults results = await Task.Run(() =>
                {
                    Thread.CurrentThread.Name = "Sequence Thread";

                    ControllerProxy controllerProxy;

                    if (_localDomain)
                    {
                        Type sequenceType = Assembly.GetEntryAssembly().GetType(Assembly.GetEntryAssembly().GetName().Name + ".Sequences" + "." + sequenceConfig.Name);
                        sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>)Activator.CreateInstance(sequenceType);
                        controllerProxy = (ControllerProxy)Activator.CreateInstance(typeof(ControllerProxy), BindingFlags.CreateInstance, null, new object[] { View }, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        List<CodeCompileUnit> codeCompileUnits = new List<CodeCompileUnit> { new BasicCodeParser(sequenceConfig.AssemblyReferences.ToArray()).Parse(new StringReader(sequenceConfig.SourceCode)) };

                        IEnumerable<CodeCompileUnit> additionalCodeCompileUnits = GenerateCodeCompileUnits();

                        DynamicCompiler generator = new DynamicCompiler(Language.CSharp);
                        string sequenceAssembly = generator.Compile(codeCompileUnits.Concat(additionalCodeCompileUnits));

                        sequenceDomain = AppDomain.CreateDomain("Sequence Domain");

                        sequence = (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>)sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, Details.SafeTestSystemName + ".Sequences" + "." + sequenceConfig.Name);
                        controllerProxy = (ControllerProxy)sequenceDomain.CreateInstanceAndUnwrap(typeof(ControllerProxy).Assembly.FullName, typeof(ControllerProxy).FullName, false, BindingFlags.CreateInstance, null, new object[] { View }, CultureInfo.CurrentCulture, null);

                        sequence.AddTraceListener(View.Listener);
                    }

                    sequence.UIContext = uiContext;

                    //sequence.InfoEventProxy = new EventProxy<TestInfo>();
                    //sequence.InfoEventProxy.Attach(controllerProxy.InfoAdded, uiContext);
                    View.TestInfoList = sequence._testInfo;

                    //sequence.MeasurementEventProxy = new EventProxy<MeasurementBase>();
                    //sequence.MeasurementEventProxy.Attach(controllerProxy.MeasurementAdded, uiContext);
                    View.MeasurementList = sequence._measurements;

                    sequence.DataEventProxy = new EventProxy<object>();
                    sequence.DataEventProxy.Attach(controllerProxy.DataAdded, uiContext);

                    _tokenSource = new CancellationTokenSource();
                    _tokenSource.Token.Register(sequence.Abort);

                    return sequence.ExecuteSequence(stationConfig, productConfig, testConfig, Details);
                });


                View.SetState(State.ReadyToTest);

                await Task.Run(() =>
                {
                    Thread.CurrentThread.Name = "Result Handler Thread";
                    Thread.CurrentThread.IsBackground = false;
                    SaveResults(results);
                    if (publishResults)
                        PublishResults(results);
                });
            }
            catch (OperationCanceledException)
            {
                View.SetState(State.ReadyToTest);
                Trace.WriteLine("Test sequence was cancelled by user.");
            }
            catch (TsdLibException ex)
            {
                View.SetState(State.ReadyToTest);
                DialogResult result = MessageBox.Show("Error details:" + Environment.NewLine + ex.Message + Environment.NewLine + "Would you like to view help for this error?", ex.GetType().Name, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Process.Start(ex.HelpLink);
            }
            catch (Exception ex)
            {
                View.SetState(State.ReadyToTest);
                MessageBox.Show("Error details:" + Environment.NewLine + ex + Environment.NewLine + "This error was unexpected and not handled by the TsdLib Application. Please contact TSD for support.", ex.GetType().Name);
            }
            finally
            {
                if (sequenceDomain != null)
                    AppDomain.Unload(sequenceDomain);

                if (_tokenSource != null)
                {
                    _tokenSource.Dispose();
                    _tokenSource = null;
                }
            }
        }
        
        #region Virtual Methods

        /// <summary>
        /// Updates the configuration lists on the user interface.
        /// </summary>
        protected virtual void PushConfigListsToView()
        {
            View.StationConfigList = StationConfigManager.GetList(true);
            View.ProductConfigList = ProductConfigManager.GetList(true);
            View.TestConfigList = TestConfigManager.GetList(true);
            View.SequenceConfigList = SequenceConfigManager.GetList(true);
        }

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
            DirectoryInfo resultsDirectory = SpecialFolders.GetResultsFolder(results.TestSystemName);

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

        ///// <summary>
        ///// Default handler for the <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.InfoEventProxy"/>. Calls <see cref="IView.AddInformation"/>.
        ///// </summary>
        ///// <param name="sender">The <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}"/> where the information was captured.</param>
        ///// <param name="testInfo">The <see cref="TestInfo"/> that was captured.</param>
        //protected internal virtual void InfoAdded(object sender, TestInfo testInfo)
        //{
        //    View.AddInformation(testInfo);
        //}

        ///// <summary>
        ///// Default handler for the <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.MeasurementEventProxy"/>. Calls <see cref="IView.AddMeasurement"/>.
        ///// </summary>
        ///// <param name="sender">The <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}"/> where the measurement was captured.</param>
        ///// <param name="measurementBase">The <see cref="MeasurementBase"/> that was captured.</param>
        //protected internal virtual void MeasurementAdded(object sender, MeasurementBase measurementBase)
        //{
        //    throw new Exception();
        //    View.AddMeasurement(measurementBase);
        //}

        ///// <summary>
        ///// Default handler for the <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.DataEventProxy"/>. Calls <see cref="IView.AddData"/>.
        ///// </summary>
        ///// <param name="sender">The <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}"/> where the measurement was captured.</param>
        ///// <param name="data">The <see cref="Data"/> that was captured.</param>
        //protected internal virtual void DataAdded(object sender, Data data)
        //{
        //    View.AddData(data);
        //}

        /// <summary>
        /// Default handler for the <see cref="IView.AbortTestSequence"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IView"/> that raised the event.</param>
        /// <param name="e">Empty EventArgs object.</param>
        protected virtual void AbortTestSequence(object sender, EventArgs e)
        {
            if (_tokenSource != null)
                _tokenSource.Cancel();
        }

        /// <summary>
        /// Generates a sequence of <see cref="CodeCompileUnit"/> objects to be dynamically compiled for the test sequence. Not used when the -localDomain command-line switch is used.
        /// </summary>
        /// <returns>A sequence of <see cref="CodeCompileUnit"/> objects.</returns>
        protected virtual IEnumerable<CodeCompileUnit> GenerateCodeCompileUnits()
        {
            return new CodeCompileUnit[0];
        }

        #endregion

    }
}
