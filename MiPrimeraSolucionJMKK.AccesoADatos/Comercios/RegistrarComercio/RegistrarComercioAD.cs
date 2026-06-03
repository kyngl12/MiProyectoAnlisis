using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.RegstrarComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;

namespace GestionPubRock.AccesoADatos.Comercios.RegistrarComercio
    {
    public class RegistrarComercioAD : IRegistrarComercioAD
    {
        Contexto _elContexto;

        public RegistrarComercioAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarComercio(ComercioDto elComercio)
        {
            ComerciosEntidad comercioAGuardar = ConvertirObjetoEntidad(elComercio);

            _elContexto.Comercios.Add(comercioAGuardar);

            int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();

            return cantidadDeRegistrosAlmacenados;
        }

        private ComerciosEntidad ConvertirObjetoEntidad(ComercioDto elComercio)
        {
            return new ComerciosEntidad
            {
                Identificacion = elComercio.Identificacion,
                TipoIdentificacion = elComercio.TipoIdentificacion,
                Nombre = elComercio.Nombre,
                TipoDeComercio = elComercio.TipoDeComercio,
                Telefono = elComercio.Telefono,
                CorreoElectronico = elComercio.CorreoElectronico,
                Direccion = elComercio.Direccion,
                FechaDeRegistro = elComercio.FechaDeRegistro,
                FechaDeModificacion = elComercio.FechaDeModificacion,
                Estado = elComercio.Estado
            };
        }
    }
}

