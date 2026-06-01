using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ValidarIdentificacion;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ValidarIdentificacion
{
    public class ValidarIdentificacionUsuarioAD : IValidarIdentificacionUsuarioAD
    {
        private Contexto _elContexto;

        public ValidarIdentificacionUsuarioAD()
        {
            _elContexto = new Contexto();
        }

        public bool ExisteIdentificacion(string identificacion)
        {
            try
            {
                bool existe = _elContexto.Usuarios.Any(u => u.Identificacion == identificacion);
                return existe;
            }
            catch
            {
                throw;
            }
        }

        public bool ExisteIdentificacionExceptoUsuario(string identificacion, int idUsuario)
        {
            try
            {
                bool existe = _elContexto.Usuarios.Any(u => u.Identificacion == identificacion && u.IdUsuario != idUsuario);
                return existe;
            }
            catch
            {
                throw;
            }
        }
    }
}
