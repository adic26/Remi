using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TestClient.Configuration;
using TsdLib;
using TsdLib.CodeGenerator;
using TsdLib.Configuration;
using TsdLib.Controller;
using TsdLib.TestResults;

namespace TestClient
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig>
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
                    using (REMIControl.Forms.Request remiForm = new REMIControl.Forms.Request())
                        if (remiForm.ShowDialog() == DialogResult.OK)
                        {
                            REMIControl.remiAPI.ScanReturnData batchInformation = remiForm.RemiData[0];
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
                    MessageBox.Show("REMIControl Exception: " + ex.Message);
                }
            }
            else
                base.EditTestDetails(sender, false);
        }

        protected override void TestComplete(object sender, TestResultCollection testResults)
        {
            base.TestComplete(sender, testResults);
            
            string dataLoggerXmlFile = testResults.Save(new DirectoryInfo(@"C:\TestResults"));
            Trace.WriteLine("XML results saved to " + dataLoggerXmlFile + " for database upload");
            REMIControl.DAL.Results.UploadXML(dataLoggerXmlFile);

        }
#endif
    }
}