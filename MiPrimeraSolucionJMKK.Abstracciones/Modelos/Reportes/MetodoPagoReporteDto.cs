using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes
{
    public class MetodoPagoReporteDto
    {
        public string MetodoPago { get; set; }

        public int CantidadTransacciones { get; set; }

        public decimal MontoTotal { get; set; }
    }
}
