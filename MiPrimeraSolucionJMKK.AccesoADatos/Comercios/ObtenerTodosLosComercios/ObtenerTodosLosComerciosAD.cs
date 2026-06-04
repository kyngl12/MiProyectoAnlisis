using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using System.Collections.Generic;
using System.Linq;

namespace GestionPubRock.AccesoADatos.Comercios.ObtenerTodosLosComercios
{
    public class ObtenerTodosLosComerciosAD : IObtenerTodosLosComerciosAD
    {
        Contexto _elContexto;

        public ObtenerTodosLosComerciosAD()
        {
            _elContexto = new Contexto();
        }

        public List<ComercioDto> Obtener()
        {
            // Intentar varias tablas candidatas en la BD sin modificar la base de datos.
            var posiblesTablas = new[] { "COMERCIOS", "PUBROCK_COMERCIO_TB", "PUBROCK_COMERCIOS_TB", "PUBROCK_COMERCIO" };

            foreach (var tabla in posiblesTablas)
            {
                try
                {
                    // Verificar existencia rápida
                    var existe = _elContexto.Database.SqlQuery<int>(
                        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @p0", tabla
                    ).FirstOrDefault();

                    if (existe <= 0) continue;

                    // Consulta cruda con alias para mapear a ComercioDto
                    string sql = $@"SELECT 
                            ID_COMERCIO AS IdComercio,
                            IDENTIFICACION AS Identificacion,
                            TIPO_IDENTIFICACION AS TipoIdentificacion,
                            NOMBRE AS Nombre,
                            TIPO_DE_COMERCIO AS TipoDeComercio,
                            TELEFONO AS Telefono,
                            CORREO_ELECTRONICO AS CorreoElectronico,
                            ESTADO AS Estado
                        FROM [{tabla}]";

                    var lista = _elContexto.Database.SqlQuery<ComercioDto>(sql).ToList();
                    return lista ?? new List<ComercioDto>();
                }
                catch
                {
                    // Ignorar y probar siguiente tabla candidata
                    continue;
                }
            }

            return new List<ComercioDto>();
        }
    }
}

