using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ObtenerTodosLosTipoUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ObtenerTodosLosTipoUsuarios
{
    public class ObtenerTodosLosTiposDeUsuarioAD : IObtenerTodosLosTiposDeUsuarioAD
    {
        private Contexto _elContexto;

        public ObtenerTodosLosTiposDeUsuarioAD()
        {
            _elContexto = new Contexto();
        }

        public List<TipoUsuarioDto> ObtenerTodos()
        {
            try
            {
                List<TipoUsuarioDto> laLista = _elContexto.TiposDeUsuario
                    .Where(t => t.IdEstado == 1)  
                    .Select(t => new TipoUsuarioDto
                    {
                        IdTipoUsuario = t.IdTipoUsuario,
                        Descripcion = t.Descripcion,
                        IdEstado = t.IdEstado
                    })
                    .ToList();

                return laLista;
            }
            catch
            {
                throw;
            }
        }
    }
}
