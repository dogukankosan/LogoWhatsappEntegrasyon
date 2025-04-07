using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using NoktaBilgiNotificationUI.Business;
using NoktaBilgiNotificationUI.Classes;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class OrderListForm : XtraForm
    {
        public OrderListForm()
        {
            InitializeComponent();
        }
        private string companyCode = "", companyPeriod = "", companyName = "", managerName = "", statusType = "", domainURL = "", createdUser = "", orderDate = "", customer = "", customerTelNr = "", customerIncharge = "";
        private string siparisNo = "", ficheLogicalRef = "", nktID = "";
        private void List()
        {
            string query = $@"SELECT 
NKT.ID,
NKT.FICHELOGICALREF 'FISLOGICALREF',
CASE  WHEN NKT.ACTIVESTATUS=0 THEN 'Gönderilmedi' WHEN NKT.ACTIVESTATUS=1 THEN 'Wp Gönderildi' WHEN NKT.ACTIVESTATUS=2 THEN 'Mail Gönderildi' ELSE 'Kontrol Edilecek' END  'Gönderim Türü',
ISNULL(NKT.MESSAGEBODY,'') 'Gelen Mesaj',
CONVERT(DATETIME ,NKT.MESSAGEDATE)  'Gelen Mesaj Tarih',
CASE WHEN ORF.STATUS=1 THEN 'Önerilen' 
WHEN ORF.STATUS=3 THEN 'Sevk Edilebilir' 
WHEN IIF(
(SELECT  SUM(AMOUNT)-SUM(SHIPPEDAMOUNT) FROM LG_001_01_ORFICHE FIC WITH (NOLOCK)
JOIN LG_001_01_ORFLINE LINE WITH (NOLOCK) ON LINE.ORDFICHEREF=FIC.LOGICALREF
WHERE  LINE.TRCODE=1  AND LINE.ORDFICHEREF=ORF.LOGICALREF)=0,'Kapandı','Sevk Edilebilir')='Kapandı' THEN 'Kapandı'
 WHEN IIF(
(SELECT TOP 1 AMOUNT-SHIPPEDAMOUNT FROM LG_001_01_ORFICHE FIC
JOIN LG_001_01_ORFLINE LINE WITH (NOLOCK) ON LINE.ORDFICHEREF=FIC.LOGICALREF
WHERE  LINE.TRCODE=1  AND LINE.ORDFICHEREF=ORF.LOGICALREF
ORDER BY LINE.LOGICALREF DESC)=0,'Kapandı','Sevk Edilebilir')='Sevk Edilebilir' THEN 'Sevk Edilebilir'
ELSE 'Kontrol Edilecek' END 'Sipariş Durumu' ,
ORF.FICHENO 'Sipariş No',CONVERT(DATE,DATE_,104) 'Sipariş Tarihi',
CL.CODE 'Cari Kodu',
CL.DEFINITION_ 'Cari Açıklaması',
CL.INCHARGE 'Cari Yetkili',
CL.TELNRS1 'Cari Telefon',
CL.EMAILADDR 'Cari Mail Adres',
ORF.ADDDISCOUNTS 'İndirim Tutarı',
ORF.TOTALVAT 'Toplam KDV',
ORF.GROSSTOTAL 'Brüt Total',
ORF.NETTOTAL 'Net Toplam',
CAPUSER.NAME 'Kayıt Eden Kullanıcı',
ORF.CAPIBLOCK_CREADEDDATE  'Kayıt Tarihi' 
FROM LG_{companyCode}_{companyPeriod}_ORFICHE ORF WITH (NOLOCK)
JOIN LG_{companyCode}_CLCARD CL WITH (NOLOCK) ON CL.LOGICALREF=ORF.CLIENTREF
LEFT JOIN L_CAPIUSER CAPUSER WITH (NOLOCK) ON CAPUSER.LOGICALREF=ORF.CAPIBLOCK_CREATEDBY
JOIN NKT_ORFFICHELOGICALREFWP NKT WITH (NOLOCK) ON NKT.FICHELOGICALREF=ORF.LOGICALREF
WHERE YEAR(ORF.DATE_)=YEAR(GETDATE()) AND ORF.CANCELLED=0 AND ORF.TRCODE=1
ORDER BY ORF.LOGICALREF DESC
";
            try
            {
                DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
                if (dt is null)
                {
                    XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dt.Rows.Count <= 0)
                {
                    XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                gridControl1.DataSource = SQLCrud.LoadDataIntoGridView(query, dt.Rows[0][0].ToString());
                if (!(gridControl1.DataSource is null))
                {
                    gridView1.Columns["Gelen Mesaj Tarih"].DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
                    gridView1.Columns["Kayıt Tarihi"].DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı SQL Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                this.Close();
            }
        }
        private async void gönderilmediYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nktID))
            {
                XtraMessageBox.Show("Lütfen gridden bir kayıt seçiniz", "Hatalı Tür İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            if (dt is null)
            {
                XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dt.Rows.Count <= 0)
            {
                XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    XtraMessageBox.Show("SQL Bağlantı Bilgileri Hatalı", "Hatalı Bağlantı İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Statü Değiştirme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }
            await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET ACTIVESTATUS=0 WHERE ID=" + nktID + "", dt.Rows[0][0].ToString());
            List();
            int rowHandle = gridView1.GetSelectedRows().FirstOrDefault();
            if (rowHandle >= 0)
            {
                 siparisNo = gridView1.GetRowCellValue(rowHandle, "Sipariş No")?.ToString();
                ficheLogicalRef = gridView1.GetRowCellValue(rowHandle, "FISLOGICALREF")?.ToString();
                nktID = gridView1.GetRowCellValue(rowHandle, "ID")?.ToString();
                statusType = gridView1.GetRowCellValue(rowHandle, "Gönderim Türü")?.ToString();
                createdUser = gridView1.GetRowCellValue(rowHandle, "Kayıt Eden Kullanıcı")?.ToString();
                orderDate = gridView1.GetRowCellValue(rowHandle, "Sipariş Tarihi")?.ToString().Replace("00:00:00", "");
                customer = gridView1.GetRowCellValue(rowHandle, "Cari Açıklaması")?.ToString();
                customerTelNr= gridView1.GetRowCellValue(rowHandle, "Cari Telefon")?.ToString();
                customerIncharge= gridView1.GetRowCellValue(rowHandle, "Cari Yetkili")?.ToString();
            }
        }
        void GridDesigner()
        {
            gridView1.Appearance.HeaderPanel.Font = new Font("Tahoma", 11, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            gridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.RowHeight = 20;
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();
            gridView1.Appearance.HeaderPanel.Font = new Font("Tahoma", 8, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.OptionsView.ShowAutoFilterRow = true;
            gridView1.Appearance.FilterPanel.Font = new Font("Tahoma", 10, FontStyle.Italic);
            gridView1.Appearance.FilterPanel.ForeColor = Color.Blue;
            gridView1.Appearance.FilterPanel.Options.UseForeColor = true;
            gridView1.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsFind.AlwaysVisible = true;
            gridView1.OptionsView.ShowGroupPanel = true;
        }
        private void PdfCreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nktID))
            {
                XtraMessageBox.Show("Lütfen gridden bir kayıt seçiniz", "Hatalı Tür İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string status = PDFCreat.PdfCreater(nktID);
            if (!string.IsNullOrEmpty(status))
                Process.Start(status);
        }
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount > 0 && !(string.IsNullOrEmpty(ficheLogicalRef)))
            {
                OrderDetailsList frm = new OrderDetailsList();
                frm.ficheLogicalRef = int.Parse(ficheLogicalRef);
                frm.ShowDialog();
            }
            else
                XtraMessageBox.Show("Lütfen gridden önce ilgili kaydı seçiniz.", "Hatalı Detay Listesi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async void mailSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nktID))
            {
                XtraMessageBox.Show("Lütfen Gridden Bir Kayıt Seçiniz", "Hatalı Seçim İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (statusType != "Gönderilmedi")
            {
                XtraMessageBox.Show("Gönderilen WP Veya Mail Tekrardan Gönderilemez", "Hatalı Tür İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable MailCount = SQLiteCrud.GetDataFromSQLite("SELECT MailCount FROM MailAndWpCount LIMIT 1");
            if (MailCount is null)
            {
                XtraMessageBox.Show("Mail Kontör Okuma İşlemi Hatalı", "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MailCount.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Mail Kontör Okuma İşlemi Hatalı", "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (int.Parse(EncryptionHelper.Decrypt(MailCount.Rows[0][0].ToString())) <= 0)
                {
                    XtraMessageBox.Show("Mail Kontör Okuma İşlemi Hatalı Veya Kontörünüz Bitmiştir Lütfen Kontör Siparişi Veriniz.", "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }
            TokenGenerate gl = new TokenGenerate();
            string token = "";
            DataTable dtSQLConnection = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            string status = PDFCreat.PdfCreater(nktID);
            if (string.IsNullOrEmpty(status))
                return;
                if (!gl.HasRecentToken(siparisNo, int.Parse(ficheLogicalRef), 8))
                token = await gl.GenerateSecureToken("mail", siparisNo, int.Parse(ficheLogicalRef),status);           
            else
            {
                DataTable dr = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 Token FROM Tokens WHERE OrderFicheNo='" + siparisNo + "' AND OrderId = " + ficheLogicalRef + " AND ExpiryDate > DATEADD(HOUR, -" + 8 + ", GETUTCDATE())", dtSQLConnection.Rows[0][0].ToString());
                if (!(dr is null))
                {
                    if (dr.Rows.Count>0)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(dr.Rows[0][0].ToString()))
                                token = dr.Rows[0][0].ToString();
                            else
                            {
                                XtraMessageBox.Show("Token Okuma İşlemi Hatalı", "Hatalı Mail İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }                             
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "Hatalı Mail İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            string link = $"{domainURL}/OrderWp/Approve?token={token}";
                DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT * FROM MailSettings LIMIT 1");
                if (!(dt is null))
                {
                    if (dt.Rows.Count <= 0)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                            {
                                XtraMessageBox.Show("EMail ayarları yapılmamış lütfen önce ayarları kaydediniz", "Hatalı Mail İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "Hatalı Mail İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                            return;
                        }
                    }
                }
                bool statusMail = await EMailManager.OrderMailSend(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), EncryptionHelper.Decrypt(dt.Rows[0][2].ToString()), dt.Rows[0][3].ToString(), int.Parse(dt.Rows[0][4].ToString()), dt.Rows[0][5].ToString() == "1" ? true : false, link, status, nktID);
                if (statusMail)
                {
                    bool sqlStatus=await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET ACTIVESTATUS=2  WHERE ID=" + nktID + "", dtSQLConnection.Rows[0][0].ToString());
                    if (sqlStatus)
                    {
                        int mailCountVariable = int.Parse(EncryptionHelper.Decrypt(MailCount.Rows[0][0].ToString())) - 1;
                        await SQLiteCrud.InserUpdateDelete($"UPDATE MailAndWpCount SET MailCount='{EncryptionHelper.Encrypt(mailCountVariable.ToString())}'", " Kalan Mail Kontör Sayınız: " + mailCountVariable.ToString());
                        List();
                    }
                }
        }
        private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                siparisNo = gridView1.GetRowCellValue(e.FocusedRowHandle, "Sipariş No")?.ToString();
                ficheLogicalRef = gridView1.GetRowCellValue(e.FocusedRowHandle, "FISLOGICALREF")?.ToString();
                nktID = gridView1.GetRowCellValue(e.FocusedRowHandle, "ID")?.ToString();
                statusType = gridView1.GetRowCellValue(e.FocusedRowHandle, "Gönderim Türü")?.ToString();
                createdUser = gridView1.GetRowCellValue(e.FocusedRowHandle, "Kayıt Eden Kullanıcı")?.ToString();
                orderDate = gridView1.GetRowCellValue(e.FocusedRowHandle, "Sipariş Tarihi")?.ToString().Replace("00:00:00", "");
                customer = gridView1.GetRowCellValue(e.FocusedRowHandle, "Cari Açıklaması")?.ToString();
                customerTelNr = gridView1.GetRowCellValue(e.FocusedRowHandle, "Cari Telefon")?.ToString();
                customerIncharge = gridView1.GetRowCellValue(e.FocusedRowHandle, "Cari Yetkili")?.ToString();
            }
        }
        private async void tool_WpSend_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nktID))
            {
                XtraMessageBox.Show("Lütfen gridden bir kayıt seçiniz", "Hatalı Seçim İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (statusType != "Gönderilmedi")
            {
                XtraMessageBox.Show("Gönderilen wp Veya Mail tekrardan gönderilemez", "Hatalı Tür İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable WpCountAndCustomerStatus = SQLiteCrud.GetDataFromSQLite("SELECT WPCount,ManAndCusStatus FROM MailAndWpCount LIMIT 1");
            if (WpCountAndCustomerStatus is null)
            {
                XtraMessageBox.Show("Whatsapp Kontör Okuma İşlemi Hatalı", "Hatalı Whatsapp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (WpCountAndCustomerStatus.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Whatsapp Kontör Okuma İşlemi Hatalı", "Hatalı Whatsapp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (int.Parse(EncryptionHelper.Decrypt(WpCountAndCustomerStatus.Rows[0][0].ToString()))<=0)
                {
                    XtraMessageBox.Show("Whatsapp Kontör Okuma İşlemi Hatalı Veya Kontörünüz Bitmiştir Lütfen Kontör Siparişi Veriniz.", "Hatalı Whatsapp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Whatsapp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }

            DataTable wpSettings=SQLiteCrud.GetDataFromSQLite("SELECT * FROM WhatsappSettings LIMIT 1");
            if (wpSettings is null)
            {
                XtraMessageBox.Show("Whatsapp Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Wp Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (wpSettings.Rows.Count<=0)
            {
                XtraMessageBox.Show("Whatsapp Bilgilerinizi Ayarları Doldurunuz !!","Hatalı Wp Gönderme",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(wpSettings.Rows[0][1].ToString()))
                {
                    XtraMessageBox.Show("Whatsapp Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Wp Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message,"Hatalı Wp Gönderme İşlemi",MessageBoxButtons.OK,MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }
            string accountSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][1].ToString());
            string authToken = EncryptionHelper.Decrypt(wpSettings.Rows[0][2].ToString());
            string messagingServiceSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][3].ToString());
            string templateSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][4].ToString());

            DataTable companySettings = SQLiteCrud.GetDataFromSQLite("SELECT * FROM CompanySettings LIMIT 1");
            if (companySettings is null)
            {
                XtraMessageBox.Show("Şirket Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Wp Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (companySettings.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Şirket Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Wp Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(companySettings.Rows[0][0].ToString()))
                {
                    XtraMessageBox.Show("Şirket Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Wp Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Wp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
            }
            string status = PDFCreat.PdfCreater(nktID);
            if (string.IsNullOrEmpty(status))
                return;
            string fileName = Path.GetFileName(status);
            string managerName = "",  toNumber = "";
            // 1 yönetici 0 müşteri

            if (WpCountAndCustomerStatus.Rows[0][1].ToString()=="1")
            {
                managerName = companySettings.Rows[0]["ManagerName"].ToString();
                 toNumber = $"whatsapp:{wpSettings.Rows[0][0].ToString()}";
            }
            else
            {
                if (!CustomerLogic.IsValidPhoneNumber(customerTelNr))
                    return;
                managerName = customer;
                customer = companySettings.Rows[0]["LogoCompanyName"].ToString();
                toNumber = $"whatsapp:{customerTelNr}";
            }               
            string ficheNo =siparisNo;
            string logouser = createdUser;
            string date = orderDate.Trim();
            TokenGenerate gl = new TokenGenerate();
            string token = "";
            DataTable dtSQLConnection = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            if (!gl.HasRecentToken(siparisNo, int.Parse(ficheLogicalRef), 8))
                token = await gl.GenerateSecureToken("wp", siparisNo, int.Parse(ficheLogicalRef), status);
            else
            {           
                DataTable dr = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 Token FROM Tokens WHERE OrderFicheNo='" + siparisNo + "' AND OrderId = " + ficheLogicalRef + " AND ExpiryDate > DATEADD(HOUR, -" + 8 + ", GETUTCDATE())", dtSQLConnection.Rows[0][0].ToString());
                if (!(dr is null))
                {
                    if (dr.Rows.Count > 0)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(dr.Rows[0][0].ToString()))
                                token = dr.Rows[0][0].ToString();
                            else
                            {
                                XtraMessageBox.Show("Token Okuma İşlemi Hatalı", "Hatalı Mail İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "Hatalı Wp İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            string code = $"{fileName}|{token}";
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
                    XtraMessageBox.Show("Whatsapp mesajı gönderme işlemi başarılı", "Başarılı Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET ACTIVESTATUS=1  WHERE ID=" + nktID + "", dtSQLConnection.Rows[0][0].ToString());
                    int wpCountVariable = int.Parse(EncryptionHelper.Decrypt(WpCountAndCustomerStatus.Rows[0][0].ToString())) - 1;
                    await SQLiteCrud.InserUpdateDelete($"UPDATE MailAndWpCount SET WPCount='{EncryptionHelper.Encrypt(wpCountVariable.ToString())}'", " Kalan Whatsapp Kontör Sayınız: " + wpCountVariable.ToString());
                    List();
                }
                else
                {
                    string hata = $"Whatsapp mesaj gönderme hatası:\nKod: {message.ErrorCode}\nMesaj: {message.ErrorMessage}";
                    XtraMessageBox.Show(hata, "Gönderim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TextLog.TextLogging(hata);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Whatsapp Mesaj Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void CompanySettingsGet()
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT * FROM CompanySettings LIMIT 1");
            if (!(dt is null))
            {
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        companyCode = dt.Rows[0][0].ToString();
                        companyPeriod = dt.Rows[0][1].ToString();
                        companyName = dt.Rows[0][2].ToString();
                        managerName = dt.Rows[0][3].ToString();
                        domainURL = dt.Rows[0][6].ToString();

                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Hatalı Şirket Bilgileri Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                        this.Close();
                    }
                }
            }
        }
        private void OrderListForm_Load(object sender, EventArgs e)
        {
            CompanySettingsGet();
            if (string.IsNullOrEmpty(companyCode))
            {
                XtraMessageBox.Show("Lütfen önce şirket bilgileri ayarları giriniz.", "SQLite Veritabanı okuma işlemi hatalı.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List();
            if (!(gridControl1.DataSource is null))
            {
                GridDesigner();
                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
            }
        }
        private void OrderListForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}