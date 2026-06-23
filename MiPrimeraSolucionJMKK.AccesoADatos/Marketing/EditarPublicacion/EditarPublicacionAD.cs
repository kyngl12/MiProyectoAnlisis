using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Marketing.EditarPublicacion
{
    public class EditarPublicacionAD : IEditarPublicacionAD
    {
        private Contexto _elContexto;

        public EditarPublicacionAD()
        {
            _elContexto = new Contexto();
        }

        public int Editar(MarketingDto publicacion)
        {
            try
            {
                var publicacionExistente = _elContexto.Publicaciones
                    .FirstOrDefault(p => p.IdPublicacion == publicacion.IdPublicacion);

                if (publicacionExistente == null)
                    return -1; 

                bool tituloDuplicado = _elContexto.Publicaciones
                    .Any(p => p.Titulo == publicacion.Titulo && p.IdPublicacion != publicacionExistente.IdPublicacion);

                if (tituloDuplicado) return -2;

                publicacionExistente.Titulo = publicacion.Titulo;
                publicacionExistente.IdTipoContenido = publicacion.IdTipoContenido;
                publicacionExistente.Descripcion = publicacion.Descripcion;
                publicacionExistente.FechaInicio = publicacion.FechaInicio;
                publicacionExistente.FechaFinalizacion = publicacion.FechaFinalizacion;
                publicacionExistente.IdEstado = publicacion.IdEstado;
                publicacionExistente.Precio = publicacion.Precio;

                return _elContexto.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
