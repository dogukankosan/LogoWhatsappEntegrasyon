namespace NoktaBilgiNotificationUI.Forms
{
    partial class OrderListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderListForm));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tool_WpSend = new System.Windows.Forms.ToolStripMenuItem();
            this.mailSendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PdfCreateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gönderilmediYapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(920, 509);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tool_WpSend,
            this.mailSendToolStripMenuItem,
            this.PdfCreateToolStripMenuItem,
            this.gönderilmediYapToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 114);
            // 
            // tool_WpSend
            // 
            this.tool_WpSend.Image = ((System.Drawing.Image)(resources.GetObject("tool_WpSend.Image")));
            this.tool_WpSend.Name = "tool_WpSend";
            this.tool_WpSend.Size = new System.Drawing.Size(180, 22);
            this.tool_WpSend.Text = "Wp Gönder";
            this.tool_WpSend.Click += new System.EventHandler(this.tool_WpSend_Click_1);
            // 
            // mailSendToolStripMenuItem
            // 
            this.mailSendToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mailSendToolStripMenuItem.Image")));
            this.mailSendToolStripMenuItem.Name = "mailSendToolStripMenuItem";
            this.mailSendToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.mailSendToolStripMenuItem.Text = "Mail Gönder";
            this.mailSendToolStripMenuItem.Click += new System.EventHandler(this.mailSendToolStripMenuItem_Click);
            // 
            // PdfCreateToolStripMenuItem
            // 
            this.PdfCreateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("PdfCreateToolStripMenuItem.Image")));
            this.PdfCreateToolStripMenuItem.Name = "PdfCreateToolStripMenuItem";
            this.PdfCreateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.PdfCreateToolStripMenuItem.Text = "Pdf Oluştur (Örnek)";
            this.PdfCreateToolStripMenuItem.Click += new System.EventHandler(this.PdfCreateToolStripMenuItem_Click);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.gridView1.Appearance.Row.Options.UseBackColor = true;
            this.gridView1.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gönderilmediYapToolStripMenuItem
            // 
            this.gönderilmediYapToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("gönderilmediYapToolStripMenuItem.Image")));
            this.gönderilmediYapToolStripMenuItem.Name = "gönderilmediYapToolStripMenuItem";
            this.gönderilmediYapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gönderilmediYapToolStripMenuItem.Text = "Gönderilmedi Yap";
            this.gönderilmediYapToolStripMenuItem.Click += new System.EventHandler(this.gönderilmediYapToolStripMenuItem_Click);
            // 
            // OrderListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(920, 509);
            this.Controls.Add(this.gridControl1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("OrderListForm.IconOptions.Image")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "OrderListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sipariş Listesi";
            this.Load += new System.EventHandler(this.OrderListForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OrderListForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tool_WpSend;
        private System.Windows.Forms.ToolStripMenuItem PdfCreateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mailSendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gönderilmediYapToolStripMenuItem;
    }
}