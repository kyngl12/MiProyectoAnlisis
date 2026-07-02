using System;
using System.Linq;
using GestionPubRock.AccesoADatos;

namespace GestionPubRock.AccesoADatos.Reservaciones
{
    public class CancelarReservacionAD
    {
        private readonly Contexto _ctx;

        public CancelarReservacionAD()
        {
            _ctx = new Contexto();
        }

        public int Cancelar(int idReservacion)
        {
            try
            {
                var existe = _ctx.Database.SqlQuery<int?>("SELECT ID_RESERVACION FROM PUBROCK_RESERVACION_TB WHERE ID_RESERVACION = @p0", idReservacion).FirstOrDefault();
                if (existe == null) return -2; // no existe

                var estado = _ctx.Database.SqlQuery<int?>("SELECT ISNULL(ID_ESTADO,0) FROM PUBROCK_RESERVACION_TB WHERE ID_RESERVACION = @p0", idReservacion).FirstOrDefault();
                // si ya está en 2 (Inactivo) considerar ya cancelada
                if (estado == 2) return -3; // ya cancelada (ID_ESTADO=2)

                // Actualizar estado a Inactivo (ID_ESTADO correspondiente)
                var res = _ctx.Database.ExecuteSqlCommand(
                    "UPDATE PUBROCK_RESERVACION_TB SET ID_ESTADO = (SELECT ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Inactivo') WHERE ID_RESERVACION = @p0",
                    idReservacion);
                return res;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return -99;
            }
            catch
            {
                throw;
            }
        }
    }
}
