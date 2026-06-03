using GestionPubRock.AccesoADatos.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using System.Collections.Generic;
using System.Linq;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.ObtenerTodosLosComercios
{
    public class ObtenerTodosLosComerciosLN : IObtenerTodosLosComerciosLN
    {
        IObtenerTodosLosComerciosAD _obtenerTodosLosComerciosAD;

        public ObtenerTodosLosComerciosLN()
        {
            _obtenerTodosLosComerciosAD = new ObtenerTodosLosComerciosAD();
        }

        public List<ComercioDto> Obtener()
        {
            List<ComercioDto> laListaDeComercios = _obtenerTodosLosComerciosAD.Obtener();

            laListaDeComercios = laListaDeComercios
                .OrderBy(elComercio => elComercio.Nombre)
                .ToList();

            return laListaDeComercios;
        }
    }
}
