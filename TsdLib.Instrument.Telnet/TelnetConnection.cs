using System;
using System.Net.Sockets;
using System.Text;

namespace TsdLib.Instrument.Telnet
{
    public class TelnetConnection : ConnectionBase
    {
        private TcpClient _tcpSocket;

        protected override bool CheckForError()
        {
            return IsConnected;
        }

        public override void Connect()
        {
            try
            {
                _tcpSocket = new TcpClient(Address, 23);
                StringBuilder sb = new StringBuilder(Read(timeout: 750));
                if (!sb.ToString().TrimEnd().EndsWith(":"))
                    throw new TelnetException("Could not connect to " + Address + " via telnet: no login prompt");
                WriteLine("root");

                sb.Append(Read());
                if (!sb.ToString().TrimEnd().EndsWith(":"))
                    throw new TelnetException("Could not connect to " + Address + " via telnet: no password prompt");
                WriteLine("root");
                
                sb.Append(Read());

                if(sb.Length == 0)
                    throw new TelnetException("Could not read any data from " + Address + " via Telnet");
            }
            catch (SocketException ex)
            {
                throw new TelnetException("Could not connect to " + Address + " via Telnet, the connection was actively refused. Possibly due to a secure device.", ex);
            }
        }

        public override void Disconnect()
        {
            _tcpSocket.Close();
        }

        public override bool IsConnected
        {
            get { return _tcpSocket != null && _tcpSocket.Connected; }
        }

        protected override string Query(string message)
        {
            WriteLine(message);
            return Read();
        }

        protected override void Write(string message)
        {
            WriteLine(message);
        }

        #region This code has been adapted from minimalistic telnet implementation conceived by Tom Janssens on 2007/06/06  for codeproject http://www.corebvba.be

        void WriteLine(string cmd)
        {
            if (!_tcpSocket.Connected) return;
            byte[] buf = Encoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF") + Environment.NewLine);
            _tcpSocket.GetStream().Write(buf, 0, buf.Length);
        }

        string Read(int minimumBytes = 0, int timeout = 200)
        {
            if (!_tcpSocket.Connected) return null;
            StringBuilder sb = new StringBuilder();
            while (_tcpSocket.Available <= minimumBytes)
                System.Threading.Thread.Sleep(100);
            do
            {
                ParseTelnet(sb);
                System.Threading.Thread.Sleep(timeout);//Needs to be longer for login
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
