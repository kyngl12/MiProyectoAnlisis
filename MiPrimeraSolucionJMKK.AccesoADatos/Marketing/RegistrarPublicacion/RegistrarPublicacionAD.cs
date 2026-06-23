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
                    "SELECT 1 FROM PUBROCK_PUBLICACION_TB WHERE TITULO = @p0",
                    publicacion.Titulo
                ).FirstOrDefault();

                if (existeTitulo != null) return -1;

                PublicacionEntidad publicacionAGuardar = ConvertirAEntidad(publicacion);

                _elContexto.Publicaciones.Add(publicacionAGuardar);
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

        private PublicacionEntidad ConvertirAEntidad(MarketingDto publicacion)
        {
            return new PublicacionEntidad
            {
                Titulo = publicacion.Titulo,
                IdTipoContenido = publicacion.IdTipoContenido,
                Descripcion = publicacion.Descripcion,
                FechaInicio = publicacion.FechaInicio,
                FechaFinalizacion = publicacion.FechaFinalizacion,
                IdEstado = publicacion.IdEstado,
                Precio = publicacion.Precio,
                FechaCreacion = DateTime.Now
            };
        }
    }
}
