using System.Linq;
using $safeprojectname$.Configuration;
using $safeprojectname$.View;
using TsdLib;
using TsdLib.Configuration;
using TsdLib.Controller;

namespace $safeprojectname$
{
    public class Controller : ControllerBase<$safeprojectname$View, StationConfig, ProductConfig, TestConfig, SequenceEventHandlers>
    {

        public Controller(TestDetails testDetails, IDatabaseConnection databaseConnection, bool localDomain)
            : base(testDetails, databaseConnection, localDomain)
        {

        }


#if INSTRUMENT_LIBRARY
        protected override System.Collections.Generic.IEnumerable<System.CodeDom.CodeCompileUnit> GenerateCodeCompileUnits()
        {
            TsdLib.InstrumentLibrary.InstrumentParser instrumentParser = new TsdLib.InstrumentLibrary.InstrumentParser(Details.TestSystemName, TsdLib.CodeGenerator.Language.CSharp.ToString());

            if (!System.IO.Directory.Exists("Instruments"))
                return new System.CodeDom.CodeCompileUnit[0];

            System.Collections.Generic.IEnumerable<System.CodeDom.CodeCompileUnit> codeCompileUnits =
                System.IO.Directory.GetFiles("Instruments", "*.xml")
                .Select(xmlFile => instrumentParser.Parse(new System.IO.StreamReader(xmlFile)));

            return codeCompileUnits;
        }
#endif

#if REMICONTROL
        protected override void EditTestDetails(object sender, bool getFromDatabase)
        {
            if (getFromDatabase)
            {
                try
                {
                    using (DBControl.Forms.Request remiForm = new DBControl.Forms.Request())
                        if (remiForm.ShowDialog() == DialogResult.OK)
                        {
                            DBControl.remiAPI.ScanReturnData batchInformation = remiForm.RemiData[0];
                            Details.TestSystemName = batchInformation.SelectedTestName;
                            string[] qraNumber = batchInformation.QRANumber.Split('-');
                            Details.JobNumber = string.Join("-", qraNumber, 0, qraNumber.Length - 1);
                            Details.TestStage = batchInformation.TestStageName;
                            Details.TestType = batchInformation.JobName;
                            Details.UnitNumber = (uint)batchInformation.UnitNumber;
                        }
                }
                catch (System.Exception ex)
                {
                    Trace.WriteLine("REMIControl Exception: " + ex.ToString(), ex.GetType().Name);
                }
            }
            else
                base.EditTestDetails(sender, false);
        }
#endif
    }
}