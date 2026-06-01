using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPESApi;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPESApi;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.AccesoADatos.SINPESApi
{
    public class SinpeApiAD : ISINPESApiAD
    {
        Contexto _elContexto;

        public SinpeApiAD()
        {
            _elContexto = new Contexto();
        }

        public List<SINPESDto> ConsultarSinpesPorTelefonoCaja(string telefonoCaja)
        {
            List<SINPESDto> laListaDeSinpes = (from elSinpeEnBD in _elContexto.SINPES
                                               where elSinpeEnBD.TelefonoDestinatario == telefonoCaja
                                               select new SINPESDto
                                               {
                                                   IdSinpe = elSinpeEnBD.IdSinpe,
                                                   TelefonoOrigen = elSinpeEnBD.TelefonoOrigen,
                                                   NombreOrigen = elSinpeEnBD.NombreOrigen,
                                                   TelefonoDestinatario = elSinpeEnBD.TelefonoDestinatario,
                                                   NombreDestinatario = elSinpeEnBD.NombreDestinatario,
                                                   Monto = elSinpeEnBD.Monto,
                                                   Descripcion = elSinpeEnBD.Descripcion,
                                                   FechaDeRegistro = elSinpeEnBD.FechaDeRegistro,
                                                   Estado = elSinpeEnBD.Estado
                                               }).ToList();

            return laListaDeSinpes;
        }

        public RespuestaAPIDto SincronizarSinpe(int idSinpe)
        {
            RespuestaAPIDto respuesta = new RespuestaAPIDto();

            var elSinpe = _elContexto.SINPES.FirstOrDefault(x => x.IdSinpe == idSinpe);

            if (elSinpe == null)
            {
                respuesta.EsValido = false;
                respuesta.Mensaje = "No se encontró el SINPE.";
                return respuesta;
            }

            elSinpe.Estado = true;

            _elContexto.SaveChanges();

            respuesta.EsValido = true;
            respuesta.Mensaje = "SINPE sincronizado correctamente.";

            return respuesta;
        }

        public RespuestaAPIDto RecibirSinpe(SINPESDto modelo)
        {
            RespuestaAPIDto respuesta = new RespuestaAPIDto();

            try
            {
                SINPESEntidad nuevoSinpe = new SINPESEntidad
                {
                    TelefonoOrigen = modelo.TelefonoOrigen,
                    NombreOrigen = modelo.NombreOrigen,
                    TelefonoDestinatario = modelo.TelefonoDestinatario,
                    NombreDestinatario = modelo.NombreDestinatario,
                    Monto = modelo.Monto,
                    Descripcion = modelo.Descripcion,
                    FechaDeRegistro = modelo.FechaDeRegistro == default(DateTime)
                                        ? DateTime.Now
                                        : modelo.FechaDeRegistro,
                    Estado = false
                };

                _elContexto.SINPES.Add(nuevoSinpe);
                _elContexto.SaveChanges();

                respuesta.EsValido = true;
                respuesta.Mensaje = "SINPE recibido correctamente.";
            }
            catch (Exception ex)
            {
                respuesta.EsValido = false;
                respuesta.Mensaje = "Error al registrar SINPE: " + ex.Message;
            }

            return respuesta;
        }
    }
}
