using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TsdLib.UI.Controls
{
    /// <summary>
    ///     Contains functionality to display multiple data visualizers in a <see cref="TabControl" />.
    /// </summary>
    public partial class DataVisualizerTabPageControl : UserControl, IDataVisualizerContainerControl
    {
        /// <summary>
        ///     Initialize a new <see cref="DataVisualizerTabPageControl" />
        /// </summary>
        public DataVisualizerTabPageControl()
        {
            InitializeComponent();
            Load += (s, o) => VisibleDataVisualizers = DataVisualizers;
        }

        /// <summary>
        ///     Override to define the data visualizers in the collection.
        /// </summary>
        protected virtual IEnumerable<IDataVisualizer> DataVisualizers
        {
            get { return Enumerable.Empty<IDataVisualizer>(); }
        }

        /// <summary>
        ///     Gets or sets the data visualizers that are visible on the UI.
        /// </summary>
        public IEnumerable<IDataVisualizer> VisibleDataVisualizers
        {
            get
            {
                if (tabControl.TabPages.Count == 0)
                    return Enumerable.Empty<IDataVisualizer>();
                return tabControl.TabPages.Cast<TabPage>().Select(tp => (IDataVisualizer)tp.Controls[0]);
            }
            set
            {
                if (value != null && value.Any())
                {
                    foreach (TabPage tabPage in tabControl.TabPages)
                        tabPage.Controls.Clear();
                    foreach (IDataVisualizer dataVisualizer in value)
                    {
                        Control datVisControl = dataVisualizer as Control;
                        if (datVisControl != null)
                        {
                            TabPage tabPage = new TabPage(dataVisualizer.Title);
                            tabPage.Controls.Clear();
                            tabPage.Controls.Add(datVisControl);
                            tabPage.Controls[0].Dock = DockStyle.Fill;
                            tabControl.TabPages.Add(tabPage);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Sets the state of all contained data visualizers.
        /// </summary>
        /// <param name="state">State to set.</param>
        public virtual void SetState(State state)
        {
            foreach (IDataVisualizer dataVisualizer in DataVisualizers)
                dataVisualizer.SetState(state);
        }

        /// <summary>
        ///     Adds data to all data visualizers that are configured to accept the specified data type.
        /// </summary>
        /// <typeparam name="T">Type of data being added.</typeparam>
        /// <param name="data">Data to add.</param>
        public void Add<T>(T data)
        {
            if (DataVisualizers != null)
            {
                foreach (IDataVisualizer dataVisualizer in DataVisualizers)
                {
                    IDataVisualizer<T> datVis = dataVisualizer as IDataVisualizer<T>;
                    if (datVis != null)
                        datVis.AddData(data);
                }
            }
        }
    }
}
