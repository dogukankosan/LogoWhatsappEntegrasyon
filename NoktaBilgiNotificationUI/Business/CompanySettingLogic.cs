using DevExpress.XtraEditors;
using System.Linq;
using System.Windows.Forms;

namespace NoktaBilgiNotificationUI.Business
{
    internal class CompanySettingLogic
    {
        internal static bool CompanyControl(TextEdit companyCode, TextEdit companyPeriod, TextEdit companyName, TextEdit managerName,PictureEdit image,TextEdit firmaNo,TextEdit domainURL,TextEdit iisPath)
        {
            #region CompanyCode
            if (string.IsNullOrEmpty(companyCode.Text.Trim()))
            {
                XtraMessageBox.Show("Şirket kodu kutusu boş geçilemez", "Hatalı Şirket Kodu Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                companyCode.Focus();
                return false;
            }
            else if (!companyCode.Text.Trim().All(char.IsDigit))
            {
                XtraMessageBox.Show("Şirket kodu sadece sayısal karakterlerden oluşmalıdır", "Hatalı Şirket Kodu Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                companyCode.Focus();
                return false;
            }
            else if (companyCode.Text.Trim().Length > 10)
            {
                XtraMessageBox.Show("Şirket kodu 10 karakterden fazla olamaz", "Hatalı Şirket Kodu Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                companyCode.Focus();
                return false;
            }
            #endregion

            #region CompanyPeriod
            if (string.IsNullOrEmpty(companyPeriod.Text.Trim()))
            {
                XtraMessageBox.Show("Şirket periyod kutusu boş geçilemez", "Hatalı Şirket Periyod Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                companyPeriod.Focus();
                return false;
            }
            else if (!companyPeriod.Text.Trim().All(char.IsDigit))
            {
                XtraMessageBox.Show("Şirket periyod sadece sayısal karakterlerden oluşmalıdır", "Hatalı Şirket Periyod Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                companyPeriod.Focus();
                return false;
            }
            else if (companyPeriod.Text.Trim().Length > 10)
            {
                XtraMessageBox.Show("Şirket periyod 10 karakterden fazla olamaz", "Hatalı Şirket Periyod Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                companyPeriod.Focus();
                return false;
            }
            #endregion

            #region FirmaNo
            if (string.IsNullOrEmpty(firmaNo.Text.Trim()))
            {
                XtraMessageBox.Show("Şirket firma no kutusu boş geçilemez", "Hatalı Şirket Firma No Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                firmaNo.Focus();
                return false;
            }
            string input = firmaNo.Text.Trim();

            if (input.Count(c => c == '-') > 1 ||
                (input.Contains('-') && !input.StartsWith("-")) ||
                !input.All(c => char.IsDigit(c) || c == '-') ||
                !input.Any(char.IsDigit))
            {
                XtraMessageBox.Show("Şirket firma no sadece rakam ve isteğe bağlı baştaki '-' karakterinden oluşmalı",
                    "Hatalı Şirket Firma No Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                firmaNo.Focus();
                return false;
            }
            else if (firmaNo.Text.Trim().Length > 10)
            {
                XtraMessageBox.Show("Şirket firma no 10 karakterden fazla olamaz", "Hatalı Şirket Firma No Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                firmaNo.Focus();
                return false;
            }
            #endregion

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

            #region ManagerName
            if (string.IsNullOrEmpty(managerName.Text.Trim()))
            {
                XtraMessageBox.Show("Yönetici adı soyadı boş geçilemez", "Hatalı Yönetici Adı Soyadı Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                managerName.Focus();
                return false;
            }
            else if (managerName.Text.Trim().Length > 100)
            {
                XtraMessageBox.Show("Yönetici adı soyadı 100 karakterden fazla olamaz", "Hatalı Yönetici Adı Soyadı Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                managerName.Focus();
                return false;
            }
            #endregion

            #region DomainURL
            if (string.IsNullOrEmpty(domainURL.Text.Trim()))
            {
                XtraMessageBox.Show("Domain URL kutusu boş geçilemez", "Hatalı Domain URL Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                domainURL.Focus();
                return false;
            }
            else if (domainURL.Text.Trim().Length > 200)
            {
                XtraMessageBox.Show("Domain URL 200 karakterden fazla olamaz", "Hatalı Domain URL Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                domainURL.Focus();
                return false;
            }
            else if (!domainURL.Text.Trim().Contains("http"))
            {
                XtraMessageBox.Show("Domain URL'de htpp içermelidir", "Hatalı Domain URL Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                domainURL.Focus();
                return false;
            }
            else if (!domainURL.Text.Trim().Contains(":"))
            {
                XtraMessageBox.Show("Domain URL'de : karakteri içermelidir", "Hatalı Domain URL Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                domainURL.Focus();
                return false;
            }
            #endregion

            #region CompanyImage
            if (image.Image is null)
            {
                XtraMessageBox.Show("Resim kutusu boş geçilemez", "Hatalı Resim Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                image.Focus();
                return false;
            }
            #endregion

            #region IISPath
            if (string.IsNullOrEmpty(iisPath.Text.Trim()))
            {
                XtraMessageBox.Show("IIS Yolu boş geçilemez", "Hatalı IIS Yolu Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iisPath.Focus();
                return false;
            }
            else if (iisPath.Text.Trim().Length > 500)
            {
                XtraMessageBox.Show("IIS Yolu 500 karakterden fazla olamaz", "Hatalı IIS Yolu Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iisPath.Focus();
                return false;
            }
            else if(!iisPath.Text.Trim().Contains(@"\"))
            {
                XtraMessageBox.Show(@"IIS Yolu \ karakteri içermelidir", "Hatalı IIS Yolu Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iisPath.Focus();
                return false;
            }
            #endregion
            return true;
        }
    }
}