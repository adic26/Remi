using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TestClient.Configuration;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class UiDeadlockTest : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            int numberOfIterations = 1000;

            Task.Run(() => TraceSomeMessages("first task", numberOfIterations), token);
            Task.Run(() => TraceSomeMessages("second task", numberOfIterations), token);
            Task.Run(() => TraceSomeMessages("third task", numberOfIterations), token);

            TraceSomeMessages("sequence thread", numberOfIterations);
        }

        private void TraceSomeMessages(string message, int numberOfTimes)
        {
            for (var i = 0; i < numberOfTimes; i++)
                Trace.WriteLine(string.Format("{0} number: {1} : Thread ID: {2}", message, i, Thread.CurrentThread.ManagedThreadId));
        }
    }
}