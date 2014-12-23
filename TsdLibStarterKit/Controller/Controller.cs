using System.Linq;
using $safeprojectname$.Configuration;
using $safeprojectname$.View;
using TsdLib;
using TsdLib.Configuration;
using TsdLib.Measurements;
using TsdLib.TestSystem.CodeGenerator;
using TsdLib.TestSystem.Controller;

namespace $safeprojectname$
{
    public class Controller : ControllerBase<$safeprojectname$View, StationConfig, ProductConfig, TestConfig>
    {
        public Controller(TestDetails testDetails, IConfigConnection databaseConnection, bool localDomain)
            : base(testDetails, databaseConnection, localDomain)
        {
#if REMICONTROL
            _webServiceInstantiateTask = System.Threading.Tasks.Task.Run(() => DBControl.Helpers.Helper.InstantiateWebServices());
#endif
#if simREMICONTROL
            _webServiceInstantiateTask = System.Threading.Tasks.Task.Run(() => DBControl.Helpers.Helper.InstantiateWebServices());
#endif

        }

#if INSTRUMENT_LIBRARY
        protected override System.Collections.Generic.IEnumerable<System.CodeDom.CodeCompileUnit> GenerateCodeCompileUnits()
        {
            System.Collections.Generic.List<System.CodeDom.CodeCompileUnit> codeCompileUnits = new System.Collections.Generic.List<System.CodeDom.CodeCompileUnit>();

            if (!System.IO.Directory.Exists("Instruments"))
                return new System.CodeDom.CodeCompileUnit[0];

            string[] instrumentXmlFiles = System.IO.Directory.GetFiles("Instruments", "*.xml");
            TsdLib.InstrumentLibrary.InstrumentParser instrumentXmlParser = new TsdLib.InstrumentLibrary.InstrumentParser(Details.TestSystemName, Language.CSharp.ToString());
            codeCompileUnits.AddRange(instrumentXmlFiles.Select(xmlFile => instrumentXmlParser.Parse(new System.IO.StreamReader(xmlFile))));

            BasicCodeParser instrumentHelperParser = new BasicCodeParser();
            string[] instrumentHelperCsFiles = System.IO.Directory.GetFiles(@"Instruments\Helpers", "*.cs");
            string[] instrumentHelperVbFiles = System.IO.Directory.GetFiles(@"Instruments\Helpers", "*.vb");
            codeCompileUnits.AddRange(instrumentHelperCsFiles.Concat(instrumentHelperVbFiles).Select(xmlFile => instrumentHelperParser.Parse(new System.IO.StreamReader(xmlFile))));

            return codeCompileUnits;
        }
#endif

#if REMICONTROL
        private readonly System.Threading.Tasks.Task _webServiceInstantiateTask;

        protected override void EditTestDetails(object sender, bool getFromDatabase)
        {
            if (getFromDatabase)
            {
                try
                {
                    _webServiceInstantiateTask.Wait(2000);
                    using (DBControl.Forms.Request remiForm = new DBControl.Forms.Request())
                        if (remiForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                    System.Diagnostics.Trace.WriteLine("DBControl Exception: " + ex);
                }
            }
            else
                base.EditTestDetails(sender, false);
        }

        protected override void PublishResults(TsdLib.TestResults.TestResultCollection results)
        {
            string dataLoggerXmlFile = results.Save(new System.IO.DirectoryInfo(@"C:\TestResults"));
            System.Diagnostics.Trace.WriteLine("Uploading results to database...");
            DBControl.DAL.Results.UploadXML(dataLoggerXmlFile);
            System.Diagnostics.Trace.WriteLine("Upload complete. Results can be viewed at " + results.Details.JobNumber);
            
        }
#endif

#if simREMICONTROL
        private readonly System.Threading.Tasks.Task _webServiceInstantiateTask;

        protected override void EditTestDetails(object sender, bool getFromDatabase)
        {
            if (getFromDatabase)
            {
                try
                {
                    _webServiceInstantiateTask.Wait(2000);
                    using (DBControl.Forms.Request remiForm = new DBControl.Forms.Request())
                        if (remiForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                    System.Diagnostics.Trace.WriteLine("DBControl Exception: " + ex);
                }
            }
            else
                base.EditTestDetails(sender, false);
        }

        protected override void PublishResults(ITestResults results)
        {
            System.Diagnostics.Trace.WriteLine("Simulating database upload");
            System.Threading.Thread.Sleep(10000);
            System.Diagnostics.Trace.WriteLine("Done uploading");
        }
#endif

    }
}