namespace TsdLib.Instrument.Dummy
{
    public class DummyConnection : ConnectionBase
    {
        public bool ReturnConnected { get; set; }
        public bool ReturnErrorOnCheck { get; set; }
        public byte ByteToRead { get; set; }
        public string StringToRead { get; set; }

        public override bool IsConnected
        {
            get { return ReturnConnected; }
        }

        public DummyConnection(string address)
            : base(address)
        {
            ReturnConnected = true;
            ReturnErrorOnCheck = false;
            ByteToRead = (byte) 'a';
            StringToRead = "aa";
        }

        protected override bool CheckForError()
        {
            return ReturnErrorOnCheck;
        }

        protected override byte ReadByte()
        {
            return ByteToRead;
        }

        protected override string ReadString()
        {
            return StringToRead;
        }

        protected override void Write(string message)
        {
            
        }
    }
}
