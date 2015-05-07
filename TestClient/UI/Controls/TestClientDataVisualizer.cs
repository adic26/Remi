using System.Collections.Generic;
using TsdLib.UI;
using TsdLib.UI.Controls;

namespace TestClient.UI.Controls
{
    /// <summary>
    ///     Contains functionality to display multiple data visualizers in a TabControl.
    /// </summary>
    public partial class TestClientDataVisualizer : DataVisualizerTabPageControl
    {
        //TODO: Fill in this array initializer by instantiating data visualizers. They will be automatically added to the tab page control.
        private readonly IDataVisualizer[] _dataVisualizers =
        {
            //new ChartBasedDataVisualizer(),
            //new TableBasedDataVisualizer
        };

        /// <summary>
        ///     Initialize a new TestClientDataVisualizer
        /// </summary>
        public TestClientDataVisualizer()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets the collection of data visualizers.
        /// </summary>
        protected override IEnumerable<IDataVisualizer> DataVisualizers
        {
            get { return _dataVisualizers; }
        }
    }
}
