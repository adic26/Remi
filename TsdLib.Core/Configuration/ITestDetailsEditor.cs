namespace TsdLib.Configuration
{
    public interface ITestDetailsEditor
    {
        bool Edit(ITestDetails testDetails, bool detailsFromDatabase);
    }
}