using System.CodeDom.Compiler;
using TestClient.Configuration;
using TsdLib;
using TsdLib.Configuration;
using TsdLib.Controller;

namespace TestClient
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig>
    {
        public Controller(TestDetails testDetails, IDatabaseConnection databaseConnection, ICodeParser instrumentParser, bool localDomain)
            : base(testDetails, databaseConnection, instrumentParser, localDomain)
        {
            
        }
    }
}