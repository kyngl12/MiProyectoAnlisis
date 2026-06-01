using MiPrimeraSolucion.AccesoADatos.Comercios.EditarComercio;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.EditarComercio;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.ObtenerComercioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.AccesoADatos.Comercios.ObtenerComercioPorId;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.RegistrarBitacora;
using System;
using System.Configuration;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.EditarComercio
{
    public class EditarComercioLN : IEditarComercioLN
    {
        IEditarComercioAD _editarComercioAD;
        IObtenerComercioPorIdAD _obtenerComercioAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public EditarComercioLN()
        {
            _editarComercioAD = new EditarComercioAD();
            _obtenerComercioAD = new ObtenerComercioPorIdAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Editar(ComercioDto elComercioAEditar)
        {
            try
            {
                var datosAntes = _obtenerComercioAD.Obtener(elComercioAEditar.IdComercio);

                elComercioAEditar.FechaDeModificacion = DateTime.Now;

                int cantidad = _editarComercioAD.Editar(elComercioAEditar);

                if (cantidad > 0)
                {
                    _bitacora.Registrar("COMERCIOS", "UPDATE", datosAntes, elComercioAEditar);
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