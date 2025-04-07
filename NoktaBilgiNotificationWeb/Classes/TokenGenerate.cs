using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NoktaBilgiNotificationWeb.Classes
{
    internal class TokenGenerate
    {
        private const string secretKey = "NoktaBilgiIslemToken";
        private string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        internal string ValidateToken(string encodedToken)
        {
            try
            {
                if (!(IsTokenValid(encodedToken))) return null;
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encodedToken));
                string[] parts = decoded.Split('|');
                if (parts.Length != 3) return null;
                string hashBase64 = parts[0];
                string orderFicheNo = parts[1].ToString();
                long ticks = long.Parse(parts[2]);
                if (DateTime.UtcNow.Ticks > ticks) return null;
                string rawData = orderFicheNo + "|" + ticks;
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                {
                    byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    if (Convert.ToBase64String(computedHash) == hashBase64)
                        return orderFicheNo;
                }
            }
            catch { return null; }
            return null;
        }
        internal string PDFValidateToken(string encodedToken, string pdf)
        {
            try
            {
                if (!(IsTokenPDFValid(encodedToken, pdf))) return null;
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encodedToken));
                string[] parts = decoded.Split('|');
                if (parts.Length != 3) return null;
                string hashBase64 = parts[0];
                string orderFicheNo = parts[1].ToString();
                long ticks = long.Parse(parts[2]);
                if (DateTime.UtcNow.Ticks > ticks) return null;
                string rawData = orderFicheNo + "|" + ticks;
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                {
                    byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    if (Convert.ToBase64String(computedHash) == hashBase64)
                        return orderFicheNo;
                }
            }
            catch { return null; }
            return null;
        }
        private bool IsTokenValid(string token)
        {
            DataTable dt = SQLCrud.LoadDataIntoGridView("SELECT COUNT(*) FROM Tokens WHERE Token = '" + token + "' AND ExpiryDate > GETUTCDATE()");
            if (dt.Rows.Count > 0)
                return int.Parse(dt.Rows[0][0].ToString()) > 0;
            return false;
        }
        private bool IsTokenPDFValid(string token, string pdf)
        {
            DataTable dt = SQLCrud.LoadDataIntoGridView("SELECT COUNT(*) FROM Tokens WHERE PDFFileName='" + pdf + "' AND Token = '" + token + "' AND ExpiryDate > GETUTCDATE()");
            if (dt.Rows.Count > 0)
                return int.Parse(dt.Rows[0][0].ToString()) > 0;
            return false;
        }
        internal async void InvalidateToken(string token)
        {
           bool status= await SQLCrud.InserUpdateDelete("DELETE FROM Tokens WHERE Token = '"+ token + "'");
            if (!status)
            {
                TextLog.TextLogging("Token silme işlemi hatalı");
            }
        }
    }
}