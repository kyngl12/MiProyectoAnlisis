using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.RegistrarCaja
{
    public interface IRegistrarCajaLN
    {
        bool Registrar(CajaDto laCaja);
    }
}