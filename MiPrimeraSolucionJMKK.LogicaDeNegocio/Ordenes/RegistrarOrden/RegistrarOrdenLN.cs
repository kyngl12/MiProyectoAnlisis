using System;
using GestionPubRock.AccesoADatos.Ordenes;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Ordenes.RegistrarOrden
{
    public class RegistrarOrdenLN
    {
        private readonly RegistrarOrdenAD _ad;

        public RegistrarOrdenLN()
        {
            _ad = new RegistrarOrdenAD();
        }

        public int Registrar(OrdenDto orden)
        {
            if (orden == null) throw new ArgumentException("Campos obligatorios vacíos en el formulario");
            if (orden.Cantidad <= 0) throw new ArgumentException("La cantidad no puede ser negativa");
            // FechaEntregaEstim es opcional en el formulario; validar solo si fue proporcionada
            if (orden.FechaEntregaEstim != default(DateTime) && orden.FechaEntregaEstim < DateTime.Now.Date)
                throw new ArgumentException("La fecha de entrega estimada no puede ser anterior a la fecha actual");

            int res = _ad.Registrar(orden);

            if (res > 0) return res;
            if (res == -2) throw new ArgumentException("El número de orden ya se encuentra registrado en el sistema");
            if (res == -3) throw new ArgumentException("La cantidad no puede ser negativa");
            if (res == -5) throw new ArgumentException("El proveedor seleccionado no se encuentra registrado en el sistema");

            return 0;
        }
    }
}
