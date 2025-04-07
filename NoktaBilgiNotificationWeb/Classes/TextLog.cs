using System;
using System.IO;
using System.Web;

namespace NoktaBilgiNotificationWeb.Classes
{
    public class TextLog
    {

        private static readonly string logFilePath = HttpContext.Current != null
            ? HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt")
            : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog.txt");
        public static void TextLogging(string message)
        {
            try
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
            }
            catch
            {

            }
        }
    }
}