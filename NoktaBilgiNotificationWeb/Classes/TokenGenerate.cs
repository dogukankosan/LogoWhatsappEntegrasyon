using System;
using System.Data;
namespace NoktaBilgiNotificationWeb.Classes
{
    internal class TokenGenerate
    {
        internal int GetCustomerIdIfValid(string token, string password,int customerID=-1)
        {
            DataTable dt = SQLCrud.LoadDataIntoGridView(
                $"SELECT ID FROM Customers WITH (NOLOCK) WHERE CustomerToken = '{token}' AND CustomerPassword = '{password}'",customerID);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["ID"]);
            return -1;
        }
    }
}