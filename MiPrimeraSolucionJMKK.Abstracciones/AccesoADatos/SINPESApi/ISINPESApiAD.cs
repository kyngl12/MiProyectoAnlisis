using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPESApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPESApi
{
    public interface ISINPESApiAD
    {
        List<SINPESDto> ConsultarSinpesPorTelefonoCaja(string telefonoCaja);
        RespuestaAPIDto SincronizarSinpe(int idSinpe);
        RespuestaAPIDto RecibirSinpe(SINPESDto modelo);
    }
}