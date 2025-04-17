using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Business;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class WebServiceSettings : XtraForm
    {
        public WebServiceSettings()
        {
            InitializeComponent();
        }
        private void WebServiceSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (!(WebSettingsLogic.ValidateWebInputs( txt_WebURL,txt_CompanyName)))
                return;
            bool statusDB =await SQLCrud.ConnectionStringControlAddWeb(txt_Servername.Text.Trim(), txt_UserName.Text.Trim(), txt_Password.Text.Trim(), txt_DatabaseName.Text.Trim(),txt_WebURL.Text.Trim(), txt_CompanyName);
            if (statusDB)
                this.Hide();
        }
        private void WebServiceSettings_Load(object sender, EventArgs e)
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString,WebPassword,WebToken,WebAdres,CompanyName FROM WebSettings LIMIT 1");
            if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
            {
                string[] parameters = EncryptionHelper.Decrypt(dt.Rows[0][0].ToString()).Split(';');
                List<string> resultList = new List<string>();
                foreach (string parameter in parameters)
                {
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        int index = parameter.IndexOf('=');
                        if (index > -1)
                        {
                            string key = parameter.Substring(0, index).Trim();
                            string value = parameter.Substring(index + 1).Trim();
                            if (key == "Server" || key == "Database" || key == "User Id" || key == "Password")
                                resultList.Add(value);
                        }
                    }
                }
                if (resultList.Count >= 4)
                {
                    txt_Servername.Text = resultList[0];
                    txt_DatabaseName.Text = resultList[1];
                    txt_UserName.Text = resultList[2];
                    txt_Password.Text = resultList[3];
                }
                txt_WebURL.Text = dt.Rows[0]["WebAdres"].ToString();
                try
                {
                    if (!(string.IsNullOrEmpty(dt.Rows[0]["CompanyName"].ToString())))
                    {
                        txt_CompanyName.Enabled = false;
                        txt_CompanyName.Text = dt.Rows[0]["CompanyName"].ToString();
                    }       
                }
                catch (Exception)
                {
                    txt_CompanyName.Enabled = false;
                }
            }
        }
    }
}