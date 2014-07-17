using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.View
{
    public interface IView
    {
        event EventHandler Configure;
        event EventHandler ExecuteTestSequence;
        event EventHandler AbortTestSequence;

        void Launch();
        void AddMeasurement(Measurement measurement);
    }
}
