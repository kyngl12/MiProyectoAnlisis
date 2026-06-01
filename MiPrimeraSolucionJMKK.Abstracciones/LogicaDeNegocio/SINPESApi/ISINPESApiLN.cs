using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPESApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.SINPESApi
{
    public interface ISINPESApiLN
    {
        List<SINPESDto> ConsultarSinpesPorTelefonoCaja(string telefonoCaja);
        RespuestaAPIDto SincronizarSinpe(int idSinpe);
        RespuestaAPIDto RecibirSinpe(SINPESDto modelo);
    }
}