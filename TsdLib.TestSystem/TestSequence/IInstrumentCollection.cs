using System;
using System.Collections.Generic;
using TsdLib.Instrument;

namespace TsdLib.TestSystem.TestSequence
{
    public interface IInstrumentCollection : IEnumerable<IInstrument>, IDisposable
    {
        event EventHandler<IInstrument> InstrumentConnected;
    }
}