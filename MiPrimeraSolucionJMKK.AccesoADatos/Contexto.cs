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
        public DbSet<MarketingEntidad> Marketing { get; set; }
        public DbSet<PromocionEntidad> Promocion { get; set; }

        public DbSet<PromocionProductoEntidad> PromocionProducto { get; set; }
	
        public DbSet<TipoMovimientoFinancieroEntidad> TiposMovimientoFinanciero { get; set; }
        public DbSet<MovimientoFinancieroEntidad> MovimientosFinancieros { get; set; }
        public DbSet<CierreCajaEntidad> CierresCaja { get; set; }
        public DbSet<ImpuestoEntidad> Impuestos { get; set; }
        public DbSet<CuentaPorPagarEntidad> CuentasPorPagar { get; set; }
        public DbSet<BitacoraContableEntidad> BitacoraContable { get; set; }



    }
}