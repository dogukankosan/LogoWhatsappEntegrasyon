using System;
using System.Data;
using System.ServiceProcess;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class HomeForm : XtraForm
    {
        public HomeForm()
        {
            InitializeComponent();
        }
        public string username = "";
        private void btn_MailSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is MailSettingForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            MailSettingForm childForm = new MailSettingForm();
            childForm.MdiParent = this;
            childForm.Show();
        }
        private void btn_CompanySettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is CompanySettingForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            CompanySettingForm childForm = new CompanySettingForm();
            childForm.MdiParent = this;
            childForm.Show();
        }
        private void btn_SQLSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is SQLSettingForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            SQLSettingForm childForm = new SQLSettingForm();
            childForm.MdiParent = this;
            childForm.homeValues = true;
            childForm.Show();
        }
        private void btn_UserForm_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is UserSettingForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            UserSettingForm childForm = new UserSettingForm();
            childForm.MdiParent = this;
            childForm.Show();
        }
        private void HomeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void HomeForm_Load(object sender, EventArgs e)
        {
            ribbonPage3.Visible = false;
            if (!(string.IsNullOrEmpty(username)) && username == "admin")
            ribbonPage3.Visible = true;
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            if (dt.Rows.Count > 0)
            {
                DataTable dr = SQLCrud.LoadDataIntoGridView("SELECT 1", dt.Rows[0][0].ToString());
                if (!(dr is null))
                {
                    return;
                }
            }
            if (dt.Rows.Count <= 0)
            {
                SQLSettingForm fr = new SQLSettingForm();
                fr.ShowDialog();
            }
            else
            {
                DataTable dr = SQLCrud.LoadDataIntoGridView("SELECT 1", dt.Rows[0][0].ToString());
                if (dr is null)
                {
                    SQLSettingForm fr = new SQLSettingForm();
                    fr.ShowDialog();
                }
            }
        }
        private void btn_OrderList_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is OrderListForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            OrderListForm childForm = new OrderListForm();
            childForm.MdiParent = this;
            childForm.Show();
        }
     
        private void btn_About_ItemClick(object sender, ItemClickEventArgs e)
        {
            AboutForm childForm = new AboutForm();
            childForm.ShowDialog();
        }
        private void btn_SQLiteCommand_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is SQLiteCommandForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            SQLiteCommandForm childForm = new SQLiteCommandForm();
            childForm.MdiParent = this;
            childForm.Show();
        }
        private void btn_MailAndWpCount_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is MailAndWpCountForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            MailAndWpCountForm childForm = new MailAndWpCountForm();
            childForm.MdiParent = this;
            childForm.Show();
        }
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is WpSetttingsForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            WpSetttingsForm childForm = new WpSetttingsForm();
            childForm.MdiParent = this;
            childForm.Show();
        }
        private void btn_CompanyFilter_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is CompanyFilterForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            CompanyFilterForm childForm = new CompanyFilterForm();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void btn_MailAndWpKontor_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataTable remoteSQL = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString,WebToken,WebPassword FROM WebSettings LIMIT 1");
            if (remoteSQL.Rows.Count <= 0)
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(remoteSQL.Rows[0][0].ToString()))
            {
                XtraMessageBox.Show("Önce Web Servis Ayarlarınızı Giriniz", "Hatalı Veritabanı Okuma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable dt = SQLCrud.LoadDataIntoGridView("SELECT WpCount,MailCount FROM Customers WITH (NOLOCK) WHERE CustomerToken='" + remoteSQL.Rows[0][1].ToString() + "' AND CustomerPassword='"+ remoteSQL.Rows[0][2].ToString() + "'", remoteSQL.Rows[0][0].ToString());
            if (!(dt is null))
            {
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        {
                            XtraMessageBox.Show($"Kalan Whatsapp Kontör Sayınız: {EncryptionHelper.Decrypt(dt.Rows[0][0].ToString())} \n Kalan Mail Kontör Sayınız: {EncryptionHelper.Decrypt(dt.Rows[0][1].ToString())}", "Kontör Sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Kontör Sayısı Hatalı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void skinRibbonGalleryBarItem1_GalleryItemClick_1(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            string selectedTheme = e.Item.Caption;
            Properties.Settings.Default.ThemeName = selectedTheme;
            Properties.Settings.Default.Save();
        }
        private void btn_StartService_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (ServiceController sc = new ServiceController("NoktaService"))
                {
                    if (sc.Status == ServiceControllerStatus.Stopped || sc.Status == ServiceControllerStatus.Paused)
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        XtraMessageBox.Show("Servis Başlatıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("Servis zaten çalışıyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Servis başlatılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void btn_StopService_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (ServiceController sc = new ServiceController("NoktaService"))
                {
                    if (sc.Status == ServiceControllerStatus.Running || sc.Status == ServiceControllerStatus.StartPending)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        XtraMessageBox.Show("Servis Durduruldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("Servis zaten durdurulmuş.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Servis durdurulurken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btn_CustomerMailAndPhone_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is CustomerTelAndMailForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            CustomerTelAndMailForm childForm = new CustomerTelAndMailForm();
            childForm.MdiParent = this;
            childForm.Show();
        }

 
        private void btn_WebSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is WebServiceSettings)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            WebServiceSettings childForm = new WebServiceSettings();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void btn_WebLog_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is WebLogForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            WebLogForm childForm = new WebLogForm();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void btn_UILog_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is UILogForm)
                {
                    openForm.BringToFront();
                    return;
                }
            }
            UILogForm childForm = new UILogForm();
            childForm.MdiParent = this;
            childForm.Show();
        }
    }
}