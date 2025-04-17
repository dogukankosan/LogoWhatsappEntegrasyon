using System;
using System.IO;

namespace NoktaBilgiNotificationService.Classes
{
    internal class TextLog
    {
        internal static void TextLogging(string message)
        {
            try
            {
                string basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\Logs"));
                string logFilePath = Path.Combine(basePath, "ServiceLog.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch (Exception)
            {
               
            }
        }
    }
}