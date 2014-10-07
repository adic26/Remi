using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.Utilities
{
    public static class Display
    {
        public static void showOnMonitor(int showOnMonitor, System.Windows.Forms.Form f)
        {
            System.Windows.Forms.Screen[] sc = System.Windows.Forms.Screen.AllScreens;

            if (showOnMonitor > sc.Length - 1)
            {
                showOnMonitor = 0;
            }

            var programsize = f.Bounds;
            Int32 left = (sc[showOnMonitor].WorkingArea.Width - programsize.Width) / 2;
            Int32 top = (sc[showOnMonitor].WorkingArea.Height - programsize.Height) / 2;

            f.Left = sc[showOnMonitor].Bounds.Left + left;
            f.Top = sc[showOnMonitor].Bounds.Top + top;
            f.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            f.Show();
        }
    }
}