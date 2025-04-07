using DevExpress.XtraEditors;
using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI.Classes
{
    internal class TokenGenerate
    {
        private const string secretKey = "NoktaBilgiIslemToken";
        internal bool HasRecentToken(string ficheNo, int orderId, int hour)
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            if (dt is null)
                return false;
            if (dt.Rows.Count <= 0)
                return false;
            try
            {
                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    return false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Token Oluşturma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return false;
            }
            DataTable dr = SQLCrud.LoadDataIntoGridView("SELECT COUNT(*) FROM Tokens WHERE OrderFicheNo='" + ficheNo + "' AND OrderId = " + orderId + " AND ExpiryDate > DATEADD(HOUR, -" + hour + ", GETUTCDATE())", dt.Rows[0][0].ToString());
            if (dr is null)
                return false;
            if (dr.Rows.Count <= 0)
                return false;
            if (dr.Rows.Count > 0)
            {
                if (int.Parse(dr.Rows[0][0].ToString())>0)
                    return true;
            }
                
            return false;
        }
        internal async Task<string> GenerateSecureToken(string type, string ficheNo, int orderId,string pdfFileName)
        {
            string rawData = ficheNo + "|" + DateTime.UtcNow.AddHours(8).Ticks;
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                string token = Convert.ToBase64String(hash) + "|" + rawData;
                string encryptedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                bool status=await SaveTokenToDatabase(type, ficheNo, orderId, encryptedToken, DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"), Path.GetFileName(pdfFileName));
                if (status)
                    return encryptedToken;
                XtraMessageBox.Show("Hatalı Token Oluşturma İşlemi", "Hatalı İşlem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private async Task<bool> SaveTokenToDatabase(string type, string ficheNo, int orderId, string token, string expiryDate,string pdfFileName)
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            if (dt is null)
                return false;
            if (dt.Rows.Count <= 0)
                return false;
            try
            {
                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    return false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hatalı Token Oluşturma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                return false;
            }
            bool status = await SQLCrud.InserUpdateDelete("INSERT INTO Tokens (OrderFicheNo,OrderId, Token, ExpiryDate,TypeWpOrMail,PDFFileName) VALUES ('" + ficheNo + "','" + orderId + "','" + token + "','" + expiryDate + "','" + type + "','"+ pdfFileName +"')", dt.Rows[0][0].ToString());
            return status;
        }
    }
}