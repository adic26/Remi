using System.Windows.Forms;

namespace TsdLib.Forms
{
    public partial class PropertyGridEditor : Form
    {
        /// <summary>
        /// Initialize a new <see cref="PropertyGridEditor"/>
        /// </summary>
        /// <param name="obj">An object with public properties.</param>
        public PropertyGridEditor(object obj)
        {
            InitializeComponent();
            
            propertyGrid.SelectedObject = obj;

            Text = obj.GetType().Name + " Editor";
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }
    }
}
