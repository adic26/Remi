using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TsdLib.Instrument;

namespace TsdLib.TestSystem.TestSequence
{
    /// <summary>
    /// Contains functionality to track and manage a collection of instruments.
    /// </summary>
    public class TestSequenceInstrumentCollection : ReadOnlyCollection<IInstrument> , IInstrumentCollection
    {
        /// <summary>
        /// Initialize a new <see cref="TestSequenceInstrumentCollection"/>
        /// </summary>
        public TestSequenceInstrumentCollection() : base(new List<IInstrument>())
        {
            InstrumentEvents.Connected += FactoryEvents_Connected;
        }

        /// <summary>
        /// Event handler invoked when a new instrument is connected to the test sequence.
        /// </summary>
        /// <param name="sender">The factory class responsible for connecting to the new instrument.</param>
        /// <param name="e">The new <see cref="IInstrument"/>.</param>
        private void FactoryEvents_Connected(object sender, IInstrument e)
        {
            Items.Add(e);
            EventHandler<IInstrument> handler = InstrumentConnected;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Disconnect from the instrument factory events and dispose all connected instruments.
        /// </summary>
        public void Dispose()
        {
            InstrumentEvents.Connected -= FactoryEvents_Connected;
            foreach (IInstrument instrument in Items)
                instrument.Dispose();
            Items.Clear();
        }

        /// <summary>
        /// Event fired when a new instrument is connected to the test sequence.
        /// </summary>
        public event EventHandler<IInstrument> InstrumentConnected;
    }
}
