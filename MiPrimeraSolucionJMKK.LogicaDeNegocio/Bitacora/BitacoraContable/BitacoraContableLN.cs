using System;
using System.Collections.Generic;
using GestionPubRock.AccesoADatos.Bitacora.BitacoraContable;
using MiPrimeraSolucionJMKK.Abstacciones.AccesoADatos.Bitacora.BitacoraContable;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.BitacoraContable;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace GestionPubRock.LogicaDeNegocio.Bitacora.BitacoraContable
{
    public class RegistrarBitacoraContableLN : IRegistrarBitacoraContableLN
    {
        private readonly IRegistrarBitacoraContableAD _registrarBitacoraContableAD;

        public RegistrarBitacoraContableLN() : this(new RegistrarBitacoraContableAD()) { }

        public RegistrarBitacoraContableLN(IRegistrarBitacoraContableAD registrarBitacoraContableAD)
        {
            _registrarBitacoraContableAD = registrarBitacoraContableAD;
        }

        public void Registrar(string accionRealizada, string descripcion, string cedula, string modulo)
        {
            // La bitacora es una funcion de auditoria transversal: nunca debe
            // interrumpir la operacion de negocio que la origino si falla.
            try
            {
                _registrarBitacoraContableAD.Registrar(new BitacoraContableDto
                {
                    FechaHora = DateTime.Now,
                    AccionRealizada = accionRealizada,
                    Descripcion = descripcion,
                    Cedula = cedula,
                    Modulo = modulo
                });
            }
            catch { }
        }
    }

    public class ObtenerBitacoraContableLN : IObtenerBitacoraContableLN
    {
        private readonly IObtenerBitacoraContableAD _obtenerBitacoraContableAD;

        public ObtenerBitacoraContableLN() : this(new ObtenerBitacoraContableAD()) { }

        public ObtenerBitacoraContableLN(IObtenerBitacoraContableAD obtenerBitacoraContableAD)
        {
            _obtenerBitacoraContableAD = obtenerBitacoraContableAD;
        }

        public List<BitacoraContableDto> Obtener(BitacoraContableFiltroDto filtro)
        {
            return _obtenerBitacoraContableAD.Obtener(filtro);
        }
    }
}
