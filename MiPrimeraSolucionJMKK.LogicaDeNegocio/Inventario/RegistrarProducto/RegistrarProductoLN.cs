using System;
using GestionPubRock.AccesoADatos.Inventario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario.RegistrarProducto
{
    public class RegistrarProductoLN
    {
        private readonly RegistrarProductoAD _ad;

        public RegistrarProductoLN()
        {
            _ad = new RegistrarProductoAD();
        }

        public bool Registrar(ProductoDto producto)
        {
            if (producto == null) throw new ArgumentException("Los datos del producto son obligatorios");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("Campos obligatorios vacíos en el formulario");

            if (producto.Cantidad < 0) throw new ArgumentException("La cantidad no puede ser negativa");
            if (producto.PrecioUnitario < 0) throw new ArgumentException("El precio debe ser un valor mayor a 0");

            int resultado = _ad.Registrar(producto);
            if (resultado > 0) return true;

            if (resultado == -1) throw new ArgumentException("El código del producto ya se encuentra registrado en el sistema");
            if (resultado == -2) throw new ArgumentException("Campos obligatorios vacíos en el formulario");
            if (resultado == -3) throw new ArgumentException("La cantidad o el precio no pueden ser negativos");

            return false;
        }
    }
}
