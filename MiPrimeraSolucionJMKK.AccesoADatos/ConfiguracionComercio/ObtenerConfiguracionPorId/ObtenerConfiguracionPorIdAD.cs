using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using System.Linq;

namespace MiPrimeraSolucionJMKK.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorId
{
    public class ObtenerConfiguracionPorIdAD : IObtenerConfiguracionPorIdAD
    {
        public ConfiguracionComercioDto Obtener(int id)
        {
            using (var contexto = new Contexto())
            {
                var configuracion = contexto.ConfiguracionComercio
                    .Where(c => c.IdConfiguracion == id)
                    .Join(contexto.Comercios,
                        config => config.IdComercio,
                        comercio => comercio.IdComercio,
                        (config, comercio) => new ConfiguracionComercioDto
                        {
                            IdConfiguracion = config.IdConfiguracion,
                            IdComercio = config.IdComercio,
                            NombreComercio = comercio.Nombre,
                            TipoConfiguracion = config.TipoConfiguracion,
                            TipoConfiguracionTexto = config.TipoConfiguracion == 1 ? "Plataforma" :
                                                     config.TipoConfiguracion == 2 ? "Externa" : "Ambas",
                            Comision = config.Comision,
                            FechaDeRegistro = config.FechaDeRegistro,
                            FechaDeModificacion = config.FechaDeModificacion,
                            Estado = config.Estado
                        })
                    .FirstOrDefault();

                return configuracion;
            }
        }
    }
}
