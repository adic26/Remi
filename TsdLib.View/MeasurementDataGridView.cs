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

            Action addRows = () => Rows.Add(measurementObjects.Select(obj => (object)obj).ToArray());
            Action addColumns = () =>
            {
                for (int i = 1; i <= measurementObjects.Length - ColumnCount; i++)
                    Columns.Add("Parameter_" + i, "Parameter_" + i);
            };

            if (InvokeRequired)
            {
                Invoke(addRows);
                Invoke(addColumns);
            }
            else
            {
                addRows();
                addColumns();
            }
        }
    }
}
