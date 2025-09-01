namespace TurnDownTheLights {
    partial class SettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblTurnOff = new System.Windows.Forms.Label();
            this.txtTurnOffKey = new System.Windows.Forms.TextBox();
            this.lblExit = new System.Windows.Forms.Label();
            this.txtExitKey = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTurnOff
            // 
            this.lblTurnOff.AutoSize = true;
            this.lblTurnOff.Location = new System.Drawing.Point(12, 35);
            this.lblTurnOff.Name = "lblTurnOff";
            this.lblTurnOff.Size = new System.Drawing.Size(125, 13);
            this.lblTurnOff.TabIndex = 0;
            this.lblTurnOff.Text = "Turn Off Lights/Monitors:";
            // 
            // txtTurnOffKey
            // 
            this.txtTurnOffKey.Location = new System.Drawing.Point(148, 32);
            this.txtTurnOffKey.Name = "txtTurnOffKey";
            this.txtTurnOffKey.ReadOnly = true;
            this.txtTurnOffKey.Size = new System.Drawing.Size(124, 20);
            this.txtTurnOffKey.TabIndex = 1;
            this.txtTurnOffKey.Text = "None";
            // 
            // lblExit
            // 
            this.lblExit.AutoSize = true;
            this.lblExit.Location = new System.Drawing.Point(12, 65);
            this.lblExit.Name = "lblExit";
            this.lblExit.Size = new System.Drawing.Size(82, 13);
            this.lblExit.TabIndex = 2;
            this.lblExit.Text = "Exit Application:";
            // 
            // txtExitKey
            // 
            this.txtExitKey.Location = new System.Drawing.Point(148, 62);
            this.txtExitKey.Name = "txtExitKey";
            this.txtExitKey.ReadOnly = true;
            this.txtExitKey.Size = new System.Drawing.Size(124, 20);
            this.txtExitKey.TabIndex = 3;
            this.txtExitKey.Text = "None";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(116, 100);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(12, 9);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(238, 13);
            this.lblInstructions.TabIndex = 6;
            this.lblInstructions.Text = "Click in a box and press the desired shortcut key.";
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 136);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtExitKey);
            this.Controls.Add(this.lblExit);
            this.Controls.Add(this.txtTurnOffKey);
            this.Controls.Add(this.lblTurnOff);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTurnOff;
        private System.Windows.Forms.TextBox txtTurnOffKey;
        private System.Windows.Forms.Label lblExit;
        private System.Windows.Forms.TextBox txtExitKey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInstructions;
    }
}
