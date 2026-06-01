using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.EditarComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.AccesoADatos;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucion.AccesoADatos.Comercios.EditarComercio
{
    public class EditarComercioAD : IEditarComercioAD
    {
        private Contexto _elContexto;

        public EditarComercioAD()
        {
            _elContexto = new Contexto();
        }

        public int Editar(ComercioDto elComercioParaEditar)
        {
            ComerciosEntidad elComercioEnBaseDeDatos = _elContexto.Comercios
                .Where(elComercioBuscado => elComercioBuscado.IdComercio == elComercioParaEditar.IdComercio)
                .FirstOrDefault();

            if (elComercioEnBaseDeDatos != null)
            {
                elComercioEnBaseDeDatos.Nombre = elComercioParaEditar.Nombre;
                elComercioEnBaseDeDatos.TipoDeComercio = elComercioParaEditar.TipoDeComercio;
                elComercioEnBaseDeDatos.Telefono = elComercioParaEditar.Telefono;
                elComercioEnBaseDeDatos.CorreoElectronico = elComercioParaEditar.CorreoElectronico;
                elComercioEnBaseDeDatos.Direccion = elComercioParaEditar.Direccion;
                elComercioEnBaseDeDatos.Estado = elComercioParaEditar.Estado;
                elComercioEnBaseDeDatos.FechaDeModificacion = elComercioParaEditar.FechaDeModificacion;

                return _elContexto.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}