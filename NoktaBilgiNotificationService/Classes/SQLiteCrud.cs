using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
namespace NoktaBilgiNotificationService.Classes
{
    internal class SQLiteCrud
    {
        private static string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private static string exeDir = Path.GetDirectoryName(exePath);
        private static string dbPath = Path.Combine(exeDir, "..", "Database", "SettingsDB.db");
        private static string dbPathFile = Path.GetFullPath(dbPath); // ".." kısmını temizler
        private static string connectionString = $"Data Source={dbPathFile};Version=3;";
        internal static DataTable GetDataFromSQLite(string query)
        {
            DataTable dataTable = new DataTable();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message +" ------- "+ex.ToString());
                    return null;
                }
                connection.Close();
            }
            return dataTable;
        }
        internal static async Task<bool>  InserUpdateDelete(string query)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message+" ---  "+ex.ToString());
                    return false;
                }
            }
        }
    }
}