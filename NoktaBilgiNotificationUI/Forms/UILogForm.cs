using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class UILogForm : XtraForm
    {
        public UILogForm()
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
        private void UILogForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
        public class LogItem
        {
            public string UILog { get; set; }
            public string ServiceLog { get; set; }
        }
        private List<LogItem> ReadLogs()
        {
            string uiPath = Path.Combine(Application.StartupPath, "Logs", "UILog.txt");
            string servicePath = Path.Combine(Application.StartupPath, "Logs", "ServiceLog.txt");
     
            var uiLines = File.Exists(uiPath) ? File.ReadAllLines(uiPath) : new string[0];
            var serviceLines = File.Exists(servicePath) ? File.ReadAllLines(servicePath) : new string[0];
            int maxLineCount = Math.Max(uiLines.Length, serviceLines.Length);
            var list = new List<LogItem>();
            for (int i = 0; i < maxLineCount; i++)
            {
                list.Add(new LogItem
                {
                    UILog = i < uiLines.Length ? uiLines[i] : "",
                    ServiceLog = i < serviceLines.Length ? serviceLines[i] : ""
                });
            }
            return list;
        }
        private void UILogForm_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = ReadLogs();
            GridDesigner();
        }
        private void excelAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel Dosyası (*.xlsx)|*.xlsx";
            saveDialog.Title = "Excel'e Aktar";
            saveDialog.FileName = "UILog.xlsx";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                gridView1.OptionsPrint.PrintDetails = true;
                gridControl1.ExportToXlsx(saveDialog.FileName);
                XtraMessageBox.Show("Excel dosyası başarıyla oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}