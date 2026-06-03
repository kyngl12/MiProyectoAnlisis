using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Comercios.ValidarIdentificacionComercio;
using GestionPubRock.AccesoADatos.SINPES.RegistrarSINPES;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPES.RegistrarSINPES;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPES.ValidacionCajaTelefonoAD;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.SINPES.RegistrarSINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Configuration;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.SINPES.RegistrarSINPES
{
    public class RegistrarSINPESLN : IRegistrarSINPESLN
    {
        private readonly IRegistrarSINPESAD _registrarSINPESAD;
        private readonly IRegistrarBitacoraLN _bitacora;
        IValidacionCajaTelefonoSINPESAD _validacionCaja;
        public RegistrarSINPESLN()
        {

            _registrarSINPESAD = new RegistrarSINPESAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Registrar(SINPESDto elPagoSINPESAGuardar)
        {
            try
            {
                if (string.IsNullOrEmpty(elPagoSINPESAGuardar.TelefonoOrigen))
                    return false;

                if (string.IsNullOrEmpty(elPagoSINPESAGuardar.NombreOrigen))
                    return false;

                if (string.IsNullOrEmpty(elPagoSINPESAGuardar.TelefonoDestinatario))
                    return false;

                if (string.IsNullOrEmpty(elPagoSINPESAGuardar.NombreDestinatario))
                    return false;

                if (elPagoSINPESAGuardar.Monto <= 0)
                    return false;

                elPagoSINPESAGuardar.TelefonoOrigen = elPagoSINPESAGuardar.TelefonoOrigen.Trim();
                elPagoSINPESAGuardar.NombreOrigen = elPagoSINPESAGuardar.NombreOrigen.Trim();
                elPagoSINPESAGuardar.TelefonoDestinatario = elPagoSINPESAGuardar.TelefonoDestinatario.Trim();
                elPagoSINPESAGuardar.NombreDestinatario = elPagoSINPESAGuardar.NombreDestinatario.Trim();

                elPagoSINPESAGuardar.FechaDeRegistro = DateTime.Now;
                elPagoSINPESAGuardar.Estado = false;

                bool cajaValida = _validacionCaja.CajaValidaParaSINPE(elPagoSINPESAGuardar.TelefonoDestinatario);

                if (!cajaValida)
                {
                    return false;
                }

                int cantidad = _registrarSINPESAD
                    .RegistrarSINPES(elPagoSINPESAGuardar);

                if (cantidad > 0)
                {
                    _bitacora.Registrar("SINPE", "INSERT", null, elPagoSINPESAGuardar);
                }

                return cantidad > 0;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("SINPE", ex);

                string errorReal = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;

                throw new Exception("ERROR REAL EN SINPE: " + errorReal);
            }
        }
    }
}