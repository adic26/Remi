using TestClient.Configuration;
using TsdLib.Controller;
using TsdLib.TestSequence;

namespace TestClient
{
    class Controller : ControllerBase<StationConfig, ProductConfig, TestConfig>
    {
        public Controller(View view, TestSequenceBase testSequence)
            : base(view, testSequence)
        {

        }
    }
}