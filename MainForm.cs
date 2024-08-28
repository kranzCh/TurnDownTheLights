using AuraServiceLib;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TurnDownTheLights {
    public partial class MainForm : Form {
        private readonly IAuraSdk2 sdk = (IAuraSdk2)new AuraSdk();
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        private Point? startPos = null;

        public MainForm() {
            Thread.Sleep(1000);
            InitializeCursorPosition();
            //GlobalMouseHandler gmh = new GlobalMouseHandler();
            //gmh.TheMouseMoved += new MouseMovedEvent(OnMouseMove);
            //Application.AddMessageFilter(gmh);
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent();
            SetAuraOff();
            SetMonitorInState(MonitorState.MonitorStateOff);
        }

        private void OnApplicationExit(object sender, EventArgs e) {
            // Console.WriteLine("Exit");
            sdk.ReleaseControl(0);
        }

        private void SetAuraOff() {
            sdk.SwitchMode();
            var devices = sdk.Enumerate(0);
            foreach (IAuraSyncDevice dev in devices) {
                foreach (IAuraRgbLight light in dev.Lights) {
                    light.Color = 0;
                }
                dev.Apply();
            }
        }

        private void InitializeCursorPosition() {
            Rectangle r = Screen.PrimaryScreen.Bounds;
            Cursor.Position = new Point((r.Width / 2), (r.Height / 2));
            startPos = Cursor.Position;
            // Console.WriteLine(startPos);
        }

        private void OnMouseClick(Object sender, EventArgs e) {
            Application.Exit();
        }

        private void OnMouseMove(Object sender, MouseEventArgs e) {
            SetMonitorInState(MonitorState.MonitorStateOff);
            Console.WriteLine("Moved.");
        }

        private void SetMonitorInState(MonitorState state) {
            SendMessage(0xFFFF, 0x112, 0xF170, (int)state);
        }
    }

    public delegate void MouseMovedEvent();

    public class GlobalMouseHandler : IMessageFilter {
        private const int WM_MOUSEMOVE = 0x0200;

        public event MouseMovedEvent TheMouseMoved;

        #region IMessageFilter Members

        public bool PreFilterMessage(ref Message m) {
            if (m.Msg == WM_MOUSEMOVE) {
                if (TheMouseMoved != null) {
                    TheMouseMoved();
                }
            }
            // Always allow message to continue to the next filter control
            return false;
        }

        #endregion
    }
    public enum MonitorState {
        MonitorStateOn = -1,
        MonitorStateOff = 2,
        MonitorStateStandBy = 1
    }
}
