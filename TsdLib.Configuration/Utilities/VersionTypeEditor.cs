using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TsdLib.Configuration.Utilities
{
    /// <summary>
    /// A <see cref="UITypeEditor"/> used to edit <see cref="Version"/> objects in a <see cref="PropertyGrid"/>.
    /// </summary>
    public class VersionTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.PropertyDescriptor == null || value == null)
                return base.EditValue(context, provider, value);

            var edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null)
                return base.EditValue(context, provider, value);

            MaskedTextBox maskedTextBox = new MaskedTextBox("##0.##0.###0.####0");
            edSvc.DropDownControl(maskedTextBox);

            Version newValue;
            if (Version.TryParse(maskedTextBox.Text, out newValue))
                return newValue;

            return base.EditValue(context, provider, value);
        }
    }
}