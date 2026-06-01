using System;
using System.Linq;
using System.Configuration;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.EditarCaja;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.EditarCaja;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using MiPrimeraSolucionJMKK.AccesoADatos;
using MiPrimeraSolucionJMKK.AccesoADatos.Cajas.EditarCaja;

using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.RegistrarBitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.EditarCaja
{
    public class EditarCajaLN : IEditarCajaLN
    {
        private readonly Contexto _elContexto;
        private readonly IEditarCajaAD _editarCajaAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public EditarCajaLN()
        {
            _elContexto = new Contexto();
            _editarCajaAD = new EditarCajaAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Editar(CajaDto laCaja)
        {
            try
            {
                var cajaExistente = _elContexto.Cajas
                    .FirstOrDefault(caja => caja.IdCaja == laCaja.IdCaja);

                if (cajaExistente == null)
                    return false;

                var datosAntes = new CajaDto
                {
                    IdCaja = cajaExistente.IdCaja,
                    IdComercio = cajaExistente.IdComercio,
                    Nombre = cajaExistente.Nombre,
                    Descripcion = cajaExistente.Descripcion,
                    TelefonoSINPE = cajaExistente.TelefonoSINPE,
                    FechaDeRegistro = cajaExistente.FechaDeRegistro,
                    FechaDeModificacion = cajaExistente.FechaDeModificacion,
                    Estado = cajaExistente.Estado
                };

                string telefonoNormalizado = laCaja.TelefonoSINPE.Trim();
                string nombreNormalizado = laCaja.Nombre.Trim().ToUpper();

                bool telefonoActivoRepetido = _elContexto.Cajas.Any(caja =>
                    caja.IdCaja != laCaja.IdCaja &&
                    caja.TelefonoSINPE == telefonoNormalizado &&
                    caja.Estado);

                if (telefonoActivoRepetido)
                    return false;

                bool nombreRepetidoPorComercio = _elContexto.Cajas.Any(caja =>
                    caja.IdCaja != laCaja.IdCaja &&
                    caja.IdComercio == cajaExistente.IdComercio &&
                    caja.Nombre.Trim().ToUpper() == nombreNormalizado);

                if (nombreRepetidoPorComercio)
                    return false;

                laCaja.IdComercio = cajaExistente.IdComercio;
                laCaja.Nombre = laCaja.Nombre.Trim();
                laCaja.Descripcion = laCaja.Descripcion.Trim();
                laCaja.TelefonoSINPE = telefonoNormalizado;
                laCaja.FechaDeRegistro = cajaExistente.FechaDeRegistro;
                laCaja.FechaDeModificacion = DateTime.Now;

                int cantidad = _editarCajaAD.Editar(laCaja);

                if (cantidad > 0)
                {
                    _bitacora.Registrar("CAJAS", "UPDATE", datosAntes, laCaja);
                }

                return cantidad > 0;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("CAJAS", ex);
                throw;
            }
        }
    }
}