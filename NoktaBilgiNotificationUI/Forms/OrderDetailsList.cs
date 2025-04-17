using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class OrderDetailsList : XtraForm
    {
        public OrderDetailsList()
        {
            InitializeComponent();
        }
        private string companyCode = "", companyPeriod = "";
        void GridDesigner()
        {
            gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 11, FontStyle.Bold);
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
        void CompanySettingsGet()
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode,LogoPeriod FROM CompanySettings LIMIT 1");
            if (!(dt is null))
            {
                if (dt.Rows.Count > 0)
                {
                    companyCode = dt.Rows[0][0].ToString();
                    companyPeriod = dt.Rows[0][1].ToString();
                }
            }
        }
        public int ficheLogicalRef = 0;
        private void OrderDetailsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        private void OrderDetailsList_Load(object sender, EventArgs e)
        {
            CompanySettingsGet();
            if (ficheLogicalRef>0)
            {
                string query = $@"SELECT ITM.CODE 'Ürün Kodu',
IIF(
    NULLIF(
        CASE 
            WHEN LINE.LINETYPE = 0 THEN ITM.NAME 
            ELSE SRV.DEFINITION_ 
        END, 
    '') IS NULL, 
    'İndirim Satırı', 
    CASE 
        WHEN LINE.LINETYPE = 0 THEN ITM.NAME 
        ELSE SRV.DEFINITION_ 
    END
) AS [Ürün Açıklama],
LINE.SPECODE 'Hareket Özel Kod', 
LINE.AMOUNT 'Miktar',
(LINE.AMOUNT-LINE.SHIPPEDAMOUNT)'Bekleyen Miktar',
ISNULL(UNIT.CODE, '') 'Birim',
FORMAT(LINE.PRICE, 'C', 'tr-TR') 'Birim Fiyat',
LINE.VAT 'KDV',
FORMAT(LINE.VATAMNT, 'C', 'tr-TR') 'KDV Tutari',
FORMAT(LINE.DISTDISC, 'C', 'tr-TR') 'İndirim_Tutari',
FORMAT(LINE.LINENET, 'C', 'tr-TR') 'Tutar',
FORMAT(LINE.TOTAL+LINE.VATAMNT-LINE.DISTCOST, 'C', 'tr-TR') 'Net Tutar',
CONCAT(LINE.TRRATE,' ',CUR.CURCODE) 'Kur',
CONVERT(DATE,LINE.DUEDATE, 104) 'Teslim Tarihi'
FROM LG_{companyCode}_{companyPeriod}_ORFLINE LINE WITH (NOLOCK)
LEFT JOIN LG_{companyCode}_ITEMS ITM WITH(NOLOCK) ON LINE.STOCKREF = ITM.LOGICALREF AND LINE.LINETYPE=0
LEFT JOIN LG_{companyCode}_SRVCARD SRV WITH(NOLOCK) ON LINE.STOCKREF = SRV.LOGICALREF AND LINE.LINETYPE=4
LEFT JOIN LG_{companyCode}_UNITSETL UNIT WITH(NOLOCK) ON UNIT.LOGICALREF = LINE.UOMREF
LEFT JOIN L_CURRENCYLIST CUR WITH(NOLOCK) ON CUR.LOGICALREF = LINE.TRCURR
WHERE LINE.ORDFICHEREF="+ ficheLogicalRef + " ORDER BY LINE.LOGICALREF";
                DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
                if (!(dt is null))
                {
                    if (dt.Rows.Count > 0)
                    {
                        gridControl1.DataSource = SQLCrud.LoadDataIntoGridView(query, dt.Rows[0][0].ToString());
                        GridDesigner();
                    }
                }
            }      
        }
    }
}