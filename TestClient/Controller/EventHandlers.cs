﻿using System.Diagnostics;
using System.IO;
using TsdLib.Controller;
using TsdLib.TestResults;
using TsdLib.View;

namespace TestClient
{
    public class EventHandlers : EventHandlersBase
    {
        public EventHandlers(IView view)
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
    }
}
