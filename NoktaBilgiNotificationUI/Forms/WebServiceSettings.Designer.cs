namespace NoktaBilgiNotificationUI.Forms
{
    partial class WebServiceSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebServiceSettings));
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Password = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_UserName = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_DatabaseName = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Servername = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_WebURL = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_CompanyName = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_DatabaseName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Servername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WebURL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Save
            // 
            this.btn_Save.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btn_Save.Appearance.Font = new System.Drawing.Font("Tahoma", 15.25F);
            this.btn_Save.Appearance.Options.UseBackColor = true;
            this.btn_Save.Appearance.Options.UseFont = true;
            this.btn_Save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.ImageOptions.Image")));
            this.btn_Save.Location = new System.Drawing.Point(175, 254);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(195, 44);
            this.btn_Save.TabIndex = 8;
            this.btn_Save.Text = "Kaydet";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label4.Location = new System.Drawing.Point(12, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 21);
            this.label4.TabIndex = 52;
            this.label4.Text = "SQL Şifresi:";
            // 
            // txt_Password
            // 
            this.txt_Password.EditValue = "@7Sp:f@7J4=a9ZOt";
            this.txt_Password.Location = new System.Drawing.Point(178, 207);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_Password.Properties.Appearance.Options.UseFont = true;
            this.txt_Password.Properties.MaxLength = 100;
            this.txt_Password.Properties.PasswordChar = '*';
            this.txt_Password.Size = new System.Drawing.Size(192, 24);
            this.txt_Password.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label3.Location = new System.Drawing.Point(12, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 21);
            this.label3.TabIndex = 51;
            this.label3.Text = "SQL Kullanıcı Adı:";
            // 
            // txt_UserName
            // 
            this.txt_UserName.EditValue = "u5705706_userC6D";
            this.txt_UserName.Location = new System.Drawing.Point(178, 167);
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_UserName.Properties.Appearance.Options.UseFont = true;
            this.txt_UserName.Properties.MaxLength = 50;
            this.txt_UserName.Properties.PasswordChar = '*';
            this.txt_UserName.Size = new System.Drawing.Size(192, 24);
            this.txt_UserName.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label1.Location = new System.Drawing.Point(12, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 21);
            this.label1.TabIndex = 50;
            this.label1.Text = "SQL Veritabanı Adı:";
            // 
            // txt_DatabaseName
            // 
            this.txt_DatabaseName.EditValue = "u5705706_dbC6D";
            this.txt_DatabaseName.Location = new System.Drawing.Point(178, 125);
            this.txt_DatabaseName.Name = "txt_DatabaseName";
            this.txt_DatabaseName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_DatabaseName.Properties.Appearance.Options.UseFont = true;
            this.txt_DatabaseName.Properties.MaxLength = 50;
            this.txt_DatabaseName.Properties.PasswordChar = '*';
            this.txt_DatabaseName.Size = new System.Drawing.Size(192, 24);
            this.txt_DatabaseName.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label5.Location = new System.Drawing.Point(12, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 21);
            this.label5.TabIndex = 49;
            this.label5.Text = "SQL Server Adı:";
            // 
            // txt_Servername
            // 
            this.txt_Servername.EditValue = "94.73.147.7";
            this.txt_Servername.Location = new System.Drawing.Point(178, 88);
            this.txt_Servername.Name = "txt_Servername";
            this.txt_Servername.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_Servername.Properties.Appearance.Options.UseFont = true;
            this.txt_Servername.Properties.MaxLength = 100;
            this.txt_Servername.Properties.PasswordChar = '*';
            this.txt_Servername.Size = new System.Drawing.Size(192, 24);
            this.txt_Servername.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label7.Location = new System.Drawing.Point(12, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 21);
            this.label7.TabIndex = 54;
            this.label7.Text = "Web Adres:";
            // 
            // txt_WebURL
            // 
            this.txt_WebURL.EditValue = "https://noktabilgiislem.tr/";
            this.txt_WebURL.Location = new System.Drawing.Point(178, 48);
            this.txt_WebURL.Name = "txt_WebURL";
            this.txt_WebURL.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_WebURL.Properties.Appearance.Options.UseFont = true;
            this.txt_WebURL.Properties.MaxLength = 250;
            this.txt_WebURL.Size = new System.Drawing.Size(192, 24);
            this.txt_WebURL.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label8.Location = new System.Drawing.Point(15, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 21);
            this.label8.TabIndex = 56;
            this.label8.Text = "Şirket Adı:";
            // 
            // txt_CompanyName
            // 
            this.txt_CompanyName.Location = new System.Drawing.Point(178, 8);
            this.txt_CompanyName.Name = "txt_CompanyName";
            this.txt_CompanyName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_CompanyName.Properties.Appearance.Options.UseFont = true;
            this.txt_CompanyName.Properties.MaxLength = 250;
            this.txt_CompanyName.Size = new System.Drawing.Size(192, 24);
            this.txt_CompanyName.TabIndex = 0;
            // 
            // WebServiceSettings
            // 
            this.AcceptButton = this.btn_Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(796, 481);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txt_CompanyName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_WebURL);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_UserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_DatabaseName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_Servername);
            this.Controls.Add(this.btn_Save);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("WebServiceSettings.IconOptions.Image")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "WebServiceSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web Servis Ayarları";
            this.Load += new System.EventHandler(this.WebServiceSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WebServiceSettings_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_DatabaseName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Servername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WebURL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txt_Password;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txt_UserName;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txt_DatabaseName;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txt_Servername;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txt_WebURL;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txt_CompanyName;
    }
}