using System.Windows.Forms;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class AdbOverWifi : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(System.Threading.CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {//Aos_WiFi aos = Aos_WiFi.Connect(token, "10.50.81.44:555");
            Aos_WiFi aos = Aos_WiFi.Connect(token);

            MessageBox.Show(aos.GetDeviceProperties());
        }
    }
}
