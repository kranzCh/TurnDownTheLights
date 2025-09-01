using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace TurnDownTheLights {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // MainForm now handles its own visibility and sizing for blackout.
            // It will start minimized and hidden from taskbar.
            Application.Run(new MainForm());
        }
    }
}
