using System.Threading.Tasks;
using System.Windows.Forms;
using DBControl.Forms;
using DBControl.Helpers;
using DBControl.remiAPI;
using TsdLib.Configuration;

namespace TsdLib.DataAccess
{
    public class DatabaseTestDetailsEditor : TestDetailsEditor
    {
        private static readonly Task webServiceInstantiateTask;

        static DatabaseTestDetailsEditor()
        {
            webServiceInstantiateTask = Task.Run(() => Helper.InstantiateWebServices());
        }

        protected override bool UpdateTestDetails(ITestDetails testDetails, bool detailsFromDatabase)
        {
            if (detailsFromDatabase)
            {
                webServiceInstantiateTask.Wait(2000);
                using (Request remiForm = new Request())
                {
                    if (remiForm.ShowDialog() == DialogResult.OK)
                    {
                        ScanReturnData batchInformation = remiForm.RemiData[0];
                        testDetails.TestSystemName = batchInformation.SelectedTestName;
                        string[] qraNumber = batchInformation.QRANumber.Split('-');
                        testDetails.RequestNumber = string.Join("-", qraNumber, 0, qraNumber.Length - 1);
                        testDetails.TestStage = batchInformation.TestStageName;
                        testDetails.TestType = batchInformation.JobName;
                        testDetails.UnitNumber = (uint) batchInformation.UnitNumber;
                        return true;
                    }
                    return false;
                }
            }
            return base.UpdateTestDetails(testDetails, false);
        }
    }
}
