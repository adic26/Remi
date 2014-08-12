using TsdLib.Controller;
using TsdLib.TestSequence;
using $rootnamespace$.Configuration;

namespace $safeprojectname$
{
    public class Controller : ControllerBase<StationConfig, ProductConfig>
    {
        public Controller(View view, TestSequenceBase<StationConfig, ProductConfig> testSequence, bool devMode)
            : base(view, testSequence, devMode)
        {

        }
    }
}