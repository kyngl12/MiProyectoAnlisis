using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.EditarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using System;
using System.Linq;

namespace GestionPubRock.AccesoADatos.ConfiguracionComercio.EditarConfiguracion
{
    public class EditarConfiguracionAD : IEditarConfiguracionAD
    {
        public bool Editar(ConfiguracionComercioDto configuracion)
        {
            using (var contexto = new Contexto())
            {
                var configuracionExistente = contexto.ConfiguracionComercio
                    .FirstOrDefault(c => c.IdConfiguracion == configuracion.IdConfiguracion);

                if (configuracionExistente == null)
                {
                    return false;
                }

                configuracionExistente.TipoConfiguracion = configuracion.TipoConfiguracion;
                configuracionExistente.Comision = configuracion.Comision;
                configuracionExistente.Estado = configuracion.Estado;
                configuracionExistente.FechaDeModificacion = DateTime.Now;

                contexto.SaveChanges();
                return true;
            }
        }
    }
}
