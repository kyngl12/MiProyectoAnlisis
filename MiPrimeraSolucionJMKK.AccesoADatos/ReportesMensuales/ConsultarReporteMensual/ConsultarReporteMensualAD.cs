using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.ConsultaReporteMensual;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using System.Collections.Generic;
using System.Linq;

namespace GestionPubRock.AccesoADatos.ReportesMensuales.ConsultarReporteMensual
{
    public class ConsultarReporteMensualAD : IConsultarReporteMensualAD
    {
        Contexto _elContexto;

        public ConsultarReporteMensualAD()
        {
            _elContexto = new Contexto();
        }

        public List<ComercioDto> ObtenerComercios()
        {
            List<ComercioDto> laListaDeComercios = (from c in _elContexto.Comercios
                                                    select new ComercioDto
                                                    {
                                                        IdComercio = c.IdComercio,
                                                        Nombre = c.Nombre
                                                    }).ToList();

            return laListaDeComercios;
        }

        public int ObtenerCantidadDeCajas(int idComercio)
        {
            int cantidadDeCajas = _elContexto.Cajas
                .Count(c => c.IdComercio == idComercio);

            return cantidadDeCajas;
        }

        public List<string> ObtenerTelefonosDeCajas(int idComercio)
        {
            List<string> telefonosDeCajas = _elContexto.Cajas
                .Where(c => c.IdComercio == idComercio)
                .Select(c => c.TelefonoSINPE)
                .ToList();

            return telefonosDeCajas;
        }

        public List<SINPESDto> ObtenerSINPESDelMes(List<string> telefonosDeCajas, int mes, int anio)
        {
            List<SINPESDto> losSINPES = (from s in _elContexto.SINPES
                                         where telefonosDeCajas.Contains(s.TelefonoDestinatario)
                                            && s.FechaDeRegistro.Month == mes
                                            && s.FechaDeRegistro.Year == anio
                                         select new SINPESDto
                                         {
                                             IdSinpe = s.IdSinpe,
                                             TelefonoOrigen = s.TelefonoOrigen,
                                             NombreOrigen = s.NombreOrigen,
                                             TelefonoDestinatario = s.TelefonoDestinatario,
                                             NombreDestinatario = s.NombreDestinatario,
                                             Monto = s.Monto,
                                             FechaDeRegistro = s.FechaDeRegistro,
                                             Descripcion = s.Descripcion,
                                             Estado = s.Estado
                                         }).ToList();

            return losSINPES;
        }

        public int ObtenerComision(int idComercio)
        {
            return 0;
        }
    }
}

