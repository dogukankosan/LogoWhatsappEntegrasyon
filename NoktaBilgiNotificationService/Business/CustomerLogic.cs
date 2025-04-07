using NoktaBilgiNotificationService.Classes;
using System.Text.RegularExpressions;

namespace NoktaBilgiNotificationService.Business
{
    internal class CustomerLogic
    {
        internal static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 10 || phoneNumber.Length > 15)
            {
                TextLog.TextLogging("Telefon numarası formatı yanlış. Format: +905xxxxxxxxx");
                return false;
            }
            Regex regex = new Regex(@"^\+\d{10,14}$");
            if (!regex.IsMatch(phoneNumber))
            {
                TextLog.TextLogging("Telefon numarası formatı yanlış. Format: +905xxxxxxxxx");
                return false;
            }
            return true;
        }
        internal static bool IsValidMailAdres(string senderEmail)
        {
            if (string.IsNullOrEmpty(senderEmail))
            {
                TextLog.TextLogging("E-Mail Boş Geçilemez");
                return false;
            }
            else if (senderEmail.Length < 5)
            {
                 TextLog.TextLogging("E-Mail 5 Haneden Daha Az Olamaz");
                return false;
            }
            else if (senderEmail.Length > 50)
            {
                 TextLog.TextLogging("E-Mail 50 Haneden Fazla Olamaz");
                return false;
            }
            else if (!senderEmail.Contains("@"))
            {
                 TextLog.TextLogging("E-Mail İçinde @ İşareti Bulunmalıdır");
                return false;
            }
            else if (!senderEmail.Contains("."))
            {
                 TextLog.TextLogging("E-Mail Kutusunun İçinde . İşareti Bulunmalıdır");
                return false;
            }
            return true;
        }
    }
}