using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using TsdLib.Configuration;
using TsdLib.TestSequence;
using TsdLib.View;

namespace TsdLib.Controller
{
    public abstract class ControllerBase<TStationConfig, TProductConfig, TTestConfig>
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
        where TTestConfig : TestConfigCommon, new()
    {
        #region Private Fields

        private readonly IView _view;
        private readonly TestSequenceBase _testSequence;

        private readonly LocalMeasurementWriter _localMeasurementWriter;

        private CancellationTokenSource _tokenSource;

        #endregion

        #region Constructor and Launch

        protected ControllerBase(IView view, TestSequenceBase testSequence)
        {
            _view = view;
            _testSequence = testSequence;

            _localMeasurementWriter = new LocalMeasurementWriter();
        }

        public void Launch()
        {
            //subscribe to view events
            _view.EditStationConfig += _view_EditStationConfig;
            _view.EditProductConfig += _view_EditProductConfig;
            _view.EditTestConfig += _view_EditTestConfig;
            _view.ExecuteTestSequence += _view_ExecuteTestSequence;
            _view.AbortTestSequence += _view_AbortTestSequence;

            //subscribe to test sequence events
            _testSequence.Measurements.ListChanged += Measurements_ListChanged;

            _view.Launch();
        }

        #endregion

        #region Config

        void _view_EditStationConfig(object sender, EventArgs e)
        {
            Config<TStationConfig>.Manager.Edit();
        }

        void _view_EditProductConfig(object sender, EventArgs e)
        {
            Config<TProductConfig>.Manager.Edit();
        }

        void _view_EditTestConfig(object sender, EventArgs e)
        {
            Config<TTestConfig>.Manager.Edit();
        }

        #endregion

        #region View event handlers

        async void _view_ExecuteTestSequence(object sender, EventArgs e)
        {
            try
            {
                _tokenSource = new CancellationTokenSource();
                await _testSequence.ExecuteAsync(_tokenSource.Token);
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
