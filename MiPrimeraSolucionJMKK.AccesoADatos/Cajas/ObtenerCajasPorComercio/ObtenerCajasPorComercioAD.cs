using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerCajasPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;



namespace MiPrimeraSolucionJMKK.AccesoADatos.Cajas.ObtenerCajasPorComercio
{
    public class ObtenerCajasPorComercioAD : IObtenerCajasPorComercioAD
    {
        private Contexto _elContexto;

        public ObtenerCajasPorComercioAD()
        {
            _elContexto = new Contexto();
        }

        public List<CajaDto> Obtener(int idComercio)
        {
            List<CajaDto> laListaDeCajas = (from laCajaEnBaseDeDatos in _elContexto.Cajas
                                            where laCajaEnBaseDeDatos.IdComercio == idComercio
                                            select new CajaDto
                                            {
                                                IdCaja = laCajaEnBaseDeDatos.IdCaja,
                                                IdComercio = laCajaEnBaseDeDatos.IdComercio,
                                                Nombre = laCajaEnBaseDeDatos.Nombre,
                                                Descripcion = laCajaEnBaseDeDatos.Descripcion,
                                                TelefonoSINPE = laCajaEnBaseDeDatos.TelefonoSINPE,
                                                FechaDeRegistro = laCajaEnBaseDeDatos.FechaDeRegistro,
                                                FechaDeModificacion = laCajaEnBaseDeDatos.FechaDeModificacion,
                                                Estado = laCajaEnBaseDeDatos.Estado
                                            }).ToList();

            return laListaDeCajas;
        }
    }
}