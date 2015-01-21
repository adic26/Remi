using System;
using System.Collections.Generic;
using System.Linq;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Base class for instrument wrappers. Allows adding complex compile-time logic to XML-based instrument definitions.
    /// The purpose of the common base class is to serve as an adapter to allow Instrument Wrappers to implement IInstrument and expose internal instrument model/serial numbers and firmware version.
    /// </summary>
    public abstract class InstrumentWrapper : IInstrument
    {
        private readonly IEnumerable<IInstrument> _instruments;

        /// <summary>
        /// Initialize a new InstrumentWrapper to wrap the specified instrument.
        /// </summary>
        /// <param name="instrument">A first-order instrument to add to the wrapper.</param>
        protected InstrumentWrapper(IInstrument instrument)
        {
            _instruments = new[] { instrument};
        }

        /// <summary>
        /// Initialize a new InstrumentWrapper to wrap the specified sequence of instruments.
        /// </summary>
        /// <param name="instruments">A sequence of first-order instruments to add to the wrapper.</param>
        protected InstrumentWrapper(IEnumerable<IInstrument> instruments)
        {
            _instruments = instruments;
        }

        /// <summary>
        /// Gets the descriptions(s) of the contained instrument(s), including connection information.
        /// </summary>
        public string Description { get { return string.Join(",",  _instruments.Select(inst => inst.Description)).TrimEnd(','); } }
        /// <summary>
        /// Gets the model number(s) of the contained instrument(s).
        /// </summary>
        public string ModelNumber { get { return string.Join(",", _instruments.Select(inst => inst.ModelNumber)).TrimEnd(','); } }
        /// <summary>
        /// Gets the serial number(s) of the contained instrument(s).
        /// </summary>
        public string SerialNumber { get { return string.Join(",", _instruments.Select(inst => inst.SerialNumber)).TrimEnd(','); } }
        /// <summary>
        /// Gets the firmware version(s) of the contained instrument(s).
        /// </summary>
        public string FirmwareVersion { get { return string.Join(",", _instruments.Select(inst => inst.FirmwareVersion)).TrimEnd(','); } }

        /// <summary>
        /// Disposes all of the contained instruments.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all of the contained instruments.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                foreach (IInstrument instrument in _instruments)
                    instrument.Dispose();
        }
    }
}
