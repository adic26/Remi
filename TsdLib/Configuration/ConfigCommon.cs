using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TsdLib.Configuration
{
    public class StationConfigCommon : ConfigItem
    {
        private readonly StringCollection _assemblies;
        private readonly int _assemblyCount;

        public StationConfigCommon()
        {
            string[] tsdLibAssemblies =
                AppDomain.CurrentDomain.GetAssemblies()
                .Where(assy => assy.FullName.Contains("TsdLib"))
                .Select(assy => Regex.Match(assy.FullName, ".*(?=,.*,.*)").Value)
                .ToArray();

            _assemblies = new StringCollection();
            _assemblies.AddRange(tsdLibAssemblies);
            _assemblyCount = _assemblies.Count;
        }

        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [ReadOnly(true)]
        [Category("Description")]
        public StringCollection Assemblies
        {
            get
            {
                //Duplicates will be added if the constructor is called and the object is deserialized
                while (_assemblyCount <= _assemblies.Count - 1)
                    _assemblies.RemoveAt(_assemblies.Count - 1);
                return _assemblies;
            }
        }

        public string StationDescription { get; set; }
    }

    public class ProductConfigCommon : ConfigItem
    {
        [Category("RF")]
        public WlanChipset WlanChipset { get; set; }
    }

    public enum WlanChipset
    {
        Broadcomm,
        TexasInstruments
    }

    public class TestConfig : ConfigItem
    {
        private string selectedFile = "TestSequence.cs";

        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [Category("Test Sequence")]
        public string TestSequenceSource
        {
            get { return File.ReadAllText(selectedFile); }
            set { File.WriteAllText(selectedFile, value); }
        }
    }
}