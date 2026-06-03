using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.RegistrarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using System;
using System.Linq;

namespace GestionPubRock.AccesoADatos.ConfiguracionComercio.RegistrarConfiguracion
{
    public class RegistrarConfiguracionAD : IRegistrarConfiguracionAD
    {
        public bool Registrar(ConfiguracionComercioDto configuracion)
        {
            using (var contexto = new Contexto())
            {
                var existeConfiguracion = contexto.ConfiguracionComercio
                    .Any(c => c.IdComercio == configuracion.IdComercio);

                if (existeConfiguracion)
                {
                    return false;
                }

                var nuevaConfiguracion = new ConfiguracionComercioEntidad
                {
                    IdComercio = configuracion.IdComercio,
                    TipoConfiguracion = configuracion.TipoConfiguracion,
                    Comision = configuracion.Comision,
                    FechaDeRegistro = DateTime.Now,
                    Estado = true
                };

                contexto.ConfiguracionComercio.Add(nuevaConfiguracion);
                contexto.SaveChanges();
                return true;
            }
        }
    }
}
