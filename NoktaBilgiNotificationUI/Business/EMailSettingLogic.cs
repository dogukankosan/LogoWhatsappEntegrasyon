using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI.Business
{
    internal class EMailSettingLogic
    {
        internal static bool EmailControl(TextEdit senderEmail, TextEdit recipientEmail, TextEdit senderPassword, TextEdit server, TextEdit port)
        {
            #region SenderMailControl
            if (string.IsNullOrEmpty(senderEmail.Text))
            {
                XtraMessageBox.Show("E-Mail Kutusu Boş Geçilemez", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderEmail.Focus();
                return false;
            }
            else if (senderEmail.Text.Length < 5)
            {
                XtraMessageBox.Show("E-Mail Kutusu 5 Haneden Daha Az Olamaz", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderEmail.Focus();
                return false;
            }
            else if (senderEmail.Text.Length > 50)
            {
                XtraMessageBox.Show("E-Mail Kutusu 50 Haneden Fazla Olamaz", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderEmail.Focus();
                return false;
            }
            else if (!senderEmail.Text.Contains("@"))
            {
                XtraMessageBox.Show("E-Mail Kutusunun İçinde @ İşareti Bulunmalıdır", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderEmail.Focus();
                return false;
            }
            else if (!senderEmail.Text.Contains("."))
            {
                XtraMessageBox.Show("E-Mail Kutusunun İçinde . İşareti Bulunmalıdır", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderEmail.Focus();
                return false;
            }
            #endregion
            #region RecipientEmailControl
            if (string.IsNullOrEmpty(recipientEmail.Text))
            {
                XtraMessageBox.Show("E-Mail Kutusu Boş Geçilemez", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                recipientEmail.Focus();
                return false;
            }
            else if (recipientEmail.Text.Length < 5)
            {
                XtraMessageBox.Show("E-Mail Kutusu 5 Haneden Daha Az Olamaz", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                recipientEmail.Focus();
                return false;
            }
            else if (recipientEmail.Text.Length > 50)
            {
                XtraMessageBox.Show("E-Mail Kutusu 50 Haneden Fazla Olamaz", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                recipientEmail.Focus();
                return false;
            }
            else if (!recipientEmail.Text.Contains("@"))
            {
                XtraMessageBox.Show("E-Mail Kutusunun İçinde @ İşareti Bulunmalıdır", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                recipientEmail.Focus();
                return false;
            }
            else if (!recipientEmail.Text.Contains("."))
            {
                XtraMessageBox.Show("E-Mail Kutusunun İçinde . İşareti Bulunmalıdır", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                recipientEmail.Focus();
                return false;
            }
            #endregion
            #region PasswordControl
            if (string.IsNullOrEmpty(senderPassword.Text))
            {
                XtraMessageBox.Show("Şifre Kutusu Boş Geçilemez", "Hatalı Şifre Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderPassword.Focus();
                return false;
            }
            else if (senderPassword.Text.Length < 3)
            {
                XtraMessageBox.Show("Şifre Kutusu 3 Karakterden Daha Az Olamaz", "Hatalı Şifre Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderPassword.Focus();
                return false;
            }
            else if (senderPassword.Text.Length > 50)
            {
                XtraMessageBox.Show("Şifre Kutusu 50 Karakterden Fazla Olamaz", "Hatalı Şifre Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderPassword.Focus();
                return false;
            }
            else if (senderPassword.Text.Length > 50)
            {
                XtraMessageBox.Show("Şifre Kutusu 50 Karakterden Fazla Olamaz", "Hatalı Şifre Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                senderPassword.Focus();
                return false;
            }
            #endregion
            #region ServerControl
            if (string.IsNullOrEmpty(server.Text))
            {
                XtraMessageBox.Show("Server Kutusu Boş Geçilemez", "Hatalı Server Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                server.Focus();
                return false;
            }
            else if (server.Text.Length < 5)
            {
                XtraMessageBox.Show("Server Kutusu 5 Haneden Daha Az Olamaz", "Hatalı Server Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                server.Focus();
                return false;
            }
            else if (server.Text.Length > 30)
            {
                XtraMessageBox.Show("Server Kutusu 30 Haneden Fazla Olamaz", "Hatalı Server Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                server.Focus();
                return false;
            }
            else if (!server.Text.Contains("."))
            {
                XtraMessageBox.Show("Server Kutusu İçinde . İşareti İçermelidir", "Hatalı Server Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                server.Focus();
                return false;
            }
            #endregion
            #region PortControl
            if (string.IsNullOrEmpty(port.Text))
            {
                XtraMessageBox.Show("Port Kutusu Boş Geçilemez", "Hatalı Port Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                port.Focus();
                return false;
            }
            else if (port.Text.Length > 4)
            {
                XtraMessageBox.Show("Port Kutusu 4 Haneden Fazla Olamaz", "Hatalı Port Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                port.Focus();
                return false;
            }
            else if (port.Text.Length > 4)
            {
                XtraMessageBox.Show("Port Kutusu 4 Haneden Fazla Olamaz", "Hatalı Port Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                port.Focus();
                return false;
            }
            else if (!(int.TryParse(port.Text, out _)))
            {
                XtraMessageBox.Show("Port Kutusu Sadece Rakamlardan Oluşmalıdır", "Hatalı Port Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                port.Focus();
                return false;
            }
            else
            {
                return true;
            }
            #endregion
        }
        internal static bool SaveMailSignature(TextEdit txtName, TextEdit txtPhone, TextEdit txtAddress, TextEdit txtWebsite, PictureEdit picture)
        {
            #region NameControl
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                XtraMessageBox.Show("Şirket Adı boş geçilemez", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }
            else if (txtName.Text.Length > 100)
            {
                XtraMessageBox.Show("Şirket Adı 100 karakterden fazla olamaz", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }
            #endregion

            #region PhoneControl
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                XtraMessageBox.Show("Telefon alanı boş geçilemez", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }
            else if (txtPhone.Text.Length > 20)
            {
                XtraMessageBox.Show("Telefon 20 karakterden fazla olamaz", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text.Trim(), @"^\+90\d{10}$"))
            {
                XtraMessageBox.Show("Telefon numarası +90 ile başlamalı ve 13 hane olmalıdır. (örn: +905321112233)", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }
            #endregion

            #region AddressControl
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                XtraMessageBox.Show("Adres alanı boş geçilemez", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return false;
            }
            else if (txtAddress.Text.Length > 300)
            {
                XtraMessageBox.Show("Adres 300 karakterden fazla olamaz", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return false;
            }
            #endregion

            #region WebsiteControl
            if (string.IsNullOrWhiteSpace(txtWebsite.Text))
            {
                XtraMessageBox.Show("Web site alanı boş geçilemez", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtWebsite.Focus();
                return false;
            }
            string websiteInput = txtWebsite.Text.Trim();
            string fullUrlForCheck = websiteInput.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? websiteInput
                : "https://" + websiteInput;

            // Uri geçerli mi kontrol
            if (!Uri.IsWellFormedUriString(fullUrlForCheck, UriKind.Absolute))
            {
                XtraMessageBox.Show("Geçerli bir web site adresi giriniz (örn: www.site.com.tr)", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtWebsite.Focus();
                return false;
            }
            #endregion

            #region LogoControl
            if (picture.Image == null)
            {
                XtraMessageBox.Show("Şirket logosu seçilmelidir", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                picture.Focus();
                return false;
            }
            #endregion

            return true;
        }
    }
}