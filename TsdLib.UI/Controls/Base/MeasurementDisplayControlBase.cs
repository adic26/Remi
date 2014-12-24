using TsdLib.Measurements;

namespace TsdLib.UI.Controls.Base
{
    /// <summary>
    /// Placeholder for a control to display measurements on the UI.
    /// </summary>
    public partial class MeasurementDisplayControlBase : TsdLibLabelledControl, IMeasurementDisplayControl
    {
        /// <summary>
        /// Initialize the control.
        /// </summary>
        public MeasurementDisplayControlBase()
        {
            InitializeComponent();
            Text = "Measurements";
        }

        /// <summary>
        /// Override to add a measurement to the UI display.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        public virtual void AddMeasurement(MeasurementBase measurement)
        {

        }
    }
}
