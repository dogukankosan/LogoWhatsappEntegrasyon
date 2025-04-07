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
            

                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeDirectory = Path.GetDirectoryName(exePath);
                string debugDirectory = Path.GetFullPath(Path.Combine(exeDirectory, ".."));
                string logDirectory = Path.Combine(debugDirectory, "Logs");
                string logFilePath = Path.Combine(logDirectory, "ServiceLog.txt");
                Directory.CreateDirectory(logDirectory);
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");

            }
            catch (Exception)
            {

            }
        }
    }
}