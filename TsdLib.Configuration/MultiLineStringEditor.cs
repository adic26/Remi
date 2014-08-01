using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TsdLib.Configuration
{
    public partial class MultiLineStringEditorForm : Form
    {
        public string Value
        {
            get { return textBox.Text; }
        }

        public MultiLineStringEditorForm(string value)
        {
            InitializeComponent();
            textBox.Lines = value.Split('\n');
        }
    }

    class MultiLineStringEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            string str = value as string;
            Debug.Assert(svc != null, "Error initializing IWindowsFormsEditorService");
            Debug.Assert(str != null, "Must pass a string.");
            using (MultiLineStringEditorForm form = new MultiLineStringEditorForm(str))
            {
                if (svc.ShowDialog(form) == DialogResult.OK)
                    str = form.Value; // update object
            }
            return str;
        }
    }
}
