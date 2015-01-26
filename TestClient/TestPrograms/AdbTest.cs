using System;
using System.Windows.Forms;
using TestClient.Instruments;
using TsdLib.Instrument.Adb;

namespace TestClient.TestPrograms
{
    static class AdbTest
    {
        public static void Init()
        {
            AdbFactory factory = new AdbFactory();
            using (Aos_BCM4339 avg = factory.GetInstrument<Aos_BCM4339>())
            {
                string chipset = avg.GetChipsetFirmwareType();
                MessageBox.Show(string.Format(" Model: {1}{0} Serial: {2}{0} Firmware: {3}{0} Chipset: {4}{0}", Environment.NewLine, avg.ModelNumber, avg.SerialNumber, avg.FirmwareVersion, chipset));
            }
        }
    }
}
