// Bitacora removida: no registrar en BD central desde Global.asax
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Helpers;
using System.Security.Claims;

namespace MiPrimeraSolucionJMKK.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        protected void Application_Error()
        {
            // Bitacora deshabilitada: registrar excepcion en archivo errors.log
            try
            {
                Exception exception = Server.GetLastError();
                if (exception != null)
                {
                    try { System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\errors.log", DateTime.Now.ToString("s") + " - " + exception.ToString() + Environment.NewLine); } catch { }
                }
            }
            catch { }
        }
    }
}
