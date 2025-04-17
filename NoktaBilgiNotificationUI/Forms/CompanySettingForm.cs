using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Business;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class CompanySettingForm : XtraForm
    {
        public CompanySettingForm()
        {
            InitializeComponent();
        }
        private void CompanySettingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            string sqlFilePath = "";
            sqlFilePath = Path.Combine(Application.StartupPath, "Queries", "CreateTableFilter.sql");
            if (!File.Exists(sqlFilePath))
            {
                XtraMessageBox.Show("Hata: CreateTableFilter.sql dosyası bulunamadı! Dosya yolu: " + sqlFilePath, "Hatalı yol", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            sqlFilePath = Path.Combine(Application.StartupPath, "Queries", "CreateTable.sql");
            if (!File.Exists(sqlFilePath))
            {
                XtraMessageBox.Show("Hata: CreateTable.sql dosyası bulunamadı! Dosya yolu: " + sqlFilePath, "Hatalı yol", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CompanySettingLogic.CompanyControl(txt_CompanyNumber
                , txt_CompanyPeriod, txt_CompanyName, txt_ManagerName, pictureEdit1, txt_CompanyNo))
            {
                DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
                if (!(dt is null))
                {
                    if (dt.Rows.Count > 0)
                    {
                        string sqlScript = "";

                        sqlScript = File.ReadAllText(Application.StartupPath + "\\Queries\\CreateTableFilter.sql").Replace("\r\n", " ")
                           .Replace("\n", " ")
                           .Replace("\t", " ")
                           .Trim();
                        bool statusTableFilter = await SQLCrud.InserUpdateDelete(sqlScript, dt.Rows[0][0].ToString());

                        sqlScript = File.ReadAllText(Application.StartupPath + "\\Queries\\CreateTable.sql").Replace("\r\n", " ")
                           .Replace("\n", " ")
                           .Replace("\t", " ")
                           .Trim();
                        sqlScript = sqlScript.Replace("LG_001", $"LG_{txt_CompanyNumber.Text.Trim()}");
                        sqlScript = sqlScript.Replace("LG_001_01", $"LG_{txt_CompanyNumber.Text.Trim()}_{txt_CompanyPeriod.Text.Trim()}");
                        bool statusCreateTable = await SQLCrud.InserUpdateDelete(sqlScript, dt.Rows[0][0].ToString());

                        if (!statusCreateTable)
                        {
                            XtraMessageBox.Show("Hata: CreateTable.sql dosyası çalıştırılamadı!", "Hatalı SQL İşlemi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!statusTableFilter)
                        {
                            XtraMessageBox.Show("Hata: CreateTableFilter.sql dosyası çalıştırılamadı!", "Hatalı SQL İşlemi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string base64String = ImageConvert.ImageToBase64(pictureEdit1.Image, System.Drawing.Imaging.ImageFormat.Png);
                        await SQLiteCrud.InserUpdateDelete("UPDATE CompanySettings SET LogoPeriod = '" + txt_CompanyPeriod.Text.Trim() + "', LogoCompanyName='" + txt_CompanyName.Text.Trim() + "' , CompanyNo='" + txt_CompanyNo.Text + "',LogoCompanyCode = '" + txt_CompanyNumber.Text.Trim() + "',ManagerName='" + txt_ManagerName.Text.Trim() + "',CompanyPicture='" + base64String + "' ", "Şirket ayarları güncelleme işlemi başarılı.");
                        if (  statusCreateTable  && statusTableFilter)
                        {
                            XtraMessageBox.Show("Şirket Bilgileri Güncelleme İşlemi Yapıldıktan Sonra Filtreleri Ayarlayınız", "Filteleri Kaydet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CompanyFilterForm fr = new CompanyFilterForm();
                            fr.ShowDialog();
                            this.Close();
                        }
                    }
                }
            }
        }
        private void CompanySettingForm_Load(object sender, EventArgs e)
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode,LogoPeriod,LogoCompanyName,ManagerName,CompanyPicture,CompanyNo FROM CompanySettings LIMIT 1");
            if (!(dt is null))
            {
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        txt_CompanyNumber.Text = dt.Rows[0][0].ToString();
                        txt_CompanyPeriod.Text = dt.Rows[0][1].ToString();
                        txt_CompanyName.Text = dt.Rows[0][2].ToString();
                        txt_ManagerName.Text = dt.Rows[0][3].ToString();
                        if (dt.Rows.Count > 0 && (dt.Rows[0][4] != DBNull.Value && dt.Rows[0][4].ToString() != ""))
                        {
                            string base64String = dt.Rows[0][4].ToString();
                            if (!string.IsNullOrEmpty(base64String))
                                pictureEdit1.Image = ImageConvert.Base64ToImage(base64String);
                        }
                        txt_CompanyNo.Text = dt.Rows[0][5].ToString();
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Hatalı Kayıt Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        return;
                    }
                }
            }
        }
        private void txt_CompanyNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
        private void txt_CompanyPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
        private void txt_CompanyNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
                e.Handled = true;
        }
    }
}