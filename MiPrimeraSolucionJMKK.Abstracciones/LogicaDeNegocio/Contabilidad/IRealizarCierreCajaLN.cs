using System;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad
{
    public interface IRealizarCierreCajaLN
    {
        /// <summary>
        /// Obtiene el resumen (ventas/ingresos/egresos) esperado por el
        /// sistema para la fecha indicada, sin registrar ningun cierre.
        /// </summary>
        CierreCajaDto ObtenerResumenDelDia(DateTime fecha);

        /// <summary>
        /// Compara el monto contado fisicamente contra lo esperado por
        /// el sistema y registra el cierre de la jornada.
        /// </summary>
        CierreCajaDto Cerrar(DateTime fecha, decimal montoContado, string cedulaRegistro);
    }
}
