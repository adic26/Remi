using System;
using NationalInstruments.VisaNS;

namespace TsdLib.Instrument.Visa
{
    public class VisaConnection : ConnectionBase
    {
        private MessageBasedSession _session;
        private readonly object _locker;

        private VisaConnection()
        {
            _locker = new object();
        }

        public override void Connect()
        {
            lock (_locker)
            {
                _session = (MessageBasedSession) ResourceManager.GetLocalManager().Open(Address);
                _session.Write("*RST");
                _session.Write("*CLS");
            }
        }

        public override void Disconnect()
        {
            lock (_locker)
                _session.Dispose();
        }

        protected override void Write(string message)
        {
            lock (_locker)
                _session.Write(message);
        }

        protected override string Query(string message)
        {
            lock (_locker)
                return _session.Query(message);
        }

        protected override bool CheckForError()
        {
            lock (_locker)
                return _session.Query("*STB?").Contains("No error");
        }

        public override bool IsConnected
        {
            get { return _session != null; }
        }
    }
}
