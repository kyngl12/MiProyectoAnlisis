using GestionPubRock.AccesoADatos.Marketing.RegistrarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Marketing.RegistrarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.RegistrarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Marketing.RegistrarPublicacion
{
    public class RegistrarPublicacionLN : IRegistrarPublicacionLN
    {
        private IRegistrarPublicacionAD _registrarPublicacionAD;

        public RegistrarPublicacionLN()
        {
            _registrarPublicacionAD = new RegistrarPublicacionAD();
        }

        public bool Registrar(MarketingDto publicacion)
        {
            try
            {
                var camposFaltantes = new System.Collections.Generic.List<string>();

                if (string.IsNullOrWhiteSpace(publicacion.Titulo)) camposFaltantes.Add("Título");
                if (string.IsNullOrWhiteSpace(publicacion.TipoContenido))
    camposFaltantes.Add("Tipo de Contenido");
                if (string.IsNullOrWhiteSpace(publicacion.Contenido)) camposFaltantes.Add("Descripción");
                if (publicacion.FechaInicio == default(DateTime)) camposFaltantes.Add("Fecha de Inicio");
                if (publicacion.FechaFinalizacion == default(DateTime)) camposFaltantes.Add("Fecha de Finalización");
                if (publicacion.IdEstado <= 0) camposFaltantes.Add("Estado");

                if (camposFaltantes.Count > 0)
                    throw new System.ArgumentException("Los siguientes campos obligatorios están vacíos: " + string.Join(", ", camposFaltantes));

                if (publicacion.FechaInicio > publicacion.FechaFinalizacion)
                    throw new System.ArgumentException("La fecha de inicio no puede ser mayor a la fecha de finalización");

                if (publicacion.TipoContenido == "Menu" &&
    (!publicacion.Precio.HasValue || publicacion.Precio < 0))
                    throw new System.ArgumentException("El precio es obligatorio y debe ser mayor o igual a cero para menús");

                publicacion.FechaPublicacion = DateTime.Now;

                int cantidad = _registrarPublicacionAD.Registrar(publicacion);

                if (cantidad > 0)
                {
                    return true;
                }

                if (cantidad == -1)
                    throw new System.ArgumentException("Ya existe una publicación con el mismo título");

                if (cantidad == -99)
                    throw new System.ArgumentException("Error en la base de datos al intentar registrar la publicación");

                return false;
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.File.AppendAllText(
                        System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\errors.log",
                        DateTime.Now.ToString("s") + " - " + ex.ToString() + Environment.NewLine
                    );
                }
                catch { }

                throw;
            }
        }
    }
}
