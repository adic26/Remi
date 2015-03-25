using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DBControl.Forms;
using DBControl.Helpers;
using DBControl.remiAPI;
using TsdLib.Configuration;

namespace TsdLib.DataAccess
{
    public class DatabaseTestDetails
    {
        private static readonly Task webServiceInstantiateTask;

        static DatabaseTestDetails()
        {
            webServiceInstantiateTask = Task.Run(() => Helper.InstantiateWebServices());
        }

        public static void EditTestDetails(ITestDetails testDetails)
        {

            try
            {
                webServiceInstantiateTask.Wait(2000);
                using (Request remiForm = new Request())
                    if (remiForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ScanReturnData batchInformation = remiForm.RemiData[0];
                        testDetails.TestSystemName = batchInformation.SelectedTestName;
                        string[] qraNumber = batchInformation.QRANumber.Split('-');
                        testDetails.RequestNumber = string.Join("-", qraNumber, 0, qraNumber.Length - 1);
                        testDetails.TestStage = batchInformation.TestStageName;
                        testDetails.TestType = batchInformation.JobName;
                        testDetails.UnitNumber = (uint)batchInformation.UnitNumber;
                    }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("DBControl Exception: " + ex);
            }
        }
    }
}
