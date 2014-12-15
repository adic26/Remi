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
    class AvengersSshConn : SshConnection
    {
        private Device DUT;
        private StreamOutputReceiver OutputReceiver;

        public override bool IsConnected { get { return (DUT != null); } }

        internal AvengersSshConn() : base("Avengers") 
        {
            List<Device> devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);

            if (!devices.Any())
                throw new Exception("No Avengers Devices Found");

            DUT = devices[0];
            OutputReceiver = StreamOutputReceiver.Instance;
        }

        protected override bool CheckForError()
        {
            return DUT.IsOffline;
        }

        protected override byte ReadByte()
        {
            throw new NotImplementedException();
        }

        protected override string ReadString()
        {
            return OutputReceiver.OutputStream.ReadToEnd();
        }

        protected override void Write(string message)
        {
            DUT.ExecuteRootShellCommand(message, OutputReceiver);
        }
    }
}
