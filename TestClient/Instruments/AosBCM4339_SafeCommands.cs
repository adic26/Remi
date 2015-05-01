using System;
using TsdLib.Instrument.Adb;

namespace TestClient.Instruments
{
    public class AosBCM4339_SafeCommands : Aos_BCM4339
    {
        public AosBCM4339_SafeCommands(string address = null)
            : base(Connect(address).Connection as AdbConnection)
        {

        }

        public override string GetChipsetFirmwareType()
        {
            try
            {
                return base.GetChipsetFirmwareType();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
