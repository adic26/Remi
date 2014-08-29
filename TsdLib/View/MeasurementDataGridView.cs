using System;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Measurements;

namespace TsdLib.View
{
    /// <summary>
    /// Adds functionality to use a standard DataGridView control to display TsdLib.Measurements objects.
    /// </summary>
    public class MeasurementDataGridView : DataGridView
    {
        /// <summary>
        /// Add a measurement to the DataGridView.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        public void AddMeasurement(Measurement measurement)
        {
            string[] measurementObjects = measurement.ToString(",").Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);

            Action addColumns = () =>
            {
                for (int i = 1; i <= measurementObjects.Length - ColumnCount; i++)
                    Columns.Add("Parameter_" + i, "Parameter_" + i);
            };
            Action addRow = () => Rows.Add(measurementObjects.Select(obj => (object)obj).ToArray());

            if (InvokeRequired)
            {
                Invoke(addColumns);
                Invoke(addRow);
            }
            else
            {
                addRow();
                addColumns();
            }
        }
    }
}
