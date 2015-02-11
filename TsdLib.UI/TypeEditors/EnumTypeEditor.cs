//http://www.neovolve.com/post/2005/03/15/BitwiseFlags-Enum-UITypeEditor.aspx

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TsdLib.Utilities;

namespace TsdLib.UI.TypeEditors
{
    internal class ListBoxItem
    {
        public object Value { get; private set; }

        public ListBoxItem(object item)
        {
            Value = item;
        }

        public override string ToString()
        {
            return Value.GetDescription();
        }
    }

    public class EnumTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private IWindowsFormsEditorService _edSvc;

        //TODO: add support for Flags enums to use CheckedListBox
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.PropertyDescriptor == null || value == null)
                return base.EditValue(context, provider, value);

            _edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (_edSvc == null)
                return base.EditValue(context, provider, value);

            ListBox listBox = new ListBox();
            listBox.Items.Clear();
            listBox.Items.AddRange(Enum.GetValues(context.PropertyDescriptor.PropertyType).Cast<Enum>().Select(e => new ListBoxItem(e)).Cast<object>().ToArray());
            listBox.SelectedIndex = listBox.Items.IndexOf(value);
            listBox.SelectedIndexChanged += ListBoxOnSelectedIndexChanged;

            _edSvc.DropDownControl(listBox);

            var selectedItem = listBox.SelectedItem as ListBoxItem;
            if (selectedItem != null)
                return selectedItem.Value;
            
            return value;
        }

        private void ListBoxOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                //if (listBox.SelectedItem is ListBoxItem)
                    _edSvc.CloseDropDown();
            }
        }
    }
}
