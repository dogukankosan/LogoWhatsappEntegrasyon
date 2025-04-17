using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Business;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class WpSetttingsForm : XtraForm
    {
        public WpSetttingsForm()
        {
            InitializeComponent();
        }
        private void WpSetttingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (WhatsappSettingLogic.Control(msk_WPNo,txt_WpClientID,txt_WpToken,txt_ServiceID,txt_TemplateID))
            {
                await SQLiteCrud.InserUpdateDelete("UPDATE WhatsappSettings SET WpNo='"+ msk_WPNo.Text.Trim()+"',WpClientID='"+ EncryptionHelper.Encrypt(txt_WpClientID.Text.Trim())+"',WpToken='"+ EncryptionHelper.Encrypt(txt_WpToken.Text.Trim())+"',WpServiceID='"+ EncryptionHelper.Encrypt(txt_ServiceID.Text)+"',TemplateID='"+ EncryptionHelper.Encrypt(txt_TemplateID.Text.Trim())+ "'", "Whatsapp güncelleme işlemi başarılı");
                this.Close();
            }
        }
        private void WpSetttingsForm_Load(object sender, EventArgs e)
        {
            DataTable dt=SQLiteCrud.GetDataFromSQLite("SELECT * FROM WhatsappSettings");
            if (!(dt is null))
            {
                if (dt.Rows.Count>0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        {
                            msk_WPNo.Text = dt.Rows[0][0].ToString();
                            txt_WpClientID.Text = EncryptionHelper.Decrypt(dt.Rows[0][1].ToString());
                            txt_WpToken.Text = EncryptionHelper.Decrypt(dt.Rows[0][2].ToString());
                            txt_ServiceID.Text = EncryptionHelper.Decrypt(dt.Rows[0][3].ToString());
                            txt_TemplateID.Text = EncryptionHelper.Decrypt(dt.Rows[0][4].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Hatalı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    }
                }
            }
        }
    }
}