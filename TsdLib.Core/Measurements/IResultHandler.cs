using System;

namespace TsdLib.Measurements
{
    public interface IResultHandler : IDisposable
    {
        void SaveResults(ITestInfo[] testInfo, IMeasurement[] measurements, DateTime start, DateTime end, bool publish);
    }
}