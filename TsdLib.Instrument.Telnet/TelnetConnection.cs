using System;

namespace TsdLib.Instrument.Telnet
{
    public class TelnetConnection : ConnectionBase
    {//TODO: implement TelnetConnection
        protected override bool CheckForError()
        {
            throw new NotImplementedException();
        }

        public override void Connect()
        {
            throw new NotImplementedException();
        }

        public override bool IsConnected
        {
            get { throw new NotImplementedException(); }
        }

        protected override string Query(string message)
        {
            throw new NotImplementedException();
        }

        protected override void Write(string message)
        {
            throw new NotImplementedException();
        }
    }
}
