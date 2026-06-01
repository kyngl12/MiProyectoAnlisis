using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerCajaPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;


namespace MiPrimeraSolucionJMKK.AccesoADatos.Cajas.ObtenerCajaPorId
{
    public class ObtenerCajaPorIdAD : IObtenerCajaPorIdAD
    {
        private Contexto _elContexto;

        public ObtenerCajaPorIdAD()
        {
            _elContexto = new Contexto();
        }

        public CajaDto Obtener(int idCaja)
        {
            CajaDto laCaja = (from laCajaEnBaseDeDatos in _elContexto.Cajas
                              where laCajaEnBaseDeDatos.IdCaja == idCaja
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
                              }).FirstOrDefault();

            return laCaja;
        }
    }
}