using TestClient.Configuration;
using TsdLib.UI.Forms;

namespace TestClient.View
{
    public partial class TestClientView : ViewBase<StationConfig, ProductConfig, TestConfig>
    {
        public TestClientView()
        {
            InitializeComponent();
        }
    }
}
