using System.Linq;

namespace GestionPubRock.AccesoADatos.Common
{
    /// <summary>
    /// Resuelve el ID_EMPLEADO asociado a la cedula del usuario autenticado.
    /// Los movimientos financieros, cierres de caja y cuentas por pagar
    /// requieren un ID_EMPLEADO valido; como el modulo de Usuarios no
    /// crea automaticamente un registro en PUBROCK_EMPLEADO_TB para los
    /// administradores, este helper lo crea de forma transparente la
    /// primera vez que se necesita (sin afectar el modulo de Usuarios).
    /// </summary>
    public static class EmpleadoResolver
    {
        public static int ObtenerOCrearIdEmpleado(Contexto contexto, string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                return 0;

            var idEmpleado = contexto.Database.SqlQuery<int?>(
                "SELECT TOP 1 ID_EMPLEADO FROM PUBROCK_EMPLEADO_TB WHERE CEDULA = @p0",
                cedula
            ).FirstOrDefault();

            if (idEmpleado.HasValue)
                return idEmpleado.Value;

            var existeUsuario = contexto.Database.SqlQuery<int?>(
                "SELECT 1 FROM PUBROCK_USUARIO_TB WHERE CEDULA = @p0",
                cedula
            ).FirstOrDefault();

            if (existeUsuario == null)
                return 0;

            contexto.Database.ExecuteSqlCommand(
                "INSERT INTO PUBROCK_EMPLEADO_TB (FECHA_INGRESO, CEDULA, ID_ESTADO) VALUES (GETDATE(), @p0, 1)",
                cedula
            );

            return contexto.Database.SqlQuery<int>(
                "SELECT ID_EMPLEADO FROM PUBROCK_EMPLEADO_TB WHERE CEDULA = @p0",
                cedula
            ).First();
        }

        public static int ObtenerIdEstadoActivo(Contexto contexto)
        {
            return contexto.Database.SqlQuery<int?>(
                "SELECT TOP 1 ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Activo'"
            ).FirstOrDefault() ?? 1;
        }
    }
}
