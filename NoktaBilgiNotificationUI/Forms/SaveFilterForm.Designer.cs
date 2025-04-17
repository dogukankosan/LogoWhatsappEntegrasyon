namespace NoktaBilgiNotificationUI.Forms
{
    partial class SaveFilterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveFilterForm));
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_Doc = new DevExpress.XtraEditors.SimpleButton();
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
            this.btn_Save.Location = new System.Drawing.Point(12, 12);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(144, 44);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "Kaydet";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Symbol", 8.25F);
            this.richTextBox1.Location = new System.Drawing.Point(175, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(691, 520);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // btn_Doc
            // 
            this.btn_Doc.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            this.btn_Doc.Appearance.Font = new System.Drawing.Font("Tahoma", 15.25F);
            this.btn_Doc.Appearance.Options.UseBackColor = true;
            this.btn_Doc.Appearance.Options.UseFont = true;
            this.btn_Doc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Doc.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Doc.ImageOptions.Image")));
            this.btn_Doc.Location = new System.Drawing.Point(12, 76);
            this.btn_Doc.Name = "btn_Doc";
            this.btn_Doc.Size = new System.Drawing.Size(144, 44);
            this.btn_Doc.TabIndex = 2;
            this.btn_Doc.Text = "Döküman";
            this.btn_Doc.Click += new System.EventHandler(this.btn_Doc_Click);
            // 
            // SaveFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(896, 562);
            this.Controls.Add(this.btn_Doc);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btn_Save);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("SaveFilterForm.IconOptions.Image")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "SaveFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filtre Uygula";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveFilterForm_FormClosing);
            this.Load += new System.EventHandler(this.XtraForm1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SaveFilterForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private DevExpress.XtraEditors.SimpleButton btn_Doc;
    }
}