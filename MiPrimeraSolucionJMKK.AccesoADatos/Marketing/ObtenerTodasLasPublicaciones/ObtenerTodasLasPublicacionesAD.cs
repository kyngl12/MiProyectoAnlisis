using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Marketing.ObtenerTodasLasPublicaciones;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing;
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
                var publicaciones = _elContexto.Marketing.ToList();

                return ConvertirAListaDto(publicaciones);
            }
            catch
            {
                throw;
            }
        }

        private List<MarketingDto> ConvertirAListaDto(List<MarketingEntidad> publicacionesEntidad)
        {
            List<MarketingDto> publicacionesDto = new List<MarketingDto>();

            foreach (var publicacion in publicacionesEntidad)
            {
                publicacionesDto.Add(ConvertirADto(publicacion));
            }

            return publicacionesDto;
        }

        private MarketingDto ConvertirADto(MarketingEntidad publicacion)
        {
            return new MarketingDto
            {
                IdPublicacion = publicacion.IdPublicacion,
                Titulo = publicacion.Titulo,
                TipoContenido = publicacion.TipoContenido,
                Contenido = publicacion.Descripcion,
                FechaInicio = publicacion.FechaInicio,
                FechaFinalizacion = publicacion.FechaFinalizacion,
                IdEstado = publicacion.IdEstado,
                Precio = publicacion.Precio,
                FechaPublicacion = publicacion.FechaRegistro,
                DescripcionTipoContenido = publicacion.TipoContenido ?? "Sin tipo",
                DescripcionEstado = publicacion.IdEstado.ToString() 
            };
        }
    }
}