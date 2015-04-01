using System;
using System.Windows.Forms;
using TsdLib;
using TsdLib.TestSystem;
using TsdLib.UI;
using TsdLib.UI.Forms;

namespace TestClient.UI.Forms
{
    /// <summary>
    /// Base UI form containing controls and code common to all TsdLib applications.
    /// Protected controls may be overridden in application implementations but private controls cannot be modified by derived UI classes.
    /// </summary>
    public partial class TestClientView : ViewBase
    {
        public override ITestCaseControl TestCaseControl { get { return testCasesMenuItem; } }

        public override IConfigControl ConfigControl { get { return multiConfigControl; } }

        public override ITestInfoDisplayControl TestInfoDisplayControl { get { return testInfoDataGridViewControl; } }

        public override IMeasurementDisplayControl MeasurementDisplayControl { get { return measurementDataGridViewControl; } }

        public override ITestSequenceControl TestSequenceControl { get { return testSequenceControl; } }

        public override ITestDetailsControl TestDetailsControl { get { return testDetailsControl; } }

        public override ITraceListenerControl TraceListenerControl { get { return traceListenerTextBoxControl; } }

        public override IProgressControl ProgressControl { get { return progressControl; } }

        /// <summary>
        /// Initializes a new instance of the UI form.
        /// </summary>
        public TestClientView()
        {
            InitializeComponent();
        }

        public override void AddArbitraryData(DataContainer dataContainer)
        {
            AddData((dynamic)dataContainer.Data);
        }

        public void AddData(Tuple<int, string> data)
        {
            MessageBox.Show("Int = " + data.Item1 + " String = " + data.Item2, "Example of user-defined data from test sequence");
        }

        public void AddData(int s)
        {
            throw new Exception("Testing appdomain exceptions");
            MessageBox.Show("GOT THE INT");
        }

        public void AddData(Tuple<int, int, int> data)
        {
            MessageBox.Show("Int1 = " + data.Item1 + " Int2 = " + data.Item2 + " Int3 = " + data.Item3, "Three-int tuple");
        }
    }
}
