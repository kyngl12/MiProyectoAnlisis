using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.RegistrarCaja;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Cajas.RegistrarCaja
{
    public class RegistrarCajaAD : IRegistrarCajaAD
    {
        private Contexto _elContexto;

        public RegistrarCajaAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarCaja(CajaDto laCaja)
        {
            CajasEntidad cajaAGuardar = ConvertirObjetoEntidad(laCaja);

            _elContexto.Cajas.Add(cajaAGuardar);

            int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();

            return cantidadDeRegistrosAlmacenados;
        }

        private CajasEntidad ConvertirObjetoEntidad(CajaDto laCaja)
        {
            return new CajasEntidad
            {
                IdComercio = laCaja.IdComercio,
                Nombre = laCaja.Nombre,
                Descripcion = laCaja.Descripcion,
                TelefonoSINPE = laCaja.TelefonoSINPE,
                FechaDeRegistro = laCaja.FechaDeRegistro,
                FechaDeModificacion = laCaja.FechaDeModificacion,
                Estado = laCaja.Estado
            };
        }
    }
}