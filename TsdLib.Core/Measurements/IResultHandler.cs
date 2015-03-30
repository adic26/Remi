using System;

namespace TsdLib.Measurements
{
    public interface IResultHandler
    {
        void SaveResults(ITestInfo[] testInfo, IMeasurement[] measurements, DateTime start, DateTime end, bool publish);
    }
}