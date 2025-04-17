using Newtonsoft.Json.Linq;
using NoktaBilgiNotificationWeb.Classes;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NoktaBilgiNotificationWeb.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFoundSync()
        {
            return View("NotFound");
        }
        public ActionResult ServerErrorSync()
        {
            return View("ServerError");
        }
        public ActionResult GeneralSync()
        {
            return View("General");
        }
        public async Task<ActionResult> NotFound()
        {
            string geo = "";   
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = Request.ServerVariables["REMOTE_ADDR"];
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync("http://ip-api.com/json/" + ip);
                    JObject json = JObject.Parse(response);
                    geo = (json["city"]?.ToString() ?? "Bilinmeyen") + ", " + (json["country"]?.ToString() ?? "Bilinmeyen");
                }
            }
            catch (Exception ex)
            {
                TextLog.TextLogging("Lokasyon bilgisi alınamadı: " + ex.Message,-1);
                geo = "Bilinmeyen";
            }
            string userAgent = Request.UserAgent ?? "Bilinmiyor";
            bool status = await SQLCrud.InserUpdateDelete("INSERT INTO WebLog (IPAdress, UserCity, UserInfo) VALUES('" + ip + "', '" + geo + "', '" + userAgent + "')",-1);
            if (!status)
                TextLog.TextLogging("Kullanıcı bilgileri kaydetme işlemi hatalı",-1);
            return View("NotFound");
        }
        public async Task<ActionResult> ServerError()
        {
            string geo = "";
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = Request.ServerVariables["REMOTE_ADDR"];
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync("http://ip-api.com/json/" + ip);
                    JObject json = JObject.Parse(response);
                    geo = (json["city"]?.ToString() ?? "Bilinmeyen") + ", " + (json["country"]?.ToString() ?? "Bilinmeyen");
                }
            }
            catch (Exception ex)
            {
                TextLog.TextLogging("Lokasyon bilgisi alınamadı: " + ex.Message,-1);
                geo = "Bilinmeyen";
            }
            string userAgent = Request.UserAgent ?? "Bilinmiyor";
            bool status = await SQLCrud.InserUpdateDelete("INSERT INTO WebLog (IPAdress, UserCity, UserInfo) VALUES('" + ip + "', '" + geo + "', '" + userAgent + "')",-1);
            if (!status)
                TextLog.TextLogging("Kullanıcı bilgileri kaydetme işlemi hatalı",-1);
            return View("ServerError");
        }
        public async Task<ActionResult> General()
        {
            ViewBag.Message = Session["Message"];
            string geo = "";
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = Request.ServerVariables["REMOTE_ADDR"];
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync("http://ip-api.com/json/" + ip);
                    JObject json = JObject.Parse(response);
                    geo = (json["city"]?.ToString() ?? "Bilinmeyen") + ", " + (json["country"]?.ToString() ?? "Bilinmeyen");
                }
            }
            catch (Exception ex)
            {
                TextLog.TextLogging("Lokasyon bilgisi alınamadı: " + ex.Message,-1);
                geo = "Bilinmeyen";
            }
            string userAgent = Request.UserAgent ?? "Bilinmiyor";
            bool status = await SQLCrud.InserUpdateDelete("INSERT INTO WebLog (IPAdress, UserCity, UserInfo) VALUES('" + ip + "', '" + geo + "', '" + userAgent + "')",-1);
            if (!status)
                TextLog.TextLogging("Kullanıcı bilgileri kaydetme işlemi hatalı",-1);
            return View("General");
        }
    }
}