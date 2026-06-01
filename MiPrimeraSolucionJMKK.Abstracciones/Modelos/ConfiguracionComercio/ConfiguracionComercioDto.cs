using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio
{
    public class ConfiguracionComercioDto
    {
        public int IdConfiguracion { get; set; }
        public int IdComercio { get; set; }
        public string NombreComercio { get; set; }
        public int TipoConfiguracion { get; set; }
        public string TipoConfiguracionTexto { get; set; }
        public int Comision { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public bool Estado { get; set; }
    }
}
