using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class CompanyFilterForm : XtraForm
    {
        public CompanyFilterForm()
        {
            InitializeComponent();
        }
        private bool btnSaveStatus = false;
        private async Task<bool> TriggerSQL(string connection,string companyCode,string companyPeriod,string customerFilter,string salesFilter)
        {
            string sqlFilePath = "";
            sqlFilePath = Path.Combine(Application.StartupPath, "Queries", "DeleteTrigger.sql");
            if (!File.Exists(sqlFilePath))
            {
                XtraMessageBox.Show("Hata: DeleteTrigger.sql dosyası bulunamadı! Dosya yolu: " + sqlFilePath, "Hatalı yol", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }  
            sqlFilePath = Path.Combine(Application.StartupPath, "Queries", "Trigger.sql");
            if (!File.Exists(sqlFilePath))
            {
                XtraMessageBox.Show("Hata: Trigger.sql dosyası bulunamadı! Dosya yolu: " + sqlFilePath, "Hatalı yol", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            string sqlScript = "";

            sqlScript = File.ReadAllText(Application.StartupPath + "\\Queries\\DeleteTrigger.sql").Replace("\r\n", " ")
                          .Replace("\n", " ")
                          .Replace("\t", " ")
                          .Trim();

            bool statusTriggerDelete = await SQLCrud.InserUpdateDelete(sqlScript, connection);

            sqlScript = File.ReadAllText(Application.StartupPath + "\\Queries\\Trigger.sql").Replace("\r\n", " ")
                       .Replace("\n", " ")
                       .Replace("\t", " ")
                       .Trim();
            sqlScript = sqlScript.Replace("LG_001", $"LG_{companyCode}");
            sqlScript = sqlScript.Replace("LG_001_01", $"LG_{companyCode}_{companyPeriod}");
            sqlScript = sqlScript.Replace("SALESFILTER",salesFilter);
            sqlScript = sqlScript.Replace("CUSTOMERFILTER",customerFilter);
            bool statusTriggerCreate = await SQLCrud.InserUpdateDelete(sqlScript, connection);
            if (statusTriggerCreate && statusTriggerDelete)
                return true;
            return false;
        }
        private void CompanyFilterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        void GridDesigner(GridView gridView1)
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
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
        }
        private void CompanyFilterForm_Load(object sender, EventArgs e)
        {
            DataTable sqlConnect = SQLiteCrud.GetDataFromSQLite("SELECT * FROM SqlConnectionString LIMIT 1");
            DataTable company = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode,LogoPeriod,CompanyNo FROM CompanySettings LIMIT 1 ");
            
            if (!(company is null) && !(sqlConnect is null))
            {
                if (company.Rows.Count > 0 && sqlConnect.Rows.Count > 0)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(company.Rows[0][0].ToString()))
                        {
                            XtraMessageBox.Show("Lütfen önce şirket bilgilerinden logo şirket kodu ve logo dönemi bilgilerini doldurunuz", "Hatalı SQL Bağlantısı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                        gridControl1.DataSource = SQLCrud.LoadDataIntoGridView($"SELECT  LOGICALREF,CODE 'Satıcı Kodu',DEFINITION_ 'Satıcı Açıklama' FROM  LG_SLSMAN  WITH (NOLOCK) WHERE FIRMNR=" + company.Rows[0][2].ToString() + " ORDER BY 3", sqlConnect.Rows[0][0].ToString());
                        GridDesigner(gridView1);
                        gridView1.Columns[0].Visible = false;
                        gridControl2.DataSource = SQLCrud.LoadDataIntoGridView($"SELECT LOGICALREF,CODE 'Cari Kod',DEFINITION_ 'Cari Açıklama' FROM LG_001_CLCARD WITH (NOLOCK) WHERE  ACTIVE=0 ORDER BY 3", sqlConnect.Rows[0][0].ToString());
                        GridDesigner(gridView2);
                        gridView2.Columns[0].Visible = false;
                        DataTable filter = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 * FROM NKT_TABLEFILTER WITH (NOLOCK)", sqlConnect.Rows[0][0].ToString());
                        if (filter != null && filter.Rows.Count > 0)
                        {
                            string customerRaw = filter.Rows[0][0]?.ToString();
                            string salesmanRaw = filter.Rows[0][1]?.ToString();
                            if (!string.IsNullOrWhiteSpace(salesmanRaw))
                            {
                                var selectedSalesmanIds = salesmanRaw.Split(',')
                                                                      .Select(s => s.Trim())
                                                                      .Where(s => !string.IsNullOrEmpty(s))
                                                                      .ToList();
                                for (int i = 0; i < gridView1.RowCount; i++)
                                {
                                    var salesmanId = gridView1.GetRowCellValue(i, "LOGICALREF")?.ToString();
                                    if (!string.IsNullOrEmpty(salesmanId) && selectedSalesmanIds.Contains(salesmanId))
                                        gridView1.SelectRow(i);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(customerRaw))
                            {
                                var selectedCustomerIds = customerRaw.Split(',')
                                                                      .Select(s => s.Trim())
                                                                      .Where(s => !string.IsNullOrEmpty(s))
                                                                      .ToList();
                                for (int i = 0; i < gridView2.RowCount; i++)
                                {
                                    var customerId = gridView2.GetRowCellValue(i, "LOGICALREF")?.ToString(); 

                                    if (!string.IsNullOrEmpty(customerId) && selectedCustomerIds.Contains(customerId))
                                        gridView2.SelectRow(i);
                                }
                            }
                            nmr_OrderMax.Value = int.Parse(filter.Rows[0][2].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Hatalı SQL Bağlantısı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        return;
                    }
                }
            }

        }
        private void nmr_OrderMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = true;
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            var selectedSalesman = gridView1.GetSelectedRows()
     .Select(rowHandle => gridView1.GetRowCellValue(rowHandle, "LOGICALREF")?.ToString())
     .Where(val => !string.IsNullOrEmpty(val))
     .ToList();
            string resultSalesman = string.Join(",", selectedSalesman);
            resultSalesman=string.Concat(",", resultSalesman, ",");

            var selectedCustomer = gridView2.GetSelectedRows()
.Select(rowHandle => gridView2.GetRowCellValue(rowHandle, "LOGICALREF")?.ToString())
.Where(val => !string.IsNullOrEmpty(val))
.ToList();
            string resultCustomer = string.Join(",", selectedCustomer);
            resultCustomer = string.Concat(",", resultCustomer, ",");

            if (string.IsNullOrEmpty(resultSalesman.Replace(",","")))
            {
                XtraMessageBox.Show("En az bir tane satıcı personel seçiniz", "Hatalı Seçim", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(resultCustomer.Replace(",","")))
            {
                XtraMessageBox.Show("En az bir tane cari seçiniz", "Hatalı Seçim", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataTable sqlConnect = SQLiteCrud.GetDataFromSQLite("SELECT * FROM SqlConnectionString LIMIT 1");
            DataTable company = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode,LogoPeriod,CompanyNo FROM CompanySettings LIMIT 1 ");

            bool filterStatus = await SQLCrud.InserUpdateDelete("UPDATE NKT_TABLEFILTER SET CustomerLogicalRef='" + resultCustomer.Trim() + "', SalesManLogicalRef='" + resultSalesman.Trim() + "',OrderMax=" + nmr_OrderMax.Value + "", sqlConnect.Rows[0][0].ToString());

            if (!filterStatus)
            {
                XtraMessageBox.Show("Şirket filtre güncelleme işlemi hatalı", "Hatalı SQL İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool status=await TriggerSQL(sqlConnect.Rows[0][0].ToString(), company.Rows[0][0].ToString(), company.Rows[0][1].ToString(), resultCustomer, resultSalesman);
            if (!status)
            {
                XtraMessageBox.Show("Triggerları kaydetme işlemi hatalı", "Hatalı SQL Trigger Kaydı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            XtraMessageBox.Show("Şirket filtreleri güncelleme işlemi başarılı", "Başarılı Şirket Filtreleri Güncelleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnSaveStatus = true;
            this.Close();
        }
        private void CompanyFilterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!btnSaveStatus)
            {
                XtraMessageBox.Show("Lütfen şirket filtrelerinizi ayarlayıp kaydediniz", "Hatalı Şirket Filtreleri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; 
            }
        }
    }
}