using System.Diagnostics;
using System.Threading;
using TsdLib.Controller;
using TsdLib.TestResults;
using TsdLib.View;

namespace $safeprojectname$
{
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
