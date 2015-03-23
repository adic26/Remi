using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TsdLib.Forms
{
    public partial class VersionEditorForm : Form
    {
        public Version TargetVersion
        {
            get { return new Version(int.Parse(numericUpDown_Major.Text), int.Parse(numericUpDown_Minor.Text)); }
        }

        public VersionEditorForm(Version existing)
        {
            InitializeComponent();
            numericUpDown_Major.Value = existing.Major;
            numericUpDown_Minor.Value = existing.Minor;
        }
    }

    public class VersionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider == null)
                throw new Exception("Could not initialize IServiceProvider");

            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (svc == null)
                throw new Exception("Could not initialize IWindowsFormsEditorService");

            Version existingVersion = value as Version;
            if (existingVersion == null)
                throw new ArgumentException("Must pass a System.Version", "value");

            using (VersionEditorForm form = new VersionEditorForm(existingVersion))
                if (svc.ShowDialog(form) == DialogResult.OK)
                    return form.TargetVersion;
                else
                    return existingVersion;
        }
    }
}
