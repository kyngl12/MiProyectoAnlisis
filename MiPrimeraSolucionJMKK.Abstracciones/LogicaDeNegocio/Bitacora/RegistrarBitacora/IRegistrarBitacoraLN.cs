using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora
{
    public interface IRegistrarBitacoraLN
    {
        void Registrar(string tabla, string tipo, object datosAntes = null, object datosDespues = null);
        void RegistrarError(string tabla, Exception ex);
    }
}
