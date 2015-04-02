using System.ComponentModel;
using TsdLib.Configuration.Connections;
using TsdLib.Configuration.Details;

namespace TsdLib.Configuration.Management
{
    public interface IConfigManagerProvider : IListSource
    {
        ITestDetails TestDetails { get; }
        IConfigConnection SharedConfigConnection { get; }
    }
}