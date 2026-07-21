using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Promocion
{
    public class DesactivarPromocionLN : IDesactivarPromocionLN
    {
        private IDesactivarPromocionAD _desactivarPromocionAD;

        public DesactivarPromocionLN()
        {
            _desactivarPromocionAD = new DesactivarPromocionAD();
        }

        public bool Desactivar(int idPromocion)
        {
            try
            {
                if (idPromocion <= 0)
                    throw new ArgumentException("La promoción no es válida.");

                int cantidad = _desactivarPromocionAD.Desactivar(idPromocion);

                if (cantidad > 0)
                    return true;

                if (cantidad == -1)
                    throw new ArgumentException("La promoción no se encuentra registrada.");

                if (cantidad == -2)
                    throw new ArgumentException("La promoción ya se encuentra inactiva.");

                if (cantidad == -99)
                    throw new ArgumentException("Error en el sistema. Por favor intente nuevamente.");

                return false;
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
