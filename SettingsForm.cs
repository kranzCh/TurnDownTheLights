using System;
using System.Windows.Forms;

namespace TurnDownTheLights {
    public partial class SettingsForm : Form {
        public Keys TurnOffHotKey { get; private set; }
        public Keys ExitHotKey { get; private set; }

        public SettingsForm(Keys currentTurnOffHotKey, Keys currentExitHotKey) {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            TurnOffHotKey = currentTurnOffHotKey;
            ExitHotKey = currentExitHotKey;

            txtTurnOffKey.Text = KeysToString(TurnOffHotKey);
            txtExitKey.Text = KeysToString(ExitHotKey);

            txtTurnOffKey.KeyDown += TxtTurnOffKey_KeyDown;
            txtExitKey.KeyDown += TxtExitKey_KeyDown;
        }

        private void TxtTurnOffKey_KeyDown(object sender, KeyEventArgs e) {
            e.SuppressKeyPress = true; // Prevents the key from being processed further by the TextBox
            TurnOffHotKey = e.KeyData;
            txtTurnOffKey.Text = KeysToString(e.KeyData);
        }

        private void TxtExitKey_KeyDown(object sender, KeyEventArgs e) {
            e.SuppressKeyPress = true;
            ExitHotKey = e.KeyData;
            txtExitKey.Text = KeysToString(e.KeyData);
        }

        private string KeysToString(Keys keys) {
            if (keys == Keys.None) return string.Empty;
            return new KeysConverter().ConvertToString(keys);
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (TurnOffHotKey == Keys.None || ExitHotKey == Keys.None) {
                MessageBox.Show("Please define both shortcut keys.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (TurnOffHotKey == ExitHotKey) {
                MessageBox.Show("Shortcut keys must be different.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Designer code will be added in the next step
    }
}
