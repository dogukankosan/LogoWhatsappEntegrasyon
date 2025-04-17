using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using NoktaBilgiNotificationUI.Classes;
using NoktaBilgiNotificationUI.Business;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class MailSettingForm : XtraForm
    {
        public MailSettingForm()
        {
            InitializeComponent();
        }
        private void MailSettingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        private bool status = false;
        private void MailSettingForm_Load(object sender, EventArgs e)
        {
            txt_Port.Properties.Mask.MaskType = MaskType.Numeric;
            txt_Port.Properties.Mask.EditMask = "d";
            txt_Port.Properties.Mask.UseMaskAsDisplayFormat = true;
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT * FROM MailSettings LIMIT 1");
            for (byte i = 0; i < dt.Rows.Count; i++)
            {
                txt_Email.Text = dt.Rows[i][0].ToString();
                txt_recipientEmail.Text = dt.Rows[i][1].ToString();
                txt_Password.Text = EncryptionHelper.Decrypt(dt.Rows[i][2].ToString());
                txt_Server.Text = dt.Rows[i][3].ToString();
                txt_Port.Text = dt.Rows[i][4].ToString();
                chk_SSL.Checked = Convert.ToBoolean(dt.Rows[i][5]);
            }

            DataTable signMail = SQLiteCrud.GetDataFromSQLite("SELECT *FROM MailSignature LIMIT 1");
            if (!(signMail is null))
            {
                if (signMail.Rows.Count > 0)
                {
                    try
                    {
                        txt_CompanyName.Text = signMail.Rows[0][0].ToString();
                        txt_Phone.Text = signMail.Rows[0][1].ToString();
                        txt_Adres.Text = signMail.Rows[0][2].ToString();
                        if (signMail.Rows.Count > 0 && (signMail.Rows[0][3] != DBNull.Value && signMail.Rows[0][3].ToString() != ""))
                        {
                            string base64String = signMail.Rows[0][3].ToString();
                            if (!string.IsNullOrEmpty(base64String))
                                pictureEdit1.Image = ImageConvert.Base64ToImage(base64String);
                        }
                        txt_Web.Text = signMail.Rows[0][4].ToString();
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
        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (status)
            {
                if (EMailSettingLogic.EmailControl(txt_Email, txt_recipientEmail, txt_Password, txt_Server, txt_Port))
                {
                    _ = SQLiteCrud.InserUpdateDelete("UPDATE MailSettings SET MailAdress='" + txt_Email.Text + "',ReceiverMail='" + txt_recipientEmail.Text + "',Password='" + EncryptionHelper.Encrypt(txt_Password.Text) + "',ServerName='" + txt_Server.Text + "',Port=" + txt_Port.Text + ",SSl=" + chk_SSL.Checked + "", "Mail Güncelleme İşlemi Başarılı");
                    status = false;
                    this.Close();
                }
            }
            else
                XtraMessageBox.Show("Önce Mailinizi Test Ediniz !!", "Mail Test Et", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async void btn_SendMail_Click(object sender, EventArgs e)
        {
            if (EMailSettingLogic.EmailControl(txt_Email, txt_recipientEmail, txt_Password, txt_Server, txt_Port))
            {
                try
                {
                    int port = int.Parse(txt_Port.Text);
                    status = await EMailManager.MailSendForm(txt_Email.Text, txt_recipientEmail.Text, txt_Password.Text, txt_Server.Text, port, chk_SSL.Checked);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "Hatalı İşlem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TextLog.TextLogging(ex.Message + " ---- " + ex.ToString());
                }
            }
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (EMailSettingLogic.SaveMailSignature(txt_CompanyName, txt_Phone, txt_Adres, txt_Web, pictureEdit1))
            {
                string base64String = ImageConvert.ImageToBase64(pictureEdit1.Image, System.Drawing.Imaging.ImageFormat.Png);
                _ = SQLiteCrud.InserUpdateDelete("UPDATE MailSignature SET CompanyName='" + txt_CompanyName.Text.Trim() + "',Phone='" + txt_Phone.Text.Trim() + "',Adress='" + txt_Adres.Text.Trim() + "',CompanyWebSite='" + txt_Web.Text.Trim() + "' , CompanyImage= '" + base64String + "'", "Mail İmza İşlemi Başarılı");
                status = false;
            }
        }
    }
}