using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Promocion;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Promocion
{
    public interface IEditarPromocionAD
    {
        bool Editar(PromocionDto laPromocion);
    }
}

