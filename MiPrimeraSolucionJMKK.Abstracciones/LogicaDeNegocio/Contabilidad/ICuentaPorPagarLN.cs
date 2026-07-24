using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad
{
    public interface IRegistrarCuentaPorPagarLN
    {
        bool Registrar(CuentaPorPagarDto cuenta);
    }

    public interface IObtenerCuentasPorPagarLN
    {
        List<CuentaPorPagarDto> Obtener();
    }

    public interface IMarcarCuentaPorPagarComoPagadaLN
    {
        bool MarcarComoPagada(int idCuentaPorPagar, string cedulaRegistro);
    }
}
