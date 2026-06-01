using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerSINPESPorTelefonoCaja;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Cajas.ObtenerSINPESPorTelefonoCaja
{
    public class ObtenerSINPESPorTelefonoCajaAD : IObtenerSINPESPorTelefonoCajaAD
    {
        private Contexto _elContexto;

        public ObtenerSINPESPorTelefonoCajaAD()
        {
            _elContexto = new Contexto();
        }

        public List<SINPESDto> Obtener(string telefonoSINPE)
        {
            var laListaDeSINPES = (from elSINPEEnBaseDeDatos in _elContexto.SINPES
                                   where elSINPEEnBaseDeDatos.TelefonoDestinatario == telefonoSINPE
                                   orderby elSINPEEnBaseDeDatos.FechaDeRegistro descending
                                   select new SINPESDto
                                   {
                                       IdSinpe = elSINPEEnBaseDeDatos.IdSinpe,
                                       TelefonoOrigen = elSINPEEnBaseDeDatos.TelefonoOrigen,
                                       NombreOrigen = elSINPEEnBaseDeDatos.NombreOrigen,
                                       TelefonoDestinatario = elSINPEEnBaseDeDatos.TelefonoDestinatario,
                                       NombreDestinatario = elSINPEEnBaseDeDatos.NombreDestinatario,
                                       Monto = elSINPEEnBaseDeDatos.Monto,
                                       FechaDeRegistro = elSINPEEnBaseDeDatos.FechaDeRegistro,
                                       Descripcion = elSINPEEnBaseDeDatos.Descripcion,
                                       Estado = elSINPEEnBaseDeDatos.Estado
                                   }).ToList();

            return laListaDeSINPES;
        }
    }
}