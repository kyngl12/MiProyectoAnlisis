using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using System.Collections.Generic;

namespace GestionPubRock.Abstracciones.AccesoADatos.Cajas.ObtenerCajasPorComercio
{
    public interface IObtenerCajasPorComercioAD
    {
        List<CajaDto> Obtener(int idComercio);
    }
}