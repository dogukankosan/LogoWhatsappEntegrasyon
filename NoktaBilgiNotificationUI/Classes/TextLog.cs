using System;
using System.IO;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI.Classes
{
    internal class TextLog
    {
        internal static void TextLogging(string message)
        {
            try
            {
                string logFilePath = $"{Application.StartupPath}\\Logs\\UILog.txt";
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
            }
            catch (Exception)
            {

            }
        }
    }
}