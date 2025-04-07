using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI
{
    static class Program
    {
        [STAThread]
        static  void Main()
        {
            try
            {
                string savedTheme = "";
                string processName = Process.GetCurrentProcess().ProcessName;
                var runningProcesses = Process.GetProcessesByName(processName);
                if (runningProcesses.Length > 1)
                {
                    XtraMessageBox.Show("Program zaten açık!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (IsServiceInstalled("NoktaService"))
                {  
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    DevExpress.UserSkins.BonusSkins.Register();
                    DevExpress.Skins.SkinManager.EnableFormSkins();
                 savedTheme = Properties.Settings.Default.ThemeName;
                    if (!string.IsNullOrEmpty(savedTheme))
                        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(savedTheme);
                    Application.Run(new LoginForm());
                    return;
                }
                if (!IsAdministrator())
                {
                    XtraMessageBox.Show("Servis kurulu değil. Lütfen programı yönetici olarak çalıştırın.", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string installUtilPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe";
                if (!ServiceExists("NoktaService"))
                    InstallService(installUtilPath, "Service\\NoktaBilgiNotificationService.exe");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.Skins.SkinManager.EnableFormSkins();
                 savedTheme = Properties.Settings.Default.ThemeName;
                if (!string.IsNullOrEmpty(savedTheme))
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(savedTheme);
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        private static bool IsServiceInstalled(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            return services.Any(service => service.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
        }
        private static bool ServiceExists(string serviceName)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {

                    var status = sc.Status;
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Servis kontrol edilirken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private static void InstallService(string installUtilPath, string servicePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = installUtilPath,
                Arguments = $"\"{Path.Combine(servicePath)}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode != 0 || !string.IsNullOrEmpty(error))
                    throw new Exception($"Servis kaydedilemedi.\nHata: {error}\nÇıktı: {output}");
            }
        }
    }
}