using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using TsdLib.TestResults;

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
        public void AddMeasurement(MeasurementBase measurement)
        {
            //measurement has 6 plottable properties, plus variable number of parameters

            while (ColumnCount - 6 < measurement.Parameters.Length)
            {
                string columnNumber = (ColumnCount - 5).ToString(CultureInfo.InvariantCulture);
                Columns.Add("Parameter_" + columnNumber, "Parameter " + columnNumber);
            }

            List<object> newRowObject = new List<object> { measurement.MeasurementName, measurement.MeasuredValue, measurement.Units, measurement.LowerLimit, measurement.UpperLimit, measurement.Result.ToString() };

            newRowObject.AddRange(measurement.Parameters.Select(mp => mp.Name + "=" + mp.Value));

            Rows.Add(newRowObject.ToArray());
        }
    }
}
