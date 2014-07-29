using System;

namespace TsdLib.View
{
    public interface IView
    {
        event EventHandler EditStationConfig;
        event EventHandler EditProductConfig;
        event EventHandler EditTestConfig;

        event EventHandler ExecuteTestSequence;
        event EventHandler AbortTestSequence;

        void Launch();
        void SetState(State state);
        void AddMeasurement(Measurement measurement);
    }
}
