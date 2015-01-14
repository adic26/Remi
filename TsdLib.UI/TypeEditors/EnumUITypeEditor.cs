//http://www.neovolve.com/post/2005/03/15/BitwiseFlags-Enum-UITypeEditor.aspx

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TsdLib.UI.TypeEditors
{
    public class EnumTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //TODO: add support for Flags enums to use CheckedListBox
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.PropertyDescriptor == null)
                return base.EditValue(context, provider, value);

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc == null)
                return base.EditValue(context, provider, value);

            ListBox listBox = new ListBox();
            listBox.Items.Clear();
            listBox.Items.AddRange(Enum.GetValues(context.PropertyDescriptor.PropertyType).Cast<object>().ToArray());
            listBox.SelectedIndexChanged += (sender, args) => edSvc.CloseDropDown();
            edSvc.DropDownControl(listBox);
            return listBox.SelectedItem;
        }
    }
}
