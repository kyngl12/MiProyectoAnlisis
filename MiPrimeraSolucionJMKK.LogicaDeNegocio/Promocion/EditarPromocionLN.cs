using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Promocion
{
    public class EditarPromocionLN : IEditarPromocionLN
    {
        private IEditarPromocionAD _editarPromocionAD;

        public EditarPromocionLN()
        {
            _editarPromocionAD = new EditarPromocionAD();
        }

        public bool Editar(PromocionDto promocion)
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
                    throw new ArgumentException(
                        "Los siguientes campos obligatorios están vacíos: " +
                        string.Join(", ", camposFaltantes));

                // Escenario 2
                if (promocion.PorcentajeDescuento <= 0 ||
                    promocion.PorcentajeDescuento > 100)
                {
                    throw new ArgumentException("El porcentaje ingresado no es válido.");
                }

                if (promocion.FechaFin < promocion.FechaInicio)
                {
                    throw new ArgumentException("La fecha de vigencia no es válida.");
                }

                bool ok = _editarPromocionAD.Editar(promocion);

                if (ok)
                    return true;

                // Escenario 3
                throw new ArgumentException("La promoción no se encuentra registrada.");
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.File.AppendAllText(
                        AppDomain.CurrentDomain.BaseDirectory + "App_Data\\errors.log",
                        DateTime.Now.ToString("s") + " - " +
                        ex.ToString() + Environment.NewLine
                    );
                }
                catch { }

                throw;
            }
        }
    }
}
