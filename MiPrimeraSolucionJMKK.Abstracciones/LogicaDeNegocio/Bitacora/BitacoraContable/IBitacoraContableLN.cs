using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.BitacoraContable
{
    public interface IRegistrarBitacoraContableLN
    {
        void Registrar(string accionRealizada, string descripcion, string cedula, string modulo);
    }

    public interface IObtenerBitacoraContableLN
    {
        List<BitacoraContableDto> Obtener(BitacoraContableFiltroDto filtro);
    }
}
