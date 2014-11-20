using System;
using System.Windows.Forms;
using TestClient.Configuration;
using TsdLib;
using TsdLib.CodeGenerator;
using TsdLib.Configuration;
using TsdLib.Controller;

namespace TestClient
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig, EventHandlers>
    {
#if INSTRUMENT_LIBRARY
        public Controller(TestDetails testDetails, IDatabaseConnection databaseConnection, bool localDomain)
            : base(testDetails, databaseConnection, new TsdLib.InstrumentLibrary.InstrumentParser(testDetails.TestSystemName, Language.CSharp.ToString()), localDomain)
        {

        }
#else
        public Controller(TestDetails testDetails, IDatabaseConnection databaseConnection, bool localDomain)
            : base(testDetails, databaseConnection, new BasicCodeParser(), localDomain)
        {

        }
#endif

#if REMICONTROL
        protected override void EditTestDetails(object sender, bool e)
        {
            if (e)
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
                    MessageBox.Show("REMIControl Exception: No tests have been registered for this PC in Remi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ex.GetType().Name);
                }
            }
            else
                base.EditTestDetails(sender, false);
        }
#endif
    }
}