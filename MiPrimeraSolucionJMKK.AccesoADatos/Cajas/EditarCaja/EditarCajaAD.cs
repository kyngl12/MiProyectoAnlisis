using System.Linq;
using GestionPubRock.Abstracciones.AccesoADatos.Cajas.EditarCaja;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;


namespace GestionPubRock.AccesoADatos.Cajas.EditarCaja
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