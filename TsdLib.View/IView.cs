using System;

namespace TsdLib.View
{
    public interface IView
    {
        event EventHandler EditStationConfig;
        event EventHandler EditProductConfig;
        event EventHandler EditTestConfig;

        event EventHandler<TestSequenceEventArgs> ExecuteTestSequence;
        event EventHandler AbortTestSequence;

        void Launch();
        void SetState(State state);
        void AddMeasurement(Measurement measurement);
    }

    public class TestSequenceEventArgs : EventArgs
    {
        public string StationConfig { get; private set; }
        public string ProductConfig { get; private set; }

        public TestSequenceEventArgs(string stationConfig, string productConfig)
        {
            StationConfig = stationConfig;
            ProductConfig = productConfig;
        }
    }
}
