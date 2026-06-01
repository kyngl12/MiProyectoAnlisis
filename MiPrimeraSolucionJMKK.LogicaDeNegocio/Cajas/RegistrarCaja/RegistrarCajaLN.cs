using System;
using System.Linq;
using System.Configuration;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.RegistrarCaja;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.RegistrarCaja;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using MiPrimeraSolucionJMKK.AccesoADatos;
using MiPrimeraSolucionJMKK.AccesoADatos.Cajas.RegistrarCaja;

using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.RegistrarBitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.RegistrarCaja
{
    public class RegistrarCajaLN : IRegistrarCajaLN
    {
        private readonly Contexto _elContexto;
        private readonly IRegistrarCajaAD _registrarCajaAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public RegistrarCajaLN()
        {
            _elContexto = new Contexto();
            _registrarCajaAD = new RegistrarCajaAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Registrar(CajaDto laCaja)
        {
            try
            {
                bool existeComercio = _elContexto.Comercios
                    .Any(comercio => comercio.IdComercio == laCaja.IdComercio);

                if (!existeComercio)
                    return false;

                bool telefonoActivoRepetido = _elContexto.Cajas.Any(caja =>
                    caja.TelefonoSINPE == laCaja.TelefonoSINPE &&
                    caja.Estado);

                if (telefonoActivoRepetido)
                    return false;

                string nombreNormalizado = laCaja.Nombre.Trim().ToUpper();

                bool nombreRepetidoPorComercio = _elContexto.Cajas.Any(caja =>
                    caja.IdComercio == laCaja.IdComercio &&
                    caja.Nombre.Trim().ToUpper() == nombreNormalizado);

                if (nombreRepetidoPorComercio)
                    return false;

                laCaja.Nombre = laCaja.Nombre.Trim();
                laCaja.Descripcion = laCaja.Descripcion.Trim();
                laCaja.TelefonoSINPE = laCaja.TelefonoSINPE.Trim();
                laCaja.FechaDeRegistro = DateTime.Now;
                laCaja.FechaDeModificacion = null;
                laCaja.Estado = true;

                int cantidad = _registrarCajaAD.RegistrarCaja(laCaja);

                if (cantidad > 0)
                {
                    _bitacora.Registrar("CAJAS", "INSERT", null, laCaja);
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