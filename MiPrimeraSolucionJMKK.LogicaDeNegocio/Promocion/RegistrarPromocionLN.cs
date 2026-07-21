using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Promocion
{
    public class RegistrarPromocionLN : IRegistrarPromocionLN
    {
        private IRegistrarPromocionAD _registrarPromocionAD;

        public RegistrarPromocionLN()
        {
            _registrarPromocionAD = new RegistrarPromocionAD();
        }

        public bool Registrar(PromocionDto promocion)
        {
            try
            {
                var camposFaltantes = new List<string>();

                if (string.IsNullOrWhiteSpace(promocion.NombrePromocion))
                    camposFaltantes.Add("Nombre de la Promoción");

                if (promocion.PorcentajeDescuento <= 0)
                    camposFaltantes.Add("Porcentaje");

                if (promocion.FechaInicio == default(DateTime))
                    camposFaltantes.Add("Fecha de Inicio");

                if (promocion.FechaFin == default(DateTime))
                    camposFaltantes.Add("Fecha de Fin");

                if (promocion.IdProducto <= 0)
                    camposFaltantes.Add("Producto");

                if (camposFaltantes.Count > 0)
                    throw new ArgumentException("Los siguientes campos obligatorios están vacíos: " + string.Join(", ", camposFaltantes));

                if (promocion.PorcentajeDescuento <= 0 || promocion.PorcentajeDescuento > 100)
                    throw new ArgumentException("El porcentaje ingresado no es válido.");

                if (promocion.FechaFin < promocion.FechaInicio)
                    throw new ArgumentException("La fecha de vigencia no es válida.");

                int cantidad = _registrarPromocionAD.Registrar(promocion);

                if (cantidad > 0)
                    return true;

                if (cantidad == -1)
                    throw new ArgumentException("Ya existe una promoción con el mismo nombre.");

                if (cantidad == -2)
                    throw new ArgumentException("El producto seleccionado no existe.");

                if (cantidad == -99)
                    throw new ArgumentException("Error en la base de datos al intentar registrar la promoción.");

                return false;
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.File.AppendAllText(
                        AppDomain.CurrentDomain.BaseDirectory + "App_Data\\errors.log",
                        DateTime.Now.ToString("s") + " - " + ex.ToString() + Environment.NewLine
                    );
                }
                catch { }

                throw;
            }
        }
    }
}



