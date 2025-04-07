using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class UserSettingForm :XtraForm
    {
        public UserSettingForm()
        {
            InitializeComponent();
        }
        private void UserSettingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (txt_UserName.Text=="admin")
            {
                XtraMessageBox.Show("Lütfen kullanıcı adı admin olamaz", "Hatalı kayıt işlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txt_Password.Text.Trim()) || string.IsNullOrEmpty(txt_UserName.Text.Trim()))
            {
                XtraMessageBox.Show("Lütfen kullanıcı adı ve şifre boş geçilemez", "Hatalı kayıt işlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (string.IsNullOrEmpty(txt_UserName.Text))
                    txt_UserName.Focus();
                else
                    txt_Password.Focus();
                return;
            }
            else
            {
                await SQLiteCrud.InserUpdateDelete("UPDATE AdminLogin SET UserName='" +EncryptionHelper.Encrypt(txt_UserName.Text.Trim()) + "' , UserPassword='" + EncryptionHelper.Encrypt(txt_Password.Text.Trim()) + "' WHERE ID=1", "Kullanıcı bilgilerini güncelleme işlemi başarılı");
                this.Close();
            }
        }
        private void UserSettingForm_Load(object sender, EventArgs e)
        {
            DataTable dt=SQLiteCrud.GetDataFromSQLite("SELECT UserName,UserPassword FROM AdminLogin LIMIT 1");
            if (!(dt is null))
            {
                if (dt.Rows.Count>0)
                {
                    try
                    {
                        txt_UserName.Text = EncryptionHelper.Decrypt(dt.Rows[0][0].ToString());
                        txt_Password.Text = EncryptionHelper.Decrypt(dt.Rows[0][1].ToString());
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message,"Hatalı Okuma İşlemi",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        return;
                    }
                }
            }
        }
    }
}