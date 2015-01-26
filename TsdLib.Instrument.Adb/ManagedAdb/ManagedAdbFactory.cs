using System;
using System.Collections.Generic;
using System.Linq;
using Managed.Adb;

namespace TsdLib.Instrument.Adb.ManagedAdb
{
    [Obsolete("Should use the AdbFactory")]
    public class ManagedAdbFactory : FactoryBase<ManagedAdbConnection>
    {
        protected override IEnumerable<string> SearchForInstruments()
        {
            return new List<Device>(AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress)).Select(d => d.SerialNumber);
        }

        protected override string GetInstrumentIdentifier(ManagedAdbConnection connection, IdQueryAttribute idAttribute)
        {
            return "Avengers";
        }

        protected override ManagedAdbConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes)
        {
            Device aosDevice = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress).FirstOrDefault(device => device.SerialNumber == address);
            if (aosDevice == null)
                throw new Exception("Could not connect to any Adb devices.");
            return new ManagedAdbConnection(aosDevice);
        }
    }
}
