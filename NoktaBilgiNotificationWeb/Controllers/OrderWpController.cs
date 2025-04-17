using NoktaBilgiNotificationWeb.Classes;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NoktaBilgiNotificationWeb.Controllers
{
    public class OrderWpController : Controller
    {
        public ActionResult Approve(string token)
        {
            int customerID = -1;
            try
            {
                string decodedCode = HttpUtility.UrlDecode(token);
                string[] parts = decodedCode.Split('|');
                if (parts.Length != 4)
                {
                    Session["Message"] = "Geçersiz Token Formatı";
                    TextLog.TextLogging("Geçersiz Token Formatı", -1);
                    return RedirectToAction("General", "Error");
                }
                string rawTokenEncrypted = parts[0];
                string passwordEncrypted = parts[1];
                string ficheNoDecrypted = parts[2];
                string orderIdDecrypted = parts[3];
                if (string.IsNullOrWhiteSpace(ficheNoDecrypted) || string.IsNullOrWhiteSpace(orderIdDecrypted))
                {
                    Session["Message"] = "Sipariş Numarası boş olamaz";
                    TextLog.TextLogging("Boş Sipariş Numarası", -1);
                    return RedirectToAction("General", "Error");
                }
                TokenGenerate tokenGenerate = new TokenGenerate();
                customerID = tokenGenerate.GetCustomerIdIfValid(rawTokenEncrypted, passwordEncrypted);
                if (customerID == -1)
                {
                    Session["Message"] = "Geçersiz Yetkili Müşteri";
                    TextLog.TextLogging("Geçersiz Yetkili Müşteri", customerID);
                    return RedirectToAction("General", "Error");
                }
                DataTable CustomerList = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 * FROM Orders WITH (NOLOCK) WHERE OrderFicheNo='" + ficheNoDecrypted + "'  AND CustomerID=" + customerID + " AND OrderFicheID=" + orderIdDecrypted + " ORDER BY ID DESC", customerID);
                if (CustomerList.Rows.Count <= 0)
                {
                    Session["Message"] = "İlgili İşleminiz Bulunamamıştır.";
                    TextLog.TextLogging("İlgili İşleminiz Bulunamamıştır.", customerID);
                    return RedirectToAction("General", "Error");
                }
                try
                {
                    object msgObj = CustomerList.Rows[0]["MessageBody"];
                    if (msgObj != DBNull.Value && !string.IsNullOrWhiteSpace(msgObj.ToString()))
                    {
                        if (msgObj.ToString().Contains("Red"))
                            Session["Message"] = $"Bu Siparişi Daha Önceden Red Verdiniz: {msgObj}";
                        else
                            Session["Message"] = $"Bu Siparişi Daha Önceden Onay Verdiniz: {msgObj}";
                        return RedirectToAction("General", "Error");
                    }
                }
                catch (Exception ex)
                {
                    Session["Message"] = "Geçersiz Yetkili Müşteri";
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString(), customerID);
                    return RedirectToAction("General", "Error");
                }
                ViewBag.orderFicheNo = ficheNoDecrypted;
                ViewBag.OrderID = orderIdDecrypted;
                ViewBag.Token = rawTokenEncrypted;
                ViewBag.Password = passwordEncrypted;

                return View();
            }
            catch (Exception ex)
            {
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString(), customerID);
                Session["Message"] = "Geçersiz Yetkili Müşteri";
                return RedirectToAction("General", "Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ConfirmOrder(string token, string password, string ficheNo, string OrderID, string action, string reason)
        {
            TokenGenerate tokenGenerate = new TokenGenerate();
            int customerID = tokenGenerate.GetCustomerIdIfValid(token, password);
            if (customerID == -1)
            {
                Session["Message"] = "Geçersiz Yetkili Müşteri";
                TextLog.TextLogging("Geçersiz Yetkili Müşteri", customerID);
                return RedirectToAction("General", "Error");
            }
            if (string.IsNullOrEmpty(ficheNo) || string.IsNullOrEmpty(OrderID) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(password))
            {
                Session["Message"] = "Geçersiz Yetkili Müşteri";
                TextLog.TextLogging("Geçersiz Yetkili Müşteri", customerID);
                return RedirectToAction("General", "Error");
            }
            DataTable CustomerList = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 * FROM Orders WITH (NOLOCK) WHERE OrderFicheNo='"+ficheNo+"'  AND CustomerID=" + customerID + " AND OrderFicheID=" + OrderID + " ORDER BY ID DESC",customerID);
            if (CustomerList.Rows.Count <= 0)
            {
                Session["Message"] = "İlgili İşleminiz Bulunamamıştır.";
                TextLog.TextLogging("İlgili İşleminiz Bulunamamıştır.", customerID);
                return RedirectToAction("General", "Error");
            }
            string message = "", updateMessage = "";
            if (action == "approve")
            {
                message = $"✅ Sipariş {ficheNo} başarıyla onaylandı!";
                updateMessage = "Onaylandı";
            }
            else if (action == "reject")
            {
                message = $"❌ Sipariş {ficheNo} reddedildi!";
                updateMessage = $"Reddedildi: {reason}";
            }
            else
            {
                Session["Message"] = "Hatalı Onay Süreci.";
                TextLog.TextLogging("Hatalı Onay Süreci.", customerID);
                return RedirectToAction("General", "Error");
            }
            string nowStr = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            bool statusDB = await SQLCrud.InserUpdateDelete("UPDATE Orders SET MessageBody = '" + updateMessage + "', ResponseDate = '" + nowStr + "' WHERE ID="+CustomerList.Rows[0]["ID"]+"",customerID);
            if (!statusDB)
            {
                Session["Message"] = "Hatalı İşlem.";
                TextLog.TextLogging("Orders TABLOSUNDA HATALI İŞLEM", customerID);
                return RedirectToAction("General", "Error");
            }
            Session["Message"] = "Sipariş İşlemi Tamamlandı";
            return RedirectToAction("General", "Error");
        }
        [HttpGet]
        public async Task<ActionResult> PDF(string code)
        {
            string decodedCode = HttpUtility.UrlDecode(code);
            string[] parts = decodedCode.Split('|');
            if (parts.Length != 4)
            {
                Session["Message"] = "Geçersiz Token Formatı";
                TextLog.TextLogging("Geçersiz Token Formatı", -1);
                return RedirectToAction("General", "Error");
            }
            string rawTokenEncrypted = parts[0];
            string passwordEncrypted = parts[1];
            string ficheNoDecrypted = parts[2];
            string orderIdDecrypted = parts[3];
            TokenGenerate tokenGenerate = new TokenGenerate();
            int customerID = tokenGenerate.GetCustomerIdIfValid(rawTokenEncrypted, passwordEncrypted);
            if (customerID == -1)
            {
                Session["Message"] = "Geçersiz Yetkili Müşteri";
                TextLog.TextLogging("Geçersiz Yetkili Müşteri", customerID);
                return RedirectToAction("General", "Error");
            }
            DataTable dt = SQLCrud.LoadDataIntoGridView(
                "SELECT TOP 1 PDFFile FROM PDFS WHERE CustomerID=" + customerID +
                " AND OrderFicheID=" + orderIdDecrypted +
                " AND OrderFicheNo='" + ficheNoDecrypted + "' ORDER BY ID DESC"
            ,customerID);
            if (dt == null || dt.Rows.Count == 0 || dt.Rows[0][0] == DBNull.Value)
            {
                Session["Message"] = "PDF Bulunamadı";
                TextLog.TextLogging("PDF BULUNAMADI", customerID);
                return RedirectToAction("General", "Error");
            }
            try
            {
                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    Session["Message"] = "PDF Bulunamadı";
                    TextLog.TextLogging("PDF BULUNAMADI", customerID);
                    return RedirectToAction("General", "Error");
                }
            }
            catch (Exception ex)
            {
                Session["Message"] = "PDF Bulunamadı";
                TextLog.TextLogging(ex.Message + " -- " + ex.ToString(), customerID);
                return RedirectToAction("General", "Error");
            }
            byte[] pdfBytes = (byte[])dt.Rows[0][0];
            Response.AppendHeader("Content-Disposition", $"inline; filename=order_{orderIdDecrypted}.pdf");
            return File(pdfBytes, "application/pdf");
        }
    }
}