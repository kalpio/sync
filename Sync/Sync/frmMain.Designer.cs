namespace Sync
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.txtFolderA = new System.Windows.Forms.TextBox();
            this.txtFolderB = new System.Windows.Forms.TextBox();
            this.btnFolderA = new System.Windows.Forms.Button();
            this.btnFolderB = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyContextMenu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.notifyContextMenu_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbIntervalType = new System.Windows.Forms.ComboBox();
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblConfigPath = new System.Windows.Forms.Label();
            this.txtStartAtDate = new System.Windows.Forms.DateTimePicker();
            this.txtStartAtTime = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblStatusInWindow = new System.Windows.Forms.Label();
            this.chkSkipDeleteFolderB = new System.Windows.Forms.CheckBox();
            this.chkSkipDeleteFolderA = new System.Windows.Forms.CheckBox();
            this.notifyContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFolderA
            // 
            this.txtFolderA.BackColor = System.Drawing.Color.White;
            this.txtFolderA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtFolderA.ForeColor = System.Drawing.Color.Black;
            this.txtFolderA.Location = new System.Drawing.Point(140, 43);
            this.txtFolderA.Name = "txtFolderA";
            this.txtFolderA.Size = new System.Drawing.Size(413, 24);
            this.txtFolderA.TabIndex = 0;
            this.txtFolderA.TextChanged += new System.EventHandler(this.txtFolderA_TextChanged);
            // 
            // txtFolderB
            // 
            this.txtFolderB.BackColor = System.Drawing.Color.White;
            this.txtFolderB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtFolderB.ForeColor = System.Drawing.Color.Black;
            this.txtFolderB.Location = new System.Drawing.Point(140, 76);
            this.txtFolderB.Name = "txtFolderB";
            this.txtFolderB.Size = new System.Drawing.Size(413, 24);
            this.txtFolderB.TabIndex = 2;
            this.txtFolderB.TextChanged += new System.EventHandler(this.txtFolderB_TextChanged);
            // 
            // btnFolderA
            // 
            this.btnFolderA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnFolderA.Location = new System.Drawing.Point(559, 39);
            this.btnFolderA.Name = "btnFolderA";
            this.btnFolderA.Size = new System.Drawing.Size(75, 32);
            this.btnFolderA.TabIndex = 1;
            this.btnFolderA.Text = "...";
            this.btnFolderA.UseVisualStyleBackColor = true;
            this.btnFolderA.Click += new System.EventHandler(this.btnFolderA_Click);
            // 
            // btnFolderB
            // 
            this.btnFolderB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnFolderB.Location = new System.Drawing.Point(559, 72);
            this.btnFolderB.Name = "btnFolderB";
            this.btnFolderB.Size = new System.Drawing.Size(75, 32);
            this.btnFolderB.TabIndex = 3;
            this.btnFolderB.Text = "...";
            this.btnFolderB.UseVisualStyleBackColor = true;
            this.btnFolderB.Click += new System.EventHandler(this.btnFolderB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "folder [A]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(12, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "and folder [B]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Do sync for";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Location = new System.Drawing.Point(512, 339);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(122, 41);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnuluj.BackColor = System.Drawing.SystemColors.Control;
            this.btnAnuluj.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnAnuluj.Location = new System.Drawing.Point(422, 339);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(84, 41);
            this.btnAnuluj.TabIndex = 7;
            this.btnAnuluj.Text = "Cancel";
            this.btnAnuluj.UseVisualStyleBackColor = false;
            this.btnAnuluj.Click += new System.EventHandler(this.btnAnuluj_Click);
            // 
            // notify
            // 
            this.notify.ContextMenuStrip = this.notifyContextMenu;
            this.notify.Icon = ((System.Drawing.Icon)(resources.GetObject("notify.Icon")));
            this.notify.Text = "...";
            this.notify.Visible = true;
            // 
            // notifyContextMenu
            // 
            this.notifyContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.notifyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyContextMenu_Open,
            this.toolStripSeparator1,
            this.notifyContextMenu_Close});
            this.notifyContextMenu.Name = "notifyContextMenu";
            this.notifyContextMenu.Size = new System.Drawing.Size(132, 58);
            // 
            // notifyContextMenu_Open
            // 
            this.notifyContextMenu_Open.Name = "notifyContextMenu_Open";
            this.notifyContextMenu_Open.Size = new System.Drawing.Size(131, 24);
            this.notifyContextMenu_Open.Text = "Otwórz";
            this.notifyContextMenu_Open.Click += new System.EventHandler(this.notifyContextMenu_Open_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(128, 6);
            // 
            // notifyContextMenu_Close
            // 
            this.notifyContextMenu_Close.Name = "notifyContextMenu_Close";
            this.notifyContextMenu_Close.Size = new System.Drawing.Size(131, 24);
            this.notifyContextMenu_Close.Text = "Zamknij";
            this.notifyContextMenu_Close.Click += new System.EventHandler(this.notifyContextMenu_Close_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(12, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Interval";
            // 
            // txtInterval
            // 
            this.txtInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtInterval.Location = new System.Drawing.Point(140, 129);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(128, 24);
            this.txtInterval.TabIndex = 4;
            this.txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtInterval.ValueChanged += new System.EventHandler(this.txtInterval_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(12, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "Direction";
            // 
            // cmbIntervalType
            // 
            this.cmbIntervalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIntervalType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cmbIntervalType.FormattingEnabled = true;
            this.cmbIntervalType.Items.AddRange(new object[] {
            "Millisecond",
            "Second",
            "Minute",
            "Hour",
            "Day",
            "Week",
            "Month",
            "Year"});
            this.cmbIntervalType.Location = new System.Drawing.Point(274, 128);
            this.cmbIntervalType.Name = "cmbIntervalType";
            this.cmbIntervalType.Size = new System.Drawing.Size(279, 26);
            this.cmbIntervalType.TabIndex = 5;
            this.cmbIntervalType.SelectedIndexChanged += new System.EventHandler(this.cmbIntervalType_SelectedIndexChanged);
            // 
            // cmbDirection
            // 
            this.cmbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDirection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cmbDirection.FormattingEnabled = true;
            this.cmbDirection.Items.AddRange(new object[] {
            "Upload changes (from A to B)",
            "Download changes (from B to A)",
            "Upload (from A to B) and download changes (from B to A)"});
            this.cmbDirection.Location = new System.Drawing.Point(140, 188);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(413, 26);
            this.cmbDirection.TabIndex = 6;
            this.cmbDirection.SelectedIndexChanged += new System.EventHandler(this.cmbDirection_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.label6.Location = new System.Drawing.Point(12, 395);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(622, 1);
            this.label6.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.label7.Location = new System.Drawing.Point(12, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(622, 1);
            this.label7.TabIndex = 15;
            // 
            // lblCopyright
            // 
            this.lblCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCopyright.Location = new System.Drawing.Point(12, 400);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(139, 17);
            this.lblCopyright.TabIndex = 16;
            this.lblCopyright.Text = "Copyright annotation";
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConfigPath
            // 
            this.lblConfigPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigPath.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblConfigPath.Location = new System.Drawing.Point(157, 398);
            this.lblConfigPath.Name = "lblConfigPath";
            this.lblConfigPath.Size = new System.Drawing.Size(471, 20);
            this.lblConfigPath.TabIndex = 17;
            this.lblConfigPath.Text = "File localization";
            this.lblConfigPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStartAtDate
            // 
            this.txtStartAtDate.CustomFormat = "";
            this.txtStartAtDate.Location = new System.Drawing.Point(140, 160);
            this.txtStartAtDate.Name = "txtStartAtDate";
            this.txtStartAtDate.Size = new System.Drawing.Size(239, 22);
            this.txtStartAtDate.TabIndex = 18;
            this.txtStartAtDate.ValueChanged += new System.EventHandler(this.txtStartAtDate_ValueChanged);
            // 
            // txtStartAtTime
            // 
            this.txtStartAtTime.CustomFormat = "HH:mm";
            this.txtStartAtTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtStartAtTime.Location = new System.Drawing.Point(385, 160);
            this.txtStartAtTime.Name = "txtStartAtTime";
            this.txtStartAtTime.ShowUpDown = true;
            this.txtStartAtTime.Size = new System.Drawing.Size(168, 22);
            this.txtStartAtTime.TabIndex = 19;
            this.txtStartAtTime.ValueChanged += new System.EventHandler(this.txtStartAtTime_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(12, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 18);
            this.label8.TabIndex = 20;
            this.label8.Text = "Start at";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.label9.Location = new System.Drawing.Point(112, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(522, 1);
            this.label9.TabIndex = 21;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.BackColor = System.Drawing.SystemColors.Control;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnClear.Location = new System.Drawing.Point(12, 339);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(174, 41);
            this.btnClear.TabIndex = 22;
            this.btnClear.Text = "Clear configuration";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblStatusInWindow
            // 
            this.lblStatusInWindow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusInWindow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStatusInWindow.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblStatusInWindow.Location = new System.Drawing.Point(12, 286);
            this.lblStatusInWindow.Name = "lblStatusInWindow";
            this.lblStatusInWindow.Size = new System.Drawing.Size(622, 50);
            this.lblStatusInWindow.TabIndex = 23;
            this.lblStatusInWindow.Text = "...";
            // 
            // chkSkipDeleteFolderB
            // 
            this.chkSkipDeleteFolderB.AutoSize = true;
            this.chkSkipDeleteFolderB.Location = new System.Drawing.Point(140, 247);
            this.chkSkipDeleteFolderB.Name = "chkSkipDeleteFolderB";
            this.chkSkipDeleteFolderB.Size = new System.Drawing.Size(205, 21);
            this.chkSkipDeleteFolderB.TabIndex = 24;
            this.chkSkipDeleteFolderB.Text = "Skip delete files in folder [B]";
            this.chkSkipDeleteFolderB.UseVisualStyleBackColor = true;
            this.chkSkipDeleteFolderB.CheckedChanged += new System.EventHandler(this.chkSkipDeleteFolderB_CheckedChanged);
            // 
            // chkSkipDeleteFolderA
            // 
            this.chkSkipDeleteFolderA.AutoSize = true;
            this.chkSkipDeleteFolderA.Location = new System.Drawing.Point(140, 220);
            this.chkSkipDeleteFolderA.Name = "chkSkipDeleteFolderA";
            this.chkSkipDeleteFolderA.Size = new System.Drawing.Size(205, 21);
            this.chkSkipDeleteFolderA.TabIndex = 25;
            this.chkSkipDeleteFolderA.Text = "Skip delete files in folder [A]";
            this.chkSkipDeleteFolderA.UseVisualStyleBackColor = true;
            this.chkSkipDeleteFolderA.CheckedChanged += new System.EventHandler(this.chkSkipDeleteFolderA_CheckedChanged);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(646, 427);
            this.Controls.Add(this.chkSkipDeleteFolderA);
            this.Controls.Add(this.chkSkipDeleteFolderB);
            this.Controls.Add(this.lblStatusInWindow);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtStartAtTime);
            this.Controls.Add(this.txtStartAtDate);
            this.Controls.Add(this.lblConfigPath);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbDirection);
            this.Controls.Add(this.cmbIntervalType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAnuluj);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFolderB);
            this.Controls.Add(this.btnFolderA);
            this.Controls.Add(this.txtFolderB);
            this.Controls.Add(this.txtFolderA);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Folder synchronization";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.notifyContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFolderA;
        private System.Windows.Forms.TextBox txtFolderB;
        private System.Windows.Forms.Button btnFolderA;
        private System.Windows.Forms.Button btnFolderB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAnuluj;
        private System.Windows.Forms.NotifyIcon notify;
        private System.Windows.Forms.ContextMenuStrip notifyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem notifyContextMenu_Open;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem notifyContextMenu_Close;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown txtInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbIntervalType;
        private System.Windows.Forms.ComboBox cmbDirection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblConfigPath;
        private System.Windows.Forms.DateTimePicker txtStartAtDate;
        private System.Windows.Forms.DateTimePicker txtStartAtTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblStatusInWindow;
        private System.Windows.Forms.CheckBox chkSkipDeleteFolderB;
        private System.Windows.Forms.CheckBox chkSkipDeleteFolderA;
    }
}

