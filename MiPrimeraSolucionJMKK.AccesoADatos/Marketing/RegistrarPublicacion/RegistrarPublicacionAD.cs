using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Marketing.RegistrarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Marketing.RegistrarPublicacion
{
    public class RegistrarPublicacionAD : IRegistrarPublicacionAD
    {
        private Contexto _elContexto;

        public RegistrarPublicacionAD()
        {
            _elContexto = new Contexto();
        }

        public int Registrar(MarketingDto publicacion)
        {
            try
            {
                var existeTitulo = _elContexto.Database.SqlQuery<int?>(
                    "SELECT 1 FROM PUBROCK_MARKETING_TB WHERE TITULO = @p0",
                    publicacion.Titulo
                ).FirstOrDefault();

                if (existeTitulo != null) return -1;

                MarketingEntidad publicacionAGuardar = ConvertirAEntidad(publicacion);

                // asegurar que IdEstado sea válido (usar 'Activo' si no se proporcionó)
                if (publicacionAGuardar.IdEstado <= 0)
                {
                    publicacionAGuardar.IdEstado = _elContexto.Database.SqlQuery<int?>("SELECT TOP 1 ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Activo'").FirstOrDefault() ?? 1;
                }

                _elContexto.Marketing.Add(publicacionAGuardar);
                int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();

                return cantidadDeRegistrosAlmacenados;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return -99;
            }
            catch
            {
                throw;
            }
        }

        private MarketingEntidad ConvertirAEntidad(MarketingDto publicacion)
        {
            return new MarketingEntidad
            {
                Titulo = publicacion.Titulo,
                TipoContenido = publicacion.TipoContenido, 
                Descripcion = publicacion.Contenido,
                FechaInicio = publicacion.FechaInicio,
                FechaFinalizacion = publicacion.FechaFinalizacion,
                IdEstado = publicacion.IdEstado,
                Precio = publicacion.Precio,
                FechaRegistro = DateTime.Now 
            };
        }
    }
}
