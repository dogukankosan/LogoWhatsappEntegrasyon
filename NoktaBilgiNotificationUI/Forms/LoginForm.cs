using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class LoginForm :XtraForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void pcb_Linkedin_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.linkedin.com/company/noktabilgiislem/",
                UseShellExecute = true // Windows 10 ve sonrası için gerekli
            });
        }
        private void pcb_WebSite_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.noktabilgiislem.com/",
                UseShellExecute = true // Windows 10 ve sonrası için gerekli
            });
        }
        private void pcb_Instagram_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.instagram.com/noktabilgislem/",
                UseShellExecute = true // Windows 10 ve sonrası için gerekli
            });
        }
        private void pcb_Wp_Click(object sender, EventArgs e)
        {
            string phoneNumber = "905324355216"; 
            string url = $"https://wa.me/{phoneNumber}";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Windows 10 ve sonrası için gerekli
            });
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt2 = SQLiteCrud.GetDataFromSQLite("SELECT 1");
                if (dt2 is null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Program klasörüne yöneticisi yetkisi veriniz.", "SQLITE DB Hatalı Bağlantı Lütfen SQLITE Bağlantısını Kontrol Ediniz", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " ---- " +ex.ToString());
                Application.Exit();
            }
            btn_Eyes.Visible = false;
        }
        private void btn_NotEye_Click(object sender, EventArgs e)
        {
            txt_Password.Focus();
            btn_Eyes.Visible = true;
            btn_NotEye.Visible = false;
            txt_Password.Properties.PasswordChar = '\0';
        }
        private void btn_Eyes_Click(object sender, EventArgs e)
        {
            txt_Password.Focus();
            btn_NotEye.Visible = true;
            btn_Eyes.Visible = false;

            txt_Password.Properties.PasswordChar = '*';
        }
        private void btn_Login_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Password.Text) || string.IsNullOrEmpty(txt_UserName.Text))
            {
                XtraMessageBox.Show("Lütfen kullanıcı adı ve şifresini giriniz", "Hatalı giriş işlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (string.IsNullOrEmpty(txt_UserName.Text))
                    txt_UserName.Focus();
                else
                    txt_Password.Focus();
                return;
            }
            else
            {
                string password = EncryptionHelper.Encrypt(txt_Password.Text);
                string username = EncryptionHelper.Encrypt(txt_UserName.Text);
                DataTable dt2 = SQLiteCrud.GetDataFromSQLite("SELECT UserName FROM AdminLogin WHERE UserName='" + username+ "' AND UserPassword='"+password+"' LIMIT 1");
                if (dt2.Rows.Count<=0)
                {
                    XtraMessageBox.Show("Girilen kullanıcı adı veya şifre yanlış girilmiştir tekrar deneyiniz", "Hatalı giriş işlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    HomeForm fr = new HomeForm();
                    fr.username =EncryptionHelper.Decrypt(dt2.Rows[0][0].ToString());
                    fr.Show();
                    this.Hide();
                } 
            }
        }
    }
}