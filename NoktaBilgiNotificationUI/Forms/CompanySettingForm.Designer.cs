namespace NoktaBilgiNotificationUI.Forms
{
    partial class CompanySettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanySettingForm));
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_CompanyPeriod = new DevExpress.XtraEditors.TextEdit();
            this.txt_CompanyNumber = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_CompanyName = new DevExpress.XtraEditors.TextEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_ManagerName = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_CompanyNo = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Domain = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_ISSPath = new DevExpress.XtraEditors.TextEdit();
            this.btn_ISSPath = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyPeriod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ManagerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Domain.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ISSPath.Properties)).BeginInit();
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
            this.btn_Save.Location = new System.Drawing.Point(175, 425);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(195, 44);
            this.btn_Save.TabIndex = 8;
            this.btn_Save.Text = "Kaydet";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 21);
            this.label2.TabIndex = 17;
            this.label2.Text = "Logo Dönem:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 21);
            this.label1.TabIndex = 16;
            this.label1.Text = "Logo Şirket Kodu:";
            // 
            // txt_CompanyPeriod
            // 
            this.txt_CompanyPeriod.Location = new System.Drawing.Point(174, 39);
            this.txt_CompanyPeriod.Name = "txt_CompanyPeriod";
            this.txt_CompanyPeriod.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_CompanyPeriod.Properties.Appearance.Options.UseFont = true;
            this.txt_CompanyPeriod.Properties.MaxLength = 10;
            this.txt_CompanyPeriod.Size = new System.Drawing.Size(195, 24);
            this.txt_CompanyPeriod.TabIndex = 1;
            this.txt_CompanyPeriod.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_CompanyPeriod_KeyPress);
            // 
            // txt_CompanyNumber
            // 
            this.txt_CompanyNumber.Location = new System.Drawing.Point(174, 8);
            this.txt_CompanyNumber.Name = "txt_CompanyNumber";
            this.txt_CompanyNumber.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_CompanyNumber.Properties.Appearance.Options.UseFont = true;
            this.txt_CompanyNumber.Properties.MaxLength = 10;
            this.txt_CompanyNumber.Size = new System.Drawing.Size(195, 24);
            this.txt_CompanyNumber.TabIndex = 0;
            this.txt_CompanyNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_CompanyNumber_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label6.Location = new System.Drawing.Point(12, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 21);
            this.label6.TabIndex = 25;
            this.label6.Text = "Şirket Adı:";
            // 
            // txt_CompanyName
            // 
            this.txt_CompanyName.Location = new System.Drawing.Point(174, 112);
            this.txt_CompanyName.Name = "txt_CompanyName";
            this.txt_CompanyName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_CompanyName.Properties.Appearance.Options.UseFont = true;
            this.txt_CompanyName.Properties.MaxLength = 250;
            this.txt_CompanyName.Size = new System.Drawing.Size(195, 24);
            this.txt_CompanyName.TabIndex = 3;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureEdit1.Location = new System.Drawing.Point(174, 294);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(195, 106);
            this.pictureEdit1.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label7.Location = new System.Drawing.Point(12, 294);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 21);
            this.label7.TabIndex = 27;
            this.label7.Text = "Şirket Logo:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label3.Location = new System.Drawing.Point(12, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 21);
            this.label3.TabIndex = 32;
            this.label3.Text = "Yetkili Adı:";
            // 
            // txt_ManagerName
            // 
            this.txt_ManagerName.Location = new System.Drawing.Point(174, 152);
            this.txt_ManagerName.Name = "txt_ManagerName";
            this.txt_ManagerName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_ManagerName.Properties.Appearance.Options.UseFont = true;
            this.txt_ManagerName.Properties.MaxLength = 100;
            this.txt_ManagerName.Size = new System.Drawing.Size(195, 24);
            this.txt_ManagerName.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label4.Location = new System.Drawing.Point(12, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 21);
            this.label4.TabIndex = 34;
            this.label4.Text = "Logo Firma No:";
            // 
            // txt_CompanyNo
            // 
            this.txt_CompanyNo.Location = new System.Drawing.Point(174, 76);
            this.txt_CompanyNo.Name = "txt_CompanyNo";
            this.txt_CompanyNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_CompanyNo.Properties.Appearance.Options.UseFont = true;
            this.txt_CompanyNo.Properties.MaxLength = 10;
            this.txt_CompanyNo.Size = new System.Drawing.Size(195, 24);
            this.txt_CompanyNo.TabIndex = 2;
            this.txt_CompanyNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_CompanyNo_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label5.Location = new System.Drawing.Point(12, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 21);
            this.label5.TabIndex = 36;
            this.label5.Text = "Domain URL:";
            // 
            // txt_Domain
            // 
            this.txt_Domain.Location = new System.Drawing.Point(174, 198);
            this.txt_Domain.Name = "txt_Domain";
            this.txt_Domain.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_Domain.Properties.Appearance.Options.UseFont = true;
            this.txt_Domain.Properties.MaxLength = 200;
            this.txt_Domain.Size = new System.Drawing.Size(195, 24);
            this.txt_Domain.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label8.Location = new System.Drawing.Point(12, 249);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 21);
            this.label8.TabIndex = 38;
            this.label8.Text = "IIS Yol:";
            // 
            // txt_ISSPath
            // 
            this.txt_ISSPath.Enabled = false;
            this.txt_ISSPath.Location = new System.Drawing.Point(174, 248);
            this.txt_ISSPath.Name = "txt_ISSPath";
            this.txt_ISSPath.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.txt_ISSPath.Properties.Appearance.Options.UseFont = true;
            this.txt_ISSPath.Properties.MaxLength = 500;
            this.txt_ISSPath.Size = new System.Drawing.Size(195, 24);
            this.txt_ISSPath.TabIndex = 6;
            // 
            // btn_ISSPath
            // 
            this.btn_ISSPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ISSPath.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton1.ImageOptions.SvgImage")));
            this.btn_ISSPath.Location = new System.Drawing.Point(113, 242);
            this.btn_ISSPath.Name = "btn_ISSPath";
            this.btn_ISSPath.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btn_ISSPath.Size = new System.Drawing.Size(39, 37);
            this.btn_ISSPath.TabIndex = 39;
            this.btn_ISSPath.Click += new System.EventHandler(this.btn_ISSPath_Click);
            // 
            // CompanySettingForm
            // 
            this.AcceptButton = this.btn_Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(994, 515);
            this.Controls.Add(this.btn_ISSPath);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txt_ISSPath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_Domain);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_CompanyNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_ManagerName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_CompanyName);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_CompanyPeriod);
            this.Controls.Add(this.txt_CompanyNumber);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("CompanySettingForm.IconOptions.Image")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "CompanySettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Şirket Bilgileri Ayarları";
            this.Load += new System.EventHandler(this.CompanySettingForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CompanySettingForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyPeriod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ManagerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CompanyNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Domain.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ISSPath.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txt_CompanyPeriod;
        private DevExpress.XtraEditors.TextEdit txt_CompanyNumber;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txt_CompanyName;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txt_ManagerName;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txt_CompanyNo;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txt_Domain;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txt_ISSPath;
        private DevExpress.XtraEditors.SimpleButton btn_ISSPath;
    }
}