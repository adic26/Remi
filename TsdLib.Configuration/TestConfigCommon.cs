using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace TsdLib.Configuration
{
    public class TestConfigCommon<TStationConfig, TProductConfig> : ConfigItem
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
    {
        [Category("Options")]
        public bool ContinueOnFailure { get; set; }

        //Temporary Execute() method for development purposes
        public void Execute(TStationConfig stationConfig, TProductConfig productConfig)
        {
            Trace.WriteLine(string.Format("Executing {0} sequence with {1} station config and {2} product config.", Name, stationConfig, productConfig));
            foreach (string testStep in TestSequenceSource)
                Trace.WriteLine("--Step: " + testStep);
        }

        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Test Sequence")]
        public StringCollection TestSequenceSource { get; set; }
    }

}