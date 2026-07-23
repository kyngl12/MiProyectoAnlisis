using System;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad
{
    /// <summary>
    /// Calcula, a partir de las ventas y movimientos financieros ya
    /// registrados en el sistema, el resumen esperado para la jornada.
    /// </summary>
    public interface IObtenerResumenDelDiaAD
    {
        CierreCajaDto ObtenerResumen(DateTime fecha);
    }

    /// <summary>
    /// Consulta si ya existe un cierre de caja vigente para una fecha.
    /// </summary>
    public interface IObtenerCierreCajaPorFechaAD
    {
        CierreCajaDto ObtenerPorFecha(DateTime fecha);
    }

    public interface IRegistrarCierreCajaAD
    {
        int Registrar(CierreCajaDto cierre);
    }
}
