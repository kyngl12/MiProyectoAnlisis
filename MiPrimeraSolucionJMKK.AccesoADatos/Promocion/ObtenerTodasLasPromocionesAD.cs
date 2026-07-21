using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Promocion
{
    public class ObtenerTodasLasPromocionesAD : IObtenerTodasLasPromocionesAD
    {
        private Contexto _elContexto;

        public ObtenerTodasLasPromocionesAD()
        {
            _elContexto = new Contexto();
        }

        public List<PromocionDto> Obtener()
        {
            try
            {
                var promociones = (from p in _elContexto.Promocion
                                   join pp in _elContexto.PromocionProducto
                                   on p.IdPromocion equals pp.IdPromocion
                                   select new PromocionDto
                                   {
                                       IdPromocion = p.IdPromocion,
                                       NombrePromocion = p.NombrePromocion,
                                       Descripcion = p.Descripcion,
                                       PorcentajeDescuento = p.PorcentajeDescuento,
                                       FechaInicio = p.FechaInicio,
                                       FechaFin = p.FechaFin,
                                       IdEstado = p.IdEstado,
                                       IdProducto = pp.IdProducto
                                   }).ToList();

                return promociones;
            }
            catch
            {
                throw;
            }
        }
    }
}
