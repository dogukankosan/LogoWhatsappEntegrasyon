using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NoktaBilgiNotificationService.Classes
{
    internal class SQLCrud
    {
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
                    TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                    return false;
                }
            }
        }
    }
}