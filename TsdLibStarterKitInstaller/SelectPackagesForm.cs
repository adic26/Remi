using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NuGet;

namespace TsdLibStarterKitInstaller
{
    public sealed partial class SelectPackagesForm : Form
    {
        private readonly List<IPackage> _packages; 
        public HashSet<IPackage> SelectedPackages { get; private set; }

        public SelectPackagesForm(string repoName, IEnumerable<IPackage> packages)
        {
            InitializeComponent();

            Text = "NuGet Repository: " + repoName;
            _packages = new List<IPackage>(packages);

            listView_Packages.Items.AddRange(_packages.Select(p => new ListViewItem(new[] 
            {
                p.Title,
                p.Version.ToString(),
                p.Description
            })).ToArray());
        }

        private void button_OK_Click(object sender, System.EventArgs e)
        {
            SelectedPackages = new HashSet<IPackage>();
            foreach (int checkedIndex in listView_Packages.CheckedItems.Cast<ListViewItem>().Select(i => i.Index))
                SelectedPackages.Add(_packages.ElementAt(checkedIndex));
            Close();
        }
    }
}
