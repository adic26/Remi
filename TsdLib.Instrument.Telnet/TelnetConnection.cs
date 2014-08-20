using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TsdLib.Instrument.Telnet
{
    /// <summary>
    /// Contains functionality to communicate with a Telnet-based instrument.
    /// </summary>
    public class TelnetConnection : ConnectionBase
    {
        private readonly TcpClient _tcpSocket;

        /// <summary>
        /// Initialize a new Connection object.
        /// </summary>
        /// <param name="tcpClient">A System.Net.Sockets.TcpClient object to provide the transport layer for the Telnet connection.</param>
        /// <param name="defaultDelay">Default delay (in ms) to wait before sending each command.</param>
        internal TelnetConnection(TcpClient tcpClient, int defaultDelay = 200)
            : base(((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString(), defaultDelay)
        {
            _tcpSocket = tcpClient;
        }

        /// <summary>
        /// Checks if there is an error with the current connection or from the last command/response.
        /// </summary>
        /// <returns>True in case of error; False otherwise.</returns>
        protected override bool CheckForError()
        {
            return !IsConnected;
        }

        /// <summary>
        /// Returns true if the Telnet-based instrument is connected and ready to communicate.
        /// </summary>
        public override bool IsConnected
        {
            get { return _tcpSocket != null && _tcpSocket.Connected; }
        }

        /// <summary>
        /// Read one byte from the Telnet-based instrument.
        /// </summary>
        /// <returns>One byte.</returns>
        protected override byte ReadByte()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Read a string from the Telnet-based instrument.
        /// </summary>
        /// <returns>A string from the instrument.</returns>
        protected override string ReadString()
        {
            return Read(DefaultDelay);
        }

        /// <summary>
        /// Write a string to the Telnet-based instrument.
        /// </summary>
        /// <param name="message">String to write.</param>
        protected override void Write(string message)
        {
            WriteLine(message);
        }

        /// <summary>
        /// Close the Telnet connection and optionally dispose of any resources.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _tcpSocket.Close();
            base.Dispose(disposing);
        }

        #region This code has been adapted from minimalistic telnet implementation conceived by Tom Janssens on 2007/06/06  for codeproject http://www.corebvba.be

        void WriteLine(string cmd)
        {
            if (!_tcpSocket.Connected) return;
            byte[] buf = Encoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF") + Environment.NewLine);
            _tcpSocket.GetStream().Write(buf, 0, buf.Length);
        }

        string Read(int timeout)
        {
            if (!_tcpSocket.Connected) return null;
            StringBuilder sb = new StringBuilder();
            while (_tcpSocket.Available == 0)
                Thread.Sleep(100);
            do
            {
                ParseTelnet(sb);
                Thread.Sleep(timeout);//Needs to be longer for login
            } while (_tcpSocket.Available > 0);
            return sb.ToString();
        }

        void ParseTelnet(StringBuilder sb)
        {
            while (_tcpSocket.Available > 0)
            {
                int input = _tcpSocket.GetStream().ReadByte();
                switch (input)
                {
                    case -1:
                        break;
                    case (int)Verbs.Iac:
                        // interpret as command
                        int inputverb = _tcpSocket.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb)
                        {
                            case (int)Verbs.Iac:
                                //literal IAC = 255 escaped, so append char 255 to string
                                sb.Append(inputverb);
                                break;
                            case (int)Verbs.Do:
                            case (int)Verbs.Dont:
                            case (int)Verbs.Will:
                            case (int)Verbs.Wont:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                int inputoption = _tcpSocket.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                _tcpSocket.GetStream().WriteByte((byte)Verbs.Iac);
                                if (inputoption == (int)Options.Sga)
                                    _tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.Do ? (byte)Verbs.Will : (byte)Verbs.Do);
                                else
                                    _tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.Do ? (byte)Verbs.Wont : (byte)Verbs.Dont);
                                _tcpSocket.GetStream().WriteByte((byte)inputoption);
                                break;
                        }
                        break;
                    default:
                        sb.Append((char)input);
                        break;
                }
            }
        }

        enum Verbs
        {
            Will = 251,
            Wont = 252,
            Do = 253,
            Dont = 254,
            Iac = 255
        }

        enum Options
        {
            Sga = 3
        }

        #endregion
    }
}
