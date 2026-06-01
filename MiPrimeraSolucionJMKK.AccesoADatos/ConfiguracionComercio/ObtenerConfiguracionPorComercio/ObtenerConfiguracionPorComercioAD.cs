using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using System.Collections.Generic;
using System.Linq;

namespace MiPrimeraSolucionJMKK.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorComercio
{
    public class ObtenerConfiguracionPorComercioAD : IObtenerConfiguracionPorComercioAD
    {
        public List<ConfiguracionComercioDto> Obtener(int idComercio)
        {
            using (var contexto = new Contexto())
            {
                var configuraciones = contexto.ConfiguracionComercio
                    .Where(c => c.IdComercio == idComercio)
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
                    .ToList();

                return configuraciones;
            }
        }
    }
}
