using System;

namespace NoktaBilgiNotificationWeb.Classes
{
    internal class TextLog
    {
        internal async static void TextLogging(string message,int customerID)
        {
            try
            {
                await SQLCrud.InserUpdateDelete("INSERT INTO Logs (CustomerID,LogDetails) VALUES (" + customerID + ",'" + message + "')",customerID);
            }
            catch (Exception)
            {

            }
        }
    }
}