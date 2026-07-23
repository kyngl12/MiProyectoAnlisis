namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// Codigos de resultado normalizados que devuelven los Flow del
    /// modulo de Ventas. El Controller traduce cada codigo a la
    /// respuesta HTTP correspondiente.
    /// </summary>
    public enum VentaResultadoCodigo
    {
        Ok = 0,
        ProductoNoExiste = 1,
        CantidadInvalida = 2,
        StockInsuficiente = 3,
        SinProductos = 4,
        VentaNoExiste = 5,
        VentaYaPagada = 6,
        VentaAnulada = 7,
        VentaYaAnulada = 8,
        DatosInvalidos = 9,
        ErrorInterno = 10
    }

    /// <summary>
    /// Resultado generico de una operacion de negocio del modulo de
    /// Ventas: indica exito/fracaso, un codigo de error y un mensaje
    /// claro para el usuario, junto con el dato de salida si aplica.
    /// </summary>
    /// <typeparam name="T">Tipo del dato retornado cuando la operacion es exitosa.</typeparam>
    public class VentaOperacionResultDto<T>
    {
        public bool Exitoso { get; set; }
        public VentaResultadoCodigo Codigo { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }

        public static VentaOperacionResultDto<T> Ok(T data)
        {
            return new VentaOperacionResultDto<T> { Exitoso = true, Codigo = VentaResultadoCodigo.Ok, Data = data };
        }

        public static VentaOperacionResultDto<T> Error(VentaResultadoCodigo codigo, string mensaje)
        {
            return new VentaOperacionResultDto<T> { Exitoso = false, Codigo = codigo, Mensaje = mensaje };
        }
    }
}
