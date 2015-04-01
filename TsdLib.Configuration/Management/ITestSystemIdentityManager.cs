using System;
using TsdLib.Configuration.Details;

namespace TsdLib.Configuration.Management
{
    public interface ITestSystemIdentityManager
    {
        void InitializeTestDetails(ITestDetails testDetails);
        event EventHandler TestSystemIdentityChanged;
        bool FireIfModified();
    }
}