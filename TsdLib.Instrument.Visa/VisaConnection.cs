using NationalInstruments.VisaNS;

namespace TsdLib.Instrument.Visa
{
    public class VisaConnection : ConnectionBase
    {
        private MessageBasedSession _session;

        internal VisaConnection(MessageBasedSession session, int defaultDelay = 0)
            : base(session.ResourceName, defaultDelay)
        {
            _session = session;
        }

        protected override void Write(string message)
        {
            _session.Write(message);
        }

        protected override string ReadString()
        {
            return _session.ReadString();
        }

        protected override byte ReadByte()
        {
            return _session.ReadByteArray(1)[0];
        }

        protected override bool CheckForError()
        {
            return _session.LastStatus != VisaStatusCode.Success && _session.LastStatus != VisaStatusCode.SuccessMaxCountRead;
        }

        public override bool IsConnected
        {
            get { return _session != null; }
        }

        protected override void Dispose(bool disposing)
        {
            _session.Dispose();
            _session = null;
            base.Dispose(disposing);
        }
    }
}
