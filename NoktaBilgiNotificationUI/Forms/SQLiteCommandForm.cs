using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class SQLiteCommandForm : XtraForm
    {
        public SQLiteCommandForm()
        {
            InitializeComponent();
        }
        private void GridDesigner()
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
        private void SQLiteCommandForm_Load(object sender, EventArgs e)
        {
            GridDesigner();
        }
        private async void SQLiteCommandForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.F5)
            {
                gridView1.Columns.Clear();
                gridControl1.DataSource = null;
                gridControl1.Refresh();
                string seciliMetin = richTextBox1.SelectedText;
                if (string.IsNullOrWhiteSpace(seciliMetin))
                {
                    XtraMessageBox.Show("Lütfen önce metin kutusunda bir metin seçiniz.", "Hatalı Seçim", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (richTextBox1.Text.Trim().ToUpper().Contains("SELECT"))
                    gridControl1.DataSource = SQLiteCrud.GetDataFromSQLite(seciliMetin);
                else
                    await SQLiteCrud.InserUpdateDelete(richTextBox1.Text, "Komut çalıştırma işlemi başarılı");
            }
        }
        private void encToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string seciliMetin = richTextBox1.SelectedText;
            if (string.IsNullOrWhiteSpace(seciliMetin))
            {
                XtraMessageBox.Show("Lütfen önce metin kutusunda bir metin seçiniz.", "Hatalı Seçim", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string message = EncryptionHelper.Decrypt(seciliMetin);
                      Clipboard.SetText(message);
                XtraMessageBox.Show(message, "Ram bellekte hafızaya alındı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Şifre Çözme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}