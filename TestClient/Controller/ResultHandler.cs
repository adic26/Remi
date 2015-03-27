﻿using TsdLib.DataAccess;
using TsdLib.Configuration;
using TsdLib.Measurements;
using TsdLib.TestSystem.Controller;

namespace TestClient
{
    public class ResultHandler : ResultHandlerBase
    {
        public ResultHandler(ITestDetails testDetails)
            : base(testDetails)
        {

        }
#if REMICONTROL
        protected override void PublishResults(ITestResults results)
        {
            DatabaseTestResults.PublishResults(results);
        }
#endif
    }
}
