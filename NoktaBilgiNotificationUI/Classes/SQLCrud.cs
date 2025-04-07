using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI.Classes
{
    internal class SQLCrud
    {
        internal static bool ConnectionStringControlAdd(string serverName, string loginName, string password, string databaseName)
        {
            string connectionString = $"Server={serverName};Database={databaseName};User Id ={loginName}; Password={password};Connection Timeout=10;TrustServerCertificate=True;Max Pool Size=100;Min Pool Size=5;Pooling=true;";
            SqlConnection sqLConnection = new SqlConnection(connectionString);
            try
            {
                sqLConnection.Open();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message, "Hatalı Veri Tabanı Bağlantısı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            sqLConnection.Close();
            _ = SQLiteCrud.InserUpdateDelete("UPDATE SqlConnectionString SET SQLConnectString='" + EncryptionHelper.Encrypt(connectionString) + "'", "Veri Tabanı Bağlantısı Başarılı");
            return true;
        }
        internal static DataTable LoadDataIntoGridView(string query,string connection)
        {
            using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(connection)))
            {
                try
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                        return dt;
                    }
                }
                catch (Exception e)
                {
                    TextLog.TextLogging(e.Message +" --- "+e.ToString());
                }
            }
            return null;
        }
        internal static async Task<bool> InserUpdateDelete(string query, string connection)
        {
            using (SqlConnection conn = new SqlConnection(EncryptionHelper.Decrypt(connection)))
            {
                try
                {
                    await conn.OpenAsync();
                    string[] commands = query.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string command in commands)
                    {
                        string cleanCommand = command.Replace("\n", " ").Replace("\r", " ").Trim();

                        if (!string.IsNullOrWhiteSpace(cleanCommand))
                        {
                            using (SqlCommand cmd = new SqlCommand(cleanCommand, conn))
                            {
                                cmd.CommandTimeout = 120;
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "Hatalı veritabanı işlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                    return false;
                }
            }
        }
    }
}
