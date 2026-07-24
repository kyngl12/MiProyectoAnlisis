# Solución: Error en Generación de Balance
**Fecha**: 2026-07-23
**Problema**: The cast to value type 'System.Decimal' failed because the materialized value is null
**Raíz Causa**: Las consultas SQL retornan NULL en SUM() cuando no hay registros, pero el DTO esperaba decimal (no-nullable)

## Cambios Aplicados

### 1. BalanceCategoriaDto.cs
- **Antes**: `public decimal Monto { get; set; }`
- **Después**: `public decimal? Monto { get; set; }`
- **Razón**: Permite que EF mapee NULL correctamente cuando no hay registros

### 2. GenerarBalanceAD.cs
- **Línea 69**: `decimal totalIngresosVariables = ingresosPorCategoria.Sum(i => i.Monto ?? 0m);`
- **Línea 70**: `decimal totalEgresos = egresosPorCategoria.Sum(e => e.Monto ?? 0m);`
- **Razón**: Usa null-coalescing (??) para convertir NULL a 0 al calcular totales

### 3. Index.cshtml (Vista Balance)
- **Línea 82**: `<td>@((fila.Monto ?? 0m).ToString("C2"))</td>`
- **Razón**: Maneja el valor nullable antes de formatearlo como moneda

## Comportamiento Después del Cambio
✓ Si el rango de fechas NO tiene ingresos/egresos: aparece $0.00  
✓ Si el rango de fechas SÍ tiene ingresos/egresos: aparece el monto correcto  
✓ No más error de "The cast to value type 'System.Decimal' failed"  

## Testing
Prueba generando un balance en una fecha sin transacciones:
1. Navega a: **Contabilidad → Balance**
2. Selecciona un rango de fechas SIN egresos ni ingresos (ej: hace 6 meses)
3. Presiona **Generar Balance**
4. **Resultado esperado**: La tabla se genera con $0.00 sin errores
