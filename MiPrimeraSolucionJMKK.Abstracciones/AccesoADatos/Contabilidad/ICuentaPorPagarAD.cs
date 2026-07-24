using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad
{
    public interface IRegistrarCuentaPorPagarAD
    {
        int Registrar(CuentaPorPagarDto cuenta);
    }

    public interface IObtenerCuentasPorPagarAD
    {
        List<CuentaPorPagarDto> Obtener();
    }

    public interface IMarcarCuentaPorPagarComoPagadaAD
    {
        int MarcarComoPagada(int idCuentaPorPagar, string cedulaRegistro);
    }
}
