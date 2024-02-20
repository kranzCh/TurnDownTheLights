using AuraServiceLib;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SleepingDevices {
    public partial class MainForm : Form {
        private readonly IAuraSdk2 sdk = (IAuraSdk2)new AuraSdk();
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        private Point? startPos = null;

        public MainForm() {
            GlobalMouseHandler gmh = new GlobalMouseHandler();
            gmh.TheMouseMoved += new MouseMovedEvent(OnMouseMove);
            Application.AddMessageFilter(gmh);
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent();
            SetAuraOff();
            Thread.Sleep(2000);
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

        private void OnMouseMove() {
            Point cur_pos = Cursor.Position;
            // Console.WriteLine(startPos);
            // Console.WriteLine(cur_pos);
            if (startPos == null) {
                startPos = cur_pos;
            } else {
                if (startPos != cur_pos) {
                    Application.Exit();
                }
            }

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
