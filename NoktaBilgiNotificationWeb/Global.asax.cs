using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoktaBilgiNotificationWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            var httpException = ex as HttpException;
            int httpCode = httpException?.GetHttpCode() ?? 500;
            string action = "GeneralSync";
            if (httpCode == 404)
                action = "NotFoundSync";
            else if (httpCode == 500)
                action = "ServerErrorSync";
            Response.Clear();
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;
            RouteData routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;
            IController controller = new NoktaBilgiNotificationWeb.Controllers.ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            Response.End();
        }
    }
}