using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
// Added for EventArgs

namespace TurnDownTheLights {
    public partial class MainForm : Form {
        // Hotkey P/Invokes and constants
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern short GlobalAddAtom(string lpString);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern short GlobalDeleteAtom(short nAtom);

        private const int WM_HOTKEY = 0x0312;

        // Modifiers for hotkeys
        private const uint MOD_NONE = 0x0000;
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;

        private short turnOffHotKeyAtom;
        private short exitHotKeyAtom;

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        private Point? startPos = null;
        private bool IsActivated = false; // Custom flag to indicate active blackout

        private NotifyIcon notifyIcon;
        private ContextMenuStrip trayContextMenu;

        public MainForm() {
            // AppSettings now handles its own initialization (loading settings or defaults) in its static constructor.
            // No explicit call to UpgradeSettingsIfRequired or SetDefaultsIfEmpty is needed here.

            InitializeCursorPosition();
            Application.ApplicationExit += new EventHandler(OnApplicationExit); // For unregistering hotkeys
            this.Load += new EventHandler(MainForm_Load); // For registering hotkeys & setting up tray
            InitializeComponent();

            // Setup for background running with NotifyIcon
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visible = false; // Hide the form itself initially
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon() {
            trayContextMenu = new ContextMenuStrip();

            // Settings Menu Item
            ToolStripMenuItem settingsMenuItem = new ToolStripMenuItem("Settings");
            settingsMenuItem.Click += SettingsMenuItem_Click;
            trayContextMenu.Items.Add(settingsMenuItem);

            // Separator
            trayContextMenu.Items.Add(new ToolStripSeparator());

            // Exit Menu Item
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Click += ExitMenuItem_Click;
            trayContextMenu.Items.Add(exitMenuItem);

            notifyIcon = new NotifyIcon();
            notifyIcon.ContextMenuStrip = trayContextMenu;
            notifyIcon.Text = "TurnDownTheLights";
            // Icon: Placeholder - ideally load from resources.
            // Using a system icon for now. This might not be ideal or might not work on all systems.
            // A proper .ico file should be added to the project resources.
            try {
                // Attempt to use a generic system icon if available
                System.Drawing.Icon sysIcon = new Icon("Resources/TurnDownTheLights.ico");
                if (sysIcon != null) {
                    notifyIcon.Icon = sysIcon;
                } else {
                    notifyIcon.Icon = new Icon(SystemIcons.Application, 40, 40); // Fallback if direct assignment fails
                }
            } catch {
                // If system icons fail, this will be an issue. A project resource is needed.
                Console.WriteLine("Failed to load system icon. Please add an icon to project resources.");
            }

            notifyIcon.Visible = true;
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e) {
            OpenSettingsForm();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            RegisterHotKeys();
            // The form is already made invisible in constructor,
            // so no need to explicitly hide it here unless behavior changes.
        }

        private void RegisterHotKeys() {
            UnregisterHotKeys(); // Clear existing ones first

            Keys turnOffKey = AppSettings.TurnOffHotKey;
            Keys exitKey = AppSettings.ExitHotKey;

            if (turnOffKey != Keys.None) {
                uint modifiers = GetModifiers(turnOffKey);
                uint vk = (uint)(turnOffKey & Keys.KeyCode);
                turnOffHotKeyAtom = GlobalAddAtom("TurnOffHotKey_" + Guid.NewGuid().ToString());
                if (!RegisterHotKey(this.Handle, turnOffHotKeyAtom, modifiers, vk)) {
                    Console.WriteLine($"Failed to register Turn Off hotkey. Error code: {Marshal.GetLastWin32Error()}");
                } else {
                    Console.WriteLine($"Turn Off Hotkey registered: {turnOffKey}");
                }
            }

            if (exitKey != Keys.None) {
                uint modifiers = GetModifiers(exitKey);
                uint vk = (uint)(exitKey & Keys.KeyCode);
                exitHotKeyAtom = GlobalAddAtom("ExitHotKey_" + Guid.NewGuid().ToString());
                if (!RegisterHotKey(this.Handle, exitHotKeyAtom, modifiers, vk)) {
                    Console.WriteLine($"Failed to register Exit hotkey. Error code: {Marshal.GetLastWin32Error()}");
                } else {
                    Console.WriteLine($"Exit Hotkey registered: {exitKey}");
                }
            }
        }

        private void UnregisterHotKeys() {
            if (this.IsDisposed || !this.IsHandleCreated) {
                return;
            }
            if (turnOffHotKeyAtom != 0) {
                UnregisterHotKey(this.Handle, turnOffHotKeyAtom);
                GlobalDeleteAtom(turnOffHotKeyAtom);
                turnOffHotKeyAtom = 0;
                Console.WriteLine("Turn Off Hotkey unregistered.");
            }
            if (exitHotKeyAtom != 0) {
                UnregisterHotKey(this.Handle, exitHotKeyAtom);
                GlobalDeleteAtom(exitHotKeyAtom);
                exitHotKeyAtom = 0;
                Console.WriteLine("Exit Hotkey unregistered.");
            }
        }

        private uint GetModifiers(Keys key) {
            uint modifiers = MOD_NONE;
            if ((key & Keys.Alt) == Keys.Alt) modifiers |= MOD_ALT;
            if ((key & Keys.Control) == Keys.Control) modifiers |= MOD_CONTROL;
            if ((key & Keys.Shift) == Keys.Shift) modifiers |= MOD_SHIFT;
            // Note: MOD_WIN is trickier as Keys.LWin/RWin are distinct from Keys.Win.
            // For simplicity, assuming Keys.Win is not typically part of user-defined shortcuts here,
            // but could be expanded if needed.
            return modifiers;
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if (m.Msg == WM_HOTKEY) {
                int id = m.WParam.ToInt32();
                if (id == turnOffHotKeyAtom) {
                    Console.WriteLine("Turn Off Hotkey Pressed!");
                    TriggerTurnOffActions();
                } else if (id == exitHotKeyAtom && this.IsActivated) {
                    Console.WriteLine("Exit Hotkey Pressed!");
                    TriggerTurnOnActions();
                }
            }
        }

        private void TriggerTurnOffActions() {
            SetMonitorInState(MonitorState.MonitorStateOff);

            // Make the form cover secondary screens
            this.Invoke((MethodInvoker)delegate {
                Rectangle r = new Rectangle();
                foreach (Screen s in Screen.AllScreens) {
                    if (s != Screen.PrimaryScreen) // Blackout only the secondary screens
                        r = Rectangle.Union(r, s.Bounds);
                }

                if (!r.IsEmpty) {
                    this.Top = r.Top;
                    this.Left = r.Left;
                    this.Width = r.Width;
                    this.Height = r.Height;
                    this.FormBorderStyle = FormBorderStyle.None; // Ensure no borders/title bar
                    this.WindowState = FormWindowState.Normal; // Ensure it's not minimized
                    this.BackColor = Color.Black; // Ensure it's black
                    this.ShowInTaskbar = false; // Hide from taskbar when it's just a blackout screen
                    this.TopMost = true; // Ensure it's on top
                    this.Show(); // Show it if it was hidden
                    this.Activate(); // Try to bring to front
                    this.IsActivated = true;
                } else {
                    // If no secondary screens, maybe just hide the form or do nothing extra
                    // For now, if no secondary screens, this part does nothing to the form's visibility/size
                }
            });
        }

        private void TriggerTurnOnActions() {
            SetMonitorInState(MonitorState.MonitorStateOn);
            this.Invoke((MethodInvoker)delegate {
                this.Hide(); // Hide the blackout form
                this.IsActivated = false;
            });
        }

        // InitializeTempSettingsButton and BtnSettingsTemp_Click removed.

        private void OpenSettingsForm() {
            // Ensure the settings form is not parented to the main form if main form is invisible,
            // otherwise the dialog might also be invisible or behave strangely.
            // ShowDialog() without a parent is generally safer for tray apps.
            UnregisterHotKeys();
            using (var settingsForm = new SettingsForm(AppSettings.TurnOffHotKey, AppSettings.ExitHotKey)) {
                if (settingsForm.ShowDialog() == DialogResult.OK) {
                    AppSettings.TurnOffHotKey = settingsForm.TurnOffHotKey;
                    AppSettings.ExitHotKey = settingsForm.ExitHotKey;
                    AppSettings.Save();
                    MessageBox.Show("Settings saved. Hotkeys are now active.", "Settings Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            RegisterHotKeys();
        }

        private void OnApplicationExit(object sender, EventArgs e) {
            UnregisterHotKeys(); // Ensure hotkeys are unregistered on exit

            // Dispose NotifyIcon
            if (notifyIcon != null) {
                notifyIcon.Visible = false; // Hide before disposing
                notifyIcon.Dispose();
                notifyIcon = null;
            }

            // Console.WriteLine("Exit");
        }

        private void InitializeCursorPosition() {
            Rectangle r = Screen.PrimaryScreen.Bounds;
            Cursor.Position = new Point((r.Width / 2), (r.Height / 2));
            startPos = Cursor.Position;
            // Console.WriteLine(startPos);
        }

        private void SetMonitorInState(MonitorState state) {
            SendMessage(0xFFFF, 0x112, 0xF170, (int)state);
        }
    }

    public enum MonitorState {
        MonitorStateOn = -1,
        MonitorStateOff = 2,
        MonitorStateStandBy = 1
    }
}
