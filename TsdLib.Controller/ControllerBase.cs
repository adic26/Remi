using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using TsdLib.Configuration;
using TsdLib.TestSequence;
using TsdLib.View;

namespace TsdLib.Controller
{
    public class ControllerBase
    {
        private readonly IView _view;
        private readonly ISettings _settings;
        private readonly TestSequenceBase _testSequence;

        private readonly LocalMeasurementWriter _localMeasurementWriter;

        private CancellationTokenSource _tokenSource;

        public ControllerBase(IView view, TestSequenceBase testSequence, ISettings settings)
        {
            _view = view;
            _testSequence = testSequence;
            _settings = settings;

            _localMeasurementWriter = new LocalMeasurementWriter();
        }

        public void Launch()
        {
            //subscribe to view events
            _view.Configure += _view_Configure;
            _view.ExecuteTestSequence += _view_ExecuteTestSequence;
            _view.AbortTestSequence += _view_AbortTestSequence;

            //subscribe to test sequence events
            _testSequence.Measurements.ListChanged += Measurements_ListChanged;

            _view.Launch(); 
        }

        void _view_Configure(object sender, EventArgs e)
        {
            //TODO: update to all-config updater
            //or put a ConfigItem type in eventargs
            Config.Manager.EditConfig<ProductConfig>();
        }

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
                //TODO: reset UI state to 'Ready to Test'
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
                Measurement measurement = (Measurement) list[e.NewIndex];
                _view.AddMeasurement(measurement);
                _localMeasurementWriter.Write(measurement);
            }
        }

        public void Configure()
        {
            _settings.Edit();
        }
    }
}
