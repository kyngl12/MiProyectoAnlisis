using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.EditarComercio
{
    public interface IEditarComercioAD
    {
        int Editar(ComercioDto elComercioParaEditar);
    }
}
