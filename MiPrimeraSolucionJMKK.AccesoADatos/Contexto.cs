using GestionPubRock.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestionPubRock.AccesoADatos
{
    public class Contexto : DbContext
    {

            public Contexto()
        {

        }

        public DbSet<ComerciosEntidad> Comercios { get; set; }
        public DbSet<SINPESEntidad> SINPES { get; set; }
        public DbSet<CajasEntidad> Cajas { get; set; }
        public DbSet<UsuariosEntidad> Usuarios { get; set; }
        public DbSet<TipoUsuarioEntidad> TiposDeUsuario { get; set; }
        public DbSet<ReportesMensualesEntidad> ReportesMensuales { get; set; }
        public DbSet<ConfiguracionComercioEntidad> ConfiguracionComercio { get; set; }
        public DbSet<BitacoraEntidad> Bitacora { get; set; }
        public DbSet<ReservacionEntidad> Reservaciones { get; set; }
        public DbSet<ProductosEntidad> Productos { get; set; }

    }
}