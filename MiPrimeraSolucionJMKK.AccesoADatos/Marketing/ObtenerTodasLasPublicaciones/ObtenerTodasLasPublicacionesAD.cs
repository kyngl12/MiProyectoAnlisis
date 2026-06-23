using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Marketing.ObtenerTodasLasPublicaciones
{
    public class ObtenerTodasLasPublicacionesAD : IObtenerTodasLasPublicacionesAD
    {
        private Contexto _elContexto;

        public ObtenerTodasLasPublicacionesAD()
        {
            _elContexto = new Contexto();
        }

        public List<MarketingDto> ObtenerTodos()
        {
            try
            {
                var publicaciones = _elContexto.Publicaciones
                    .Include("TipoContenido")
                    .Include("Estado")
                    .ToList();

                return ConvertirAListaDto(publicaciones);
            }
            catch
            {
                throw;
            }
        }

        private List<MarketingDto> ConvertirAListaDto(List<PublicacionEntidad> publicacionesEntidad)
        {
            List<MarketingDto> publicacionesDto = new List<MarketingDto>();

            foreach (var publicacion in publicacionesEntidad)
                publicacionesDto.Add(ConvertirADto(publicacion));

            return publicacionesDto;
        }

        private MarketingDto ConvertirADto(PublicacionEntidad publicacion)
        {
            return new MarketingDto
            {
                IdPublicacion = publicacion.IdPublicacion,
                Titulo = publicacion.Titulo,
                IdTipoContenido = publicacion.IdTipoContenido,
                Descripcion = publicacion.Descripcion,
                FechaInicio = publicacion.FechaInicio,
                FechaFinalizacion = publicacion.FechaFinalizacion,
                IdEstado = publicacion.IdEstado,
                Precio = publicacion.Precio,
                FechaCreacion = publicacion.FechaCreacion,

                DescripcionTipoContenido = publicacion.TipoContenido != null
                    ? publicacion.TipoContenido.Descripcion
                    : "Sin tipo",

                DescripcionEstado = publicacion.Estado != null
                    ? publicacion.Estado.Descripcion
                    : "Sin estado"
            };
        }
    }
}