namespace EUSignDFS
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbSettings = new System.Windows.Forms.ToolStripButton();
            this.gbPrivateKey = new System.Windows.Forms.GroupBox();
            this.tbCertOwnerInfo = new System.Windows.Forms.TextBox();
            this.btnShowOwnCertificate = new System.Windows.Forms.Button();
            this.btnReadPrivateKey = new System.Windows.Forms.Button();
            this.gbSign = new System.Windows.Forms.GroupBox();
            this.btnSignStamp = new System.Windows.Forms.Button();
            this.btnSignDirector = new System.Windows.Forms.Button();
            this.btnSignAccountant = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslInitialize = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbEnvelop = new System.Windows.Forms.GroupBox();
            this.btnEnvelop = new System.Windows.Forms.Button();
            this.tbRecipient = new System.Windows.Forms.TextBox();
            this.btnGetRecipient = new System.Windows.Forms.Button();
            this.gbDevelop = new System.Windows.Forms.GroupBox();
            this.btnDevelop = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip.SuspendLayout();
            this.gbPrivateKey.SuspendLayout();
            this.gbSign.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.gbEnvelop.SuspendLayout();
            this.gbDevelop.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSettings});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(484, 39);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // tsbSettings
            // 
            this.tsbSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbSettings.Image = global::EUSignDFS.Properties.Resources.settings_32x32;
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(125, 36);
            this.tsbSettings.Text = "Налаштування";
            this.tsbSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // gbPrivateKey
            // 
            this.gbPrivateKey.AutoSize = true;
            this.gbPrivateKey.Controls.Add(this.tbCertOwnerInfo);
            this.gbPrivateKey.Controls.Add(this.btnShowOwnCertificate);
            this.gbPrivateKey.Controls.Add(this.btnReadPrivateKey);
            this.gbPrivateKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbPrivateKey.Location = new System.Drawing.Point(0, 39);
            this.gbPrivateKey.Name = "gbPrivateKey";
            this.gbPrivateKey.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.gbPrivateKey.Size = new System.Drawing.Size(484, 121);
            this.gbPrivateKey.TabIndex = 1;
            this.gbPrivateKey.TabStop = false;
            this.gbPrivateKey.Text = "Особистий ключ";
            // 
            // tbCertOwnerInfo
            // 
            this.tbCertOwnerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCertOwnerInfo.Location = new System.Drawing.Point(12, 19);
            this.tbCertOwnerInfo.Multiline = true;
            this.tbCertOwnerInfo.Name = "tbCertOwnerInfo";
            this.tbCertOwnerInfo.Size = new System.Drawing.Size(334, 86);
            this.tbCertOwnerInfo.TabIndex = 1;
            // 
            // btnShowOwnCertificate
            // 
            this.btnShowOwnCertificate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowOwnCertificate.AutoSize = true;
            this.btnShowOwnCertificate.Enabled = false;
            this.btnShowOwnCertificate.Image = global::EUSignDFS.Properties.Resources.certificate_32x32;
            this.btnShowOwnCertificate.Location = new System.Drawing.Point(352, 65);
            this.btnShowOwnCertificate.Name = "btnShowOwnCertificate";
            this.btnShowOwnCertificate.Size = new System.Drawing.Size(120, 40);
            this.btnShowOwnCertificate.TabIndex = 0;
            this.btnShowOwnCertificate.Text = "Переглянути";
            this.btnShowOwnCertificate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShowOwnCertificate.UseVisualStyleBackColor = true;
            this.btnShowOwnCertificate.Click += new System.EventHandler(this.btnShowOwnCertificate_Click);
            // 
            // btnReadPrivateKey
            // 
            this.btnReadPrivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadPrivateKey.AutoSize = true;
            this.btnReadPrivateKey.Image = global::EUSignDFS.Properties.Resources.key_32x32;
            this.btnReadPrivateKey.Location = new System.Drawing.Point(352, 19);
            this.btnReadPrivateKey.Name = "btnReadPrivateKey";
            this.btnReadPrivateKey.Size = new System.Drawing.Size(120, 40);
            this.btnReadPrivateKey.TabIndex = 0;
            this.btnReadPrivateKey.Text = "Зчитати";
            this.btnReadPrivateKey.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReadPrivateKey.UseVisualStyleBackColor = true;
            this.btnReadPrivateKey.Click += new System.EventHandler(this.btnReadPrivateKey_Click);
            // 
            // gbSign
            // 
            this.gbSign.AutoSize = true;
            this.gbSign.Controls.Add(this.btnSignStamp);
            this.gbSign.Controls.Add(this.btnSignDirector);
            this.gbSign.Controls.Add(this.btnSignAccountant);
            this.gbSign.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSign.Location = new System.Drawing.Point(0, 160);
            this.gbSign.Name = "gbSign";
            this.gbSign.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.gbSign.Size = new System.Drawing.Size(484, 75);
            this.gbSign.TabIndex = 2;
            this.gbSign.TabStop = false;
            this.gbSign.Text = "Підпис повідомлення особистим ключом";
            // 
            // btnSignStamp
            // 
            this.btnSignStamp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignStamp.AutoSize = true;
            this.btnSignStamp.Enabled = false;
            this.btnSignStamp.Image = global::EUSignDFS.Properties.Resources.sign_32x32;
            this.btnSignStamp.Location = new System.Drawing.Point(264, 19);
            this.btnSignStamp.Name = "btnSignStamp";
            this.btnSignStamp.Size = new System.Drawing.Size(120, 40);
            this.btnSignStamp.TabIndex = 0;
            this.btnSignStamp.Text = "Печатка";
            this.btnSignStamp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSignStamp.UseVisualStyleBackColor = true;
            this.btnSignStamp.Click += new System.EventHandler(this.btnSignStamp_Click);
            // 
            // btnSignDirector
            // 
            this.btnSignDirector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignDirector.AutoSize = true;
            this.btnSignDirector.Enabled = false;
            this.btnSignDirector.Image = global::EUSignDFS.Properties.Resources.sign_32x32;
            this.btnSignDirector.Location = new System.Drawing.Point(138, 19);
            this.btnSignDirector.Name = "btnSignDirector";
            this.btnSignDirector.Size = new System.Drawing.Size(120, 40);
            this.btnSignDirector.TabIndex = 0;
            this.btnSignDirector.Text = "Директор";
            this.btnSignDirector.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSignDirector.UseVisualStyleBackColor = true;
            this.btnSignDirector.Click += new System.EventHandler(this.btnSignDirector_Click);
            // 
            // btnSignAccountant
            // 
            this.btnSignAccountant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignAccountant.AutoSize = true;
            this.btnSignAccountant.Enabled = false;
            this.btnSignAccountant.Image = global::EUSignDFS.Properties.Resources.sign_32x32;
            this.btnSignAccountant.Location = new System.Drawing.Point(12, 19);
            this.btnSignAccountant.Name = "btnSignAccountant";
            this.btnSignAccountant.Size = new System.Drawing.Size(120, 40);
            this.btnSignAccountant.TabIndex = 0;
            this.btnSignAccountant.Text = "Бухгалтер";
            this.btnSignAccountant.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSignAccountant.UseVisualStyleBackColor = true;
            this.btnSignAccountant.Click += new System.EventHandler(this.btnSignAccountant_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslInitialize});
            this.statusStrip.Location = new System.Drawing.Point(0, 437);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(484, 25);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip";
            // 
            // tsslInitialize
            // 
            this.tsslInitialize.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslInitialize.Image = global::EUSignDFS.Properties.Resources.checked_16x16;
            this.tsslInitialize.Name = "tsslInitialize";
            this.tsslInitialize.Size = new System.Drawing.Size(102, 20);
            this.tsslInitialize.Text = "EUSignCP.DLL";
            this.tsslInitialize.ToolTipText = "Бібліотеку не завантажено.";
            // 
            // gbEnvelop
            // 
            this.gbEnvelop.AutoSize = true;
            this.gbEnvelop.Controls.Add(this.btnEnvelop);
            this.gbEnvelop.Controls.Add(this.tbRecipient);
            this.gbEnvelop.Controls.Add(this.btnGetRecipient);
            this.gbEnvelop.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbEnvelop.Location = new System.Drawing.Point(0, 235);
            this.gbEnvelop.Name = "gbEnvelop";
            this.gbEnvelop.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.gbEnvelop.Size = new System.Drawing.Size(484, 121);
            this.gbEnvelop.TabIndex = 4;
            this.gbEnvelop.TabStop = false;
            this.gbEnvelop.Text = "Упакування повідомлення печаткою на сертифікат одержувача";
            // 
            // btnEnvelop
            // 
            this.btnEnvelop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnvelop.AutoSize = true;
            this.btnEnvelop.Enabled = false;
            this.btnEnvelop.Image = global::EUSignDFS.Properties.Resources.envelop_32x32;
            this.btnEnvelop.Location = new System.Drawing.Point(352, 65);
            this.btnEnvelop.Name = "btnEnvelop";
            this.btnEnvelop.Size = new System.Drawing.Size(120, 40);
            this.btnEnvelop.TabIndex = 0;
            this.btnEnvelop.Text = "Упакувати";
            this.btnEnvelop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnvelop.UseVisualStyleBackColor = true;
            this.btnEnvelop.Click += new System.EventHandler(this.btnEnvelop_Click);
            // 
            // tbRecipient
            // 
            this.tbRecipient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRecipient.Location = new System.Drawing.Point(12, 19);
            this.tbRecipient.Multiline = true;
            this.tbRecipient.Name = "tbRecipient";
            this.tbRecipient.Size = new System.Drawing.Size(334, 86);
            this.tbRecipient.TabIndex = 1;
            // 
            // btnGetRecipient
            // 
            this.btnGetRecipient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetRecipient.AutoSize = true;
            this.btnGetRecipient.Image = global::EUSignDFS.Properties.Resources.recipient_32x32;
            this.btnGetRecipient.Location = new System.Drawing.Point(352, 19);
            this.btnGetRecipient.Name = "btnGetRecipient";
            this.btnGetRecipient.Size = new System.Drawing.Size(120, 40);
            this.btnGetRecipient.TabIndex = 0;
            this.btnGetRecipient.Text = "Одержувач";
            this.btnGetRecipient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGetRecipient.UseVisualStyleBackColor = true;
            this.btnGetRecipient.Click += new System.EventHandler(this.btnGetRecipient_Click);
            // 
            // gbDevelop
            // 
            this.gbDevelop.AutoSize = true;
            this.gbDevelop.Controls.Add(this.btnDevelop);
            this.gbDevelop.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDevelop.Location = new System.Drawing.Point(0, 356);
            this.gbDevelop.Name = "gbDevelop";
            this.gbDevelop.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.gbDevelop.Size = new System.Drawing.Size(484, 75);
            this.gbDevelop.TabIndex = 5;
            this.gbDevelop.TabStop = false;
            this.gbDevelop.Text = "Розкриття квитанції печаткою";
            // 
            // btnDevelop
            // 
            this.btnDevelop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDevelop.AutoSize = true;
            this.btnDevelop.Enabled = false;
            this.btnDevelop.Image = global::EUSignDFS.Properties.Resources.develop_32x32;
            this.btnDevelop.Location = new System.Drawing.Point(12, 19);
            this.btnDevelop.Name = "btnDevelop";
            this.btnDevelop.Size = new System.Drawing.Size(120, 40);
            this.btnDevelop.TabIndex = 0;
            this.btnDevelop.Text = "Розкрити";
            this.btnDevelop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDevelop.UseVisualStyleBackColor = true;
            this.btnDevelop.Click += new System.EventHandler(this.btnDevelop_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 462);
            this.Controls.Add(this.gbDevelop);
            this.Controls.Add(this.gbEnvelop);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.gbSign);
            this.Controls.Add(this.gbPrivateKey);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EUSignDFS";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.gbPrivateKey.ResumeLayout(false);
            this.gbPrivateKey.PerformLayout();
            this.gbSign.ResumeLayout(false);
            this.gbSign.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.gbEnvelop.ResumeLayout(false);
            this.gbEnvelop.PerformLayout();
            this.gbDevelop.ResumeLayout(false);
            this.gbDevelop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbSettings;
        private System.Windows.Forms.GroupBox gbPrivateKey;
        private System.Windows.Forms.Button btnReadPrivateKey;
        private System.Windows.Forms.Button btnShowOwnCertificate;
        private System.Windows.Forms.GroupBox gbSign;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslInitialize;
        private System.Windows.Forms.TextBox tbCertOwnerInfo;
        private System.Windows.Forms.Button btnSignStamp;
        private System.Windows.Forms.Button btnSignDirector;
        private System.Windows.Forms.Button btnSignAccountant;
        private System.Windows.Forms.GroupBox gbEnvelop;
        private System.Windows.Forms.TextBox tbRecipient;
        private System.Windows.Forms.Button btnGetRecipient;
        private System.Windows.Forms.Button btnEnvelop;
        private System.Windows.Forms.GroupBox gbDevelop;
        private System.Windows.Forms.Button btnDevelop;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}

