namespace NoktaBilgiNotificationUI.Forms
{
    partial class MailAndWpCountForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailAndWpCountForm));
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nmr_mail = new System.Windows.Forms.NumericUpDown();
            this.nmr_wp = new System.Windows.Forms.NumericUpDown();
            this.rd_Wp = new System.Windows.Forms.RadioButton();
            this.rdb_Mail = new System.Windows.Forms.RadioButton();
            this.rdb_Customer = new System.Windows.Forms.RadioButton();
            this.rdb_Manager = new System.Windows.Forms.RadioButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_mail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_wp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
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
            this.btn_Save.Location = new System.Drawing.Point(150, 254);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(195, 44);
            this.btn_Save.TabIndex = 2;
            this.btn_Save.Text = "Kaydet";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 21);
            this.label2.TabIndex = 13;
            this.label2.Text = "Whatsapp Sayi:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 21);
            this.label1.TabIndex = 12;
            this.label1.Text = "Mail Sayi:";
            // 
            // nmr_mail
            // 
            this.nmr_mail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.nmr_mail.Location = new System.Drawing.Point(150, 7);
            this.nmr_mail.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nmr_mail.Name = "nmr_mail";
            this.nmr_mail.Size = new System.Drawing.Size(195, 23);
            this.nmr_mail.TabIndex = 0;
            this.nmr_mail.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmr_mail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nmr_mail_KeyPress);
            // 
            // nmr_wp
            // 
            this.nmr_wp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.nmr_wp.Location = new System.Drawing.Point(150, 52);
            this.nmr_wp.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nmr_wp.Name = "nmr_wp";
            this.nmr_wp.Size = new System.Drawing.Size(195, 23);
            this.nmr_wp.TabIndex = 1;
            this.nmr_wp.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmr_wp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nmr_wp_KeyPress);
            // 
            // rd_Wp
            // 
            this.rd_Wp.AutoSize = true;
            this.rd_Wp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.rd_Wp.Location = new System.Drawing.Point(136, 29);
            this.rd_Wp.Name = "rd_Wp";
            this.rd_Wp.Size = new System.Drawing.Size(167, 24);
            this.rd_Wp.TabIndex = 3;
            this.rd_Wp.TabStop = true;
            this.rd_Wp.Text = "Whatsapp İle Çalış";
            this.rd_Wp.UseVisualStyleBackColor = true;
            // 
            // rdb_Mail
            // 
            this.rdb_Mail.AutoSize = true;
            this.rdb_Mail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.rdb_Mail.Location = new System.Drawing.Point(7, 28);
            this.rdb_Mail.Name = "rdb_Mail";
            this.rdb_Mail.Size = new System.Drawing.Size(123, 24);
            this.rdb_Mail.TabIndex = 2;
            this.rdb_Mail.TabStop = true;
            this.rdb_Mail.Text = "Mail İle Çalış";
            this.rdb_Mail.UseVisualStyleBackColor = true;
            // 
            // rdb_Customer
            // 
            this.rdb_Customer.AutoSize = true;
            this.rdb_Customer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.rdb_Customer.Location = new System.Drawing.Point(174, 32);
            this.rdb_Customer.Name = "rdb_Customer";
            this.rdb_Customer.Size = new System.Drawing.Size(157, 24);
            this.rdb_Customer.TabIndex = 15;
            this.rdb_Customer.TabStop = true;
            this.rdb_Customer.Text = "Müşteri İçin Çalış";
            this.rdb_Customer.UseVisualStyleBackColor = true;
            // 
            // rdb_Manager
            // 
            this.rdb_Manager.AutoSize = true;
            this.rdb_Manager.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.rdb_Manager.Location = new System.Drawing.Point(8, 31);
            this.rdb_Manager.Name = "rdb_Manager";
            this.rdb_Manager.Size = new System.Drawing.Size(160, 24);
            this.rdb_Manager.TabIndex = 14;
            this.rdb_Manager.TabStop = true;
            this.rdb_Manager.Text = "Yönetici İçin Çalış";
            this.rdb_Manager.UseVisualStyleBackColor = true;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rd_Wp);
            this.groupControl1.Controls.Add(this.rdb_Mail);
            this.groupControl1.Location = new System.Drawing.Point(16, 85);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(329, 59);
            this.groupControl1.TabIndex = 16;
            this.groupControl1.Text = "Gönderim Türü";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rdb_Customer);
            this.groupControl2.Controls.Add(this.rdb_Manager);
            this.groupControl2.Location = new System.Drawing.Point(16, 157);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(329, 68);
            this.groupControl2.TabIndex = 17;
            this.groupControl2.Text = "Çalışma Şekli";
            // 
            // MailAndWpCountForm
            // 
            this.AcceptButton = this.btn_Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(726, 465);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.nmr_wp);
            this.Controls.Add(this.nmr_mail);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("MailAndWpCountForm.IconOptions.Image")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MailAndWpCountForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mail Ve Wp Gönderme Sayısı";
            this.Load += new System.EventHandler(this.MailAndWpCountForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MailAndWpCountForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.nmr_mail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_wp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmr_mail;
        private System.Windows.Forms.NumericUpDown nmr_wp;
        private System.Windows.Forms.RadioButton rd_Wp;
        private System.Windows.Forms.RadioButton rdb_Mail;
        private System.Windows.Forms.RadioButton rdb_Customer;
        private System.Windows.Forms.RadioButton rdb_Manager;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}