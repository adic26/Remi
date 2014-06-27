using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NationalInstruments.VisaNS;

namespace TsdLib.Instrument.Visa
{
    public class VisaConnection : ConnectionBase
    {
        private readonly object _locker;
        private readonly MessageBasedSession _session;

        internal VisaConnection(MessageBasedSession session)
            : base(session.ResourceName)
        {
            _locker = new object();
            _session = session;
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
        {//TODO: this won't work for some serial devices
            lock (_locker)
                return _session.Query("*STB?").Contains("No error");
        }

        public override bool IsConnected
        {
            get { return _session != null; }
        }
    }
}
