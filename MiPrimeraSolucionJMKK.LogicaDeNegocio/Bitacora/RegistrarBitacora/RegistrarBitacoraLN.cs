using System;
using Newtonsoft.Json;
using MiPrimeraSolucionJMKK.Abstacciones.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora
{
    public class RegistrarBitacoraLN : IRegistrarBitacoraLN
    {
        private readonly IRegistrarBitacoraAD _bitacoraAD;

        public RegistrarBitacoraLN(IRegistrarBitacoraAD bitacoraAD)
        {
            _bitacoraAD = bitacoraAD;
        }

        public void Registrar(string tabla, string tipo, object datosAntes = null, object datosDespues = null)
        {
            var evento = new BitacoraEventoDto
            {
                TablaDeEvento = tabla,
                TipoDeEvento = tipo,
                FechaDeEvento = DateTime.Now,
                DescripcionDeEvento = $"Evento {tipo} en {tabla}",
                StackTrace = "", // ⚠️ requerido por la BD
                DatosAnteriores = datosAntes != null ? JsonConvert.SerializeObject(datosAntes) : null,
                DatosPosteriores = datosDespues != null ? JsonConvert.SerializeObject(datosDespues) : null
            };

            _bitacoraAD.RegistrarEvento(evento);
        }

        public void RegistrarError(string tabla, Exception ex)
        {
            var evento = new BitacoraEventoDto
            {
                TablaDeEvento = tabla,
                TipoDeEvento = "Error",
                FechaDeEvento = DateTime.Now,
                DescripcionDeEvento = ex.Message,
                StackTrace = ex.StackTrace ?? "", 
                DatosAnteriores = null,
                DatosPosteriores = null
            };

            _bitacoraAD.RegistrarEvento(evento);
        }
    }
}