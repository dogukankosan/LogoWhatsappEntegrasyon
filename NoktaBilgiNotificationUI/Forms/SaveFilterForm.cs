using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class SaveFilterForm : XtraForm
    {
        public SaveFilterForm()
        {
            InitializeComponent();
        }

        private string sqlScript = "", type = "";
        private bool btnSave = false;

        private void LoadTriggerFilterIntoRichTextBox()
        {
            DataTable sqlConnect = SQLiteCrud.GetDataFromSQLite("SELECT * FROM SqlConnectionString LIMIT 1");

            try
            {
                DataTable dt = SQLCrud.LoadDataIntoGridView(
                    "SELECT OBJECT_DEFINITION(OBJECT_ID('NKT_ORFICHE_WP_TRIGGER')) AS TriggerText",
                    sqlConnect.Rows[0][0].ToString());
                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    sqlScript = dt.Rows[0][0].ToString();
                    type = "ALTER";
                    Match match = Regex.Match(sqlScript, @"/\*f\*/(.*?)/\*d\*/", RegexOptions.Singleline);
                    if (match.Success)
                    {
                        richTextBox1.Text = match.Groups[1].Value.Trim();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "SQL Okuma Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
            }
            string filePath = Path.Combine(Application.StartupPath, "Queries", "Trigger.sql");
            if (File.Exists(filePath))
            {
                sqlScript = File.ReadAllText(filePath);
                type = "CREATE";
                Match match = Regex.Match(sqlScript, @"/\*f\*/(.*?)/\*d\*/", RegexOptions.Singleline);
                if (match.Success)
                {
                    richTextBox1.Text = match.Groups[1].Value.Trim();
                    return;
                }
            }
            XtraMessageBox.Show("Filtre bloğu ne SQL'den ne de dosyadan okunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async Task<bool> TriggerSQL()
        {
            DataTable sqlConnect = SQLiteCrud.GetDataFromSQLite("SELECT * FROM SqlConnectionString LIMIT 1");
            DataTable company = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode,LogoPeriod FROM CompanySettings LIMIT 1");

            string yeniFiltre = richTextBox1.Text.Trim();
            sqlScript = Regex.Replace(sqlScript,
                @"/\*f\*/.*?/\*d\*/",
                $"/*f*/\n{yeniFiltre}\n/*d*/",
                RegexOptions.Singleline);
            sqlScript = sqlScript.Replace("LG_001", $"LG_{company.Rows[0][0]}")
                                 .Replace("LG_001_01", $"LG_{company.Rows[0][0]}_{company.Rows[0][1]}");
            sqlScript = sqlScript.Replace("\r\n", " ").Replace("\n", " ").Replace("\t", " ");
            if (type == "CREATE")
                sqlScript=sqlScript.Replace("ALTER", "CREATE");
            else
                sqlScript = sqlScript.Replace("CREATE", "ALTER");

            return await SQLCrud.InserUpdateDelete(sqlScript, sqlConnect.Rows[0][0].ToString());
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (await TriggerSQL())
            {
                btnSave = true;
                XtraMessageBox.Show("Filtre Güncelleme İşlemi Tamamlandı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private void HighlightSQLKeywords(RichTextBox box)
        {
            string[] keywords =
            {
        "SELECT", "FROM", "WHERE", "AND", "OR", "NOT", "NULL", "IN", "IS", "LIKE", "BETWEEN",
        "INNER", "JOIN", "LEFT", "RIGHT", "FULL", "OUTER", "ON", "GROUP", "BY", "ORDER", "HAVING",
        "AS", "DISTINCT", "TOP", "EXISTS", "CASE", "WHEN", "THEN", "END", "UNION", "ALL", "INSERT",
        "INTO", "VALUES", "UPDATE", "SET", "DELETE", "CREATE", "TABLE", "PRIMARY", "KEY", "FOREIGN",
        "ALTER", "DROP", "TRUNCATE", "VIEW", "PROCEDURE", "FUNCTION", "DECLARE", "BEGIN", "END"
    };
            string[] operators = { "=", "<", ">", "<=", ">=", "<>", "!=" };
            int originalStart = box.SelectionStart;
            int originalLength = box.SelectionLength;
            box.SelectAll();
            box.SelectionColor = Color.Black;
            foreach (string keyword in keywords)
            {
                foreach (Match match in Regex.Matches(box.Text, $@"\b{keyword}\b", RegexOptions.IgnoreCase))
                {
                    box.Select(match.Index, match.Length);
                    box.SelectionColor = Color.Blue; 
                    box.SelectionFont = new Font(box.Font, FontStyle.Bold);
                }
            }
            foreach (string op in operators)
            {
                int startIndex = 0;
                while ((startIndex = box.Text.IndexOf(op, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    box.Select(startIndex, op.Length);
                    box.SelectionColor = Color.DarkRed;
                    box.SelectionFont = new Font(box.Font, FontStyle.Bold);
                    startIndex += op.Length;
                }
            }
            box.Select(originalStart, originalLength);
            box.SelectionColor = Color.Black;
        }
        private void XtraForm1_Load(object sender, EventArgs e)
        {
            richTextBox1.Font = new Font("Consolas", 10);
            LoadTriggerFilterIntoRichTextBox();
            HighlightSQLKeywords(richTextBox1);
        }
        private void SaveFilterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
        private void btn_Doc_Click(object sender, EventArgs e)
        {
            DocumentSQLTriggercs fr = new DocumentSQLTriggercs();
            fr.ShowDialog();
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            HighlightSQLKeywords(richTextBox1);
        }
        private void SaveFilterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!btnSave)
            {
                XtraMessageBox.Show("Lütfen SQL Filtrelerinizi ayarlayıp kaydediniz", "Hatalı SQL Filtreleri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
    }
}