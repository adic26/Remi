using TestClient.Configuration;
using TestClient.UI.Forms;
using TsdLib.Configuration.Connections;
using TsdLib.Configuration.Details;
using TsdLib.Configuration.Management;
using TsdLib.TestSystem.Controller;

namespace TestClient
{
    public class Controller : ControllerBase<TestClientView, StationConfig, ProductConfig, TestConfig>
    {
        public Controller(ITestDetails testDetails, IConfigConnection databaseConnection, bool localDomain)
            : base(testDetails, databaseConnection, localDomain)
        {

        }

#if INSTRUMENT_LIBRARY
        protected override System.Collections.Generic.IEnumerable<System.CodeDom.CodeCompileUnit> GenerateAdditionalCodeCompileUnits(string nameSpace)
        {
            return TsdLib.InstrumentLibrary.Tools.InstrumentFinder.GenerateCodeCompileUnits(new System.IO.DirectoryInfo(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Instruments")), nameSpace);
        }
#endif

#if REMICONTROL
        protected override ITestDetailsEditor CreateTestDetailsEditor()
        {
            return new TsdLib.DataAccess.DatabaseTestDetailsEditor();
        }

        protected override TsdLib.Measurements.IResultHandler CreateResultHandler(ITestDetails testDetails)
        {
            return new TsdLib.DataAccess.DatabaseResultHandler(testDetails);
        }
#endif
    }
}