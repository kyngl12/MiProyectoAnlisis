using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad
{
    public class EgresoDto
    {
        public int IdMovimientoFinanciero { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto ingresado no es valido. Debe ser un valor mayor a cero.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        [StringLength(255)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado")]
        public int IdEmpleado { get; set; }

        public string RegistradoPor { get; set; }

        // Lista de empleados disponibles para la UI
        public List<EmpleadoDto> Empleados { get; set; }
    }

    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }
        public string Cedula { get; set; }
        public string NombreCompleto { get; set; }
    }
}
