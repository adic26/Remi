using System;
using System.Windows.Forms;
using TsdLib.Observer;
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

        /// <summary>
        /// Add a general data object to the UI. Uses dynamic method overload resolution to automatically bind to the correct method based on the type of data.
        /// </summary>
        /// <param name="dataContainer">Data to add.</param>
        public override void AddData(DataContainer dataContainer)
        {
            addData((dynamic)dataContainer.Data);
        }

        public void addData(Tuple<int, string> data)
        {
            MessageBox.Show("Int = " + data.Item1 + " String = " + data.Item2);
        }
    }
}
