using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        
        private readonly Assembly _clientAssembly;


        private readonly LocalMeasurementWriter _localMeasurementWriter;

        private CancellationTokenSource _tokenSource;

        public virtual void EditStationConfig()
        {
            Trace.WriteLine("Station config has not been defined for this system.");
        }

        public virtual void EditProductConfig()
        {
            Trace.WriteLine("Product config has not been defined for this system.");
        }

        public virtual void EditTestConfig()
        {
            Trace.WriteLine("Test config has not been defined for this system.");
        }

        //TODO: figure out how to remove type arguments from config
        protected ControllerBase(IView view, TestSequenceBase testSequence)
        {
            _view = view;
            _testSequence = testSequence;

            _clientAssembly = Assembly.GetCallingAssembly();

            var types = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ConfigItem)));

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

        void _view_EditStationConfig(object sender, EventArgs e)
        {
            EditStationConfig();
        }

        void _view_EditProductConfig(object sender, EventArgs e)
        {
            EditProductConfig();
        }

        void _view_EditTestConfig(object sender, EventArgs e)
        {
            EditTestConfig();
        }

        //void _view_Configure(object sender, ConfigEventArgs e)
        //{
        //    Assembly asy = Assembly.GetAssembly(typeof (ConfigItem));
        //    Type configBaseType = asy.GetType(asy.GetName().Name + "." +  e.ConfigType);

        //    IEnumerable<Type> configTypes = _clientAssembly
        //        .GetTypes()
        //        .Where(t => t.IsSubclassOf(configBaseType));

        //    foreach (Type configType in configTypes)
        //    {
        //        Config<ProductConfig>.Manager.Edit();
        //    }

        //    throw new NotImplementedException();
        //}

        //void _view_Configure(object sender, EventArgs e)
        //{
        //    //TODO: update to all-config updater or make this abstract

        //    var types = _clientAssembly
        //        .GetTypes()
        //        .Where(t => t.IsSubclassOf(typeof (ConfigItem)));

        //    //foreach (IConfigGroup<ProductConfig> configGroup in _configGroups)
        //    //{
                
        //    //}
        //}

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
