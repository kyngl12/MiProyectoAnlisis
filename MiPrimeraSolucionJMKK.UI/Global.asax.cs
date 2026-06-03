using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstacciones.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
        }

        protected void Application_Error()
        {
            try
            {
                Exception exception = Server.GetLastError();

                if (exception != null)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["Contexto"].ConnectionString;
                    IRegistrarBitacoraAD bitacoraAD = new RegistrarBitacoraAD(connectionString);
                    IRegistrarBitacoraLN bitacoraLN = new RegistrarBitacoraLN(bitacoraAD);

                    bitacoraLN.RegistrarError("Sistema", exception);
                }
            }
            catch
            {
            }
        }
    }
}
