using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Promocion
{
    public class DesactivarPromocionAD : IDesactivarPromocionAD
    {
        private Contexto _elContexto;

        public DesactivarPromocionAD()
        {
            _elContexto = new Contexto();
        }

        public int Desactivar(int idPromocion)
        {
            try
            {
                var promocion = _elContexto.Promocion
                    .FirstOrDefault(p => p.IdPromocion == idPromocion);

                // No existe
                if (promocion == null)
                    return -1;

                // Ya estaba inactiva
                if (promocion.IdEstado == 2)
                    return -2;

                promocion.IdEstado = 2;

                var promocionProducto = _elContexto.PromocionProducto
                    .FirstOrDefault(pp => pp.IdPromocion == idPromocion);

                if (promocionProducto != null)
                {
                    promocionProducto.IdEstado = 2;
                }

                _elContexto.SaveChanges();

                return 1;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return -99;
            }
            catch
            {
                throw;
            }
        }
    }
}