using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.Adb;
using TsdLib.Instrument.Ssh.AdbUtilities;

namespace TsdLib.Instrument.Ssh
{
    /// <summary>
    /// An SSH Connection with Avengers devices
    /// </summary>
    class AvengersSshConn : SshConnection
    {
        private Device DUT;
        private StreamOutputReceiver OutputReceiver;

        public override bool IsConnected { get { return (DUT != null); } }

        /// <summary>
        /// Constructor for this connection. Will throw an exception if no devices are detected
        /// </summary>
        internal AvengersSshConn() : base("Avengers") 
        {
            List<Device> devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);

            if (!devices.Any())
                throw new Exception("No Avengers Devices Found");

            DUT = devices[0];
            OutputReceiver = StreamOutputReceiver.Instance;
        }

        /// <summary>
        /// true if device has been disconnected
        /// </summary>
        /// <returns></returns>
        protected override bool CheckForError()
        {
            return DUT.IsOffline;
        }

        protected override byte ReadByte()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// reads the remote shell output
        /// </summary>
        /// <returns>Shell output</returns>
        protected override string ReadString()
        {
            return OutputReceiver.OutputStream.ReadToEnd();
        }
        /// <summary>
        /// sends a remote shell command
        /// </summary>
        /// <param name="message">command</param>
        protected override void Write(string message)
        {
            DUT.ExecuteRootShellCommand(message, OutputReceiver);
        }
    }
}
