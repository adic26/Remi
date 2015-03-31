using System;

namespace TsdLib.Configuration.Details
{
    public interface ITestDetailsEditor : IDisposable
    {
        bool Edit(ITestDetails testDetails, bool detailsFromDatabase);
    }
}