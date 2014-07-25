using System;

namespace TsdLib.View
{
    public interface IView
    {
        event EventHandler Configure;
        event EventHandler ExecuteTestSequence;
        event EventHandler AbortTestSequence;

        void Launch();
        void SetState(State state);
        void AddMeasurement(Measurement measurement);
    }
}
