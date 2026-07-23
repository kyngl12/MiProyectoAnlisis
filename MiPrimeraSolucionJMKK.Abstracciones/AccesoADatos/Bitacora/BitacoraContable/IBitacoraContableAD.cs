using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace MiPrimeraSolucionJMKK.Abstacciones.AccesoADatos.Bitacora.BitacoraContable
{
    public interface IRegistrarBitacoraContableAD
    {
        void Registrar(BitacoraContableDto evento);
    }

    public interface IObtenerBitacoraContableAD
    {
        List<BitacoraContableDto> Obtener(BitacoraContableFiltroDto filtro);
    }
}
