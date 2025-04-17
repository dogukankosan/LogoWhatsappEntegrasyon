using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace NoktaBilgiNotificationService.Classes
{
    internal class EMailManager
    {
        internal async static Task<bool> OrderMailSend(
     string senderEmail,
     string recipientEmail,
     string senderPassword,
     string server,
     int port,
     bool ssl,
     byte[] pdfPath,
     string nktid)
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            try
            {
                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    TextLog.TextLogging("Database bağlantısı boş");
                    return false;
                }
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " -- " +ex.ToString());
                return false;
            }
            DataTable company = SQLiteCrud.GetDataFromSQLite("SELECT * FROM CompanySettings");
            if (company is null || company.Rows.Count <= 0 || string.IsNullOrEmpty(company.Rows[0][0].ToString()))
                return false;

            DataTable fiche = SQLCrud.LoadDataIntoGridView($@"SELECT CL.DEFINITION_ 'CUSTOMERNAME', CAPI.NAME 'USERCREATED',FIC.FICHENO,CONVERT(DATE,FIC.DATE_) 'FICHEDATE',CL.EMAILADDR 'MAIL',FIC.LOGICALREF 'FISLOGICAL' 
        FROM NKT_ORFFICHELOGICALREFWP NKT WITH(NOLOCK) 
        JOIN LG_{company.Rows[0][0]}_{company.Rows[0][1]}_ORFICHE FIC WITH(NOLOCK) ON NKT.FICHELOGICALREF = FIC.LOGICALREF AND NKT.FICHENO = FIC.FICHENO
        JOIN LG_{company.Rows[0][0]}_CLCARD CL WITH(NOLOCK) ON CL.LOGICALREF = FIC.CLIENTREF
        LEFT JOIN L_CAPIUSER CAPI WITH(NOLOCK) ON CAPI.NR = FIC.CAPIBLOCK_CREATEDBY
        WHERE NKT.ID = {nktid}", dt.Rows[0][0].ToString());
            if (fiche is null || fiche.Rows.Count <= 0 || string.IsNullOrEmpty(fiche.Rows[0][0].ToString()))
                return false;
            DataTable linkData = SQLiteCrud.GetDataFromSQLite("SELECT WebAdres,WebToken,WebPassword,SQLConnectString,CompanyName FROM WebSettings LIMIT 1");
            if (linkData.Rows.Count <= 0 || string.IsNullOrEmpty(linkData.Rows[0][0].ToString()))
                return false;
            DataTable CustomerAndManager = SQLCrud.LoadDataIntoGridView(
                "SELECT ManAndCusStatus,ID FROM Customers WHERE CustomerName='" + linkData.Rows[0]["CompanyName"] +
                "' AND CustomerToken='" + linkData.Rows[0]["WebToken"] +
                "' AND CustomerPassword='" + linkData.Rows[0]["WebPassword"] + "'",
                linkData.Rows[0]["SQLConnectString"].ToString());
            if (CustomerAndManager is null || CustomerAndManager.Rows.Count <= 0 || string.IsNullOrEmpty(CustomerAndManager.Rows[0][0].ToString()))
                return false;
            string tokenValue = $"{linkData.Rows[0]["WebToken"].ToString()}|{linkData.Rows[0]["WebPassword"].ToString()}|{fiche.Rows[0]["FICHENO"].ToString()}|{fiche.Rows[0]["FISLOGICAL"].ToString()}";
            string encodedToken = HttpUtility.UrlEncode(tokenValue);
            string link = $"{linkData.Rows[0]["WebAdres"]}OrderWp/Approve?token={encodedToken}";

            string manager = "", companyInfo = "";
            if (CustomerAndManager.Rows[0][0].ToString() == "1")
            {
                manager = company.Rows[0]["ManagerName"].ToString();
                companyInfo = fiche.Rows[0]["CUSTOMERNAME"].ToString();
            }
            else
            {
                manager = fiche.Rows[0]["CUSTOMERNAME"].ToString();
                companyInfo = company.Rows[0]["LogoCompanyName"].ToString();
                recipientEmail = fiche.Rows[0]["MAIL"].ToString();
            }
            DataTable signMail = SQLiteCrud.GetDataFromSQLite("SELECT * FROM MailSignature LIMIT 1");
            string htmlSignature = "";
            if (signMail != null && signMail.Rows.Count > 0)
            {
                string name = signMail.Rows[0]["CompanyName"]?.ToString();
                string phone = signMail.Rows[0]["Phone"]?.ToString();
                string address = signMail.Rows[0]["Adress"]?.ToString();
                string website = signMail.Rows[0]["CompanyWebSite"]?.ToString();
                string rawBase64 = signMail.Rows[0]["CompanyImage"]?.ToString();
                string logoBase64 = string.IsNullOrWhiteSpace(rawBase64) ? "" : "data:image/png;base64," + rawBase64;
                if (!string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(phone) &&
                    !string.IsNullOrWhiteSpace(address) &&
                    !string.IsNullOrWhiteSpace(website))
                {
                    htmlSignature = $@"
<br/>
<hr />
<table style='font-family: Arial, sans-serif; font-size: 14px;'>
    <tr>
        <td style='vertical-align: top; padding-right: 15px;'>
            <img src='cid:firmaLogo' alt='Firma Logo' style='width: 150px; height: auto; border-radius: 5px;'/>
        </td>
        <td style='vertical-align: top;'>
            <p style='margin: 0;'><strong style='font-size: 16px; color: #333;'>{name}</strong></p>
            <p style='margin: 2px 0;'>📞 <a href='tel:{phone}' style='color: #000; text-decoration: none;'>{phone}</a></p>
            <p style='margin: 2px 0;'>💬 <a href='https://wa.me/{phone.Replace("+", "").Replace(" ", "")}' style='color: #25D366; text-decoration: none;'>WhatsApp ile yaz</a></p>
            <p style='margin: 2px 0;'>📍 {address}</p>
            <p style='margin: 2px 0;'>🌐 <a href='https://{website}' style='color: #1a0dab;' target='_blank'>{website}</a></p>
        </td>
    </tr>
</table>";
                }
            }
            string subject = $"{fiche.Rows[0]["FICHENO"].ToString()} Nolu LOGO Sipariş Onay Bekleniyor.";
            string body = $@"
<html>
<body>
    <p>Sn. <strong>{manager}</strong>, <strong>{Convert.ToDateTime(fiche.Rows[0]["FICHEDATE"]).ToString("dd.MM.yyyy")}</strong> tarihli 
    <strong>{fiche.Rows[0]["USERCREATED"].ToString()}</strong> kullanıcısı tarafından 
    <strong><i>{fiche.Rows[0]["FICHENO"].ToString()}</i></strong> nolu, 
    <strong>{companyInfo}</strong> firmasına ait siparişiniz tarafınızdan onay <strong>beklenmektedir</strong>.
    Sipariş detayları ekteki PDF'tedir.</p>

    <p>
        <a href='{link}' style='
            background-color: #4CAF50;
            color: white;
            padding: 10px 15px;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
        '>📄 Siparişi Onayla Veya Reddet</a>
    </p>

    {htmlSignature}
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
                        IsBodyHtml = true
                    };
                    mail.To.Add(recipientEmail);
                    string rawBase64 = signMail.Rows[0]["CompanyImage"]?.ToString();
                    byte[] logoBytes = Convert.FromBase64String(rawBase64);
                    MemoryStream logoStream = new MemoryStream(logoBytes);
                    LinkedResource inlineLogo = new LinkedResource(logoStream, MediaTypeNames.Image.Jpeg)
                    {
                        ContentId = "firmaLogo",
                        TransferEncoding = TransferEncoding.Base64,
                        ContentType = new ContentType(MediaTypeNames.Image.Jpeg)
                    };
                    inlineLogo.ContentType.Name = "LOGO"; 
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                    avHtml.LinkedResources.Add(inlineLogo);
                    mail.AlternateViews.Add(avHtml);
                    if (pdfPath != null && pdfPath.Length > 0)
                    {
                        MemoryStream pdfStream = new MemoryStream(pdfPath);
                        Attachment pdfAttachment = new Attachment(pdfStream, $"{fiche.Rows[0]["FICHENO"]}.pdf", "application/pdf");
                        mail.Attachments.Add(pdfAttachment);
                    }
                    await client.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " ---- " + ex.ToString());
                return false;
            }
            return true;
        }
    }
}