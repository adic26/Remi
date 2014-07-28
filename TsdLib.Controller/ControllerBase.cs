using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using TsdLib.Configuration;
using TsdLib.TestSequence;
using TsdLib.View;

namespace TsdLib.Controller
{
    public abstract class ControllerBase
    {
        private readonly IView _view;
        private readonly TestSequenceBase _testSequence;
        private readonly Assembly _callingAssembly;

        private readonly LocalMeasurementWriter _localMeasurementWriter;

        private CancellationTokenSource _tokenSource;

        protected ControllerBase(IView view, TestSequenceBase testSequence)
        {
            _view = view;
            _testSequence = testSequence;
            _callingAssembly = Assembly.GetCallingAssembly();

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
            //TODO: update to all-config updater or make this abstract
            Config.Manager.EditConfig<ProductConfig>();
            //Config.Manager.EditConfig(_callingAssembly);
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
    }
}
