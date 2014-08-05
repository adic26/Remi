using System.ComponentModel;
using System.Drawing.Design;
using System.IO;

namespace TsdLib.Configuration
{
    public class TestConfigCommon : ConfigItem
    {
        [Category("Options")]
        public bool ContinueOnFailure { get; set; }

        private string _testSequenceSourceString;

        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [Category("Test Sequence")]
        public string TestSequenceSource
        {
            get
            {
                if (string.IsNullOrEmpty(_testSequenceSourceString))
                    _testSequenceSourceString = File.ReadAllText("TestSequence.cs");
                return _testSequenceSourceString;
            }
            set
            {
                _testSequenceSourceString = value;
                File.WriteAllText("TestSequence.cs", value);
            }
        }
    }
}