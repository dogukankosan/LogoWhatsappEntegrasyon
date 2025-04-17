using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NoktaBilgiNotificationWeb.Attributes
{
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private static readonly Dictionary<string, List<DateTime>> requestLog = new Dictionary<string, List<DateTime>> ();
        public int Limit { get; set; } = 100;
        public int Minutes { get; set; } = 10;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string ip = filterContext.HttpContext.Request.UserHostAddress;
            DateTime now = DateTime.UtcNow;
            if (!requestLog.ContainsKey(ip))
                requestLog[ip] = new List<DateTime>();
            requestLog[ip] = requestLog[ip].Where(t => (now - t).TotalMinutes <= Minutes).ToList();
            requestLog[ip].Add(now);
            if (requestLog[ip].Count > Limit)
            {
                filterContext.Result = new ContentResult
                {
                    Content = "⚠️ Çok fazla istek gönderdiniz. Lütfen birkaç dakika sonra tekrar deneyin.",
                    ContentType = "text/plain"
                };
            }
        }
    }
}