using System.Linq;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Promocion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Promocion;

namespace GestionPubRock.AccesoADatos.Promocion
{
    public class RegistrarPromocionAD : IRegistrarPromocionAD
    {
        private Contexto _elContexto;

        public RegistrarPromocionAD()
        {
            _elContexto = new Contexto();
        }

        public int Registrar(PromocionDto promocion)
        {
            try
            {
                var existePromocion = _elContexto.Database.SqlQuery<int?>(
                    "SELECT 1 FROM PUBROCK_PROMOCION_TB WHERE NOMBRE_PROMOCION = @p0",
                    promocion.NombrePromocion
                ).FirstOrDefault();

                if (existePromocion != null)
                    return -1;

                PromocionEntidad promocionAGuardar = ConvertirAEntidad(promocion);

                if (promocionAGuardar.IdEstado <= 0)
                {
                    promocionAGuardar.IdEstado =
                        _elContexto.Database.SqlQuery<int?>(
                        "SELECT TOP 1 ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION='Activo'")
                        .FirstOrDefault() ?? 1;
                }

                _elContexto.Promocion.Add(promocionAGuardar);
                _elContexto.SaveChanges();

                PromocionProductoEntidad promocionProducto = new PromocionProductoEntidad
                {
                    IdPromocion = promocionAGuardar.IdPromocion,
                    IdProducto = promocion.IdProducto,
                    IdEstado = promocionAGuardar.IdEstado
                };

                _elContexto.PromocionProducto.Add(promocionProducto);

                return _elContexto.SaveChanges();
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

        private PromocionEntidad ConvertirAEntidad(PromocionDto promocion)
        {
            return new PromocionEntidad
            {
                NombrePromocion = promocion.NombrePromocion,
                Descripcion = promocion.Descripcion,
                PorcentajeDescuento = promocion.PorcentajeDescuento,
                FechaInicio = promocion.FechaInicio,
                FechaFin = promocion.FechaFin,
                IdEstado = promocion.IdEstado
            };
        }
    }
}
