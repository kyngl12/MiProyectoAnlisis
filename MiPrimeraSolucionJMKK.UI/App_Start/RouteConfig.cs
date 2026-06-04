using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MiPrimeraSolucionJMKK.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // Rutas deshabilitadas para funcionalidades fuera del alcance del Sprint
            // Mantener las controladoras pero evitar acceso por URL directo
            // Ejemplo: Cajas, SINPES, ReportesMensuales, Bitacora, ConfiguracionComercio
            // routes.MapRoute("Cajas", "Cajas/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            // routes.MapRoute("SINPES", "SINPES/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            // routes.MapRoute("ReportesMensuales", "ReportesMensuales/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            // routes.MapRoute("Bitacora", "Bitacora/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            // routes.MapRoute("ConfiguracionComercio", "ConfiguracionComercio/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
