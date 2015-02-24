using System.Collections.Generic;
using System.Linq;

namespace TsdLib.Configuration.TestCases
{
    /// <summary>
    /// Represents a grouping of one or more test configs combined with one or more test sequences.
    /// </summary>
    public class TestCase : ITestCase
    {

        public string Name { get; private set; }
        public IEnumerable<string> TestConfigs { get; private set; }
        public IEnumerable<string> Sequences { get; private set; }

        public TestCase(string name, IEnumerable<ITestConfig> testConfigs, IEnumerable<ISequenceConfig> sequences)
            : this(name, testConfigs.Select(tc => tc.Name), sequences.Select(seq => seq.Name))
        {

        }

        public TestCase(string name, IEnumerable<string> testConfigNames, IEnumerable<string> sequenceNames)
        {
            Name = name;
            TestConfigs = testConfigNames;
            Sequences = sequenceNames;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
