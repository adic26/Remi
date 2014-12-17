using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Measurements;

namespace TsdLib.UI.Controls
{
    //[Designer(typeof(System.Windows.Forms.Design.ControlDesigner))]
    public partial class MeasurementDataGridView: UserControl
    {
        public MeasurementDataGridView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add a measurement to the DataGridView.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        public void AddMeasurement(MeasurementBase measurement)
        {
            //measurement has 6 plottable properties, plus variable number of parameters

            while (dataGridView.ColumnCount - 6 < measurement.Parameters.Length)
            {
                string columnNumber = (dataGridView.ColumnCount - 5).ToString(CultureInfo.InvariantCulture);
                dataGridView.Columns.Add("Parameter_" + columnNumber, "Parameter " + columnNumber);
            }

            List<object> newRowObject = new List<object> { measurement.MeasurementName, measurement.MeasuredValue, measurement.Units, measurement.LowerLimit, measurement.UpperLimit, measurement.Result.ToString() };

            newRowObject.AddRange(measurement.Parameters.Select(mp => mp.Name + "=" + mp.Value));

            dataGridView.Rows.Add(newRowObject.ToArray());
        }
    }
}
