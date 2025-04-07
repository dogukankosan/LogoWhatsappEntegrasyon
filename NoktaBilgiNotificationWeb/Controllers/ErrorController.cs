using Newtonsoft.Json.Linq;
using NoktaBilgiNotificationWeb.Classes;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NoktaBilgiNotificationWeb.Controllers
{
    public class ErrorController : Controller
    {
        public async Task<ActionResult> NotFound()
        {
            string geo = "";   
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = Request.ServerVariables["REMOTE_ADDR"];
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync("http://ip-api.com/json/"+ip+"");
                JObject json = JObject.Parse(response);
                geo = json["city"] + ", " + json["country"];
            }
            string userAgent = Request.UserAgent ?? "Bilinmiyor";
            bool status=await SQLCrud.InserUpdateDelete("INSERT INTO WebIPAdresLog (IPADRES, USERCITY, USERINFO) VALUES('" + ip + "', '" + geo + "', '" + userAgent + "')");
            if (!status)
                TextLog.TextLogging("Kullanıcı bilgileri kaydetme işlemi hatalı");
            return View("NotFound");
        }
        public async Task<ActionResult> ServerError()
        {
            string geo = "";
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = Request.ServerVariables["REMOTE_ADDR"];
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync("http://ip-api.com/json/" + ip + "");
                JObject json = JObject.Parse(response);
                geo = json["city"] + ", " + json["country"];
            }
            string userAgent = Request.UserAgent ?? "Bilinmiyor";
            bool status = await SQLCrud.InserUpdateDelete("INSERT INTO WebIPAdresLog (IPADRES, USERCITY, USERINFO) VALUES('" + ip + "', '" + geo + "', '" + userAgent + "')");
            if (!status)
                TextLog.TextLogging("Kullanıcı bilgileri kaydetme işlemi hatalı");
            return View("ServerError");
        }
        public async Task<ActionResult> General()
        {
            ViewBag.Message = Session["Message"];
            string geo = "";
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = Request.ServerVariables["REMOTE_ADDR"];
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync("http://ip-api.com/json/" + ip + "");
                JObject json = JObject.Parse(response);
                geo = json["city"] + ", " + json["country"];
            }
            string userAgent = Request.UserAgent ?? "Bilinmiyor";
            bool status = await SQLCrud.InserUpdateDelete("INSERT INTO WebIPAdresLog (IPADRES, USERCITY, USERINFO) VALUES('" + ip + "', '" + geo + "', '" + userAgent + "')");
            if (!status)
                TextLog.TextLogging("Kullanıcı bilgileri kaydetme işlemi hatalı");
            return View("General");
        }
    }
}