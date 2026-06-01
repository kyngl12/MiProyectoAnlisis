using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.ObtenerComercioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.AccesoADatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Comercios.ObtenerComercioPorId
{
    public class ObtenerComercioPorIdAD : IObtenerComercioPorIdAD
    {
        Contexto _elContexto;

        public ObtenerComercioPorIdAD()
        {
            _elContexto = new Contexto();
        }

        public ComercioDto Obtener(int id)
        {
            ComercioDto elComercio = (from elComercioEnBaseDeDatos
                                      in _elContexto.Comercios
                                      where elComercioEnBaseDeDatos.IdComercio == id
                                      select new ComercioDto
                                      {
                                          IdComercio = elComercioEnBaseDeDatos.IdComercio,
                                          Identificacion = elComercioEnBaseDeDatos.Identificacion,
                                          TipoIdentificacion = elComercioEnBaseDeDatos.TipoIdentificacion,
                                          Nombre = elComercioEnBaseDeDatos.Nombre,
                                          TipoDeComercio = elComercioEnBaseDeDatos.TipoDeComercio,
                                          Telefono = elComercioEnBaseDeDatos.Telefono,
                                          CorreoElectronico = elComercioEnBaseDeDatos.CorreoElectronico,
                                          Direccion = elComercioEnBaseDeDatos.Direccion,
                                          FechaDeRegistro = elComercioEnBaseDeDatos.FechaDeRegistro,
                                          FechaDeModificacion = elComercioEnBaseDeDatos.FechaDeModificacion,
                                          Estado = elComercioEnBaseDeDatos.Estado
                                      }).FirstOrDefault();

            return elComercio;
        }
    }
}