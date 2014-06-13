using System;

namespace TsdLib.Instrument.Visa
{
    public class VisaConnection : ConnectionBase
    {//TODO: implement VisaConnection
        private VisaConnection() { }

        protected override void Write(string message)
        {
            throw new NotImplementedException();
        }

        protected override string Query(string message)
        {
            throw new NotImplementedException();
        }

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
    }
}
