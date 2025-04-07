using NoktaBilgiNotificationWeb.Classes;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NoktaBilgiNotificationWeb.Controllers
{
    public class OrderWpController : Controller
    {

        [HttpGet]
        public ActionResult Approve(string token)
        {
            TokenGenerate tokenGenerate = new TokenGenerate();
            string orderFicheNo = tokenGenerate.ValidateToken(token);
            if (orderFicheNo == null)
            {
                Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
                return RedirectToAction("General", "Error");
            }
            ViewBag.OrderFicheNo = orderFicheNo;
            ViewBag.Token = token;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ConfirmOrder(string token, string action, string reason)
        {
            TokenGenerate tokenGenerate = new TokenGenerate();
            string ficheno = tokenGenerate.ValidateToken(token);
            if (ficheno == null)
            {
                Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
                return RedirectToAction("General", "Error");
            }           
            string message = "", updateMessage = "";
            if (action == "approve")
            {
                message = $"✅ Sipariş {ficheno} başarıyla onaylandı!";
                updateMessage = "Onaylandı";
            }
            else if (action == "reject")
            {
                message = $"❌ Sipariş {ficheno} reddedildi!";
                updateMessage = $"Reddedildi: {reason}";
            }
            else
            {
                Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
                tokenGenerate.InvalidateToken(token);
                return RedirectToAction("General", "Error");
            }
            string nowStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
            bool statusDB = await SQLCrud.InserUpdateDelete("UPDATE NKT_ORFFICHELOGICALREFWP SET MESSAGEBODY = '" + updateMessage + "', MESSAGEDATE = '" + nowStr + "' WHERE FICHENO = '" + ficheno + "'");
            if (!statusDB)
            { 
                Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
                TextLog.TextLogging("NKT_ORFFICHELOGICALREFWP tablosunda update işlemi hatalı");
                tokenGenerate.InvalidateToken(token);
                return RedirectToAction("General", "Error");
            }
            DataTable dr = SQLCrud.LoadDataIntoGridView("SELECT TOP 1 TabloNo FROM TableNoAndCount WITH(NOLOCK)");
            if (dr is null)
            {
                if (dr.Rows.Count <= 0)
                {
                    Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
                    TextLog.TextLogging("TableNoAndCount okuma işlemi hatalı");
                    tokenGenerate.InvalidateToken(token);
                    return RedirectToAction("General", "Error");
                }
            }
            bool statusDBLogo = false;
            if (updateMessage == "Onaylandı")
            {
                try
                {
                     statusDBLogo = await SQLCrud.InserUpdateDelete($"UPDATE {dr.Rows[0][0].ToString()}_ORFICHE SET STATUS=3 WHERE FICHENO='" + ficheno + "' AND TRCODE=1 AND CANCELLED=0");
                }
                catch (Exception ex)
                {
                    Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
                    TextLog.TextLogging(ex.Message + " -- "+ex.ToString());
                    tokenGenerate.InvalidateToken(token);
                    return RedirectToAction("General", "Error");
                }
            }
            else
            {
                try
                {
                     statusDBLogo = await SQLCrud.InserUpdateDelete($"UPDATE {dr.Rows[0][0].ToString()}_ORFICHE SET STATUS=1 WHERE FICHENO='" + ficheno + "' AND TRCODE=1 AND CANCELLED=0");
                }
                catch (Exception ex)
                {
                    Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    tokenGenerate.InvalidateToken(token);
                    return RedirectToAction("General", "Error");
                }
            }
            if (statusDBLogo)
            {
                Session["Message"] = "Token silme işlemi başarıyla gerçekleştirildi.";
                tokenGenerate.InvalidateToken(token);
                return RedirectToAction("General", "Error");
            }
            Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
            return RedirectToAction("General", "Error");
        }
        [HttpGet]
        public ActionResult PDF(string code)
        {
            if (string.IsNullOrEmpty(code) || !code.Contains("|"))
                return RedirectToAction("General", "Error");
            string[] parts = HttpUtility.UrlDecode(code).Split('|');
            string pdf = parts[0];
            string token = parts[1];
            if (!pdf.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("General", "Error");
            TokenGenerate tg = new TokenGenerate();
            string orderFicheNo = tg.PDFValidateToken(token, Path.GetFileName(pdf));
            if (orderFicheNo == null)
            {
                Session["Message"] = "Geçersiz veya süresi dolmuş bağlantı";
                return RedirectToAction("General", "Error");
            }
            string fullPath = Path.Combine(Server.MapPath("~/PDF"), Path.GetFileName(pdf));
            if (!System.IO.File.Exists(fullPath))
                return RedirectToAction("General", "Error");
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "application/pdf");
        }
    }
}