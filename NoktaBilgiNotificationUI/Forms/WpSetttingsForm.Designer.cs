namespace NoktaBilgiNotificationUI.Forms
{
    partial class WpSetttingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WpSetttingsForm));
            this.msk_WPNo = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_WpToken = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_WpClientID = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_ServiceID = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_TemplateID = new DevExpress.XtraEditors.TextEdit();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WpToken.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WpClientID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ServiceID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_TemplateID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // msk_WPNo
            // 
            this.msk_WPNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.msk_WPNo.Location = new System.Drawing.Point(174, 9);
            this.msk_WPNo.Mask = "+900000000000";
            this.msk_WPNo.Name = "msk_WPNo";
            this.msk_WPNo.Size = new System.Drawing.Size(195, 23);
            this.msk_WPNo.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label5.Location = new System.Drawing.Point(12, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 21);
            this.label5.TabIndex = 29;
            this.label5.Text = "Whatsapp Token:";
            // 
            // txt_WpToken
            // 
            this.txt_WpToken.EditValue = "383d93c58301e29276eb914640ced618";
            this.txt_WpToken.Location = new System.Drawing.Point(174, 75);
            this.txt_WpToken.Name = "txt_WpToken";
            this.txt_WpToken.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_WpToken.Properties.Appearance.Options.UseFont = true;
            this.txt_WpToken.Properties.MaxLength = 250;
            this.txt_WpToken.Properties.PasswordChar = '*';
            this.txt_WpToken.Size = new System.Drawing.Size(195, 24);
            this.txt_WpToken.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label4.Location = new System.Drawing.Point(12, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 21);
            this.label4.TabIndex = 28;
            this.label4.Text = "Whatsapp ClientID:";
            // 
            // txt_WpClientID
            // 
            this.txt_WpClientID.EditValue = "AC0d37922d84b7373f712175b1e59b84c5";
            this.txt_WpClientID.Location = new System.Drawing.Point(174, 43);
            this.txt_WpClientID.Name = "txt_WpClientID";
            this.txt_WpClientID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_WpClientID.Properties.Appearance.Options.UseFont = true;
            this.txt_WpClientID.Properties.MaxLength = 250;
            this.txt_WpClientID.Properties.PasswordChar = '*';
            this.txt_WpClientID.Size = new System.Drawing.Size(195, 24);
            this.txt_WpClientID.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 21);
            this.label3.TabIndex = 27;
            this.label3.Text = "Alıcı Numara:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label7.Location = new System.Drawing.Point(12, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 21);
            this.label7.TabIndex = 31;
            this.label7.Text = "Servis ID:";
            // 
            // txt_ServiceID
            // 
            this.txt_ServiceID.EditValue = "MGd9674a5df108741733755b68397c691e";
            this.txt_ServiceID.Location = new System.Drawing.Point(174, 110);
            this.txt_ServiceID.Name = "txt_ServiceID";
            this.txt_ServiceID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_ServiceID.Properties.Appearance.Options.UseFont = true;
            this.txt_ServiceID.Properties.MaxLength = 250;
            this.txt_ServiceID.Properties.PasswordChar = '*';
            this.txt_ServiceID.Size = new System.Drawing.Size(195, 24);
            this.txt_ServiceID.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label1.Location = new System.Drawing.Point(12, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 21);
            this.label1.TabIndex = 34;
            this.label1.Text = "Template ID:";
            // 
            // txt_TemplateID
            // 
            this.txt_TemplateID.EditValue = "HX79357ab71fea4feea2d95db5817cf7c8";
            this.txt_TemplateID.Location = new System.Drawing.Point(174, 145);
            this.txt_TemplateID.Name = "txt_TemplateID";
            this.txt_TemplateID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_TemplateID.Properties.Appearance.Options.UseFont = true;
            this.txt_TemplateID.Properties.MaxLength = 250;
            this.txt_TemplateID.Properties.PasswordChar = '*';
            this.txt_TemplateID.Size = new System.Drawing.Size(195, 24);
            this.txt_TemplateID.TabIndex = 4;
            // 
            // btn_Save
            // 
            this.btn_Save.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btn_Save.Appearance.Font = new System.Drawing.Font("Tahoma", 15.25F);
            this.btn_Save.Appearance.Options.UseBackColor = true;
            this.btn_Save.Appearance.Options.UseFont = true;
            this.btn_Save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.ImageOptions.Image")));
            this.btn_Save.Location = new System.Drawing.Point(174, 196);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(195, 44);
            this.btn_Save.TabIndex = 7;
            this.btn_Save.Text = "Kaydet";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // WpSetttingsForm
            // 
            this.AcceptButton = this.btn_Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(858, 485);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_TemplateID);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_ServiceID);
            this.Controls.Add(this.msk_WPNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_WpToken);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_WpClientID);
            this.Controls.Add(this.label3);
            this.IconOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("WpSetttingsForm.IconOptions.LargeImage")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "WpSetttingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Whatsapp Ayarları";
            this.Load += new System.EventHandler(this.WpSetttingsForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WpSetttingsForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txt_WpToken.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WpClientID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ServiceID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_TemplateID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox msk_WPNo;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txt_WpToken;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txt_WpClientID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txt_ServiceID;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txt_TemplateID;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
    }
}