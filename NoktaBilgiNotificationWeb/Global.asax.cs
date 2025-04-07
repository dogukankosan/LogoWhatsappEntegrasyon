using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoktaBilgiNotificationWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();   
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            Response.Clear();
            HttpException httpException = ex as HttpException;
            RouteData routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            if (httpException == null)
            {
                routeData.Values["action"] = "General";
            }
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values["action"] = "NotFound";
                        break;
                    case 500:
                        routeData.Values["action"] = "ServerError";
                        break;
                    default:
                        routeData.Values["action"] = "General";
                        break;
                }
            }
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;
            IController errorController = new Controllers.ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}