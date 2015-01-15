﻿using System.Linq;
using TestClient.Configuration;
using TestClient.View;
using TsdLib;
using TsdLib.Configuration;
using TsdLib.TestSystem.CodeGenerator;
using TsdLib.TestSystem.Controller;

namespace TestClient
{
    public class Controller : ControllerBase<TestClientView, StationConfig, ProductConfig, TestConfig>
    {
        public Controller(ITestDetails testDetails, IConfigConnection databaseConnection, bool localDomain)
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
        protected override System.Collections.Generic.IEnumerable<System.CodeDom.CodeCompileUnit> GenerateAdditionalCodeCompileUnits(string nameSpace)
        {
            System.Collections.Generic.List<System.CodeDom.CodeCompileUnit> codeCompileUnits = new System.Collections.Generic.List<System.CodeDom.CodeCompileUnit>();

            if (!System.IO.Directory.Exists("Instruments"))
                return new System.CodeDom.CodeCompileUnit[0];

            string[] instrumentXmlFiles = System.IO.Directory.GetFiles("Instruments", "*.xml");
            TsdLib.InstrumentLibrary.InstrumentParser instrumentXmlParser = new TsdLib.InstrumentLibrary.InstrumentParser(nameSpace, Language.CSharp.ToString());
            codeCompileUnits.AddRange(instrumentXmlFiles.Select(xmlFile => instrumentXmlParser.Parse(new System.IO.StreamReader(xmlFile))));

            if (System.IO.Directory.Exists(@"Instruments\Helpers"))
            {
                string[] instrumentHelperCsFiles = System.IO.Directory.GetFiles(@"Instruments\Helpers", "*.cs");
                string[] instrumentHelperVbFiles = System.IO.Directory.GetFiles(@"Instruments\Helpers", "*.vb");
                BasicCodeParser instrumentHelperParser = new BasicCodeParser();
                codeCompileUnits.AddRange(instrumentHelperCsFiles.Concat(instrumentHelperVbFiles).Select(xmlFile => instrumentHelperParser.Parse(new System.IO.StreamReader(xmlFile))));
            }

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

        protected override void PublishResults(TsdLib.Measurements.ITestResults results)
        {
            string xmlFile = TsdLib.SpecialFolders.GetResultsFile(results.Details, results.Summary, "xml").Name;
            System.Diagnostics.Trace.WriteLine("Uploading results to database...");
            DBControl.DAL.Results.UploadXML(xmlFile);
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

        protected override void PublishResults(TsdLib.Measurements.ITestResults results)
        {
            System.Diagnostics.Trace.WriteLine("Simulating database upload");
            System.Threading.Thread.Sleep(10000);
            System.Diagnostics.Trace.WriteLine("Done uploading");
        }
#endif

    }
}