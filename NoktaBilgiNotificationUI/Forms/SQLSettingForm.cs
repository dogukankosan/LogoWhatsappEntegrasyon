using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraEditors;
using NoktaBilgiNotificationUI.Classes;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class SQLSettingForm : XtraForm
    {
        public SQLSettingForm()
        {
            InitializeComponent();
        }
        public bool homeValues = false;
        private void btn_Save_Click(object sender, EventArgs e)
        {
            bool statusDB = SQLCrud.ConnectionStringControlAdd(txt_Servername.Text, txt_UserName.Text, txt_Password.Text, txt_DatabaseName.Text);
            if (statusDB)
            {
                DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
                DataTable iis = SQLiteCrud.GetDataFromSQLite("SELECT IISPath FROM CompanySettings LIMIT 1");
                try
                {
                    string fullPath = Path.Combine(iis.Rows[0][0].ToString(), "web.config");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fullPath);
                    XmlNode node = doc.SelectSingleNode("//connectionStrings/add[@name='DBConnection']");
                    if (node != null && node.Attributes["connectionString"] != null)
                    {
                        node.Attributes["connectionString"].Value = EncryptionHelper.Decrypt(dt.Rows[0][0].ToString());
                        doc.Save(fullPath);
                    }
                }
                catch (Exception)
                {
                    
                }
                this.Hide();
                homeValues = true;
            }        
        }
        private void SQLSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!homeValues)
            {
                bool statusDB = SQLCrud.ConnectionStringControlAdd(txt_Servername.Text, txt_UserName.Text, txt_Password.Text, txt_DatabaseName.Text);
                if (!statusDB)
                    e.Cancel = true;
            }
        }
        private void SQLSettingForm_Load(object sender, EventArgs e)
        {
            DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
            if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                return;
            if (dt.Rows.Count>0)
            {
                {
                    string[] parameters = EncryptionHelper.Decrypt(dt.Rows[0][0].ToString()).Split(';');
                    List<string> resultList = new List<string>();
                    foreach (string parameter in parameters)
                    {
                        if (!string.IsNullOrEmpty(parameter))
                        {
                            string[] keyValue = parameter.Split('=');
                            string key = keyValue[0].Trim();
                            string value = keyValue.Length > 1 ? keyValue[1].Trim() : string.Empty;
                            if (key == "Server" || key == "Database" || key == "User Id" || key == "Password")
                            {
                                resultList.Add(value);
                            }
                        }
                    }
                    txt_Servername.Text = resultList[0];
                    txt_DatabaseName.Text = resultList[1];
                    txt_UserName.Text = resultList[2];
                    txt_Password.Text = resultList[3];
                }
            }
        }
    }
}