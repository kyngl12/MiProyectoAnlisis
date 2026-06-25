using GestionPubRock.AccesoADatos.Inventario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Inventario
{
    public class EditarProductoLN
    {
        private readonly EditarProductoAD _ad;

        public EditarProductoLN()
        {
            _ad = new EditarProductoAD();
        }

        public bool Editar(ProductoDto producto)
        {
            if (producto == null)
                throw new ArgumentException("Los datos del producto son obligatorios");

            if (producto.IdProducto <= 0)
                throw new ArgumentException("El identificador del producto es inválido");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("Campos obligatorios vacíos en el formulario");

            if (producto.Cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa");

            if (producto.PrecioUnitario < 0)
                throw new ArgumentException("El precio debe ser un valor mayor a 0");

            int resultado = _ad.Editar(producto);

            if (resultado > 0)
                return true;

            if (resultado == -1)
                throw new ArgumentException("El código del producto ya se encuentra registrado en el sistema");

            if (resultado == -2)
                throw new ArgumentException("Campos obligatorios vacíos en el formulario");

            if (resultado == -3)
                throw new ArgumentException("La cantidad o el precio no pueden ser negativos");

            if (resultado == -4)
                throw new ArgumentException("El ID del producto no es válido");

            if (resultado == -5)
                throw new ArgumentException("El producto no existe en el sistema");

            return false;
        }
    }
}
