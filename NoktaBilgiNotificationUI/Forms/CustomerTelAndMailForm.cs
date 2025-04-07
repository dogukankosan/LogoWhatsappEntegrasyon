using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using NoktaBilgiNotificationUI.Classes;
using System.IO;
using ExcelDataReader;
using NoktaBilgiNotificationUI.Business;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class CustomerTelAndMailForm : XtraForm
    {
        public CustomerTelAndMailForm()
        {
            InitializeComponent();
        }
        string companyCode = "", companyPeriod = "";
        private void CompanySettingsGet()
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode,LogoPeriod FROM CompanySettings LIMIT 1");
            if (!(dt is null))
            {
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        companyCode = dt.Rows[0][0].ToString();
                        companyPeriod = dt.Rows[0][1].ToString();
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Hatalı Şirket Bilgileri Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        this.Close();
                    }
                }
            }
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
        private void List()
        {
            string query = $@"SELECT LOGICALREF,CODE 'Cari Kodu',DEFINITION_ 'Cari Açıklaması',TELNRS1 'Telefon',EMAILADDR 'Mail' FROM LG_{companyCode}_CLCARD WITH (NOLOCK) ORDER BY 2";
            try
            {
                DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
                if (dt is null)
                {
                    XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dt.Rows.Count <= 0)
                {
                    XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                gridControl1.DataSource = SQLCrud.LoadDataIntoGridView(query, dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı SQL Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                this.Close();
            }
        }
        private void CustomerTelAndMailForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        private async void btn_Logo_Click(object sender, EventArgs e)
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            if (dt is null)
            {
                XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dt.Rows.Count <= 0)
            {
                XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
            {
                XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool status = false;
            bool GeneralStatus = true;
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                var logicalRef = gridView1.GetListSourceRowCellValue(i, "LOGICALREF")?.ToString();
                var customerName = gridView1.GetRowCellValue(i, "Cari Açıklaması")?.ToString();
                var customerCode = gridView1.GetRowCellValue(i, "Cari Kodu")?.ToString();
                var telefon = gridView1.GetRowCellValue(i, "Telefon")?.ToString();
                var mail = gridView1.GetRowCellValue(i, "Mail")?.ToString();
                if (!(CustomerLogic.IsValidMailAdres(mail) && CustomerLogic.IsValidPhoneNumber(telefon)))
                {
                    GeneralStatus = false;
                    break;
                }
                    
                status = await SQLCrud.InserUpdateDelete($"UPDATE LG_{companyCode}_CLCARD SET TELNRS1='{telefon}', EMAILADDR='{mail}' WHERE LOGICALREF='{logicalRef}' AND CODE='"+customerCode+"' ", dt.Rows[0][0].ToString());
                if (!status)
                { GeneralStatus = false;
                    XtraMessageBox.Show($"Satır {i + 1} güncellenemedi. Müşteri Kodu {customerCode}  Müşteri Adı: {customerName}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            if (GeneralStatus)
                XtraMessageBox.Show("Aktarım İşlemi Tamamlandı", "Başarılı Logoya Aktarım", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btn_Excel_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Excel Files|*.xls;*.xlsx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                        LoadExcelToGrid(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Excel Seçimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void LoadExcelToGrid(string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    DataTable dataTable = result.Tables[0];
                    gridControl1.DataSource = dataTable;
                }
            }
        }
        private void dışarıAktarExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Dosyası (*.xlsx)|*.xlsx";
                saveDialog.FileName = "MüşterilerTelefonMail.xlsx";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    gridView1.ExportToXlsx(saveDialog.FileName);
                    XtraMessageBox.Show("Excel dosyası başarıyla kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }
        private void CustomerTelAndMailForm_Load(object sender, EventArgs e)
        {
            CompanySettingsGet();
            if (string.IsNullOrEmpty(companyCode))
            {
                XtraMessageBox.Show("Lütfen önce şirket bilgileri ayarları giriniz.", "SQLite Veritabanı okuma işlemi hatalı.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List();
            if (!(gridControl1.DataSource is null))
            {
                GridDesigner();
            }
        }
    }
}