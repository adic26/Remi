using System.Linq;
using TestClient.Configuration;
using TestClient.UI.Forms;
using TsdLib.Configuration;
using TsdLib.Configuration.Connections;
using TsdLib.CodeGenerator;
using TsdLib.DataAccess;
using TsdLib.TestSystem;
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
        //TODO: move this to TsdLib.InstrumentLibrary.Tools
        protected override System.Collections.Generic.IEnumerable<System.CodeDom.CodeCompileUnit> GenerateAdditionalCodeCompileUnits(string nameSpace)
        {
            System.Collections.Generic.List<System.CodeDom.CodeCompileUnit> codeCompileUnits = new System.Collections.Generic.List<System.CodeDom.CodeCompileUnit>();

            if (!System.IO.Directory.Exists("Instruments"))
                return new System.CodeDom.CodeCompileUnit[0];

            string[] instrumentXmlFiles = System.IO.Directory.GetFiles("Instruments", "*.xml");
            TsdLib.InstrumentLibrary.Tools.InstrumentParser instrumentXmlParser = new TsdLib.InstrumentLibrary.Tools.InstrumentParser(nameSpace, Language.CSharp.ToString());
            codeCompileUnits.AddRange(instrumentXmlFiles.Select(xmlFile => instrumentXmlParser.Parse(new System.IO.StreamReader(xmlFile))));

            if (System.IO.Directory.Exists(@"Instruments\Helpers"))
            {
                string[] instrumentHelperCsFiles = System.IO.Directory.GetFiles(@"Instruments\Helpers", "*.cs");
                string[] instrumentHelperVbFiles = System.IO.Directory.GetFiles(@"Instruments\Helpers", "*.vb");
                TsdLib.BasicCodeParser instrumentHelperParser = new TsdLib.BasicCodeParser();
                codeCompileUnits.AddRange(instrumentHelperCsFiles.Concat(instrumentHelperVbFiles).Select(xmlFile => instrumentHelperParser.Parse(new System.IO.StreamReader(xmlFile))));
            }

            return codeCompileUnits;
        }
#endif

#if REMICONTROL
        protected override ITestDetailsEditor CreateTestDetailsEditor()
        {
            return new DatabaseTestDetailsEditor();
        }

        protected override IResultHandler CreateResultHandler(ITestDetails testDetails)
        {
            return new DatabaseResultHandler(testDetails);
        }
#endif
    }
}