using NationalInstruments.VisaNS;

namespace TsdLib.Instrument.Visa
{
    /// <summary>
    /// Contains functionality to communicate with a Visa-based instrument.
    /// </summary>
    public class VisaConnection : ConnectionBase
    {
        private readonly MessageBasedSession _session;

        /// <summary>
        /// Returns true if the Visa-based instrument is connected and ready to communicate.
        /// </summary>
        public override bool IsConnected
        {
            get { return _session != null; }
        }

        /// <summary>
        /// Initialize a new Connection object.
        /// </summary>
        /// <param name="session">A NationalInstruments.VisaNS.MessageBasedSession object to provide the transport layer for the Visa connection.</param>
        /// <param name="defaultDelay">Default delay (in ms) to wait before sending each command.</param>
        internal VisaConnection(MessageBasedSession session, int defaultDelay = 0)
            : base(session.ResourceName, defaultDelay)
        {
            _session = session;
        }

        /// <summary>
        /// Write a string to the Visa-based instrument.
        /// </summary>
        /// <param name="message">String to write.</param>
        protected override void Write(string message)
        {
            _session.Write(message);
        }

        /// <summary>
        /// Read a string from the Visa-based instrument.
        /// </summary>
        /// <returns>A string from the instrument.</returns>
        protected override string ReadString()
        {
            return _session.ReadString();
        }

        /// <summary>
        /// Read one byte from the Visa-based instrument.
        /// </summary>
        /// <returns>One byte.</returns>
        protected override byte ReadByte()
        {
            return _session.ReadByteArray(1)[0];
        }

        /// <summary>
        /// Checks if there is an error with the current connection or from the last command/response.
        /// </summary>
        /// <returns>True in case of error; False otherwise.</returns>
        protected override bool CheckForError()
        {
            return _session.LastStatus != VisaStatusCode.Success && _session.LastStatus != VisaStatusCode.SuccessMaxCountRead && _session.LastStatus != VisaStatusCode.SuccessTerminationCharacterRead;
        }

        /// <summary>
        /// Close the Visa connection and optionally dispose of any resources.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _session.Dispose();
            base.Dispose(disposing);
        }
    }
}
