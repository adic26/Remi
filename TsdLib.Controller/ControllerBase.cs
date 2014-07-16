using System;
using System.ComponentModel;
using System.Windows.Forms;
using TsdLib.TestSequence;
using TsdLib.View;

namespace TsdLib.Controller
{
    public class ControllerBase //TODO: make abstract
    {
        private readonly IView _view;
        private readonly ISettings _settings;
        private readonly ITestSequence _testSequence;

        public ControllerBase(IView view, ITestSequence testSequence,  ISettings settings)
        {
            //subscribe to view events
            _view = view;
            _view.Configure += _view_Configure;
            _view.ExecuteTestSequence += _view_ExecuteTestSequence;
            _view.AbortTestSequence += _view_AbortTestSequence;
            
            //subscribe to test sequence events
            _testSequence = testSequence;
            _testSequence.Measurements.ListChanged += Measurements_ListChanged;

            _settings = settings;

            _view.Launch(); //TODO: should this be removed from constructor?
        }

        void _view_Configure(object sender, EventArgs e)
        {
            _settings.Edit();
        }

        void _view_ExecuteTestSequence(object sender, EventArgs e)
        {
            _testSequence.Execute();
        }

        void _view_AbortTestSequence(object sender, EventArgs e)
        {
            //TODO: call TestSequence.Abort, which will set cancellation token
        }

        void Measurements_ListChanged(object sender, ListChangedEventArgs e)
        {
            IBindingList list = sender as IBindingList;
            if (list != null)
                _view.AddMeasurement((Measurement)list[e.NewIndex]);
        }

        public void Configure()
        {
            _settings.Edit();
        }
    }
}
