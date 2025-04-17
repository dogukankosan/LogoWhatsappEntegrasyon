using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class GridFilterAdd : XtraForm
    {
        public GridFilterAdd()
        {
            InitializeComponent();
        }
        void GridDesigner()
        {
            gridView2.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 11, FontStyle.Bold);
            gridView2.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView2.Appearance.HeaderPanel.Options.UseBackColor = true;
            gridView2.Appearance.HeaderPanel.Options.UseForeColor = true;
            gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            gridView2.RowHeight = 20;
            gridView2.OptionsView.EnableAppearanceEvenRow = true;
            gridView2.OptionsView.EnableAppearanceOddRow = true;
            gridView2.OptionsView.ColumnAutoWidth = false;
            gridView2.BestFitColumns();
            gridView2.Appearance.HeaderPanel.Font = new Font("Tahoma", 8, FontStyle.Bold);
            gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            gridView2.OptionsView.ShowAutoFilterRow = true;
            gridView2.Appearance.FilterPanel.Font = new Font("Tahoma", 10, FontStyle.Italic);
            gridView2.Appearance.FilterPanel.ForeColor = Color.Blue;
            gridView2.Appearance.FilterPanel.Options.UseForeColor = true;
            gridView2.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridView2.OptionsSelection.MultiSelect = true;
            gridView2.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            gridView2.OptionsBehavior.Editable = false;
            gridView2.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            gridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            gridView2.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            gridView2.OptionsFind.AlwaysVisible = true;
            gridView2.OptionsView.ShowGroupPanel = true;
        }
        public string type = "";
        private void GridFilterAdd_Load(object sender, EventArgs e)
        {
            DataTable sqlConnect = SQLiteCrud.GetDataFromSQLite("SELECT * FROM SqlConnectionString LIMIT 1");
            DataTable company = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode,LogoPeriod,CompanyNo FROM CompanySettings LIMIT 1 ");
            if (type=="cari")
            {
                gridControl2.DataSource = SQLCrud.LoadDataIntoGridView($"SELECT LOGICALREF,CODE 'Cari Kod',DEFINITION_ 'Cari Açıklama' FROM LG_{company.Rows[0][0]}_CLCARD WHERE ACTIVE=0 ORDER BY 3", sqlConnect.Rows[0][0].ToString());
            }
            if (type=="odeme")
            {
                gridControl2.DataSource = SQLCrud.LoadDataIntoGridView($"SELECT LOGICALREF,CODE 'Odeme Planı Kodu',DEFINITION_ 'Açıklaması' FROM LG_{company.Rows[0][0]}_PAYPLANS WHERE ACTIVE=0 ORDER BY 1", sqlConnect.Rows[0][0].ToString());
            }
            if (type=="sales")
            {
                gridControl2.DataSource = SQLCrud.LoadDataIntoGridView($"SELECT LOGICALREF,DEFINITION_ 'Satıcı Kodu',DEFINITION_ 'Satıcı Açıklama' FROM LG_SLSMAN WITH (NOLOCK) WHERE FIRMNR={company.Rows[0][2].ToString()}", sqlConnect.Rows[0][0].ToString());
            }
            GridDesigner();
        }
        public List<object[]> SelectedRowsData { get; private set; } = new List<object[]>();
        private void btn_Save_Click(object sender, EventArgs e)
        {
            SelectedRowsData = new List<object[]>();
            foreach (int rowHandle in gridView2.GetSelectedRows())
            {
                var values = new object[gridView2.Columns.Count];
                for (int i = 0; i < gridView2.Columns.Count; i++)
                    values[i] = gridView2.GetRowCellValue(rowHandle, gridView2.Columns[i]);     
                SelectedRowsData.Add(values);
            }
            if (SelectedRowsData.Count == 0)
            {
                XtraMessageBox.Show("Lütfen en az bir satır seçiniz.", "Seçim Yok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void GridFilterAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
    }
}