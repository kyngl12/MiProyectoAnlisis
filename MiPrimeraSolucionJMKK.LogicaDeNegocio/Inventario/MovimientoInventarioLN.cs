using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Inventario
{
    public class MovimientoInventarioLN
    {
        private readonly MovimientoInventarioAD _ad;

        public MovimientoInventarioLN()
        {
            _ad = new MovimientoInventarioAD();
        }

        public string RegistrarMovimiento(int idProducto, int cantidad, string tipo, string motivo)
        {
            var resultado = _ad.RegistrarMovimiento(idProducto, cantidad, tipo, motivo);

            if (resultado == "CANTIDAD_INVALIDA")
                return "La cantidad ingresada no es válida. Debe ser un número entero mayor a cero";

            if (resultado.StartsWith("STOCK_INSUFICIENTE"))
            {
                var stock = resultado.Split('|')[1];
                return $"La cantidad de salida excede el stock disponible del producto. Stock actual: {stock}";
            }

            if (resultado.StartsWith("STOCK_BAJO"))
            {
                var stock = resultado.Split('|')[1];
                return $"Stock bajo. Cantidad actual: {stock}";
            }

            if (resultado == "OK")
                return "Movimiento registrado correctamente";

            return "Ocurrió un error al registrar el movimiento";
        }
    }
}
