using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Comercios.RegistrarComercio;
using GestionPubRock.AccesoADatos.Comercios.ValidarIdentificacionComercio;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.RegstrarComercio;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.RegistrarComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Configuration;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.RegistrarComercio
{
    public class RegistrarComercioLN : IRegistrarComercioLN
    {
        IRegistrarComercioAD _registrarComercioAD;
        ValidarCajaTelefonoSINPESAD _validarIdentificacionComercioAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public RegistrarComercioLN()
        {
            _registrarComercioAD = new RegistrarComercioAD();

            _validarIdentificacionComercioAD = new ValidarCajaTelefonoSINPESAD();


            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Registrar(ComercioDto elComercioAGuardar)
        {
            try
            {
                if (string.IsNullOrEmpty(elComercioAGuardar.Identificacion)) return false;
                if (string.IsNullOrEmpty(elComercioAGuardar.Nombre)) return false;
                if (string.IsNullOrEmpty(elComercioAGuardar.Telefono)) return false;
                if (string.IsNullOrEmpty(elComercioAGuardar.CorreoElectronico)) return false;
                if (string.IsNullOrEmpty(elComercioAGuardar.Direccion)) return false;
                if (elComercioAGuardar.TipoIdentificacion < 1 || elComercioAGuardar.TipoIdentificacion > 2) return false;
                if (elComercioAGuardar.TipoDeComercio < 1 || elComercioAGuardar.TipoDeComercio > 4) return false;

                bool existeIdentificacion = _validarIdentificacionComercioAD.ExisteIdentificacion(elComercioAGuardar.Identificacion);

                if (existeIdentificacion)
                {
                    return false;
                }

                elComercioAGuardar.FechaDeRegistro = DateTime.Now;
                elComercioAGuardar.FechaDeModificacion = null;
                elComercioAGuardar.Estado = true;

                int cantidad = _registrarComercioAD.RegistrarComercio(elComercioAGuardar);

                if (cantidad > 0)
                {
                    _bitacora.Registrar("COMERCIOS", "INSERT", null, elComercioAGuardar);
                }

                return cantidad > 0;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("COMERCIOS", ex);
                throw;
            }
        }
    }
}