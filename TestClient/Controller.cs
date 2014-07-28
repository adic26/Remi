using TsdLib.Controller;
using TsdLib.TestSequence;

namespace TestClient
{
    public class Controller : ControllerBase
    {
        public Controller(View view, TestSequenceBase testSequence)
            : base(view, testSequence)
        {

        }
    }
}