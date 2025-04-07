using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class AboutForm : XtraForm
    {
        public AboutForm()
        {
            InitializeComponent();
        }
        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
    }
}