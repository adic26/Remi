using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Contains base station config properties common to every system. Station config properties include those related to a specific system (based on host PC), eg. port/instrument addresses, physical location, etc.
    /// Station config is used to parameterize the test sequence, customizing the sequence to operate on different stations (ie. instruments with different addresses).
    /// </summary>
    public class StationConfigCommon : ConfigItem
    {
        private readonly StringCollection _assemblies;
        private readonly int _assemblyCount;

        /// <summary>
        /// Default constuctor required to initialize default values.
        /// </summary>
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

        /// <summary>
        /// Gets a collection of the TsdLib assemblies currently loaded.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a description of the test station.
        /// </summary>
        public string StationDescription { get; set; }
    }

    /// <summary>
    /// Contains base station config properties common to every type of product. Product config properties include those related to a specific DUT model, eg. radio bands, CPU chipset, etc.
    /// Product config is used to parameterize the test sequence, customizing the sequence to operate on different DUT models.
    /// </summary>
    public class ProductConfigCommon : ConfigItem
    {
        /// <summary>
        /// Gets or sets the type of WLAN chipset.
        /// </summary>
        [Category("RF")]
        public WlanChipset WlanChipset { get; set; }
    }

    /// <summary>
    /// Describes the various types of WLAN chipsets. Different platforms require different communication/control implementations.
    /// </summary>
    public enum WlanChipset
    {
        /// <summary>
        /// WLAN chipset based on the Broadcomm platform.
        /// </summary>
        Broadcomm,
        /// <summary>
        /// WLAN chipset based on the Texas Instruments platform.
        /// </summary>
        TexasInstruments
    }

    ///// <summary>
    ///// Contains base test config properties common to every type of test. Test config properties include those related to a test system, eg. temperature profile, loop iterations, etc.
    ///// Test config is used to parameterize the test sequence, allowing the same sequence to perform different test cases (ie. strict vs. relaxed or functional vs. parametric)
    ///// </summary>
    //public class TestConfigCommon : ConfigItem
    //{

    //}

    ///// <summary>
    ///// Conatins the step-by-step instrument control and measurement capturing instructions that make up a test sequence.
    ///// Can be parameterized by StationConfig, ProductConfig, TestConfig.
    ///// </summary>
    //public class TestSequence : ConfigItem
    //{
    //    private const string LocalFile = "TestSequence.cs";

    //    /// <summary>
    //    /// The source code containing the step-by-step instructions.
    //    /// </summary>
    //    [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
    //    [Category("Test Sequence")]
    //    public string TestSequenceSourceCode
    //    {
    //        get { return File.ReadAllText(LocalFile); }
    //        set { File.WriteAllText(LocalFile, value); }
    //    }
    //}

    /// <summary>
    /// Conatins the step-by-step instrument control and measurement capturing instructions that make up a test sequence.
    /// </summary>
    //[Obsolete("Should create a system-specific TestConfig derived from TestConfigCommon")]
    public class TestConfig : ConfigItem
    {
        private const string SelectedFile = "TestSequence.cs";

        /// <summary>
        /// The source code containing the step-by-step instructions.
        /// </summary>
        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [Category("Test Sequence")]
        public string TestSequenceSource
        {
            get { return File.ReadAllText(SelectedFile); }
            set { File.WriteAllText(SelectedFile, value); }
        }
    }
}