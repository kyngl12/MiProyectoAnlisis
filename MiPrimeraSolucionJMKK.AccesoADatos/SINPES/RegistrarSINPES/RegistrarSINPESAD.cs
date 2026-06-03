using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPES.RegistrarSINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;

namespace GestionPubRock.AccesoADatos.SINPES.RegistrarSINPES
{
    public class RegistrarSINPESAD : IRegistrarSINPESAD
    {
        Contexto _elContexto;

        public RegistrarSINPESAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarSINPES(SINPESDto elPagoSINPES)
        {
            SINPESEntidad pagoAGuardar = ConvertirObjetoEntidad(elPagoSINPES);

            _elContexto.SINPES.Add(pagoAGuardar);

            int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();

            return cantidadDeRegistrosAlmacenados;
        }

        private SINPESEntidad ConvertirObjetoEntidad(SINPESDto elPago)
        {
            return new SINPESEntidad
            {

                TelefonoOrigen = elPago.TelefonoOrigen,
                NombreOrigen = elPago.NombreOrigen,
                TelefonoDestinatario = elPago.TelefonoDestinatario,
                NombreDestinatario = elPago.NombreDestinatario,
                Monto = elPago.Monto,
                FechaDeRegistro = elPago.FechaDeRegistro,
                Descripcion = elPago.Descripcion,
                Estado = elPago.Estado
            };
        }
    }
}