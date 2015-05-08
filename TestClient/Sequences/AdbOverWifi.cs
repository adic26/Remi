using System.Windows.Forms;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class AdbOverWifi : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(System.Threading.CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            Aos_WiFi aos = Aos_WiFi.Connect(token);

            MessageBox.Show("Connected to AOS device via WiFi. Please disconnect USB cable");

            MessageBox.Show(aos.GetDeviceProperties());
        }
    }
}
