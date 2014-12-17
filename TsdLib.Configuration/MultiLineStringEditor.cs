using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TsdLib.Configuration
{
    /// <summary>
    /// UI that can be used to edit a multi-line string at design time or from a PropertyGrid.
    /// </summary>
    public partial class MultiLineStringEditorForm : Form
    {
        /// <summary>
        /// Gets the edited value.
        /// </summary>
        public string Value
        {
            get { return textBox.Text; }
        }

        /// <summary>
        /// Initialize a new MultiLineStringEditorForm to edit the specified string.
        /// </summary>
        /// <param name="value">String to edit.</param>
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
            bool hashSet = false;
            if (provider == null)
                throw new Exception("Could not initialize IServiceProvider");

            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (svc == null)
                throw new Exception("Could not initialize IWindowsFormsEditorService");

            string str;
// ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (value is HashSet<string>)
            {
                hashSet = true;
                str = string.Join(Environment.NewLine, (IEnumerable<string>) value);
            }
            else
                str = value as string;

            if (str == null)
                throw new ArgumentException("Must pass a string or IEnumerable<string>.", "value");
            
            using (MultiLineStringEditorForm form = new MultiLineStringEditorForm(str))
            {
                if (svc.ShowDialog(form) == DialogResult.OK)
                    str = form.Value; // update object
            }
            if (hashSet)
            {
                HashSet<string> seq = new HashSet<string>( str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) );
                return seq;
            }

            string[] lines = str.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(Environment.NewLine, lines);
            
        }
    }
}
