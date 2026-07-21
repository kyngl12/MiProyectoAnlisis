using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Promocion
{
    public class EditarPromocionAD : IEditarPromocionAD
    {
        private Contexto _elContexto;

        public EditarPromocionAD()
        {
            _elContexto = new Contexto();
        }

        public bool Editar(PromocionDto promocion)
        {
            try
            {
                var promocionEditar = _elContexto.Promocion
                    .FirstOrDefault(p => p.IdPromocion == promocion.IdPromocion);

                if (promocionEditar == null)
                    return false;

                promocionEditar.NombrePromocion = promocion.NombrePromocion;
                promocionEditar.Descripcion = promocion.Descripcion;
                promocionEditar.PorcentajeDescuento = promocion.PorcentajeDescuento;
                promocionEditar.FechaInicio = promocion.FechaInicio;
                promocionEditar.FechaFin = promocion.FechaFin;
                promocionEditar.IdEstado = promocion.IdEstado;

                var promocionProducto = _elContexto.PromocionProducto
                    .FirstOrDefault(pp => pp.IdPromocion == promocion.IdPromocion);

                if (promocionProducto != null)
                {
                    promocionProducto.IdProducto = promocion.IdProducto;
                    promocionProducto.IdEstado = promocion.IdEstado;
                }

                _elContexto.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}