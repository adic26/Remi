using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TsdLib.Configuration;

namespace TsdLib.UI.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip)]
    public partial class TestCasesMenuItem : ToolStripMenuItem, ITestCaseControl
    {
        public TestCasesMenuItem()
        {
            InitializeComponent();
        }

        public TestCasesMenuItem(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

        }

        public event EventHandler TestCaseSaved;
        private void toolStripMenuItem_TestCases_SaveAs_Click(object sender, EventArgs e)
        {
            EventHandler handler = TestCaseSaved;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public void DisplayTestCases(IEnumerable<ITestCase> testCases)
        {
            toolStripMenuItem_TestCases_Load.DropDownItems.Clear();
            List<ToolStripItem> testCasesItems = new List<ToolStripItem>();

            foreach (ITestCase testCase in testCases)
            {
                ToolStripItem testCaseItem = new ToolStripMenuItem(testCase.Name);
                testCaseItem.Click += testCaseItem_Click;
                testCaseItem.Name = testCase.Name;
                testCasesItems.Add(testCaseItem);
            }

            toolStripMenuItem_TestCases_Load.DropDownItems.AddRange(testCasesItems.ToArray());
        }

        public event EventHandler<string> TestCaseSelected;
        void testCaseItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            EventHandler<string> handler = TestCaseSelected;
            if (handler != null && item != null)
                handler(this, item.Name);
        }
    }


}
