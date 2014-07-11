using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsdLib.Instrument.Visa;

namespace TsdLib.Instrument
{
    //TODO: figure out how to avoid type parameter for InstrumentBase
    public class InstrumentWrapper<T>
        where T : InstrumentBase<VisaConnection>
    {
        private T _instrument;

        public InstrumentWrapper(T instrument)
        {
            _instrument = instrument;
        }
    }
}
