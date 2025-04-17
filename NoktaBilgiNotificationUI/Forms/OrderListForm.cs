using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using NoktaBilgiNotificationUI.Business;
using NoktaBilgiNotificationUI.Classes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private string companyCode = "", companyPeriod = "", companyName = "", managerName = "", statusType = "", createdUser = "", orderDate = "", customer = "", customerTelNr = "", customerIncharge = "";
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
WHEN ORF.STATUS=0 THEN 'Sevk Edilebilir' 
WHEN ORF.STATUS=3 THEN 'Sevk Edilemez' 
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
LEFT JOIN L_CAPIUSER CAPUSER WITH (NOLOCK) ON CAPUSER.NR=ORF.CAPIBLOCK_CREATEDBY
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
                customerTelNr = gridView1.GetRowCellValue(rowHandle, "Cari Telefon")?.ToString();
                customerIncharge = gridView1.GetRowCellValue(rowHandle, "Cari Yetkili")?.ToString();
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
            byte[] pdfData = PDFCreat.PdfCreater(nktID);
            if (pdfData != null)
            {
                string tempPdfPath = Path.Combine(Path.GetTempPath(), $"preview_{Guid.NewGuid()}.pdf");
                File.WriteAllBytes(tempPdfPath, pdfData);
                Process.Start(tempPdfPath);
            }
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
            #region Kontrol
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
            DataTable remoteSQL = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString,WebToken,WebPassword,CompanyName FROM WebSettings LIMIT 1");
            if (remoteSQL.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(remoteSQL.Rows[0]["SQLConnectString"].ToString()))
                {
                    XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable MailCountAndCustomerStatus = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 MailCount,ManAndCusStatus,ID FROM Customers WITH (NOLOCK) WHERE CustomerToken='" + remoteSQL.Rows[0]["WebToken"] + "' AND CustomerPassword='" + remoteSQL.Rows[0]["WebPassword"] + "' AND CustomerName='" + remoteSQL.Rows[0]["CompanyName"].ToString() + "' ORDER BY ID DESC", remoteSQL.Rows[0]["SQLConnectString"].ToString());
            if (MailCountAndCustomerStatus is null)
            {
                XtraMessageBox.Show("Mail Kontör Okuma İşlemi Hatalı", "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MailCountAndCustomerStatus.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Mail Kontör Okuma İşlemi Hatalı", "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(MailCountAndCustomerStatus.Rows[0]["MailCount"].ToString()))
                {
                    XtraMessageBox.Show("Mail Kontör Okuma İşlemi Hatalı", "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Mail Kontör Okuma İşlemi Hatalı", "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (int.Parse(EncryptionHelper.Decrypt(MailCountAndCustomerStatus.Rows[0][0].ToString())) <= 0)
                {
                    XtraMessageBox.Show("Mail Kontör Okuma İşlemi Hatalı Veya Kontörünüz Bitmiştir Lütfen Kontör Siparişi Veriniz.", "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Whatsapp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }
            DataTable mailSettings = SQLiteCrud.GetDataFromSQLite("SELECT * FROM MailSettings LIMIT 1");
            if (mailSettings is null)
            {
                XtraMessageBox.Show("Mail Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Mail Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (mailSettings.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Mail Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Mail Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(mailSettings.Rows[0][1].ToString()))
                {
                    XtraMessageBox.Show("Mail Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Mail Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Mail Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }
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
            DataTable dtSQLConnection = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            #endregion

            byte[] pdfData = PDFCreat.PdfCreater(nktID);
            if (pdfData is null)
                return;

            bool statusMail = await EMailManager.OrderMailSend(mailSettings.Rows[0][0].ToString(), mailSettings.Rows[0][1].ToString(), EncryptionHelper.Decrypt(mailSettings.Rows[0][2].ToString()), mailSettings.Rows[0][3].ToString(), int.Parse(mailSettings.Rows[0][4].ToString()), mailSettings.Rows[0][5].ToString() == "1" ? true : false, pdfData, nktID);
            if (statusMail)
            {
                bool sqlStatus = await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET MESSAGEBODY=NULL , MESSAGEDATE=NULL, ACTIVESTATUS=2  WHERE ID=" + nktID + "", dtSQLConnection.Rows[0][0].ToString());
                if (sqlStatus)
                {
                    DataTable ordersTableRemote = SQLCrud.LoadDataIntoGridView("SELECT * FROM Orders WITH (NOLOCK) WHERE CustomerID=" + MailCountAndCustomerStatus.Rows[0]["ID"] + " AND OrderFicheID=" + ficheLogicalRef + "", remoteSQL.Rows[0][0].ToString());
                    if (!(ordersTableRemote is null) || ordersTableRemote.Rows.Count > 0)
                    {
                        
                            bool insertStatusSQL = await SQLCrud.InserUpdateDelete("UPDATE Orders SET OrderFicheNo='" + siparisNo + "' ,WpStatus=2, MessageBody=NULL ,SendDate_= '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "' WHERE OrderFicheID=" + ficheLogicalRef + " AND CustomerID=" + MailCountAndCustomerStatus.Rows[0]["ID"] + "", remoteSQL.Rows[0][0].ToString());
                        if (!insertStatusSQL)
                        {
                            XtraMessageBox.Show("Sipariş Gönderme İşlemi Hatalı", "Hatalı Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(remoteSQL.Rows[0][0].ToString())))
                        {
                            await conn.OpenAsync();
                            using (SqlCommand cmd = new SqlCommand(
                                "UPDATE PDFS SET OrderFicheNo='" + siparisNo + "' , PDFFile = @PDFFile " +
                                "WHERE CustomerID = @CustomerID  AND OrderFicheID = @OrderFicheID ", conn))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(MailCountAndCustomerStatus.Rows[0]["ID"]));
                                cmd.Parameters.AddWithValue("@OrderFicheID", ficheLogicalRef);
                                cmd.Parameters.Add("@PDFFile", SqlDbType.VarBinary).Value = pdfData;
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    else
                    {
                        bool insertStatusOrders = await SQLCrud.InserUpdateDelete(
"INSERT INTO Orders (CustomerID, OrderFicheID, OrderFicheNo, WpStatus, SendDate_) " +
"VALUES (" + MailCountAndCustomerStatus.Rows[0]["ID"] + ", " + ficheLogicalRef + ", '" + siparisNo + "', 2, '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "')",
remoteSQL.Rows[0][0].ToString()
);
                        if (!insertStatusOrders)
                        {
                            XtraMessageBox.Show("Sipariş Ekleme İşlemi Hatalı", "Hatalı Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(remoteSQL.Rows[0][0].ToString())))
                        {
                            await conn.OpenAsync();
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO PDFS (CustomerID, OrderFicheID, OrderFicheNo, PDFFile) VALUES (@CustomerID, @OrderFicheID, @OrderFicheNo, @PDFFile)", conn))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(MailCountAndCustomerStatus.Rows[0]["ID"]));
                                cmd.Parameters.AddWithValue("@OrderFicheID", ficheLogicalRef);
                                cmd.Parameters.AddWithValue("@OrderFicheNo", siparisNo);
                                cmd.Parameters.Add("@PDFFile", SqlDbType.VarBinary).Value = pdfData;
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }              
                    int mailCountVariable = int.Parse(EncryptionHelper.Decrypt(MailCountAndCustomerStatus.Rows[0][0].ToString())) - 1;
                    if (await SQLCrud.InserUpdateDelete($"UPDATE Customers SET MailCount='{EncryptionHelper.Encrypt(mailCountVariable.ToString())}' WHERE ID="+ MailCountAndCustomerStatus.Rows[0]["ID"].ToString() + "", remoteSQL.Rows[0][0].ToString()))
                        XtraMessageBox.Show($"Kalan Mail Kontör Sayınız {mailCountVariable}", "Kalan Kontör Sayınız", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            #region Kontrol
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
            DataTable remoteSQL = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString,WebToken,WebPassword FROM WebSettings LIMIT 1");
            if (remoteSQL.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(remoteSQL.Rows[0]["SQLConnectString"].ToString()))
                {
                    XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable WpCountAndCustomerStatus = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 WPCount,ManAndCusStatus,ID FROM Customers WITH (NOLOCK) WHERE CustomerToken='" + remoteSQL.Rows[0]["WebToken"] + "' AND CustomerPassword='" + remoteSQL.Rows[0]["WebPassword"] + "' ORDER BY ID DESC", remoteSQL.Rows[0]["SQLConnectString"].ToString());
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
                if (string.IsNullOrEmpty(WpCountAndCustomerStatus.Rows[0]["WPCount"].ToString()))
                {
                    XtraMessageBox.Show("Whatsapp Kontör Okuma İşlemi Hatalı", "Hatalı Whatsapp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Whatsapp Kontör Okuma İşlemi Hatalı", "Hatalı Whatsapp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (int.Parse(EncryptionHelper.Decrypt(WpCountAndCustomerStatus.Rows[0][0].ToString())) <= 0)
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
            DataTable wpSettings = SQLiteCrud.GetDataFromSQLite("SELECT * FROM WhatsappSettings LIMIT 1");
            if (wpSettings is null)
            {
                XtraMessageBox.Show("Whatsapp Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Wp Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (wpSettings.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Whatsapp Bilgilerinizi Ayarları Doldurunuz !!", "Hatalı Wp Gönderme", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                XtraMessageBox.Show(ex.Message, "Hatalı Wp Gönderme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return;
            }
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
            DataTable dtSQLConnection = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            #endregion

            string accountSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][1].ToString());
            string authToken = EncryptionHelper.Decrypt(wpSettings.Rows[0][2].ToString());
            string messagingServiceSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][3].ToString());
            string templateSid = EncryptionHelper.Decrypt(wpSettings.Rows[0][4].ToString());
            string managerName = "", toNumber = "";
            // 1 yönetici 0 müşteri
            if (WpCountAndCustomerStatus.Rows[0]["ManAndCusStatus"].ToString() == "1")
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
            string ficheNo = siparisNo.Trim();
            string logouser = createdUser;
            string date = orderDate.Trim();
            string token = $"{remoteSQL.Rows[0]["WebToken"].ToString()}|{remoteSQL.Rows[0]["WebPassword"].ToString()}|{siparisNo}|{ficheLogicalRef}";
            string code = $"{token}";
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
                    byte[] pdfData = PDFCreat.PdfCreater(nktID);
                    if (pdfData is null)
                        return;
                   bool update =await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET MESSAGEBODY=NULL , MESSAGEDATE=NULL,ACTIVESTATUS=1  WHERE ID=" + nktID + "", dtSQLConnection.Rows[0][0].ToString());
                    if (!update)
                        return;
                    DataTable ordersTableRemote = SQLCrud.LoadDataIntoGridView("SELECT * FROM Orders WITH (NOLOCK) WHERE CustomerID=" + WpCountAndCustomerStatus.Rows[0]["ID"] + " AND OrderFicheID=" + ficheLogicalRef + "", remoteSQL.Rows[0][0].ToString());
                   
                    if (ordersTableRemote is null || ordersTableRemote.Rows.Count > 0)
                    {

                        bool insertStatus = await SQLCrud.InserUpdateDelete("UPDATE Orders SET OrderFicheNo='" + ficheNo + "' ,WpStatus=1, MessageBody=NULL ,SendDate_= '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "' WHERE OrderFicheID="+ficheLogicalRef+" AND CustomerID="+ WpCountAndCustomerStatus.Rows[0]["ID"]+ "", remoteSQL.Rows[0][0].ToString());
                        if (!insertStatus)
                        {
                            XtraMessageBox.Show("Sipariş Gönderme İşlemi Hatalı", "Hatalı Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(remoteSQL.Rows[0][0].ToString())))
                        {
                            await conn.OpenAsync();
                            using (SqlCommand cmd = new SqlCommand(
                                "UPDATE PDFS SET OrderFicheNo='" + siparisNo + "' , PDFFile = @PDFFile " +
                                "WHERE CustomerID = @CustomerID  AND OrderFicheID = @OrderFicheID ", conn))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(WpCountAndCustomerStatus.Rows[0]["ID"]));
                                cmd.Parameters.AddWithValue("@OrderFicheID", ficheLogicalRef);
                                cmd.Parameters.Add("@PDFFile", SqlDbType.VarBinary).Value = pdfData;
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    else
                    {
                        bool insertStatus = await SQLCrud.InserUpdateDelete(
       "INSERT INTO Orders (CustomerID, OrderFicheID, OrderFicheNo, WpStatus, SendDate_) " +
       "VALUES (" + WpCountAndCustomerStatus.Rows[0]["ID"] + ", " + ficheLogicalRef + ", '" + ficheNo + "', 1, '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "')",
       remoteSQL.Rows[0][0].ToString()
   );
                        if (!insertStatus)
                        {
                            XtraMessageBox.Show("Sipariş Ekleme İşlemi Hatalı", "Hatalı Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(remoteSQL.Rows[0][0].ToString())))
                        {
                            await conn.OpenAsync();
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO PDFS (CustomerID, OrderFicheID, OrderFicheNo, PDFFile) VALUES (@CustomerID, @OrderFicheID, @OrderFicheNo, @PDFFile)", conn))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(WpCountAndCustomerStatus.Rows[0][2]));
                                cmd.Parameters.AddWithValue("@OrderFicheID", ficheLogicalRef);
                                cmd.Parameters.AddWithValue("@OrderFicheNo", siparisNo);
                                cmd.Parameters.Add("@PDFFile", SqlDbType.VarBinary).Value = pdfData;
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    XtraMessageBox.Show("Whatsapp mesajı gönderme işlemi başarılı", "Başarılı Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET ACTIVESTATUS=1  WHERE ID=" + nktID + "", dtSQLConnection.Rows[0][0].ToString()))
                    {
                        int wpCountVariable = int.Parse(EncryptionHelper.Decrypt(WpCountAndCustomerStatus.Rows[0][0].ToString())) - 1;
                        if (await SQLCrud.InserUpdateDelete($"UPDATE Customers SET WPCount='{EncryptionHelper.Encrypt(wpCountVariable.ToString())}' WHERE ID=" + WpCountAndCustomerStatus.Rows[0]["ID"].ToString() + "", remoteSQL.Rows[0][0].ToString()))
                            XtraMessageBox.Show($"Kalan Whatsapp Kontör Sayınız {wpCountVariable}", "Kalan Kontör Sayınız", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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