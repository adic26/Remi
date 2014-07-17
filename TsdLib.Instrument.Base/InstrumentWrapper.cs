using System.Collections.Generic;
using System.Linq;

namespace TsdLib.Instrument
{
    public abstract class InstrumentWrapper : IInstrument
    {
        private readonly IEnumerable<IInstrument> _instruments;

        protected InstrumentWrapper(IInstrument instrument)
        {
            _instruments = new[] { instrument};
        }

        protected InstrumentWrapper(IEnumerable<IInstrument> instruments)
        {
            _instruments = instruments;
        }

        public string ModelNumber { get { return string.Join(",", _instruments.Select(inst => inst.ModelNumber)).TrimEnd(','); } }
        public string SerialNumber { get { return string.Join(",", _instruments.Select(inst => inst.SerialNumber)).TrimEnd(','); } }
        public string FirmwareVersion { get { return string.Join(",", _instruments.Select(inst => inst.FirmwareVersion)).TrimEnd(','); } }

        public void Dispose()
        {
            foreach (IInstrument instrument in _instruments)
                instrument.Dispose();
        }
    }
}
