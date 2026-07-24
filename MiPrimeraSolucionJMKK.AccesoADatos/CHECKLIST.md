# CHECKLIST - Resolución de Error "Error en el sistema"

## ESTADO ACTUAL
✓ Código compilado correctamente  
✓ DTOs actualizados con IdEmpleado  
✓ Controlador preparado para cargar empleados  
✓ Scripts de diagnóstico creados  

## PRÓXIMOS PASOS - ANTES DE PROBAR

### PASO 1: Diagnosticar Base de Datos (USUARIO)
**Archivo**: `MiPrimeraSolucionJMKK.AccesoADatos/Diagnostico_BD.sql`
**Instrucciones**:
1. Abre SQL Server Management Studio
2. Conéctate a: `DESKTOP-9J1P7PN` (Trusted Connection / Windows Auth)
3. Abre el script `Diagnostico_BD.sql`
4. Verifica estás en BD: `PUBROCK_CR`
5. Ejecuta el script (F5)
6. **ANOTA LOS RESULTADOS**, especialmente:
   - ¿Cantidad_Egreso = 0 o 1?
   - ¿Existen las columnas en PUBROCK_MOVIMIENTO_FINANCIERO_TB?
   - ¿Existe la columna MODULO en PUBROCK_BITACORA_TB?

### PASO 2: Corregir Base de Datos si es Necesario (USUARIO)
**IF Cantidad_Egreso = 0:**
1. Abre el archivo: `MiPrimeraSolucionJMKK.AccesoADatos/Correccion_BD.sql`
2. Verifica estás en BD: `PUBROCK_CR`
3. Ejecuta el script (F5)
4. Espera a ver: "Tipo Egreso insertado correctamente"
5. Ejecuta nuevamente Diagnostico_BD.sql para confirmar que ahora existe

**IF Cantidad_Egreso >= 1:**
✓ No es necesario hacer nada en BD

### PASO 3: Limpiar Caché en Visual Studio (USUARIO)
1. En Visual Studio, **detén el debugger** (si está corriendo)
   - Presiona Shift+F5 o cierra la ventana del navegador
2. Menú: **Build → Clean Solution**
3. Espera a que termine
4. Menú: **Build → Build Solution** (o Ctrl+Shift+B)
5. Espera a que compile correctamente

### PASO 4: Prueba End-to-End (USUARIO)
1. Inicia la aplicación: F5
2. Navega a: **Contabilidad → Egresos → Agregar Nuevo**
3. Llena el formulario:
   - Monto: 100.00
   - Descripción: Test
   - **Empleado**: Selecciona uno del dropdown (IMPORTANTE)
   - Presiona **Guardar**
4. **RESULTADO ESPERADO**:
   - ✓ "El gasto se registró de manera exitosa."
   - ✓ Redirige a Index
   - ✓ El registro aparecelistado

5. **SI FALLA**:
   - Verifica App_Data/errors.log
   - Copia el error exacto
   - Vuelve al Script Diagnostico_BD.sql y confirma que 'Egreso' existe

## ARCHIVOS IMPORTANTES

| Archivo | Propósito |
|---------|----------|
| Diagnostico_BD.sql | Verificar estado de BD |
| Correccion_BD.sql | Insertar tipos faltantes |
| README_DIAGNOSTICO.md | Guía detallada |
| EgresoDto.cs | DTO con IdEmpleado |
| EgresosController.cs | Submit form |
| RegistrarEgresoAD.cs | Valida y guarda |
| Web.config | Connection string |

## SOLUCIÓN DE PROBLEMAS

### Error: "Invalid column name 'MONTO_CONTADO'"
- Es un error heredado de estado anterior de BD  
- Ejecuta Diagnostico_BD.sql para ver todas las columnas
- El código nuevo usa MONTO (sin CONTADO)

### Error: "Invalid object name 'PUBROCK_CUENTA_POR_PAGAR_TB'"
- Es en bitácora, ya está en código como mitigation
- Si persiste, verifica que no haya otras consultas usando esa tabla

### Error: "No fue posible identificar al empleado"
- El form NO está enviando IdEmpleado
- Verifica que el dropdown está nombrado `IdEmpleado` en HTML
- Check Browser DevTools > Network para ver POST payload

### Error: "SqlException - column does not exist"
- Ejecuta Diagnostico_BD.sql
- Compara columnas listadas contra lo que el código espera
- Avisa si encuentras diferencias

## CONTACTO
Si después de todo persisten errores, comparte:
1. Resultado de Diagnostico_BD.sql (captura)
2. Contenido de App_Data/errors.log
3. Mensaje de error exacto en la UI
