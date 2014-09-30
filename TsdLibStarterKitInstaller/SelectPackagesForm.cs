using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TsdLibStarterKitInstaller
{
    public partial class SelectPackagesForm : Form
    {
        public SelectPackagesForm()
        {
            InitializeComponent();
        }

        public SelectPackagesForm(IEnumerable<string> data)
        {
            InitializeComponent();
            
            listView_Packages.Items.AddRange(data.Select(d => new ListViewItem(d)).ToArray());
        }
    }
}
