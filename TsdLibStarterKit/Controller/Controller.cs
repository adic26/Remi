using System.Diagnostics;
using System.Linq;
using System.Threading;
using TsdLib;
using TsdLib.Configuration;
using TsdLib.Controller;
using TsdLib.TestResults;
using TsdLib.View;
using $safeprojectname$.Configuration;
using $safeprojectname$.View;

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
                    Trace.WriteLine("REMIControl Exception: " + ex.ToString(), ex.GetType().Name);
                }
            }
            else
                base.EditTestDetails(sender, false);
        }
#endif
    }

    public class SequenceEventHandlers : SequenceEventHandlersBase
    {
        public SequenceEventHandlers(IView view)
            : base(view) { }

#if REMICONTROL
        protected override void TestComplete(object sender, TestCompleteEventArgs eventArgs)
        {
            base.TestComplete(sender, eventArgs);

            if (eventArgs.PublishResults)
            {
                string dataLoggerXmlFile = eventArgs.TestResults.Save(new System.IO.DirectoryInfo(@"C:\TestResults"));
                Trace.WriteLine("Uploading results to database...");
                DBControl.DAL.Results.UploadXML(dataLoggerXmlFile);
                Trace.WriteLine("Upload complete. Results can be viewed at " + eventArgs.TestResults.Details.JobNumber);
            }
        }
#endif

#if simREMICONTROL
        protected override void TestComplete(object sender, TestCompleteEventArgs eventArgs)
        {
            base.TestComplete(sender, eventArgs);

            if (eventArgs.PublishResults)
            {
                Trace.WriteLine("Simulating database upload");
                System.Threading.Thread.Sleep(10000);
                Trace.WriteLine("Done uploading");
            }
        }
#endif
    }
}