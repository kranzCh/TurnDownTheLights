using System;
using System.Drawing;
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
            Form mainForm = new MainForm();
            Rectangle r = new Rectangle();
            foreach (Screen s in Screen.AllScreens) {
                if (s != Screen.PrimaryScreen) // Blackout only the secondary screens
                    r = Rectangle.Union(r, s.Bounds);
            }
            // Console.WriteLine(r);
            mainForm.Top = r.Top;
            mainForm.Left = r.Left;
            mainForm.Width = r.Width;
            mainForm.Height = r.Height;
            mainForm.TopMost = true; // This will bring your window in front of all other windows including the taskbar
            Application.Run(mainForm);
        }
    }
}
