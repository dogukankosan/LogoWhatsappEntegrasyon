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
        internal static DataTable LoadDataIntoGridView(string query,int customerID)
        {
            if (ContainsDangerousSql(query))
            {
                TextLog.TextLogging($"[SQL Injection Engellendi] => Zararlı içerik bulundu: {query}", customerID);
                return null;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            da.Fill(dt);
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString(), customerID);
                }
            }
            return null;
        }
        internal static async Task<bool> InserUpdateDelete(string query,int customerID)
        {
            if (ContainsDangerousSql(query))
            {
                TextLog.TextLogging($"[SQL Injection Engellendi] => Zararlı içerik bulundu: {query}", customerID);
                return false;
            }
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
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString(), customerID);
                    return (false);
                }
            }
        }
        private static bool ContainsDangerousSql(string query)
        {
            string[] blacklist = {
    "char(", "nchar(", "varchar(", "alter ", "begin ", "cast(", "create ",
    "cursor ", "declare ", "delete ", "drop ", "exec(", "execute ", "fetch ",
    "kill ", "sys.", "sysobjects", "syscolumns", "union ", "information_schema"
};
            foreach (var dangerous in blacklist)
            {
                if (query.IndexOf(dangerous, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }
    }
}