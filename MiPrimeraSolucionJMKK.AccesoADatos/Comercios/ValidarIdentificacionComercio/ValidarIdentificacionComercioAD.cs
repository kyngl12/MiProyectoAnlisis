using GestionPubRock.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Comercios.ValidarIdentificacionComercio
{
    public class ValidarCajaTelefonoSINPESAD
    {
        Contexto _elContexto;

        public ValidarCajaTelefonoSINPESAD()
        {
            _elContexto = new Contexto();
        }

        public bool ExisteIdentificacion(string identificacion)
        {
            ComerciosEntidad comercio = _elContexto.Comercios.Where(c => c.Identificacion == identificacion).FirstOrDefault();

            if (comercio != null)
            {
                return true;
            }

            return false;
        }
    }
}