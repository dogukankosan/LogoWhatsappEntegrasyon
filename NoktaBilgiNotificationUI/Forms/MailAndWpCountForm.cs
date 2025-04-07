using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class MailAndWpCountForm : XtraForm
    {
        public MailAndWpCountForm()
        {
            InitializeComponent();
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            int status = 1;
            if (rdb_Mail.Checked)
                status = 0;
            int statusCustomer = 0;
            if (rdb_Manager.Checked)
                statusCustomer = 1;
                        await SQLiteCrud.InserUpdateDelete("UPDATE MailAndWpCount SET WPCount='"+EncryptionHelper.Encrypt(nmr_wp.Value.ToString())+ "', MailCount='" + EncryptionHelper.Encrypt(nmr_mail.Value.ToString()) + "',WpStatus="+status+ ",WpStartCount='"+ EncryptionHelper.Encrypt(nmr_wp.Value.ToString()) + "' ,MailStartCount='"+ EncryptionHelper.Encrypt(nmr_mail.Value.ToString()) + "', ManAndCusStatus="+statusCustomer+" ", "Mail Ve Wp Sayısı Güncelleme İşlemi Başarılı.");
            this.Close();
        }
        private void MailAndWpCountForm_Load(object sender, EventArgs e)
        {
            nmr_wp.Focus();
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT * FROM MailAndWpCount");
            if (!(dt is null))
            {
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        {
                            nmr_wp.Value = int.Parse(EncryptionHelper.Decrypt(dt.Rows[0][0].ToString()));
                            nmr_mail.Value = int.Parse(EncryptionHelper.Decrypt(dt.Rows[0][1].ToString()));
                            if (dt.Rows[0][2].ToString()=="1") 
                                rd_Wp.Checked = true;
                            else
                                rdb_Mail.Checked = true;
                            if (dt.Rows[0]["ManAndCusStatus"].ToString() == "1")
                                rdb_Manager.Checked = true;
                            else
                                rdb_Customer.Checked = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        return;
                    }
                }
            }
        }
        private void MailAndWpCountForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
        private void nmr_mail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = true;
        }
        private void nmr_wp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = true;
        }
    }
}