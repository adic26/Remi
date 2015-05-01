namespace TsdLib.Instrument.Dummy
{
    /// <summary>
    /// Simulates a connection object to communicate with instruments.
    /// </summary>
    public class DummyConnection : ConnectionBase
    {
        /// <summary>
        /// Gets or sets the response if the IsConnected property.
        /// </summary>
        public bool ReturnConnected { get; set; }
        /// <summary>
        /// Gets or sets the reponse of the CheckForError method.
        /// </summary>
        public bool ReturnErrorOnCheck { get; set; }
        /// <summary>
        /// Gets or sets the byte that will be returned by the ReadByte method.
        /// </summary>
        public byte ByteToRead { get; set; }
        /// <summary>
        /// Gets or sets the string that will be returned by the ReadString method.
        /// </summary>
        public string StringToRead { get; set; }

        /// <summary>
        /// Returns true if the instrument is connected and ready to communicate. Can be controlled by setting the ReturnConnected property.
        /// </summary>
        public override bool IsConnected
        {
            get { return ReturnConnected; }
        }

        /// <summary>
        /// Initialize a new DummyConnection wit hthe specified address.
        /// </summary>
        /// <param name="address">Address to assign to the DummyConnection.</param>
        public DummyConnection(string address)
            : base(address)
        {
            ReturnConnected = true;
            ReturnErrorOnCheck = false;
            ByteToRead = (byte) 'a';
            StringToRead = "aa";
        }

        /// <summary>
        /// Checks if there is an error with the current connection or from the last command/response.
        /// </summary>
        /// <param name="errorString">A description of the error.</param>
        /// <returns>True in case of error; False otherwise.</returns>
        protected override bool CheckForError(out string errorString)
        {
            errorString = "ReturnErrorOnCheck was set to true";
            return ReturnErrorOnCheck;
        }
        /// <summary>
        /// Read one byte from the connection.
        /// </summary>
        /// <returns>One byte.</returns>
        protected override byte ReadByte()
        {
            return ByteToRead;
        }
        /// <summary>
        /// Read a string from the instrument.
        /// </summary>
        /// <returns>A string from the instrument.</returns>
        protected override string ReadString()
        {
            return StringToRead;
        }
        /// <summary>
        /// Write a string to the instrument.
        /// </summary>
        /// <param name="message">String to write.</param>
        protected override void Write(string message)
        {
            
        }
    }
}
