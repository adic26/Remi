using TsdLib.Measurements;

namespace TsdLib.UI.Controls
{
    public partial class MeasurementDisplayControlBase : TsdLibLabelledControl
    {
        public MeasurementDisplayControlBase()
        {
            InitializeComponent();
            Text = "Measurements";
        }

        public virtual void AddMeasurement(MeasurementBase measurement)
        {

        }
    }
}
