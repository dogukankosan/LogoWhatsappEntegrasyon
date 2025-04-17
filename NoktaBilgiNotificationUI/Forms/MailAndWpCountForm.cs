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
            DataTable remoteSQL = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString,WebToken,WebPassword FROM WebSettings LIMIT 1");
            if (remoteSQL is null || remoteSQL.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Lütfen Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(remoteSQL.Rows[0][0].ToString()))
                {
                    XtraMessageBox.Show("Lütfen Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Veritabanı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable dt = SQLCrud.LoadDataIntoGridView("SELECT * FROM Customers WITH (NOLOCK) WHERE CustomerToken='" + remoteSQL.Rows[0]["WebToken"].ToString() + "' AND CustomerPassword='" + remoteSQL.Rows[0]["WebPassword"].ToString() + "'", remoteSQL.Rows[0][0].ToString());
            if (dt is null || dt.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Lütfen Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    XtraMessageBox.Show("Lütfen Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Veritabanı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int status = 1;
            if (rdb_Mail.Checked)
                status = 0;
            int statusCustomer = 0;
            if (rdb_Manager.Checked)
                statusCustomer = 1;
            await SQLCrud.InserUpdateDelete("UPDATE Customers SET WpCount='" + EncryptionHelper.Encrypt(nmr_wp.Value.ToString()) + "', MailCount='" + EncryptionHelper.Encrypt(nmr_mail.Value.ToString()) + "',WpStatus=" + status + ",WpStartCount='" + EncryptionHelper.Encrypt(nmr_wp.Value.ToString()) + "' ,MailStartCount='" + EncryptionHelper.Encrypt(nmr_mail.Value.ToString()) + "', ManAndCusStatus=" + statusCustomer + " WHERE ID="+dt.Rows[0][0].ToString()+"", remoteSQL.Rows[0][0].ToString());
            this.Close();
        }
        private void MailAndWpCountForm_Load(object sender, EventArgs e)
        {
            DataTable remoteSQL = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString,WebToken,WebPassword FROM WebSettings LIMIT 1");
            if (remoteSQL.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(remoteSQL.Rows[0][0].ToString()))
                {
                    XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            nmr_wp.Focus();
            DataTable dt = SQLCrud.LoadDataIntoGridView("SELECT * FROM Customers WITH (NOLOCK) WHERE CustomerToken='" + remoteSQL.Rows[0]["WebToken"].ToString() + "' AND CustomerPassword='" + remoteSQL.Rows[0]["WebPassword"].ToString() + "'", remoteSQL.Rows[0][0].ToString());
            if (dt is null || dt.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Lütfen Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!(dt is null))
            {
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        object wpCountObj = dt.Rows[0]["WpCount"];
                        if (wpCountObj != DBNull.Value && !string.IsNullOrWhiteSpace(wpCountObj.ToString()))
                        {
                            string decryptedWp = EncryptionHelper.Decrypt(wpCountObj.ToString());
                            nmr_wp.Value = int.TryParse(decryptedWp, out int wpVal) ? wpVal : 0;
                        }
                        else
                            nmr_wp.Value = 0;
                        object mailCountObj = dt.Rows[0]["MailCount"];
                        if (mailCountObj != DBNull.Value && !string.IsNullOrWhiteSpace(mailCountObj.ToString()))
                        {
                            string decryptedMail = EncryptionHelper.Decrypt(mailCountObj.ToString());
                            nmr_mail.Value = int.TryParse(decryptedMail, out int mailVal) ? mailVal : 0;
                        }
                        else
                        {
                            nmr_mail.Value = 0;
                        }
                        object wpStatusObj = dt.Rows[0]["WpStatus"];
                        string wpStatus = wpStatusObj != DBNull.Value ? wpStatusObj.ToString() : "";
                        if (wpStatus == "1")
                            rd_Wp.Checked = true;
                        else
                            rdb_Mail.Checked = true;
                        object manCusStatusObj = dt.Rows[0]["ManAndCusStatus"];
                        string manCusStatus = manCusStatusObj != DBNull.Value ? manCusStatusObj.ToString() : "";
                        if (manCusStatus == "1")
                            rdb_Manager.Checked = true;
                        else
                            rdb_Customer.Checked = true;
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
            if (e.KeyCode == Keys.Escape)
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