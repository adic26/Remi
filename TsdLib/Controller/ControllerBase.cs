using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using TsdLib.Configuration;
using TsdLib.TestSequence;
using TsdLib.View;

namespace TsdLib.Controller
{
    public abstract class ControllerBase<TStationConfig, TProductConfig>
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
    {
        #region Private Fields

        private readonly IView _view;
        private readonly TestSequenceBase<TStationConfig, TProductConfig> _testSequence;
        private readonly bool _devMode;

        private readonly LocalMeasurementWriter _localMeasurementWriter;

        private CancellationTokenSource _tokenSource;

        #endregion

        #region Constructor and Launch

        protected ControllerBase(IView view, TestSequenceBase<TStationConfig, TProductConfig> testSequence, bool devMode)
        {

            _view = view;
            _testSequence = testSequence;
            _devMode = devMode;

            _localMeasurementWriter = new LocalMeasurementWriter();

            //subscribe to view events
            _view.EditStationConfig += _view_EditStationConfig;
            _view.EditProductConfig += _view_EditProductConfig;
            _view.ExecuteTestSequence += _view_ExecuteTestSequence;
            _view.AbortTestSequence += _view_AbortTestSequence;

            //subscribe to test sequence events
            _testSequence.Measurements.ListChanged += Measurements_ListChanged;
        }

        #endregion

        #region Config

        void _view_EditStationConfig(object sender, EventArgs e)
        {
            Config<TStationConfig>.Edit(_devMode);
        }

        void _view_EditProductConfig(object sender, EventArgs e)
        {
            Config<TProductConfig>.Edit(_devMode);
        }

        #endregion

        #region View event handlers

        async void _view_ExecuteTestSequence(object sender, TestSequenceEventArgs e)
        {
            try
            {
                _tokenSource = new CancellationTokenSource();
                _view.SetState(State.TestInProgress);
                await _testSequence.ExecuteAsync(e.StationConfig as TStationConfig, e.ProductConfig as TProductConfig, _tokenSource.Token);
                _view.SetState(State.ReadyToTest);
            }
            catch (OperationCanceledException)
            {
                _tokenSource.Dispose();
                Trace.WriteLine("Test sequence cancelled.");
                _view.SetState(State.ReadyToTest);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        void _view_AbortTestSequence(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }

        void Measurements_ListChanged(object sender, ListChangedEventArgs e)
        {
            IBindingList list = sender as IBindingList;
            if (list != null)
            {
                Measurement measurement = (Measurement)list[e.NewIndex];
                _view.AddMeasurement(measurement);
                _localMeasurementWriter.Write(measurement);
            }
        }

        #endregion
    }
}
