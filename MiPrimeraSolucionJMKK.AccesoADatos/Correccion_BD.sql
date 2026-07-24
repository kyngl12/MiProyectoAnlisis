-- Script de Corrección para PUBROCK_CR
-- Ejecuta este script si el diagnóstico muestra que falta el tipo 'Egreso'

-- 1. Insertar el tipo de movimiento 'Egreso' si no existe
IF NOT EXISTS (SELECT 1 FROM PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB WHERE DESCRIPCION = 'Egreso')
BEGIN
	INSERT INTO PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB (DESCRIPCION, ID_ESTADO)
	VALUES ('Egreso', 1);
	SELECT 'Tipo Egreso insertado correctamente' AS [Resultado];
END
ELSE
BEGIN
	SELECT 'El tipo Egreso ya existe' AS [Resultado];
END

-- 2. Insertar el tipo de movimiento 'Ingreso' si no existe
IF NOT EXISTS (SELECT 1 FROM PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB WHERE DESCRIPCION = 'Ingreso')
BEGIN
	INSERT INTO PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB (DESCRIPCION, ID_ESTADO)
	VALUES ('Ingreso', 1);
	SELECT 'Tipo Ingreso insertado correctamente' AS [Resultado];
END
ELSE
BEGIN
	SELECT 'El tipo Ingreso ya existe' AS [Resultado];
END

-- 3. Verificar que los tipos fueron insertados
SELECT 
	ISNULL(ID_TIPO_MOVIMIENTO_FINANCIERO, 'N/A') AS [TipoID],
	DESCRIPCION,
	ID_ESTADO
FROM PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB
WHERE DESCRIPCION IN ('Egreso', 'Ingreso')
ORDER BY DESCRIPCION;
