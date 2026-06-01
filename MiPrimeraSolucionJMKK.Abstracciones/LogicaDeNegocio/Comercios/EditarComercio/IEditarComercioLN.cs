using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.EditarComercio
{
    public interface IEditarComercioLN
    {
        bool Editar(ComercioDto elComercioAEditar);
    }
}
