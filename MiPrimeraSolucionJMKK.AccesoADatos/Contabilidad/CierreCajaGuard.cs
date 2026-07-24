using System;
using System.Linq;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    /// <summary>
    /// Punto de integracion liviano entre el modulo de Ventas y el
    /// cierre de caja diario (CON-002, Escenario 1): una vez cerrada
    /// la jornada no deben registrarse nuevas ventas para esa fecha.
    /// </summary>
    public static class CierreCajaGuard
    {
        public static bool EstaLaCajaCerradaParaLaFecha(DateTime fecha)
        {
            using (var contexto = new Contexto())
            {
                return contexto.Database.SqlQuery<int?>(
                    "SELECT TOP 1 1 FROM PUBROCK_CIERRE_CAJA_TB WHERE FECHA_CIERRE = @p0 AND ID_ESTADO = 1",
                    fecha.Date
                ).FirstOrDefault() != null;
            }
        }
    }
}
