using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales
{
    public class ReportesMensualesDto
    {
        public int IdReporte { get; set; }

        public int IdComercio { get; set; }

        [Display(Name = "Comercio")]
        public string NombreComercio { get; set; }

        [Display(Name = "Cantidad de cajas")]
        public int CantidadDeCajas { get; set; }

        [Display(Name = "Monto total recaudado")]
        public decimal MontoTotalRecaudado { get; set; }

        [Display(Name = "Cantidad de SINPES")]
        public int CantidadDeSINPES { get; set; }

        [Display(Name = "Monto total de comisión")]
        public decimal MontoTotalComision { get; set; }

        [Display(Name = "Fecha del reporte")]
        public DateTime FechaDelReporte { get; set; }
    }
}
