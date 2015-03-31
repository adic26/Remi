using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestClient.Configuration;
using TsdLib.Configuration;
using TsdLib.Configuration.Details;
using TsdLib.DataAccess;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class IntervalMeasurementUpload : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //This is needed to create the header information in the measurement results file - may want to pass some of this info through station/product/test configs.
            ITestDetails testDetails = new TestDetails("TSD_Slider", new Version(1, 0), OperatingMode.Engineering, "QRA-XX-TEST", 1, "", "");



            //This is the object that saves the measurement objects to disk and uploads the files
            DatabaseResultHandler resultHandler = new DatabaseResultHandler(testDetails);


            for (int i = 0; i < 10; i++)
            {
                //The start time is used to generate the file name, we need to make sure it is always a unique file name when we're using a fast loop like this
                DateTime startTime = DateTime.Now + TimeSpan.FromMinutes(i); 

                //Create a measurement
                Measurement<double> someMeasurement = new Measurement<double>("Liftoff", 4, "lbs", 1, 10);

                DateTime endTime = startTime + TimeSpan.FromSeconds(10);

                //Save the measurement(s) to disk and upload them
                resultHandler.SaveResults(TestInfo.ToArray(), new IMeasurement[] {someMeasurement}, startTime, endTime, false);
            }

            //Make sure all logging is complete before we allow the measurement data to go out of scope when the test sequence completes
            Task.WaitAll(resultHandler.LoggingTasks.ToArray());
        }
    }
}
