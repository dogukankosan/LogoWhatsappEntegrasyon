using Newtonsoft.Json;
using NoktaBilgiNotificationService.Business;
using NoktaBilgiNotificationService.Classes;
using System;
using System.Data;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NoktaBilgiNotificationService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        private Timer timers;
        protected override void OnStart(string[] args)
        {
            if (timers == null)
                timers = new Timer();
            timers.Interval = 5_000;
            timers.Elapsed += OnElapsedTime;
            timers.Start();
        }
        protected override void OnStop()
        {
            timers?.Stop();
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            //System.Diagnostics.Debugger.Launch();
            SQLCommadStart();
        }
        private async static void SQLCommadStart()
        {
            DataTable SQLLITECompany = SQLiteCrud.GetDataFromSQLite("SELECT * FROM CompanySettings LIMIT 1");
            try
            {
                if (string.IsNullOrEmpty(SQLLITECompany.Rows[0][0].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                return;
            }
            DataTable SQLConnectionString = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            try
            {
                if (string.IsNullOrEmpty(SQLConnectionString.Rows[0][0].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                return;
            }
            DataTable OrderList = SQLCrud.LoadDataIntoGridView($@"SELECT TOP  1 NKT.*,CONVERT(DATE,ORF.DATE_) 'TARIH', ORF.FICHENO 'FISNO',LCAPI.NAME 'KAYITKULLANICI',CL.DEFINITION_ 'CUSTOMER',CL.TELNRS1 'TEL' FROM NKT_ORFFICHELOGICALREFWP NKT WITH(NOLOCK) 
            JOIN LG_{SQLLITECompany.Rows[0][0].ToString()}_{SQLLITECompany.Rows[0][1].ToString()}_ORFICHE ORF WITH(NOLOCK) ON  ORF.LOGICALREF = NKT.FICHELOGICALREF 
            JOIN L_CAPIUSER LCAPI WITH(NOLOCK) ON LCAPI.LOGICALREF = ORF.CAPIBLOCK_CREATEDBY
            JOIN LG_{SQLLITECompany.Rows[0][0].ToString()}_CLCARD CL WITH(NOLOCK) ON CL.LOGICALREF = ORF.CLIENTREF
            WHERE NKT.ACTIVESTATUS = 0 AND NKT.MESSAGEBODY IS NULL ORDER BY NKT.ID DESC", SQLConnectionString.Rows[0][0].ToString());
            if (OrderList.Rows.Count<=0)
                return;
            try
            {
                if (string.IsNullOrEmpty(OrderList.Rows[0][0].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                return;
            }
            DataTable SQLITEMail = SQLiteCrud.GetDataFromSQLite("SELECT * FROM MailSettings LIMIT 1");
            try
            {
                if (string.IsNullOrEmpty(SQLITEMail.Rows[0][0].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                return;
            }
            DataTable WpOrMailCountAndStatus = SQLiteCrud.GetDataFromSQLite("SELECT * FROM MailAndWpCount LIMIT 1");
            try
            {
                if (string.IsNullOrEmpty(WpOrMailCountAndStatus.Rows[0][0].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }
            string sendType = "";
            if (WpOrMailCountAndStatus.Rows[0]["WpStatus"].ToString() == "1")
            {
                if (int.Parse(EncryptionHelper.Decrypt(WpOrMailCountAndStatus.Rows[0]["WPCount"].ToString())) <= 0)
                    return;
                sendType = "wp";
            }
            else if (WpOrMailCountAndStatus.Rows[0]["WpStatus"].ToString() == "0")
            {
                if (int.Parse(EncryptionHelper.Decrypt(WpOrMailCountAndStatus.Rows[0]["MailCount"].ToString())) <= 0)
                    return;
                sendType = "mail";
            }
            else
                return;
            TokenGenerate gl = new TokenGenerate();
            string status = PDFCreat.PdfCreater(OrderList.Rows[0][0].ToString());
            if (string.IsNullOrEmpty(status))
                return;
            string token = "";
            if (!gl.HasRecentToken(OrderList.Rows[0]["FICHENO"].ToString(), int.Parse(OrderList.Rows[0]["FICHELOGICALREF"].ToString()), 8))
                token = await gl.GenerateSecureToken(sendType, OrderList.Rows[0]["FICHENO"].ToString(), int.Parse(OrderList.Rows[0]["FICHELOGICALREF"].ToString()),status);
            else
            {
                DataTable dr = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 Token FROM Tokens WHERE OrderFicheNo='" + OrderList.Rows[0]["FICHENO"] + "' AND OrderId = " + OrderList.Rows[0]["FICHELOGICALREF"] + " AND ExpiryDate > DATEADD(HOUR, -" + 8 + ", GETUTCDATE())", SQLConnectionString.Rows[0][0].ToString());
                if (!(dr is null))
                {
                    if (dr.Rows.Count > 0)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(dr.Rows[0][0].ToString()))
                                token = dr.Rows[0][0].ToString();
                            else
                                return;
                        }
                        catch (Exception ex)
                        {
                            TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                            return;
                        }
                    }
                }
            }
            DataTable wpSettings = SQLiteCrud.GetDataFromSQLite("SELECT * FROM WhatsappSettings LIMIT 1");
            if (wpSettings is null)
                return;
            if (wpSettings.Rows.Count <= 0)
                return;
            try
            {
                if (string.IsNullOrEmpty(wpSettings.Rows[0][1].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }
            string id = OrderList.Rows[0]["ID"].ToString();
            if (sendType=="wp")
            {
                string fileName = Path.GetFileName(status);

                string ficheNo = OrderList.Rows[0]["FISNO"].ToString();
                string logouser = OrderList.Rows[0]["KAYITKULLANICI"].ToString();
                string date = Convert.ToDateTime(OrderList.Rows[0]["TARIH"]).ToString("dd.MM.yyyy");
                string customer = OrderList.Rows[0]["CUSTOMER"].ToString();
                string accountSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][1].ToString());
                string authToken = EncryptionHelper.Decrypt(wpSettings.Rows[0][2].ToString());
                string messagingServiceSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][3].ToString());
                string templateSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][4].ToString());
                string customerTelNr = OrderList.Rows[0]["TEL"].ToString();
                string code = $"{fileName}|{token}";

                string managerName = "",  toNumber = "";
                // 1 yönetici 0 müşteri

                if (WpOrMailCountAndStatus.Rows[0]["ManAndCusStatus"].ToString() == "1")
                {
                    managerName = SQLLITECompany.Rows[0]["ManagerName"].ToString();
                    toNumber = $"whatsapp:{wpSettings.Rows[0][0].ToString()}";
                }
                else
                {
                    if (!CustomerLogic.IsValidPhoneNumber(customerTelNr))
                        return;
                    managerName = customer;
                    customer = SQLLITECompany.Rows[0]["LogoCompanyName"].ToString();
                    toNumber = $"whatsapp:{customerTelNr}";
                }

                string contentVariables = JsonConvert.SerializeObject(new
                {
                    manager = managerName,
                    company = customer,
                    ficheno = ficheNo,
                    token = token,
                    logouser = logouser,
                    date = date,
                    mergedParam = code
                });
                try
                {
                    TwilioClient.Init(accountSid, authToken);
                    var message = MessageResource.Create(
                        messagingServiceSid: messagingServiceSid,
                        to: new PhoneNumber(toNumber),
                        body: "",
                        contentSid: templateSid,
                        contentVariables: contentVariables
                    );
                    if (message.ErrorCode == null)
                    {
                        await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET ACTIVESTATUS=1  WHERE ID=" + id + "", SQLConnectionString.Rows[0][0].ToString());
                        int wpCountVariable = int.Parse(EncryptionHelper.Decrypt(WpOrMailCountAndStatus.Rows[0]["WPCount"].ToString())) - 1;
                        await SQLiteCrud.InserUpdateDelete($"UPDATE MailAndWpCount SET WPCount='{EncryptionHelper.Encrypt(wpCountVariable.ToString())}'");
                        return;
                    }
                    else
                    {
                        TextLog.TextLogging(message.ErrorMessage + " -- "+ message.ErrorCode);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    return;
                }
            }
            if (sendType=="mail")
            {
                string link = $"{SQLLITECompany.Rows[0]["DomainURL"]}/OrderWp/Approve?token={token}";
                bool statusMail = await EMailManager.OrderMailSend(SQLITEMail.Rows[0][0].ToString(), SQLITEMail.Rows[0][1].ToString(), EncryptionHelper.Decrypt(SQLITEMail.Rows[0][2].ToString()), SQLITEMail.Rows[0][3].ToString(), int.Parse(SQLITEMail.Rows[0][4].ToString()), SQLITEMail.Rows[0][5].ToString() == "1" ? true : false, link, status, id);
                if (statusMail)
                {
                    bool sqlStatus = await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET ACTIVESTATUS=2  WHERE ID=" + id + "", SQLConnectionString.Rows[0][0].ToString());
                    if (sqlStatus)
                    {
                        int mailCountVariable = int.Parse(EncryptionHelper.Decrypt(WpOrMailCountAndStatus.Rows[0]["MailCount"].ToString())) - 1;
                        await SQLiteCrud.InserUpdateDelete($"UPDATE MailAndWpCount SET MailCount='{EncryptionHelper.Encrypt(mailCountVariable.ToString())}'");
                        return;
                    }
                }
            }
        }
    }
}