using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Promocion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Promocion;
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

        public List<PromocionDto> Obtener(
            string criterio = "",
            int? idEstado = null
        )
        {
            try
            {
                var consulta =
                    from p in _elContexto.Promocion
                    join pp in _elContexto.PromocionProducto
                        on p.IdPromocion equals pp.IdPromocion
                    join pr in _elContexto.Productos
                        on pp.IdProducto equals pr.IdProducto
                    select new
                    {
                        Promocion = p,
                        Producto = pr
                    };

                if (!string.IsNullOrWhiteSpace(criterio))
                {
                    criterio = criterio.Trim().ToLower();

                    consulta = consulta.Where(x =>
                        x.Promocion.NombrePromocion
                            .ToLower()
                            .Contains(criterio) ||
                        x.Producto.NombreProducto
                            .ToLower()
                            .Contains(criterio)
                    );
                }

                if (idEstado.HasValue)
                {
                    consulta = consulta.Where(x =>
                        x.Promocion.IdEstado == idEstado.Value
                    );
                }

                return consulta
                    .Select(x => new PromocionDto
                    {
                        IdPromocion = x.Promocion.IdPromocion,
                        NombrePromocion = x.Promocion.NombrePromocion,
                        Descripcion = x.Promocion.Descripcion,
                        PorcentajeDescuento =
                            x.Promocion.PorcentajeDescuento,
                        FechaInicio = x.Promocion.FechaInicio,
                        FechaFin = x.Promocion.FechaFin,
                        IdEstado = x.Promocion.IdEstado,
                        IdProducto = x.Producto.IdProducto
                    })
                    .ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}

