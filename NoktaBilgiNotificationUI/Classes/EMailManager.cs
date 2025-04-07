using DevExpress.XtraEditors;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI.Classes
{
    internal class EMailManager
    {
        internal async static Task<bool> MailSendForm(string senderEmail, string recipientEmail, string senderPassword, string server, int port, bool ssl)
        {
            string subject = "Test E-posta Başlığı";
            try
            {
                using (SmtpClient client = new SmtpClient(server, port))
                {
                    client.EnableSsl = ssl;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(senderEmail),
                        Subject = subject,
                        Body = ""
                    };
                    mail.To.Add(recipientEmail);
                    await client.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("E-Mail gönderilirken bir hata oluştu: " + ex.Message, "Hatalı E-Posta Gönderimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " ---- " + ex.ToString());
                return false;
            }
            XtraMessageBox.Show("E-Mail Başarıyla Gönderildi.", "Başarılı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        internal async static Task<bool> OrderMailSend(
     string senderEmail,
     string recipientEmail,
     string senderPassword,
     string server,
     int port,
     bool ssl,
     string link,
     string pdfPath,
     string ficheLogicalRef)
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
   
            DataTable company = SQLiteCrud.GetDataFromSQLite("SELECT * FROM CompanySettings");
            if (company is null)
            {
                if (company.Rows.Count <= 0)
                {
                    if (string.IsNullOrEmpty(company.Rows[0][0].ToString()))
                    {
                        XtraMessageBox.Show("Şirket Bilgileri Listesi Hatalı", "Hatalı Şirket Bilgileri", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            DataTable fiche = SQLCrud.LoadDataIntoGridView($@"SELECT CL.DEFINITION_ 'CUSTOMERNAME', CAPI.NAME 'USERCREATED',FIC.FICHENO,CONVERT(DATE,FIC.DATE_) 'FICHEDATE',CL.EMAILADDR 'MAIL' FROM NKT_ORFFICHELOGICALREFWP NKT WITH(NOLOCK) JOIN LG_{company.Rows[0][0].ToString()}_{company.Rows[0][1].ToString()}_ORFICHE FIC WITH(NOLOCK) ON NKT.FICHELOGICALREF = FIC.LOGICALREF AND NKT.FICHENO = FIC.FICHENO
               JOIN LG_{company.Rows[0][0].ToString()}_CLCARD CL WITH(NOLOCK) ON CL.LOGICALREF = FIC.CLIENTREF
                JOIN L_CAPIUSER CAPI WITH(NOLOCK) ON CAPI.LOGICALREF = FIC.CAPIBLOCK_CREATEDBY
                WHERE NKT.ID = "+ficheLogicalRef+"", dt.Rows[0][0].ToString());
            if (fiche is null)
            {
                if (fiche.Rows.Count<=0)
                {
                    if (string.IsNullOrEmpty(fiche.Rows[0][0].ToString()))
                    {
                        XtraMessageBox.Show("Mail Gönderilecek Sipariş Bulunamadı !!", "Hatalı Sipariş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            //0 ise Customer 1 Manager
            DataTable CustomerAndManager = SQLiteCrud.GetDataFromSQLite("SELECT ManAndCusStatus FROM MailAndWpCount LIMIT 1");
            if (!(CustomerAndManager is null))
            {
                if (CustomerAndManager.Rows.Count>0)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(CustomerAndManager.Rows[0][0].ToString()))
                        {
                            XtraMessageBox.Show("Hatalı SQLLITE MailAndWpCount Okuma İşlemi", "Hatalı Okuma", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Hatalı Okuma", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        return false;
                    }
                }
            }
            string manager = "", companyInfo = "";
            if (CustomerAndManager.Rows[0][0].ToString()=="1")
            {
                manager = company.Rows[0]["ManagerName"].ToString();
                companyInfo= fiche.Rows[0]["CUSTOMERNAME"].ToString();
            }
            else
            {
                manager = fiche.Rows[0]["CUSTOMERNAME"].ToString();
                companyInfo = company.Rows[0]["LogoCompanyName"].ToString();
                recipientEmail = fiche.Rows[0]["MAIL"].ToString();
            }

            string subject = $"{fiche.Rows[0][2]} nolu sipariş onay bekleniyor.";
            string body = $@"
        <html>
        <body>
            <p>Sn. <strong>{manager}</strong>, <strong>{Convert.ToDateTime(fiche.Rows[0]["FICHEDATE"]).ToString("dd.MM.yyyy")}</strong> tarihli <strong>{fiche.Rows[0]["USERCREATED"].ToString()}</strong> kullanıcısı tarafından <i>{fiche.Rows[0]["FICHENO"].ToString()}</i> nolu, <strong>{companyInfo}</strong> firmasına ait siparişiniz tarafınızdan onay <strong>beklenmektedir</strong> .Sipariş detayları ekteki PDF'tedir.</p>
            <p>
</br>
</br>
</br>
</br>
                <a href='{link}' style='
                    background-color: #4CAF50;
                    color: white;
                    padding: 10px 15px;
                    text-decoration: none;
                    border-radius: 5px;
                    font-weight: bold;
                '>📄 Siparişi Onayla Veya Reddet</a>
            </p>
        </body>
        </html>";
            try
            {
                using (SmtpClient client = new SmtpClient(server, port))
                {
                    client.EnableSsl = ssl;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(senderEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mail.To.Add(recipientEmail);
                    if (File.Exists(pdfPath))
                    {
                        Attachment pdfAttachment = new Attachment(pdfPath);
                        mail.Attachments.Add(pdfAttachment);
                    }
                    await client.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("E-Mail gönderilirken bir hata oluştu: " + ex.Message, "Hatalı E-Posta Gönderimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " ---- " + ex.ToString());
                return false;
            }
            XtraMessageBox.Show("E-Mail Başarıyla Gönderildi.", "Başarılı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
    }
}