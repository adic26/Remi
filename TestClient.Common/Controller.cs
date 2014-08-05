using TsdLib.Controller;
using TsdLib.TestSequence;

namespace TestClient
{
    public class Controller : ControllerBase<StationConfig, ProductConfig, TestConfig>
    {
        public Controller(View view, TestSequenceBase<StationConfig, ProductConfig> testSequence, bool devMode)
            : base(view, testSequence, devMode)
        {

        }
    }
}