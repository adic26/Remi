using System;
using System.Diagnostics;
using TsdLib.Instrument.Dummy;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Common base class for all instrument implementations.
    /// </summary>
    /// <typeparam name="TConnection">Type of connection to use for communication with the instrument.</typeparam>
    public abstract class InstrumentBase<TConnection> : IInstrument
        where TConnection : ConnectionBase
    {
        /// <summary>
        /// A connection object to used to communicate with the instrument.
        /// </summary>
        public ConnectionBase Connection { get; private set; }

        /// <summary>
        /// Gets a description of the instrument, including connection information.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Initialize a new instrument object with the specified connection.
        /// </summary>
        /// <param name="connection">A connection object to communicate with the instrument.</param>
        protected InstrumentBase(TConnection connection)
        {
            Connection = connection;
            Description = GetType().Name + " via " + connection.Description;
        }

        /// <summary>
        /// Initialize a new instrument object with a dummy connection.
        /// </summary>
        /// <param name="dummyConnection">A <see cref="DummyConnection"/> object to simulate communication with the instrument.</param>
        protected InstrumentBase(DummyConnection dummyConnection)
        {
            Connection = dummyConnection;
        }

        /// <summary>
        /// Gets the initialization commands that are sent to the instrument as part of the connection process.
        /// </summary>
        protected virtual string InitCommands { get { return ""; } }

        /// <summary>
        /// Gets the message to send to the instrument to query the model number.
        /// </summary>
        protected internal abstract string ModelNumberMessage { get; }
        /// <summary>
        /// Gets the regular expression used to parse the response when querying the model number.
        /// </summary>
        protected internal virtual string ModelNumberRegEx { get { return ".*"; } }
        /// <summary>
        /// Gets the termination character (if any) that the instrument will send to signal the end of the model number query response.
        /// </summary>
        protected internal virtual char ModelNumberTermChar { get { return '\uD800'; } }
        /// <summary>
        /// Gets the instrument model number.
        /// </summary>
        public string ModelNumber { get; internal set; }

        /// <summary>
        /// Gets the message to send to the instrument to query the serial number.
        /// </summary>
        protected internal abstract string SerialNumberMessage { get; }
        /// <summary>
        /// Gets the regular expression used to parse the response when querying the serial number.
        /// </summary>
        protected internal virtual string SerialNumberRegEx { get { return ".*"; } }
        /// <summary>
        /// Gets the termination character (if any) that the instrument will send to signal the end of the serial number query response.
        /// </summary>
        protected internal virtual char SerialNumberTermChar { get { return '\uD800'; } }
        /// <summary>
        /// Gets the instrument serial number.
        /// </summary>
        public string SerialNumber { get; internal set; }

        /// <summary>
        /// Gets the message to send to the instrument to query the firmware version.
        /// </summary>
        protected internal abstract string FirmwareVersionMessage { get; }
        /// <summary>
        /// Gets the regular expression used to parse the response when querying the firmware version.
        /// </summary>
        protected internal virtual string FirmwareVersionRegEx { get { return ".*"; } }
        /// <summary>
        /// Gets the termination character (if any) that the instrument will send to signal the end of the firmware version query response.
        /// </summary>
        protected internal virtual char FirmwareVersionTermChar { get { return '\uD800'; } }
        /// <summary>
        /// Gets the instrument firmware version.
        /// </summary>
        public string FirmwareVersion { get; internal set; }

        /// <summary>
        /// Closes the instrument connection and disposes of any resources being used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Closes the instrument connection and disposes of any resources being used.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Debug.WriteLine("Disposing " + Description);
                Connection.Dispose();
            }
        }
    }
}
