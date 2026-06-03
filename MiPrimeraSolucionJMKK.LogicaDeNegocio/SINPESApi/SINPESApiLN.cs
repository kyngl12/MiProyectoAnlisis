using GestionPubRock.AccesoADatos.SINPESApi;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPESApi;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.SINPESApi;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPESApi;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.SINPESApi
{
    public class SinpeApiLN : ISINPESApiLN
    {
        private readonly ISINPESApiAD _sinpeApiAD;

        public SinpeApiLN()
        {
            _sinpeApiAD = new SinpeApiAD();
        }

        public List<SINPESDto> ConsultarSinpesPorTelefonoCaja(string telefonoCaja)
        {
            return _sinpeApiAD.ConsultarSinpesPorTelefonoCaja(telefonoCaja);
        }

        public RespuestaAPIDto SincronizarSinpe(int idSinpe)
        {
            return _sinpeApiAD.SincronizarSinpe(idSinpe);
        }

        public RespuestaAPIDto RecibirSinpe(SINPESDto modelo)
        {
            return _sinpeApiAD.RecibirSinpe(modelo);
        }
    }
}