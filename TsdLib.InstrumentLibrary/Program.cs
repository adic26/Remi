﻿using System;
using System.Windows.Forms;

namespace TsdLib.InstrumentLibrary
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TsdLibInstrumentLibraryVisualizer());
        }
    }
}