using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TsdLib.View
{
    class MeasurementDataGridView : DataGridView
    {
        public void AddMeasurement(Measurement measurement)
        {
            string[] measurementObjects = measurement.ToString(",").Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i <= measurementObjects.Length - ColumnCount; i ++)
                Columns.Add("Parameter_" + i, "Parameter_" + i);

            Rows.Add(measurementObjects.Select(obj => (object)obj).ToArray());
        }
    }
}
