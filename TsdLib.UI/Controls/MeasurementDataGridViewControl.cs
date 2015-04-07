using System;
using System.Windows.Forms;
using TsdLib.Measurements;

namespace TsdLib.UI.Controls
{
    public partial class MeasurementDataGridViewControl : UserControl, IMeasurementDisplayControl
    {
        public virtual bool DisplayMeasurementName
        {
            get { return Column_MeasurementName.Visible; }
            set { Column_MeasurementName.Visible = value; }
        }
        public virtual bool DisplayMeasuredValue
        {
            get { return Column_MeasuredValue.Visible; }
            set { Column_MeasuredValue.Visible = value; }
        }
        public virtual bool DisplayUnits
        {
            get { return Column_Units.Visible; }
            set { Column_Units.Visible = value; }
        }
        public virtual bool DisplayLowerLimit
        {
            get { return Column_LowerLimit.Visible; }
            set { Column_LowerLimit.Visible = value; }
        }
        public virtual bool DisplayUpperLimit
        {
            get { return Column_UpperLimit.Visible; }
            set { Column_UpperLimit.Visible = value; }
        }
        public virtual bool DisplayResult
        {
            get { return Column_Result.Visible; }
            set { Column_Result.Visible = value; }
        }

        public MeasurementDataGridViewControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add a measurement to the UI display.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
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

        /// <summary>
        /// Clears all measurements from the UI display.
        /// </summary>
        public void ClearMeasurements()
        {
            dataGridView.Rows.Clear();
        }

        /// <summary>
        /// Set the behaviour of the control based on the current state of the test system.
        /// </summary>
        /// <param name="state">The current state of the test system.</param>
        public void SetState(State state)
        {
            if (state.HasFlag(State.TestStarting))
                ClearMeasurements();
        }
    }
}
