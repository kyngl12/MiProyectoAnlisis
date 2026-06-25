using GestionPubRock.AccesoADatos.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Inventario
{
    public class EliminarProductoLN
    {
        private readonly EliminarProductoAD _ad;

        public EliminarProductoLN()
        {
            _ad = new EliminarProductoAD();
        }

        public string Eliminar(int idProducto)
        {
            var resultado = _ad.Eliminar(idProducto);

            switch (resultado)
            {
                case "NO_EXISTE":
                    return "El producto no se encuentra registrado en el sistema";

                case "TIENE_ORDENES":
                    return "No es posible eliminar el producto. Tiene órdenes activas asociadas. Resuelva las órdenes antes de continuar";

                case "OK":
                    return "Producto eliminado correctamente";

                default:
                    return "Ocurrió un error al eliminar el producto";
            }
        }
    }
}
