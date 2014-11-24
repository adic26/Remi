using System.Diagnostics;
using System.Threading;
using TsdLib.Controller;
using TsdLib.TestResults;
using TsdLib.View;

namespace TestClient
{
    public class SequenceEventHandlers : SequenceEventHandlersBase
    {
        public SequenceEventHandlers(IView view)
            : base(view) { }

#if REMICONTROL
        protected override void TestComplete(object sender, TestResultCollection testResults)
        {
            base.TestComplete(sender, testResults);

            string dataLoggerXmlFile = testResults.Save(new DirectoryInfo(@"C:\TestResults"));
            Trace.WriteLine("XML results saved to " + dataLoggerXmlFile + " for database upload");
            DBControl.DAL.Results.UploadXML(dataLoggerXmlFile);
        }
#endif

#if simREMICONTROL
        protected override void TestComplete(object sender, TestResultCollection testResults)
        {
            base.TestComplete(sender, testResults);

            Trace.WriteLine("Simulating database upload");
            Thread.Sleep(10000);
            Trace.WriteLine("Done uploading");
        }
#endif
    }
}
