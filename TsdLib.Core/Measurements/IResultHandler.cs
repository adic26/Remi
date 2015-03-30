using System;
using TsdLib.Measurements;

namespace TsdLib.TestSystem
{
    public interface IResultHandler
    {
        void SaveResults(ITestInfo[] testInfo, IMeasurement[] measurements, DateTime start, DateTime end, bool publish);
    }
}