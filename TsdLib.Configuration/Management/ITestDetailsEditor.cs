using TsdLib.Configuration.Details;

namespace TsdLib.Configuration.Management
{
    public interface ITestDetailsEditor
    {
        ITestSystemIdentityManager IdentityManager { get; }
        void Edit(ITestDetails testDetails, bool detailsFromDatabase);
    }
}