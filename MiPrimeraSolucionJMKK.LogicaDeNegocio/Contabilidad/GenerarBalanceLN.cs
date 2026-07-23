using System;
using GestionPubRock.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.LogicaDeNegocio.Contabilidad
{
    public class GenerarBalanceLN : IGenerarBalanceLN
    {
        private readonly IGenerarBalanceAD _generarBalanceAD;

        public GenerarBalanceLN() : this(new GenerarBalanceAD()) { }

        public GenerarBalanceLN(IGenerarBalanceAD generarBalanceAD)
        {
            _generarBalanceAD = generarBalanceAD;
        }

        public BalanceDto Generar(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                if (fechaInicio == default(DateTime) || fechaFin == default(DateTime))
                    throw new ArgumentException("Debe indicar la fecha de inicio y la fecha de finalización.");

                // Escenario 5: fecha de inicio mayor a la fecha de finalización.
                if (fechaInicio.Date > fechaFin.Date)
                    throw new ArgumentException("La fecha de inicio no puede ser mayor a la fecha de finalización.");

                // Escenario 2: periodo sin datos contables.
                if (!_generarBalanceAD.ExistenDatosEnRango(fechaInicio, fechaFin))
                    throw new InvalidOperationException("No existen datos contables suficientes para generar un balance en este rango.");

                // Escenario 1 (calculo) y Escenario 3 (perdida, EsPerdida = true).
                return _generarBalanceAD.Generar(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.File.AppendAllText(
                        AppDomain.CurrentDomain.BaseDirectory + "App_Data\\errors.log",
                        DateTime.Now.ToString("s") + " - " + ex.ToString() + Environment.NewLine);
                }
                catch { }

                throw;
            }
        }
    }
}
