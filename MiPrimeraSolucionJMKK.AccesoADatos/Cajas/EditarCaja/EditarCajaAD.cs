using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.EditarCaja;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;


namespace MiPrimeraSolucionJMKK.AccesoADatos.Cajas.EditarCaja
{
    public class EditarCajaAD : IEditarCajaAD
    {
        private Contexto _elContexto;

        public EditarCajaAD()
        {
            _elContexto = new Contexto();
        }

        public int Editar(CajaDto laCaja)
        {
            CajasEntidad laCajaEnBaseDeDatos = _elContexto.Cajas
                .Where(cajaBuscada => cajaBuscada.IdCaja == laCaja.IdCaja)
                .FirstOrDefault();

            if (laCajaEnBaseDeDatos != null)
            {
                laCajaEnBaseDeDatos.Nombre = laCaja.Nombre;
                laCajaEnBaseDeDatos.Descripcion = laCaja.Descripcion;
                laCajaEnBaseDeDatos.TelefonoSINPE = laCaja.TelefonoSINPE;
                laCajaEnBaseDeDatos.FechaDeModificacion = laCaja.FechaDeModificacion;
                laCajaEnBaseDeDatos.Estado = laCaja.Estado;

                return _elContexto.SaveChanges();
            }

            return 0;
        }
    }
}