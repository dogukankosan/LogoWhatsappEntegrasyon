using Newtonsoft.Json;
using NoktaBilgiNotificationService.Business;
using NoktaBilgiNotificationService.Classes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            timers.Interval = 15_000;
            timers.Elapsed += OnElapsedTime;
            timers.Start();
        }
        protected override void OnStop()
        {
            TextLog.TextLogging("SERVİS DURDURULDU");
            timers?.Stop();
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            //System.Diagnostics.Debugger.Launch();
            SQLListener();
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
            DataTable SQLConnectionRemote = SQLiteCrud.GetDataFromSQLite("SELECT * FROM WebSettings LIMIT 1");
            try
            {
                if (string.IsNullOrEmpty(SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                return;
            }
            DataTable OrderList = SQLCrud.LoadDataIntoGridView($@"SELECT TOP  1 NKT.*,CONVERT(DATE,ORF.DATE_) 'TARIH', ORF.FICHENO 'FISNO',LCAPI.NAME 'KAYITKULLANICI',CL.DEFINITION_ 'CUSTOMER',CL.TELNRS1 'TEL',CL.EMAILADDR 'CARIMAIL',ORF.LOGICALREF 'FISLOGICALREF' FROM NKT_ORFFICHELOGICALREFWP NKT WITH(NOLOCK) 
            JOIN LG_{SQLLITECompany.Rows[0][0].ToString()}_{SQLLITECompany.Rows[0][1].ToString()}_ORFICHE ORF WITH(NOLOCK) ON  ORF.LOGICALREF = NKT.FICHELOGICALREF 
            LEFT JOIN L_CAPIUSER LCAPI WITH(NOLOCK) ON LCAPI.NR = ORF.CAPIBLOCK_CREATEDBY
            JOIN LG_{SQLLITECompany.Rows[0][0].ToString()}_CLCARD CL WITH(NOLOCK) ON CL.LOGICALREF = ORF.CLIENTREF
            WHERE NKT.ACTIVESTATUS = 0 AND NKT.MESSAGEBODY IS NULL ORDER BY NKT.ID DESC", SQLConnectionString.Rows[0][0].ToString());
            if (OrderList.Rows.Count <= 0)
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
            DataTable WpOrMailCountAndStatus = SQLCrud.LoadDataIntoGridView("SELECT * FROM Customers WITH (NOLOCK) WHERE CustomerToken='" + SQLConnectionRemote.Rows[0]["WebToken"].ToString() + "' AND CustomerPassword='" + SQLConnectionRemote.Rows[0]["WebPassword"].ToString() + "' AND CustomerName='" + SQLConnectionRemote.Rows[0]["CompanyName"].ToString() + "' ", SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString());
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
            DataTable wpSettings = SQLiteCrud.GetDataFromSQLite("SELECT * FROM WhatsappSettings LIMIT 1");
            DataTable SQLITEMail = SQLiteCrud.GetDataFromSQLite("SELECT * FROM MailSettings LIMIT 1");
            if (WpOrMailCountAndStatus.Rows[0]["WpStatus"].ToString() == "1")
            {
                if (int.Parse(EncryptionHelper.Decrypt(WpOrMailCountAndStatus.Rows[0]["WPCount"].ToString())) <= 0)
                    return;
                sendType = "wp";
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
            }
            else if (WpOrMailCountAndStatus.Rows[0]["WpStatus"].ToString() == "0")
            {
                if (int.Parse(EncryptionHelper.Decrypt(WpOrMailCountAndStatus.Rows[0]["MailCount"].ToString())) <= 0)
                    return;
                sendType = "mail";
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
            }
            else
                return;

            string id = OrderList.Rows[0]["ID"].ToString();
            string ficheLogicalref = OrderList.Rows[0]["FISLOGICALREF"].ToString();
            string ficheNo = OrderList.Rows[0]["FISNO"].ToString().Trim();
            if (sendType == "wp")
            {
                string logouser = OrderList.Rows[0]["KAYITKULLANICI"].ToString().Trim();
                string date = Convert.ToDateTime(OrderList.Rows[0]["TARIH"]).ToString("dd.MM.yyyy").Trim();
                string customer = OrderList.Rows[0]["CUSTOMER"].ToString().Trim();
                string accountSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][1].ToString());
                string authToken = EncryptionHelper.Decrypt(wpSettings.Rows[0][2].ToString());
                string messagingServiceSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][3].ToString());
                string templateSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][4].ToString());
                string customerTelNr = OrderList.Rows[0]["TEL"].ToString().Trim();
                string token = $"{SQLConnectionRemote.Rows[0]["WebToken"].ToString()}|{SQLConnectionRemote.Rows[0]["WebPassword"].ToString()}|{ficheNo}|{ficheLogicalref}";
                string code = $"{token}";
                string managerName = "", toNumber = "";
                byte[] pdfData = PDFCreat.PdfCreater(id);
                if (pdfData is null)
                    return;
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
                    manager = managerName.Trim(),
                    company = customer.Trim(),
                    ficheno = ficheNo.Trim(),
                    token = token,
                    logouser = logouser.Trim(),
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
                        DataTable ordersTableRemote = SQLCrud.LoadDataIntoGridView("SELECT * FROM Orders WITH (NOLOCK) WHERE CustomerID=" + WpOrMailCountAndStatus.Rows[0]["ID"] + " AND OrderFicheID=" + ficheLogicalref + "", SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString());

                        if (ordersTableRemote.Rows.Count > 0)
                        {
                            bool insertStatus = await SQLCrud.InserUpdateDelete("UPDATE Orders SET OrderFicheNo='" + ficheNo + "' ,WpStatus=1, MessageBody=NULL ,SendDate_= '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "' WHERE OrderFicheID=" + ficheLogicalref + " AND CustomerID=" + WpOrMailCountAndStatus.Rows[0]["ID"].ToString() + "", SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString());
                            if (!insertStatus)
                                return;
                            try
                            {
                                using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString())))
                                {
                                    await conn.OpenAsync();
                                    using (SqlCommand cmd = new SqlCommand(
                                        "UPDATE PDFS SET OrderFicheNo='" + ficheNo + "' , PDFFile = @PDFFile " +
                                        "WHERE CustomerID = @CustomerID  AND OrderFicheID = @OrderFicheID ", conn))
                                    {
                                        cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(WpOrMailCountAndStatus.Rows[0]["ID"]));
                                        cmd.Parameters.AddWithValue("@OrderFicheID", ficheLogicalref);
                                        cmd.Parameters.Add("@PDFFile", SqlDbType.VarBinary).Value = pdfData;
                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                                return;
                            }
                        }
                        else
                        {
                            bool insertStatus = await SQLCrud.InserUpdateDelete(
           "INSERT INTO Orders (CustomerID, OrderFicheID, OrderFicheNo, WpStatus, SendDate_) " +
           "VALUES (" + WpOrMailCountAndStatus.Rows[0]["ID"] + ", " + ficheLogicalref + ", '" + ficheNo + "', 1, '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "')",
           SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString()
       );
                            if (!insertStatus)
                                return;
                            try
                            {
                                using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString())))
                                {
                                    await conn.OpenAsync();
                                    using (SqlCommand cmd = new SqlCommand("INSERT INTO PDFS (CustomerID, OrderFicheID, OrderFicheNo, PDFFile) VALUES (@CustomerID, @OrderFicheID, @OrderFicheNo, @PDFFile)", conn))
                                    {
                                        cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(WpOrMailCountAndStatus.Rows[0]["ID"]));
                                        cmd.Parameters.AddWithValue("@OrderFicheID", ficheLogicalref);
                                        cmd.Parameters.AddWithValue("@OrderFicheNo", ficheNo);
                                        cmd.Parameters.Add("@PDFFile", SqlDbType.VarBinary).Value = pdfData;
                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                                return;
                            }
                        }
                        bool statusNKTActive = await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET ACTIVESTATUS=1  WHERE ID=" + id + "", SQLConnectionString.Rows[0][0].ToString());
                        if (!(statusNKTActive))
                            return;
                        int wpCountVariable = int.Parse(EncryptionHelper.Decrypt(WpOrMailCountAndStatus.Rows[0]["WPCount"].ToString())) - 1;
                        await SQLCrud.InserUpdateDelete(
          $"UPDATE Customers SET WPCount='{EncryptionHelper.Encrypt(wpCountVariable.ToString())}' WHERE ID={WpOrMailCountAndStatus.Rows[0]["ID"].ToString()}",
          SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString()
      );

                        return;
                    }
                    else
                    {
                        TextLog.TextLogging(message.ErrorMessage + " -- " + message.ErrorCode);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    return;
                }
            }
            else if (sendType == "mail")
            {
           
                byte[] pdfData = PDFCreat.PdfCreater(id);
                if (pdfData is null)
                    return;
                try
                {
                    if (string.IsNullOrEmpty(SQLITEMail.Rows[0][0].ToString()))
                        return;
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    return;
                }
                bool statusMail = await EMailManager.OrderMailSend(SQLITEMail.Rows[0][0].ToString(), SQLITEMail.Rows[0][1].ToString(), EncryptionHelper.Decrypt(SQLITEMail.Rows[0][2].ToString()), SQLITEMail.Rows[0][3].ToString(), int.Parse(SQLITEMail.Rows[0][4].ToString()), SQLITEMail.Rows[0][5].ToString() == "1" ? true : false, pdfData, id);
                if (!statusMail)
                    return;
            
                bool sqlStatus1 = await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET ACTIVESTATUS=2  WHERE ID=" + id + "", SQLConnectionString.Rows[0][0].ToString());
                if (!sqlStatus1)
                    return;

                DataTable ordersTableRemote = SQLCrud.LoadDataIntoGridView("SELECT * FROM Orders WITH (NOLOCK) WHERE CustomerID=" + WpOrMailCountAndStatus.Rows[0]["ID"].ToString() + " AND OrderFicheID=" + ficheLogicalref + "", SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString());
               // System.Diagnostics.Debugger.Launch();
                if (ordersTableRemote.Rows.Count > 0)
                {
                    bool insertStatusSQL = await SQLCrud.InserUpdateDelete("UPDATE Orders SET OrderFicheNo='" + ficheNo + "' ,WpStatus=2, MessageBody=NULL ,SendDate_= '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "' WHERE OrderFicheID=" + ficheLogicalref + " AND CustomerID=" + WpOrMailCountAndStatus.Rows[0]["ID"] + "", SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString());
                    if (!insertStatusSQL)
                        return;
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString())))
                        {
                            await conn.OpenAsync();
                            using (SqlCommand cmd = new SqlCommand(
                                "UPDATE PDFS SET OrderFicheNo='" + ficheNo + "' , PDFFile = @PDFFile " +
                                "WHERE CustomerID = @CustomerID  AND OrderFicheID = @OrderFicheID ", conn))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(WpOrMailCountAndStatus.Rows[0]["ID"].ToString()));
                                cmd.Parameters.AddWithValue("@OrderFicheID", ficheLogicalref);
                                cmd.Parameters.Add("@PDFFile", SqlDbType.VarBinary).Value = pdfData;
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        return;
                    }
                }
                else
                {
                  
                    bool insertStatusOrders = await SQLCrud.InserUpdateDelete(
"INSERT INTO Orders (CustomerID, OrderFicheID, OrderFicheNo, WpStatus, SendDate_) " +
"VALUES (" + WpOrMailCountAndStatus.Rows[0]["ID"].ToString() + ", " + ficheLogicalref + ", '" + ficheNo + "', 2, '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "')",
SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString()
);
                    if (!insertStatusOrders)
                        return;
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString())))
                        {
                            await conn.OpenAsync();
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO PDFS (CustomerID, OrderFicheID, OrderFicheNo, PDFFile) VALUES (@CustomerID, @OrderFicheID, @OrderFicheNo, @PDFFile)", conn))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(WpOrMailCountAndStatus.Rows[0]["ID"].ToString()));
                                cmd.Parameters.AddWithValue("@OrderFicheID", ficheLogicalref);
                                cmd.Parameters.AddWithValue("@OrderFicheNo", ficheNo);
                                cmd.Parameters.Add("@PDFFile", SqlDbType.VarBinary).Value = pdfData;
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        return;
                    }
                }
                int mailCountVariable = int.Parse(EncryptionHelper.Decrypt(WpOrMailCountAndStatus.Rows[0]["MailCount"].ToString())) - 1;
                bool sqlinsert = await SQLCrud.InserUpdateDelete($"UPDATE Customers SET MailCount='{EncryptionHelper.Encrypt(mailCountVariable.ToString())}' WHERE ID=" + WpOrMailCountAndStatus.Rows[0]["ID"].ToString() + "", SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString());
                if (!sqlinsert)
                    return;
            }
        }
        private async static void SQLListener()
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
            DataTable SQLConnectionRemote = SQLiteCrud.GetDataFromSQLite("SELECT * FROM WebSettings LIMIT 1");
            try
            {
                if (string.IsNullOrEmpty(SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                return;
            }
           // System.Diagnostics.Debugger.Launch();
            DataTable ordersTableRemote = SQLCrud.LoadDataIntoGridView(
           "SELECT ORD.OrderFicheID, ORD.MessageBody, ORD.ResponseDate FROM Orders Ord WITH (NOLOCK) " +
           "JOIN Customers Cus WITH (NOLOCK) ON Cus.ID = Ord.CustomerID " +
           "WHERE Cus.CustomerToken = '" + SQLConnectionRemote.Rows[0]["WebToken"].ToString() + "' " +
           "AND Cus.CustomerPassword = '" + SQLConnectionRemote.Rows[0]["WebPassword"].ToString() + "' " +
           "AND Cus.CustomerName = '" + SQLConnectionRemote.Rows[0]["CompanyName"].ToString() + "' " +
           "AND MessageBody IS NOT NULL AND MessageBody <> ''",
           SQLConnectionRemote.Rows[0]["SQLConnectString"].ToString()
       );

            if (ordersTableRemote.Rows.Count <= 0)
                return;
            try
            {
                if (string.IsNullOrEmpty(ordersTableRemote.Rows[0][0].ToString()))
                    return;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                return;
            }
            DataTable ordersTableSQL = SQLCrud.LoadDataIntoGridView(
                "SELECT * FROM NKT_ORFFICHELOGICALREFWP WITH (NOLOCK) WHERE MESSAGEBODY IS  NULL",
                SQLConnectionString.Rows[0][0].ToString()
            );
            var joinedList = from remote in ordersTableRemote.AsEnumerable()
                             join local in ordersTableSQL.AsEnumerable()
    on remote.Field<int>("OrderFicheID") equals local.Field<int>("FICHELOGICALREF")
                             where string.IsNullOrEmpty(local.Field<string>("MessageBody"))
                             select new
                             {
                                 FicheID = remote.Field<int>("orderFicheID"),
                                 Message = remote.Field<string>("MessageBody"),
                                 ResponseDate = remote.Field<DateTime?>("ResponseDate"),
                                 LocalID = local.Field<int>("ID"),
                                 Status = local.Field<int>("ACTIVESTATUS")
                             };
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("FicheID");
            resultTable.Columns.Add("Message");
            resultTable.Columns.Add("ResponseDate");
            resultTable.Columns.Add("LocalID");
            resultTable.Columns.Add("Status");

            foreach (var item in joinedList)
            {
                resultTable.Rows.Add(
                    item.FicheID,
                    item.Message,
                    item.ResponseDate,
                    item.LocalID,
                    item.Status
                );
            }
            if (resultTable.Rows.Count <= 0)
                return;
           
            for (int i = 0; i < resultTable.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(resultTable.Rows[i]["FicheID"]?.ToString()))
                    continue;
                string messageBody = resultTable.Rows[i]["Message"]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(messageBody))
                    continue;

                string safeMessage = messageBody.Replace("'", "''");

                string trimmedMessage = messageBody.Length > 50 ? messageBody.Substring(0, 50) : messageBody;

                string responseDate = resultTable.Rows[i]["ResponseDate"] != DBNull.Value
                    ? Convert.ToDateTime(resultTable.Rows[i]["ResponseDate"]).ToString("yyyy-MM-dd HH:mm:ss")
                    : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                int statusValue = 1;
                if (messageBody.IndexOf("Reddedildi:", StringComparison.OrdinalIgnoreCase) >= 0)
                    statusValue = 2;
                else if (messageBody.IndexOf("Onaylandı", StringComparison.OrdinalIgnoreCase) >= 0)
                    statusValue = 0;

                bool status = await SQLCrud.InserUpdateDelete(
                     "UPDATE NKT_ORFFICHELOGICALREFWP SET MESSAGEBODY='" + trimmedMessage.Trim() +
                     "', MESSAGEDATE='" + responseDate +
                     "' WHERE FICHELOGICALREF=" + resultTable.Rows[i]["FicheID"].ToString(),
                     SQLConnectionString.Rows[0][0].ToString());

                if (!status)
                    break;
                bool status2 = await SQLCrud.InserUpdateDelete(
                    $"UPDATE LG_{SQLLITECompany.Rows[0]["LogoCompanyCode"]}_{SQLLITECompany.Rows[0]["LogoPeriod"]}_ORFICHE SET GENEXP4='{safeMessage}', STATUS={statusValue} WHERE LOGICALREF={resultTable.Rows[i]["FicheID"]}",
                    SQLConnectionString.Rows[0][0].ToString());

                if (!status2)
                    break;
            }
        }
    }
}