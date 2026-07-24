using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstacciones.AccesoADatos.Bitacora.BitacoraContable;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace GestionPubRock.AccesoADatos.Bitacora.BitacoraContable
{
    public class ObtenerBitacoraContableAD : IObtenerBitacoraContableAD
    {
        private readonly Contexto _elContexto;

        public ObtenerBitacoraContableAD()
        {
            _elContexto = new Contexto();
        }

        public List<BitacoraContableDto> Obtener(BitacoraContableFiltroDto filtro)
        {
            var consulta =
                from b in _elContexto.BitacoraContable
                join u in _elContexto.Usuarios on b.Cedula equals u.Cedula into usuarios
                from u in usuarios.DefaultIfEmpty()
                where b.IdEstado == 1
                select new { Bitacora = b, Usuario = u };

            if (filtro != null)
            {
                if (!string.IsNullOrWhiteSpace(filtro.Cedula))
                    consulta = consulta.Where(x => x.Bitacora.Cedula == filtro.Cedula);

                if (filtro.FechaInicio.HasValue)
                    consulta = consulta.Where(x => x.Bitacora.FechaHora >= filtro.FechaInicio.Value.Date);

                if (filtro.FechaFin.HasValue)
                    consulta = consulta.Where(x => x.Bitacora.FechaHora < filtro.FechaFin.Value.Date.AddDays(1));
            }

            return consulta
                .OrderByDescending(x => x.Bitacora.FechaHora)
                .ToList()
                .Select(x => new BitacoraContableDto
                {
                    IdBitacora = x.Bitacora.IdBitacora,
                    FechaHora = x.Bitacora.FechaHora,
                    AccionRealizada = x.Bitacora.AccionRealizada,
                    Descripcion = x.Bitacora.Descripcion,
                    Cedula = x.Bitacora.Cedula,
                    Modulo = x.Bitacora.Modulo ?? string.Empty,
                    NombreUsuario = x.Usuario != null
                        ? (x.Usuario.Nombre + " " + x.Usuario.ApellidoPaterno).Trim()
                        : x.Bitacora.Cedula
                })
                .ToList();
        }
    }
}
