using System;
using System.Collections.Generic;
using TsdLib.Instrument;

namespace TsdLib.TestSystem
{
    public interface IInstrumentCollection : IEnumerable<IInstrument>, IDisposable
    {
        event EventHandler<IInstrument> InstrumentConnected;
    }
}