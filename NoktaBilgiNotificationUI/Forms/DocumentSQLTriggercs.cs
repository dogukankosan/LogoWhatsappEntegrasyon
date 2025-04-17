using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NoktaBilgiNotificationUI.Forms
{
    public partial class DocumentSQLTriggercs : XtraForm
    {
        public DocumentSQLTriggercs()
        {
            InitializeComponent();
        }
        private void DocumentSQLTriggercs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
    }
}