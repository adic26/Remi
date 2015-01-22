using System;
using System.Collections.Generic;
using System.Linq;
using Managed.Adb;
using TsdLib.Instrument.Ssh.AdbUtilities;

namespace TsdLib.Instrument.Ssh
{
    /// <summary>
    /// An SSH Connection with Avengers devices
    /// </summary>
    class AvengersSshConn : SshConnection
    {
        public Device Dut;
        private readonly StreamOutputReceiver _outputReceiver;

        public override bool IsConnected { get { return (Dut != null); } }

        /// <summary>
        /// Constructor for this connection. Will throw an exception if no devices are detected
        /// </summary>
        internal AvengersSshConn()
            : base("Avengers") 
        {
            List<Device> devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);

            if (!devices.Any())
                throw new Exception("No Avengers Devices Found");
            
            Dut = devices[0];
            _outputReceiver = StreamOutputReceiver.Instance;
        }

        internal AvengersSshConn(Device device)
            : base(device.SerialNumber)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            Dut = device;
            _outputReceiver = StreamOutputReceiver.Instance;
        }

        /// <summary>
        /// true if device has been disconnected
        /// </summary>
        /// <returns></returns>
        protected override bool CheckForError()
        {
            return Dut.IsOffline;
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
            return _outputReceiver.OutputStream.ReadToEnd();
        }
        /// <summary>
        /// sends a remote shell command
        /// </summary>
        /// <param name="message">command</param>
        protected override void Write(string message)
        {
            Dut.ExecuteRootShellCommand(message, _outputReceiver);
        }
    }
}
