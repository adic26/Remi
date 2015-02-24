using System.Windows.Forms;
using TsdLib.Measurements;

namespace TsdLib.UI.Controls
{
    public partial class MeasurementDataGridViewControl : UserControl, IMeasurementDisplayControl
    {
        public MeasurementDataGridViewControl()
        {
            InitializeComponent();
        }

        public void AddMeasurement(IMeasurement measurement)
        {
            int newRowIndex = dataGridView.Rows.Add();

            dataGridView.Rows[newRowIndex].Cells["Column_MeasurementName"].Value = measurement.MeasurementName;
            dataGridView.Rows[newRowIndex].Cells["Column_MeasuredValue"].Value = measurement.MeasuredValue;
            dataGridView.Rows[newRowIndex].Cells["Column_Units"].Value = measurement.Units;
            dataGridView.Rows[newRowIndex].Cells["Column_LowerLimit"].Value = measurement.LowerLimit;
            dataGridView.Rows[newRowIndex].Cells["Column_UpperLimit"].Value = measurement.UpperLimit;
            dataGridView.Rows[newRowIndex].Cells["Column_Result"].Value = measurement.Result;

            foreach (IMeasurementParameter measurementParameter in measurement.Parameters)
            {
                string parameterColumnName = "Column_" + measurementParameter.Name.Replace(" ", "");
                if (!dataGridView.Columns.Contains(parameterColumnName))
                {
                    dataGridView.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = parameterColumnName,
                        HeaderText = measurementParameter.Name,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        FillWeight = 150
                    });

                }
                dataGridView.Rows[newRowIndex].Cells[parameterColumnName].Value = measurementParameter.Value;
            }

        }

        public void ClearMeasurements()
        {
            dataGridView.Rows.Clear();
        }

        private bool _limitsAndResultdisplayed = true;
        public bool DisplayLimitsAndResult
        {
            set
            {
                dataGridView.Columns["Column_LowerLimit"].Visible = value;
                dataGridView.Columns["Column_UpperLimit"].Visible = value;
                dataGridView.Columns["Column_Result"].Visible = value;
                _limitsAndResultdisplayed = value;
            }
            get { return _limitsAndResultdisplayed; }
        }

        public void SetState(State state)
        {
            if (state.HasFlag(State.TestStarting))
                ClearMeasurements();
        }
    }
}
