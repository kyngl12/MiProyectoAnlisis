using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Marketing.EditarPublicacion;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Marketing.EditarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.EditarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Marketing.EditarPublicacion
{
    public class EditarPublicacionLN : IEditarPublicacionLN
    {
        private IEditarPublicacionAD _editarPublicacionAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public EditarPublicacionLN()
        {
            _editarPublicacionAD = new EditarPublicacionAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Editar(MarketingDto publicacion)
        {
            try
            {
                var camposFaltantes = new System.Collections.Generic.List<string>();

                if (publicacion.IdPublicacion <= 0) camposFaltantes.Add("Id Publicación");
                if (string.IsNullOrWhiteSpace(publicacion.Titulo)) camposFaltantes.Add("Título");
                if (string.IsNullOrWhiteSpace(publicacion.TipoContenido))
                    camposFaltantes.Add("Tipo de Contenido");
                if (string.IsNullOrWhiteSpace(publicacion.Descripcion)) camposFaltantes.Add("Descripción");
                if (publicacion.FechaInicio == default(DateTime)) camposFaltantes.Add("Fecha de Inicio");
                if (publicacion.FechaFinalizacion == default(DateTime)) camposFaltantes.Add("Fecha de Finalización");
                if (publicacion.IdEstado <= 0) camposFaltantes.Add("Estado");

                if (camposFaltantes.Count > 0)
                    throw new System.ArgumentException("Campos obligatorios en blanco: " + string.Join(", ", camposFaltantes));

                if (publicacion.FechaInicio > publicacion.FechaFinalizacion)
                    throw new System.ArgumentException("La fecha de inicio no puede ser mayor a la fecha de finalización");

                if (publicacion.TipoContenido == "Menu" &&
    (!publicacion.Precio.HasValue || publicacion.Precio < 0))
                    throw new System.ArgumentException("El precio es obligatorio y debe ser mayor o igual a cero para menús");

                int cantidad = _editarPublicacionAD.Editar(publicacion);

                if (cantidad > 0)
                {
                    try { _bitacora.Registrar("PUBROCK_PUBLICACION_TB", "UPDATE", null, publicacion); } catch { }
                    return true;
                }

                if (cantidad == -1)
                    throw new System.ArgumentException("Error al editar. La publicación no existe");

                if (cantidad == -2)
                    throw new System.ArgumentException("Ya existe otra publicación con el mismo título");

                throw new System.ArgumentException("Error al editar. Verifique los datos ingresados.");
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("PUBROCK_PUBLICACION_TB", ex);
                throw;
            }
        }
    }
}
