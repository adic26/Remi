﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TsdLib.CodeGenerator;
using TsdLib.Configuration;
using TsdLib.Proxies;
using TsdLib.TestSequence;
using TsdLib.View;
using TsdLib.TestResults;

namespace TsdLib.Controller
{//TODO: add string properties for application name and version - add to constructor
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
        #region Private Fields

        private readonly bool _devMode;
        private CancellationTokenSource _tokenSource;

        private readonly ConfigManager<TStationConfig> _stationConfigManager;
        private readonly ConfigManager<TProductConfig> _productConfigManager;
        private readonly ConfigManager<TTestConfig> _testConfigManager;
        private readonly ConfigManager<SequenceConfig> _sequenceConfigManager;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a reference to the user interface.
        /// </summary>
        public IView View { get; private set; }
        /// <summary>
        /// Gets the name of the client application. Also used as the client's namespace.
        /// </summary>
        public string ApplicationName { get; private set; }
        /// <summary>
        /// Gets the client application version.
        /// </summary>
        public string ApplicationVersion { get; private set; }

        #endregion

        #region Constructor



        /// <summary>
        /// Initialize a new system controller.
        /// </summary>
        /// <param name="devMode">True to enable Developer Mode - config can be modified but results are stored in the Analysis category.</param>
        /// <param name="applicationName">Name of the client application.</param>
        /// <param name="applicationVersion">Version of the client application.</param>
        protected ControllerBase(bool devMode, string applicationName, string applicationVersion)
        {
            _devMode = devMode;
            ApplicationName = applicationName;
            ApplicationVersion = applicationVersion;

            //TODO: if _devMode, do not pull from Remi??

            _stationConfigManager = ConfigManager<TStationConfig>.GetInstance(applicationName, applicationVersion);
            _productConfigManager = ConfigManager<TProductConfig>.GetInstance(applicationName, applicationVersion);
            _testConfigManager = ConfigManager<TTestConfig>.GetInstance(applicationName, applicationVersion);
            _sequenceConfigManager = ConfigManager<SequenceConfig>.GetInstance(applicationName, applicationVersion);

            //set up view
            View = new TView
            {
                StationConfigList = _stationConfigManager.GetConfigGroup().GetList(),
                ProductConfigList = _productConfigManager.GetConfigGroup().GetList(),
                TestConfigList = _testConfigManager.GetConfigGroup().GetList(),
                SequenceConfigList = _sequenceConfigManager.GetConfigGroup().GetList()
            };

            //subscribe to view events
            View.EditStationConfig += _view_EditStationConfig;
            View.EditProductConfig += _view_EditProductConfig;
            View.EditTestConfig += _view_EditTestConfig;
            View.EditSequenceConfig += View_EditSequenceConfig;
            View.ExecuteTestSequence += _view_ExecuteTestSequence;
            View.AbortTestSequence += _view_AbortTestSequence;
        }

        #endregion

        #region Config

        void _view_EditStationConfig(object sender, EventArgs e)
        {
            _stationConfigManager.Edit(_devMode);
        }

        void _view_EditProductConfig(object sender, EventArgs e)
        {
            _productConfigManager.Edit(_devMode);
        }

        void _view_EditTestConfig(object sender, EventArgs e)
        {
            _testConfigManager.Edit(_devMode);
        }

        void View_EditSequenceConfig(object sender, EventArgs e)
        {
            _sequenceConfigManager.Edit(_devMode);
        }

        #endregion

        #region View event handlers

        async void _view_ExecuteTestSequence(object sender, TestSequenceEventArgs e)
        {
            string sequenceAssembly = null;
            AppDomain sequenceDomain = null;

            try
            {
                View.SetState(State.TestInProgress);

                TStationConfig stationConfig = (TStationConfig) e.StationConfig;
                TProductConfig productConfig = (TProductConfig) e.ProductConfig;
                TTestConfig testConfig = (TTestConfig) e.TestConfig;
                SequenceConfig sequenceConfig = (SequenceConfig) e.SequenceConfig;

                await Task.Run(() =>
                {
                    _tokenSource = new CancellationTokenSource();

                    sequenceAssembly = Generator.GenerateDynamicAssembly(
                        ApplicationName,
                        new[] { @"C:\Users\jmckee\Source\Repos\TsdLib\TestClient\Instruments\DummyPowerSupply.xml" },
                        @"CodeGenerator\TsdLib.Instruments.xsd",
                        sequenceConfig.LocalFile,
                        Language.CSharp);

                    sequenceDomain = AppDomain.CreateDomain("SequenceDomain");

                    TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> sequence =
                        (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>)
                            sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, sequenceConfig.GetNamespace()+ "." + sequenceConfig.GetClassName() );

                    sequence.AddTraceListener(View.Listener);

                    EventProxy<MeasurementEventArgs> measurementEventProxy = new EventProxy<MeasurementEventArgs>();
                    sequence.MeasurementEventProxy = measurementEventProxy;

                    measurementEventProxy.Event += measurementEventHandler;

                    _tokenSource.Token.Register(sequence.Abort);

                    sequence.ExecuteSequence(stationConfig, productConfig, testConfig);
                });

            }
            catch (Exception ex)
            {
                var result = MessageBox.Show("Error details:" + Environment.NewLine + ex.Message + Environment.NewLine + "Would you like to view help for this error?", ex.GetType().Name, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Process.Start(ex.HelpLink);
            }
            finally
            {
                if (sequenceDomain != null)
                    AppDomain.Unload(sequenceDomain);

                if (sequenceAssembly != null)
                {
                    if (File.Exists(sequenceAssembly))
                        File.Delete(sequenceAssembly);

                    string sequencePdb = Path.ChangeExtension(sequenceAssembly, "pdb");
                    if (File.Exists(sequencePdb))
                        File.Delete(sequencePdb);

                    string sequenceTmp = Path.ChangeExtension(sequenceAssembly, null);
                    if (File.Exists(sequenceTmp))
                        File.Delete(sequenceTmp);
                }
                View.SetState(State.ReadyToTest);
            }
        }

        void measurementEventHandler(object sender, MeasurementEventArgs e)
        {
            Measurement measurement = e.Measurement;
            View.AddMeasurement(measurement);
        }

        void _view_AbortTestSequence(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }

        #endregion
    }
}
