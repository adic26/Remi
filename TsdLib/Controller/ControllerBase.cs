using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TsdLib.Configuration;
using TsdLib.Proxies;
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
        #region Private Fields

        private readonly bool _devMode;

        private CancellationTokenSource _tokenSource;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a reference to the user interface.
        /// </summary>
        public IView View { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new system controller.
        /// </summary>
        /// <param name="devMode">True to enable Developer Mode - config can be modified but results are stored in the Analysis category.</param>
        protected ControllerBase(bool devMode)
        {
            _devMode = devMode;

            //TODO: if _devMode, do not pull from Remi??

            //set up view
            View = new TView
            {
                StationConfigList = ConfigManager<TStationConfig>.GetConfigGroup().GetList(),
                ProductConfigList = ConfigManager<TProductConfig>.GetConfigGroup().GetList(),
                TestConfigList = ConfigManager<TTestConfig>.GetConfigGroup().GetList(),
                SequenceConfigList = ConfigManager<SequenceConfig>.GetConfigGroup().GetList()
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
            ConfigManager<TStationConfig>.Edit(_devMode);
        }

        void _view_EditProductConfig(object sender, EventArgs e)
        {
            ConfigManager<TProductConfig>.Edit(_devMode);
        }

        void _view_EditTestConfig(object sender, EventArgs e)
        {
            ConfigManager<TTestConfig>.Edit(_devMode);
        }

        void View_EditSequenceConfig(object sender, EventArgs e)
        {
            ConfigManager<SequenceConfig>.Edit(_devMode);
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

                    sequenceAssembly = CodeGenerator.CodeGenerator.GenerateTestSequenceFromFile(sequenceConfig.LocalFile);

                    sequenceDomain = AppDomain.CreateDomain("SequenceDomain");

                    TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> sequence =
                        (TestSequenceBase<TStationConfig, TProductConfig, TTestConfig>)
                            sequenceDomain.CreateInstanceFromAndUnwrap(sequenceAssembly, sequenceConfig.Namespace + "." + sequenceConfig.ClassName );

                    sequence.AddTraceListener(View.Listener);

                    EventProxy<MeasurementEventArgs> measurementEventProxy = new EventProxy<MeasurementEventArgs>();
                    sequence.MeasurementEventProxy = measurementEventProxy;

                    measurementEventProxy.Event += measurementEventHandler;

                    _tokenSource.Token.Register(sequence.Abort);

                    sequence.ExecuteSequence(stationConfig, productConfig, testConfig);
                });

                View.SetState(State.ReadyToTest);

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
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
