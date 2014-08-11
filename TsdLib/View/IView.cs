using System;
using System.Collections;

namespace TsdLib.View
{
    public interface IView
    {
        event EventHandler EditStationConfig;
        event EventHandler EditProductConfig;

        event EventHandler<TestSequenceEventArgs> ExecuteTestSequence;
        event EventHandler AbortTestSequence;

        void SetState(State state);
        void AddMeasurement(Measurement measurement);

        IList StationConfigList { set; }
        IList ProductConfigList { set; }
    }

    public class TestSequenceEventArgs : EventArgs
    {
        public object StationConfig { get; private set; }
        public object ProductConfig { get; private set; }

        public TestSequenceEventArgs(object stationConfig, object productConfig)
        {
            StationConfig = stationConfig;
            ProductConfig = productConfig;
        }
    }
}
