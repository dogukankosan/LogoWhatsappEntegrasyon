using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI.Business
{
    internal class CustomerLogic
    {
        internal static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 10 || phoneNumber.Length > 15)
            {
                XtraMessageBox.Show("Telefon numarası geçersiz. Format: +[ülke kodu][numara] şeklinde olmalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Regex regex = new Regex(@"^\+\d{10,14}$");
            if (!regex.IsMatch(phoneNumber))
            {
                XtraMessageBox.Show("Telefon numarası formatı yanlış. Format: +[ülke kodu][numara] (örn: +905331234567)", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        internal static bool IsValidMailAdres(string senderEmail)
        {
            if (string.IsNullOrEmpty(senderEmail))
            {
                XtraMessageBox.Show("E-Mail Boş Geçilemez", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (senderEmail.Length < 5)
            {
                XtraMessageBox.Show("E-Mail 5 Haneden Daha Az Olamaz", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (senderEmail.Length > 50)
            {
                XtraMessageBox.Show("E-Mail 50 Haneden Fazla Olamaz", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!senderEmail.Contains("@"))
            {
                XtraMessageBox.Show("E-Mail İçinde @ İşareti Bulunmalıdır", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!senderEmail.Contains("."))
            {
                XtraMessageBox.Show("E-Mail Kutusunun İçinde . İşareti Bulunmalıdır", "Hatalı E-Mail Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}