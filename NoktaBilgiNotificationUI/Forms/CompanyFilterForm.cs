using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
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

        private void CompanyFilterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        void GridDesigner(GridView gridView)
        {
            gridView.Appearance.HeaderPanel.Font = new Font("Tahoma", 11, FontStyle.Bold);
            gridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView.Appearance.HeaderPanel.Options.UseFont = true;
            gridView.RowHeight = 20;
            gridView.OptionsView.EnableAppearanceEvenRow = true;
            gridView.OptionsView.EnableAppearanceOddRow = true;
            gridView.OptionsView.ColumnAutoWidth = false;
            gridView.BestFitColumns();
            gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridView.OptionsSelection.MultiSelect = true;
            gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            gridView.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            gridView.OptionsFind.AlwaysVisible = true;

            // Tüm sütunları readonly yap
            foreach (GridColumn col in gridView.Columns)
                col.OptionsColumn.AllowEdit = false;

            // Sadece "Sipariş Tutarı" editable olsun
            if (gridView.Name == "gridView2" && gridView.Columns["Sipariş Tutarı"] != null)
            {
                gridView.Columns["Sipariş Tutarı"].OptionsColumn.AllowEdit = true;
            }

            // Satır ekleme/silme kapalı
            gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            gridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
        }

        private void SiparisTutariniEkle(decimal tutar)
        {
            DataTable dt = gridControl2.DataSource as DataTable;
            if (dt == null) return;
            if (!dt.Columns.Contains("Sipariş Tutarı"))
                dt.Columns.Add("Sipariş Tutarı", typeof(decimal));
            foreach (DataRow row in dt.Rows)
            {
                if (row.IsNull("Sipariş Tutarı") || Convert.ToDecimal(row["Sipariş Tutarı"]) <= 0)
                    row["Sipariş Tutarı"] = Math.Max(tutar, 1); 
            }
        }
        private void CompanyFilterForm_Load(object sender, EventArgs e)
        {
            DataTable sqlConnect = SQLiteCrud.GetDataFromSQLite("SELECT * FROM SqlConnectionString LIMIT 1");
            DataTable company = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode,LogoPeriod,CompanyNo FROM CompanySettings LIMIT 1 ");
            if (company != null && sqlConnect != null && company.Rows.Count > 0 && sqlConnect.Rows.Count > 0)
            {
                try
                {
                    if (string.IsNullOrEmpty(company.Rows[0][0].ToString()))
                    {
                        XtraMessageBox.Show("Lütfen şirket bilgilerini doldurunuz", "Hatalı SQL Bağlantısı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                    DataTable customerLogical = SQLCrud.LoadDataIntoGridView("SELECT DISTINCT CustomerLogicalRef, OrderMax FROM NKT_TABLEFILTER WITH (NOLOCK)", sqlConnect.Rows[0][0].ToString());
                    DataTable salesLogical = SQLCrud.LoadDataIntoGridView("SELECT DISTINCT SalesmanLogicalRef FROM NKT_TABLEFILTER WITH (NOLOCK)", sqlConnect.Rows[0][0].ToString());
                    DataTable paydayLogical = SQLCrud.LoadDataIntoGridView("SELECT DISTINCT PayDay FROM NKT_TABLEFILTER WITH (NOLOCK)", sqlConnect.Rows[0][0].ToString());

                    // 2️⃣ String filtre değerlerini hazırla
                    string queryCustomerLogicalref = string.Join(",", customerLogical.AsEnumerable().Select(r => r[0].ToString()));
                    string querySalesmanLogicalref = string.Join(",", salesLogical.AsEnumerable().Select(r => r[0].ToString()));
                    string queryPayDay = string.Join(",", paydayLogical.AsEnumerable().Select(r => r[0].ToString()));

                    // 3️⃣ Cari verisi
                    string cariSql = string.IsNullOrEmpty(queryCustomerLogicalref)
                        ? $"SELECT LOGICALREF,CODE 'Cari Kod',DEFINITION_ 'Cari Açıklama' FROM LG_{company.Rows[0][0]}_CLCARD WHERE ACTIVE=0 ORDER BY 3"
                        : $"SELECT LOGICALREF,CODE 'Cari Kod',DEFINITION_ 'Cari Açıklama' FROM LG_{company.Rows[0][0]}_CLCARD WHERE LOGICALREF IN ({queryCustomerLogicalref}) AND ACTIVE=0 ORDER BY 3";

                    DataTable dtCari = SQLCrud.LoadDataIntoGridView(cariSql, sqlConnect.Rows[0][0].ToString());

                    if (!dtCari.Columns.Contains("Sipariş Tutarı"))
                        dtCari.Columns.Add("Sipariş Tutarı", typeof(decimal));

                    foreach (DataRow row in dtCari.Rows)
                    {
                        string cariRef = row["LOGICALREF"].ToString();
                        var match = customerLogical.Select($"CustomerLogicalRef = {cariRef}").FirstOrDefault();
                        if (match != null && decimal.TryParse(match["OrderMax"].ToString(), out decimal tutar))
                            row["Sipariş Tutarı"] = Math.Max(tutar, 1); 
                        else
                            row["Sipariş Tutarı"] = 1;

                    }

                    gridControl2.DataSource = dtCari;
                    GridDesigner(gridView2);
                    gridView2.Columns[0].Visible = false;
                    if (gridView2.Columns["Sipariş Tutarı"] != null)
                    {
                        gridView2.Columns["Sipariş Tutarı"].Visible = true;
                        gridView2.Columns["Sipariş Tutarı"].OptionsColumn.AllowEdit = true;
                    }

                    // 4️⃣ Satıcı verisi
                    string salesSql = string.IsNullOrEmpty(querySalesmanLogicalref)
                        ? $"SELECT LOGICALREF,CODE 'Satıcı Kodu',DEFINITION_ 'Satıcı Açıklama' FROM LG_SLSMAN WHERE FIRMNR={company.Rows[0][2].ToString()}"
                        : $"SELECT LOGICALREF,CODE 'Satıcı Kodu',DEFINITION_ 'Satıcı Açıklama' FROM LG_SLSMAN WHERE FIRMNR={company.Rows[0][2].ToString()} AND LOGICALREF IN ({querySalesmanLogicalref})";

                    gridControl1.DataSource = SQLCrud.LoadDataIntoGridView(salesSql, sqlConnect.Rows[0][0].ToString());
                    GridDesigner(gridView1);
                    gridView1.Columns[0].Visible = false;

                    // 5️⃣ Ödeme planı verisi
                    string paySql = string.IsNullOrEmpty(queryPayDay)
                        ? $"SELECT LOGICALREF,CODE 'Odeme Planı Kodu',DEFINITION_ 'Açıklaması' FROM LG_{company.Rows[0][0]}_PAYPLANS WHERE ACTIVE=0 ORDER BY 1"
                        : $"SELECT LOGICALREF,CODE 'Odeme Planı Kodu',DEFINITION_ 'Açıklaması' FROM LG_{company.Rows[0][0]}_PAYPLANS WHERE ACTIVE=0 AND LOGICALREF IN ({queryPayDay}) ORDER BY 1";

                    gridControl3.DataSource = SQLCrud.LoadDataIntoGridView(paySql, sqlConnect.Rows[0][0].ToString());
                    GridDesigner(gridView3);
                    gridView3.Columns[0].Visible = false;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "SQL Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                }
            }
        }
        private List<string> GetLogicalRefList(GridView gridView)
        {
            return Enumerable.Range(0, gridView.DataRowCount)
                .Select(i => gridView.GetRowCellValue(i, "LOGICALREF")?.ToString()?.Trim())
                .Where(val => !string.IsNullOrEmpty(val))
                .Distinct()
                .ToList();
        }
        private decimal GetSiparisTutari(string cariRef)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                var refVal = gridView2.GetRowCellValue(i, "LOGICALREF")?.ToString()?.Trim();
                if (string.Equals(refVal, cariRef?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    var tutarObj = gridView2.GetRowCellValue(i, "Sipariş Tutarı");
                    if (tutarObj != null && decimal.TryParse(tutarObj.ToString(), out decimal parsed) && parsed > 0)
                        return parsed;
                    else
                        return 1;
                }
            }
            return 1;
        }
        private void nmr_OrderMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = true;
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            var cariRefList = GetLogicalRefList(gridView2);
            var satıcıRefList = GetLogicalRefList(gridView1);
            var odemeRefList = GetLogicalRefList(gridView3);

            if (!cariRefList.Any() || !satıcıRefList.Any() || !odemeRefList.Any())
            {
                XtraMessageBox.Show("Cari, Satıcı veya Ödeme Planı filtresi eksik!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable sqlConnect = SQLiteCrud.GetDataFromSQLite("SELECT * FROM SqlConnectionString LIMIT 1");
            await SQLCrud.InserUpdateDelete("TRUNCATE TABLE NKT_TABLEFILTER", sqlConnect.Rows[0][0].ToString());
            btn_Save.Enabled = false;
            int batchSize = 1000;
            List<string> currentBatch = new List<string>();
            int toplamKayitSayisi = 0;
            using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(sqlConnect.Rows[0][0].ToString())))
            {
                await conn.OpenAsync();
                foreach (var cari in cariRefList)
                {
                    decimal tutar = GetSiparisTutari(cari);
                    foreach (var satici in satıcıRefList)
                    {
                        foreach (var odeme in odemeRefList)
                        {
                            string row = $"({cari}, {satici}, {odeme}, {tutar.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
                            currentBatch.Add(row);
                            toplamKayitSayisi++;

                            if (currentBatch.Count == batchSize)
                            {
                                string sql = $@"
INSERT INTO NKT_TABLEFILTER 
(CustomerLogicalRef, SalesmanLogicalRef, PayDay, OrderMax) 
VALUES {string.Join(",", currentBatch)};";

                                using (SqlCommand cmd = new SqlCommand(sql, conn))
                                {
                                    await cmd.ExecuteNonQueryAsync();
                                }
                                currentBatch.Clear(); 
                            }
                        }
                    }
                }
                if (currentBatch.Any())
                {
                    string sql = $@"
INSERT INTO NKT_TABLEFILTER 
(CustomerLogicalRef, SalesmanLogicalRef, PayDay, OrderMax) 
VALUES {string.Join(",", currentBatch)};";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            SaveFilterForm fr = new SaveFilterForm();
            fr.ShowDialog();
            btnSaveStatus = true;
            btn_Save.Enabled = true;
            this.Close();
        }
        private void CompanyFilterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!btnSaveStatus)
            {
                XtraMessageBox.Show("Lütfen Şirket Filtrelerinizi Ayarlayıp Kaydediniz", "Hatalı Şirket Filtreleri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
        private void nmr_OrderMax_ValueChanged(object sender, EventArgs e)
        {
            DataTable dt = gridControl2.DataSource as DataTable;
            if (dt == null || !dt.Columns.Contains("Sipariş Tutarı")) return;
            foreach (DataRow row in dt.Rows)
            {
                if (row.IsNull("Sipariş Tutarı") || Convert.ToDecimal(row["Sipariş Tutarı"]) == 1)
                    row["Sipariş Tutarı"] = nmr_OrderMax.Value;
            }
        }
        private void clearOrderSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = gridControl2.DataSource as DataTable;
            if (dt == null || !dt.Columns.Contains("Sipariş Tutarı")) return;
            foreach (DataRow row in dt.Rows)
                row["Sipariş Tutarı"] = 1;
            gridControl2.RefreshDataSource();
        }

        private void payAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridFilterAdd form = new GridFilterAdd();
            form.type = "odeme";
            form.Text = "Yeni Ödeme Planı Ekle";
            DataTable yourDataTable = gridControl3.DataSource as DataTable;
            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach (var rowValues in form.SelectedRowsData)
                {
                    object logicalRef = rowValues[0];
                    bool exists = yourDataTable.AsEnumerable()
                        .Any(r => r.Field<object>("LOGICALREF")?.ToString() == logicalRef?.ToString());
                    if (exists)
                        continue;
                    DataRow newRow = yourDataTable.NewRow();
                    for (int i = 0; i < rowValues.Length; i++)
                        newRow[i] = rowValues[i];
                    yourDataTable.Rows.Add(newRow);
                }
                gridControl3.DataSource = yourDataTable;
            }
        }
        private void cariEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridFilterAdd form = new GridFilterAdd();
            form.type = "cari";
            form.Text = "Yeni Cari Ekle";
            DataTable yourDataTable = gridControl2.DataSource as DataTable;

            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach (var rowValues in form.SelectedRowsData)
                {
                    object logicalRef = rowValues[0];
                    bool exists = yourDataTable.AsEnumerable()
                        .Any(r => r.Field<object>("LOGICALREF")?.ToString() == logicalRef?.ToString());
                    if (exists)
                        continue;
                    DataRow newRow = yourDataTable.NewRow();
                    for (int i = 0; i < rowValues.Length; i++)
                        newRow[i] = rowValues[i];
                    if (yourDataTable.Columns.Contains("Sipariş Tutarı"))
                        newRow["Sipariş Tutarı"] = 1;
                    yourDataTable.Rows.Add(newRow);
                }
                gridControl2.DataSource = yourDataTable;
            }

        }
        private void salesmanAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridFilterAdd form = new GridFilterAdd();
            form.type = "sales";
            form.Text = "Yeni Satıcı Ekle";
            DataTable yourDataTable = gridControl1.DataSource as DataTable;
            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach (var rowValues in form.SelectedRowsData)
                {
                    object logicalRef = rowValues[0];
                    bool exists = yourDataTable.AsEnumerable()
                        .Any(r => r.Field<object>("LOGICALREF")?.ToString() == logicalRef?.ToString());
                    if (exists)
                        continue;
                    DataRow newRow = yourDataTable.NewRow();
                    for (int i = 0; i < rowValues.Length; i++)
                        newRow[i] = rowValues[i];
                    yourDataTable.Rows.Add(newRow);
                }
                gridControl1.DataSource = yourDataTable;
            }
        }
    }
}