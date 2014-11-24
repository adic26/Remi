using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using $safeprojectname$.Configuration;
using TsdLib;
using TsdLib.CodeGenerator;
using TsdLib.Configuration;
using TsdLib.Controller;

namespace $safeprojectname$
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig, SequenceEventHandlers>
    {

        public Controller(TestDetails testDetails, IDatabaseConnection databaseConnection, bool localDomain)
            : base(testDetails, databaseConnection, localDomain)
        {

        }


#if INSTRUMENT_LIBRARY
        protected override IEnumerable<CodeCompileUnit> GenerateCodeCompileUnits()
        {
            TsdLib.InstrumentLibrary.InstrumentParser instrumentParser = new TsdLib.InstrumentLibrary.InstrumentParser(Details.TestSystemName, Language.CSharp.ToString());

            if (!Directory.Exists("Instruments"))
                return new CodeCompileUnit[0];

            IEnumerable<CodeCompileUnit> codeCompileUnits = 
                Directory.GetFiles("Instruments", "*.xml")
                .Select(xmlFile => instrumentParser.Parse(new StreamReader(xmlFile)));

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
                            Details.JobNumber = String.Join("-", qraNumber, 0, qraNumber.Length - 1);
                            Details.TestStage = batchInformation.TestStageName;
                            Details.TestType = batchInformation.JobName;
                            Details.UnitNumber = (uint)batchInformation.UnitNumber;
                        }
                }
                catch (NullReferenceException)
                {
                    Trace.WriteLine("REMIControl Exception: No tests have been registered for this PC in Remi.");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString(), ex.GetType().Name);
                }
            }
            else
                base.EditTestDetails(sender, false);
        }
#endif
    }
}