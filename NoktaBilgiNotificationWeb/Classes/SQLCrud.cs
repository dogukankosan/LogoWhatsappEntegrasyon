using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NoktaBilgiNotificationWeb.Classes
{
    internal class SQLCrud
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        internal static DataTable LoadDataIntoGridView(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                    TextLog.TextLogging(e.Message + " --- " + e.ToString());
                }
            }
            return null;
        }
        internal static async Task<bool> InserUpdateDelete(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    string cleanCommand = query.Replace("\n", " ").Replace("\r", " ").Trim();
                    if (!string.IsNullOrWhiteSpace(cleanCommand))
                    {
                        using (SqlCommand cmd = new SqlCommand(cleanCommand, conn))
                        {
                            cmd.CommandTimeout = 120;
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                    return (false);
                }
            }
        }
    }
}