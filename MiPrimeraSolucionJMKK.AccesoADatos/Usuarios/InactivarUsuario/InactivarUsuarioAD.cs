using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.InactivarUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.InactivarUsuario
{
    public class InactivarUsuarioAD : IInactivarUsuarioAD
    {
        private Contexto _elContexto;

        public InactivarUsuarioAD()
        {
            _elContexto = new Contexto();
        }

        public int Inactivar(string cedula)
        {
            try
            {
                var usuarioExistente = _elContexto.Usuarios
                    .FirstOrDefault(u => u.Cedula == cedula);

                if (usuarioExistente == null)
                    return -1;

                if (usuarioExistente.IdEstado == 2)
                    return -2;

                usuarioExistente.IdEstado = 2;
                return _elContexto.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}