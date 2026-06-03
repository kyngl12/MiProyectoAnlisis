using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;

namespace GestionPubRock.Abstracciones.AccesoADatos.Cajas.ObtenerSINPESPorTelefonoCaja
{
    public interface IObtenerSINPESPorTelefonoCajaAD
    {
        List<SINPESDto> Obtener(string telefonoSINPE);
    }
}