using System.Collections.Generic;

namespace TsdLib.Configuration
{
    public interface ITestCase
    {
        string Name { get; }
        IEnumerable<string> TestConfigs { get; }
        IEnumerable<string> Sequences { get;}
    }
}