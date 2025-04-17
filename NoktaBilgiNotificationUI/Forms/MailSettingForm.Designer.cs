namespace NoktaBilgiNotificationUI.Forms
{
    partial class MailSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailSettingForm));
            this.label5 = new System.Windows.Forms.Label();
            this.txt_recipientEmail = new DevExpress.XtraEditors.TextEdit();
            this.btn_SendMail = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Update = new DevExpress.XtraEditors.SimpleButton();
            this.chk_SSL = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Port = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Password = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Server = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Email = new DevExpress.XtraEditors.TextEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_Web = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_Adres = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_Phone = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_CompanyName = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_recipientEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Port.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Server.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Email.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Web.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Adres.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Phone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label5.Location = new System.Drawing.Point(12, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 20);
            this.label5.TabIndex = 58;
            this.label5.Text = "Kime Gönderilecek:";
            // 
            // txt_recipientEmail
            // 
            this.txt_recipientEmail.Location = new System.Drawing.Point(182, 35);
            this.txt_recipientEmail.Name = "txt_recipientEmail";
            this.txt_recipientEmail.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_recipientEmail.Properties.Appearance.Options.UseFont = true;
            this.txt_recipientEmail.Properties.MaxLength = 50;
            this.txt_recipientEmail.Size = new System.Drawing.Size(189, 24);
            this.txt_recipientEmail.TabIndex = 1;
            // 
            // btn_SendMail
            // 
            this.btn_SendMail.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            this.btn_SendMail.Appearance.Font = new System.Drawing.Font("Tahoma", 15.25F);
            this.btn_SendMail.Appearance.Options.UseBackColor = true;
            this.btn_SendMail.Appearance.Options.UseFont = true;
            this.btn_SendMail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_SendMail.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_SendMail.ImageOptions.Image")));
            this.btn_SendMail.Location = new System.Drawing.Point(16, 208);
            this.btn_SendMail.Name = "btn_SendMail";
            this.btn_SendMail.Size = new System.Drawing.Size(320, 58);
            this.btn_SendMail.TabIndex = 6;
            this.btn_SendMail.Text = "Test Maili Gönder";
            this.btn_SendMail.Click += new System.EventHandler(this.btn_SendMail_Click);
            // 
            // btn_Update
            // 
            this.btn_Update.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btn_Update.Appearance.Font = new System.Drawing.Font("Tahoma", 15.25F);
            this.btn_Update.Appearance.Options.UseBackColor = true;
            this.btn_Update.Appearance.Options.UseFont = true;
            this.btn_Update.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Update.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Update.ImageOptions.Image")));
            this.btn_Update.Location = new System.Drawing.Point(16, 285);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(320, 58);
            this.btn_Update.TabIndex = 7;
            this.btn_Update.Text = "Kaydet";
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // chk_SSL
            // 
            this.chk_SSL.AutoSize = true;
            this.chk_SSL.Location = new System.Drawing.Point(184, 173);
            this.chk_SSL.Name = "chk_SSL";
            this.chk_SSL.Size = new System.Drawing.Size(106, 17);
            this.chk_SSL.TabIndex = 5;
            this.chk_SSL.Text = "SSL Olucak Mı ?";
            this.chk_SSL.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label1.Location = new System.Drawing.Point(12, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 20);
            this.label1.TabIndex = 57;
            this.label1.Text = "Port:";
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(182, 129);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_Port.Properties.Appearance.Options.UseFont = true;
            this.txt_Port.Properties.MaxLength = 4;
            this.txt_Port.Size = new System.Drawing.Size(189, 24);
            this.txt_Port.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label4.Location = new System.Drawing.Point(12, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 20);
            this.label4.TabIndex = 56;
            this.label4.Text = "Şifre:";
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(182, 67);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_Password.Properties.Appearance.Options.UseFont = true;
            this.txt_Password.Properties.MaxLength = 50;
            this.txt_Password.Properties.PasswordChar = '*';
            this.txt_Password.Size = new System.Drawing.Size(189, 24);
            this.txt_Password.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label3.Location = new System.Drawing.Point(12, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 55;
            this.label3.Text = "Sunucu:";
            // 
            // txt_Server
            // 
            this.txt_Server.Location = new System.Drawing.Point(182, 96);
            this.txt_Server.Name = "txt_Server";
            this.txt_Server.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_Server.Properties.Appearance.Options.UseFont = true;
            this.txt_Server.Properties.MaxLength = 30;
            this.txt_Server.Size = new System.Drawing.Size(189, 24);
            this.txt_Server.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 54;
            this.label2.Text = "E-Mail:";
            // 
            // txt_Email
            // 
            this.txt_Email.Location = new System.Drawing.Point(182, 5);
            this.txt_Email.Name = "txt_Email";
            this.txt_Email.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_Email.Properties.Appearance.Options.UseFont = true;
            this.txt_Email.Properties.MaxLength = 50;
            this.txt_Email.Size = new System.Drawing.Size(189, 24);
            this.txt_Email.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupControl1.CaptionImageOptions.Image")));
            this.groupControl1.Controls.Add(this.label10);
            this.groupControl1.Controls.Add(this.txt_Web);
            this.groupControl1.Controls.Add(this.label9);
            this.groupControl1.Controls.Add(this.txt_Adres);
            this.groupControl1.Controls.Add(this.label8);
            this.groupControl1.Controls.Add(this.txt_Phone);
            this.groupControl1.Controls.Add(this.label7);
            this.groupControl1.Controls.Add(this.pictureEdit1);
            this.groupControl1.Controls.Add(this.btn_Save);
            this.groupControl1.Controls.Add(this.label6);
            this.groupControl1.Controls.Add(this.txt_CompanyName);
            this.groupControl1.Location = new System.Drawing.Point(693, 8);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(345, 373);
            this.groupControl1.TabIndex = 59;
            this.groupControl1.Text = "Mail İmza Tanımları";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label10.Location = new System.Drawing.Point(5, 159);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 20);
            this.label10.TabIndex = 64;
            this.label10.Text = "Şirket Web Site:";
            // 
            // txt_Web
            // 
            this.txt_Web.Location = new System.Drawing.Point(140, 158);
            this.txt_Web.Name = "txt_Web";
            this.txt_Web.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_Web.Properties.Appearance.Options.UseFont = true;
            this.txt_Web.Properties.MaxLength = 75;
            this.txt_Web.Size = new System.Drawing.Size(189, 24);
            this.txt_Web.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label9.Location = new System.Drawing.Point(5, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 20);
            this.label9.TabIndex = 62;
            this.label9.Text = "Şirket Adres:";
            // 
            // txt_Adres
            // 
            this.txt_Adres.Location = new System.Drawing.Point(140, 119);
            this.txt_Adres.Name = "txt_Adres";
            this.txt_Adres.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_Adres.Properties.Appearance.Options.UseFont = true;
            this.txt_Adres.Properties.MaxLength = 300;
            this.txt_Adres.Size = new System.Drawing.Size(189, 24);
            this.txt_Adres.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label8.Location = new System.Drawing.Point(5, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 20);
            this.label8.TabIndex = 60;
            this.label8.Text = "Şirket Telefon:";
            // 
            // txt_Phone
            // 
            this.txt_Phone.Location = new System.Drawing.Point(140, 86);
            this.txt_Phone.Name = "txt_Phone";
            this.txt_Phone.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_Phone.Properties.Appearance.Options.UseFont = true;
            this.txt_Phone.Properties.MaxLength = 20;
            this.txt_Phone.Size = new System.Drawing.Size(189, 24);
            this.txt_Phone.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label7.Location = new System.Drawing.Point(5, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 20);
            this.label7.TabIndex = 58;
            this.label7.Text = "Şirket Resim:";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureEdit1.Location = new System.Drawing.Point(140, 198);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(189, 106);
            this.pictureEdit1.TabIndex = 4;
            // 
            // btn_Save
            // 
            this.btn_Save.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btn_Save.Appearance.Font = new System.Drawing.Font("Tahoma", 15.25F);
            this.btn_Save.Appearance.Options.UseBackColor = true;
            this.btn_Save.Appearance.Options.UseFont = true;
            this.btn_Save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.ImageOptions.Image")));
            this.btn_Save.Location = new System.Drawing.Point(20, 327);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(320, 41);
            this.btn_Save.TabIndex = 5;
            this.btn_Save.Text = "Kaydet";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label6.Location = new System.Drawing.Point(5, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 20);
            this.label6.TabIndex = 55;
            this.label6.Text = "Şirket Adı:";
            // 
            // txt_CompanyName
            // 
            this.txt_CompanyName.Location = new System.Drawing.Point(140, 45);
            this.txt_CompanyName.Name = "txt_CompanyName";
            this.txt_CompanyName.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.txt_CompanyName.Properties.Appearance.Options.UseFont = true;
            this.txt_CompanyName.Properties.MaxLength = 100;
            this.txt_CompanyName.Size = new System.Drawing.Size(189, 24);
            this.txt_CompanyName.TabIndex = 0;
            // 
            // MailSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1166, 444);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_recipientEmail);
            this.Controls.Add(this.btn_SendMail);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.chk_SSL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_Port);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Server);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_Email);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("MailSettingForm.IconOptions.Image")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MailSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mail Ayarları";
            this.Load += new System.EventHandler(this.MailSettingForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MailSettingForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txt_recipientEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Port.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Server.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Email.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Web.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Adres.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Phone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txt_recipientEmail;
        private DevExpress.XtraEditors.SimpleButton btn_SendMail;
        private DevExpress.XtraEditors.SimpleButton btn_Update;
        private System.Windows.Forms.CheckBox chk_SSL;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txt_Port;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txt_Password;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txt_Server;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txt_Email;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txt_CompanyName;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.TextEdit txt_Web;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txt_Adres;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txt_Phone;
    }
}