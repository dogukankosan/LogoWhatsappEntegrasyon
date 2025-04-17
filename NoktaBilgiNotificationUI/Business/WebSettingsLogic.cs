using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI.Business
{
    internal class WebSettingsLogic
    {
        internal static bool ValidateWebInputs( TextEdit webUrl,TextEdit companyName)
        {
            #region CompanyName
            if (string.IsNullOrEmpty(companyName.Text.Trim()))
            {
                XtraMessageBox.Show("Şirket adı kutusu boş geçilemez", "Hatalı Şirket Adı Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                companyName.Focus();
                return false;
            }
            else if (companyName.Text.Trim().Length > 250)
            {
                XtraMessageBox.Show("Şirket adı 250 karakterden fazla olamaz", "Hatalı Şirket Adı Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                companyName.Focus();
                return false;
            }
            #endregion
            #region WebURL
            if (string.IsNullOrWhiteSpace(webUrl.Text.Trim()))
            {
                XtraMessageBox.Show("Web Url Boş Geçilemez.", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                webUrl.Focus();
                return false;
            }
            if (!Uri.IsWellFormedUriString(webUrl.Text.Trim(), UriKind.Absolute) || !(webUrl.Text.Trim().StartsWith("http://") || webUrl.Text.Trim().StartsWith("https://")))
            {
                XtraMessageBox.Show("Geçerli bir URL giriniz (http veya https ile başlamalı).", "URL Hatalı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                webUrl.Focus();
                return false;
            }
            #endregion   
            return true;
        }
    }
}