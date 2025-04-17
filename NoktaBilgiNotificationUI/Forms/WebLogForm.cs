using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class WebLogForm : XtraForm
    {
        public WebLogForm()
        {
            InitializeComponent();
        }
        void GridDesigner()
        {
            gridView1.Appearance.HeaderPanel.Font = new Font("Tahoma", 11, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            gridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.RowHeight = 20;
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();
            gridView1.Appearance.HeaderPanel.Font = new Font("Tahoma", 8, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.OptionsView.ShowAutoFilterRow = true;
            gridView1.Appearance.FilterPanel.Font = new Font("Tahoma", 10, FontStyle.Italic);
            gridView1.Appearance.FilterPanel.ForeColor = Color.Blue;
            gridView1.Appearance.FilterPanel.Options.UseForeColor = true;
            gridView1.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsFind.AlwaysVisible = true;
            gridView1.OptionsView.ShowGroupPanel = true;
        }
        private void WebLogForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
        private void WebLogForm_Load(object sender, EventArgs e)
        {
            DataTable remoteSQL = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString,WebToken,WebPassword FROM WebSettings LIMIT 1");
            if (remoteSQL.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(remoteSQL.Rows[0]["SQLConnectString"].ToString()))
                {
                    XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable WpCountAndCustomerStatus = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 ID FROM Customers WITH (NOLOCK) WHERE CustomerToken='" + remoteSQL.Rows[0]["WebToken"] + "' AND CustomerPassword='" + remoteSQL.Rows[0]["WebPassword"] + "' ORDER BY ID DESC", remoteSQL.Rows[0]["SQLConnectString"].ToString());
            if (WpCountAndCustomerStatus is null)
            {
                XtraMessageBox.Show("Whatsapp Kontör Okuma İşlemi Hatalı", "Hatalı Whatsapp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(WpCountAndCustomerStatus.Rows[0][0].ToString()))
                {
                    XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (WpCountAndCustomerStatus.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            gridControl1.DataSource=SQLCrud.LoadDataIntoGridView("SELECT LogDetails 'Hata Mesajı',CONVERT(DATETIME ,Date_) 'Hata Tarih'  FROM Logs WITH (NOLOCK) WHERE CustomerID=" + WpCountAndCustomerStatus.Rows[0][0].ToString() + "", remoteSQL.Rows[0][0].ToString());
            if (!(gridControl1.DataSource is null))
                gridView1.Columns["Hata Tarih"].DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            GridDesigner();
        }
        private void excelAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel Dosyası (*.xlsx)|*.xlsx";
            saveDialog.Title = "Excel'e Aktar";
            saveDialog.FileName = "WebLogRapor.xlsx";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                gridView1.OptionsPrint.PrintDetails = true; 
                gridControl1.ExportToXlsx(saveDialog.FileName);
                XtraMessageBox.Show("Excel dosyası başarıyla oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}