using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.AccesoADatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Comercios.ObtenerTodosLosComercios
{
    public class ObtenerTodosLosComerciosAD : IObtenerTodosLosComerciosAD
    {
        Contexto _elContexto;

        public ObtenerTodosLosComerciosAD()
        {
            _elContexto = new Contexto();
        }

        public List<ComercioDto> Obtener()
        {
            List<ComercioDto> laListaDeComercios = (from elComercioEnBaseDeDatos in _elContexto.Comercios
                                                    select new ComercioDto
                                                    {
                                                        IdComercio = elComercioEnBaseDeDatos.IdComercio,
                                                        Identificacion = elComercioEnBaseDeDatos.Identificacion,
                                                        TipoIdentificacion = elComercioEnBaseDeDatos.TipoIdentificacion,
                                                        Nombre = elComercioEnBaseDeDatos.Nombre,
                                                        TipoDeComercio = elComercioEnBaseDeDatos.TipoDeComercio,
                                                        Telefono = elComercioEnBaseDeDatos.Telefono,
                                                        CorreoElectronico = elComercioEnBaseDeDatos.CorreoElectronico,
                                                        Estado = elComercioEnBaseDeDatos.Estado
                                                    }).ToList();

            return laListaDeComercios;
        }
    }
}

