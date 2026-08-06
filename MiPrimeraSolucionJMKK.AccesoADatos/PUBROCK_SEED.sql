/* =====================================================================
   PUBROCK_SEED.sql
   Datos de prueba realistas para PUBROCK_CR (restaurante en Costa Rica)
   Requiere haber ejecutado PUBROCK_CR.sql previamente (usa los estados
   y tipos de usuario ya sembrados por ese script: ID_ESTADO 1=Activo,
   2=Inactivo; ID_TIPO_USUARIO 1=Administrador, 2=Empleado, 3=Cliente,
   4=Proveedor).
   Todos los INSERT respetan el orden de dependencias de llaves foraneas.
   ===================================================================== */

USE PUBROCK_CR;
GO

-- =====================================================================
-- 1. UBICACIONES: Costa Rica > Provincias > Cantones > Distritos
-- =====================================================================
INSERT INTO PUBROCK_PAIS_TB (NOMBRE, ID_ESTADO) VALUES
('Costa Rica', 1);
GO

INSERT INTO PUBROCK_PROVINCIA_TB (NOMBRE, ID_PAIS, ID_ESTADO) VALUES
('San Jose', 1, 1),
('Cartago', 1, 1),
('Alajuela', 1, 1);
GO

INSERT INTO PUBROCK_CANTON_TB (NOMBRE, ID_PROVINCIA, ID_ESTADO) VALUES
('San Jose Centro', 1, 1),
('Paraiso', 2, 1),
('Alajuela Centro', 3, 1);
GO

INSERT INTO PUBROCK_DISTRITO_TB (NOMBRE, ID_CANTON, ID_ESTADO) VALUES
('Carmen', 1, 1),
('Paraiso', 2, 1),
('Alajuela', 3, 1);
GO

-- =====================================================================
-- 2. PUESTOS, ROLES Y TIPOS DE USUARIO ADICIONALES
-- (PUBROCK_TIPO_USUARIO_TB ya viene sembrado desde PUBROCK_CR.sql)
-- =====================================================================
INSERT INTO PUBROCK_PUESTO_TB (NOMBRE_PUESTO, DESCRIPCION, ID_ESTADO) VALUES
('Gerente', 'Responsable general del restaurante', 1),
('Chef', 'Encargado de cocina', 1),
('Mesero', 'Atencion a mesas y toma de pedidos', 1),
('Cajero', 'Cobro y manejo de caja', 1);
GO

INSERT INTO PUBROCK_ROL_TB (NOMBRE_ROL, DESCRIPCION, ID_ESTADO) VALUES
('Administrador', 'Acceso total al sistema', 1),
('Cocina', 'Acceso al modulo de ordenes y cocina', 1),
('Meseros', 'Acceso al modulo de ordenes y mesas', 1),
('Caja', 'Acceso al modulo de ventas y facturacion', 1);
GO

-- =====================================================================
-- 3. USUARIOS (personas fisicas: empleados, clientes, proveedores)
-- El usuario administrador (cedula 123456789) ya fue insertado por
-- PUBROCK_CR.sql.
-- =====================================================================
INSERT INTO PUBROCK_USUARIO_TB (CEDULA, NOMBRE, APELLIDO_PATERNO, APELLIDO_MATERNO, FECHA_REGISTRO, ID_TIPO_USUARIO, CORREO, TELEFONO, CONTRASENIA, ID_ESTADO) VALUES
('204560123', 'Marco',    'Jimenez',  'Solano',   '2024-02-01', 2, 'marco.jimenez@pubrock.com',   '87001122', 'Chef2024',    1),
('205670234', 'Andrea',   'Vargas',   'Mora',     '2024-02-15', 2, 'andrea.vargas@pubrock.com',   '87002233', 'Mesera2024',  1),
('206780345', 'Luis',     'Castro',   'Rojas',    '2024-03-01', 2, 'luis.castro@pubrock.com',     '87003344', 'Caja2024',    1),
('207890456', 'Gabriela', 'Fonseca',  'Chinchilla','2024-01-10', 2, 'gabriela.fonseca@pubrock.com','87004455', 'Gerente2024', 1),
('109870001', 'Jose',     'Ramirez',  'Alvarado', '2024-04-05', 3, 'jose.ramirez@gmail.com',      '88101122', NULL,          1),
('110870002', 'Mariana',  'Solis',    'Brenes',   '2024-04-10', 3, 'mariana.solis@gmail.com',     '88102233', NULL,          1),
('111870003', 'Diego',    'Araya',    'Quiros',   '2024-05-02', 3, 'diego.araya@gmail.com',       '88103344', NULL,          1),
('301230001', 'Carlos',   'Mendez',   'Ugalde',   '2024-01-20', 4, 'ventas@distribuidoralacentral.cr', '22201122', NULL,     1),
('302340002', 'Silvia',   'Barrantes','Leon',     '2024-01-25', 4, 'contacto@carnesselectascr.com',    '22202233', NULL,     1);
GO

-- Direcciones para el administrador y los empleados principales
INSERT INTO PUBROCK_DIRECCION_TB (CEDULA, ID_PAIS, ID_PROVINCIA, ID_CANTON, ID_DISTRITO, OTRAS_SENAS, ID_ESTADO) VALUES
('123456789', 1, 1, 1, 1, '100 metros norte del Parque Morazan', 1),
('204560123', 1, 2, 2, 2, 'Barrio San Rafael, casa esquinera', 1),
('207890456', 1, 3, 3, 3, 'Residencial Los Almendros, casa 12', 1);
GO

INSERT INTO PUBROCK_TELEFONO_TB (CEDULA, TELEFONO, ID_ESTADO) VALUES
('123456789', '88888888', 1),
('204560123', '87001122', 1),
('205670234', '87002233', 1);
GO

INSERT INTO PUBROCK_CORREO_ELECTRONICO_TB (CEDULA, CORREO, ID_ESTADO) VALUES
('123456789', 'admin@pubrock.com', 1),
('301230001', 'facturacion@distribuidoralacentral.cr', 1);
GO

-- =====================================================================
-- 4. EMPLEADOS Y USUARIOS DEL SISTEMA
-- =====================================================================
INSERT INTO PUBROCK_EMPLEADO_TB (FECHA_INGRESO, CEDULA, ID_PUESTO, ID_ESTADO) VALUES
('2024-01-10', '207890456', 1, 1),  -- Gerente
('2024-02-01', '204560123', 2, 1),  -- Chef
('2024-02-15', '205670234', 3, 1),  -- Mesera
('2024-03-01', '206780345', 4, 1);  -- Cajero
GO

INSERT INTO PUBROCK_USUARIO_SISTEMA_TB (NOMBRE_USUARIO, CLAVE, FECHA_REGISTRO, ID_EMPLEADO, ID_ROL, ID_ESTADO) VALUES
('gfonseca', 'HashGerente2024',  '2024-01-10', 1, 1, 1),
('mjimenez', 'HashChef2024',     '2024-02-01', 2, 2, 1),
('avargas',  'HashMesera2024',   '2024-02-15', 3, 3, 1),
('lcastro',  'HashCajero2024',   '2024-03-01', 4, 4, 1);
GO

INSERT INTO PUBROCK_REGISTRO_ACTIVIDAD_TB (FECHA_HORA, ACCION_REALIZADA, DESCRIPCION, ID_USUARIO_SISTEMA, ID_ESTADO) VALUES
('2024-06-01 08:05:00', 'INICIO_SESION', 'Ingreso al sistema', 1, 1),
('2024-06-01 09:15:00', 'REGISTRO_VENTA', 'Registro de venta #1', 4, 1),
('2024-06-01 18:30:00', 'CIERRE_CAJA',   'Cierre de caja del turno', 4, 1);
GO

-- =====================================================================
-- 5. CLIENTES Y PROVEEDORES
-- =====================================================================
INSERT INTO PUBROCK_CLIENTE_TB (FECHA_INGRESO_CLIENTE, CEDULA, ID_ESTADO) VALUES
('2024-04-05', '109870001', 1),
('2024-04-10', '110870002', 1),
('2024-05-02', '111870003', 1);
GO

INSERT INTO PUBROCK_PROVEEDOR_TB (CONTACTO_PRINCIPAL, FECHA_INGRESO_PROVEEDOR, CEDULA, ID_ESTADO) VALUES
('Carlos Mendez',    '2024-01-20', '301230001', 1),
('Silvia Barrantes',  '2024-01-25', '302340002', 1);
GO

-- =====================================================================
-- 6. CATEGORIAS, PRODUCTOS E INVENTARIO
-- =====================================================================
INSERT INTO PUBROCK_CATEGORIA_PRODUCTO_TB (NOMBRE_CATEGORIA, DESCRIPCION, ID_ESTADO) VALUES
('Entradas',      'Boquitas y entradas para compartir', 1),
('Platos Fuertes','Platos principales de la carta', 1),
('Bebidas',       'Bebidas frias y calientes sin alcohol', 1),
('Cervezas',      'Cervezas artesanales y comerciales', 1),
('Postres',       'Postres de la casa', 1);
GO

INSERT INTO PUBROCK_PRODUCTO_TB (NOMBRE_PRODUCTO, DESCRIPCION, CODIGO_BARRAS, PRECIO_VENTA, FECHA_VENCIMIENTO, ID_CATEGORIA_PRODUCTO, ID_PROVEEDOR, ID_ESTADO) VALUES
('Alitas BBQ (8 uds)',        'Alitas de pollo banadas en salsa BBQ',        NULL, 5200.00, NULL, 1, 2, 1),
('Nachos PUBROCK',            'Nachos con queso, frijoles y pico de gallo',  NULL, 4500.00, NULL, 1, 2, 1),
('Hamburguesa Rockera',       'Carne de res 200g, queso cheddar y tocineta', '7501234567891', 6800.00, NULL, 2, 2, 1),
('Costillas BBQ',             'Costillas de cerdo ahumadas',                 NULL, 8900.00, NULL, 2, 2, 1),
('Limonada Natural',          'Limonada fresca de la casa',                  NULL, 1800.00, NULL, 3, 1, 1),
('Refresco de Cola',          'Bebida gaseosa 350ml',                        '7501234567892', 1500.00, '2026-12-31', 3, 1, 1),
('Cerveza Artesanal IPA',     'Cerveza artesanal costarricense 355ml',       '7501234567893', 2800.00, '2026-10-31', 4, 1, 1),
('Cheesecake de Maracuya',    'Postre de la casa con maracuya',              NULL, 3200.00, NULL, 5, 2, 1);
GO

INSERT INTO PUBROCK_INVENTARIO_TB (STOCK_ACTUAL, STOCK_MINIMO, STOCK_MAXIMO, FECHA_ULTIMA_ACTUALIZACION, ID_PRODUCTO, ID_ESTADO) VALUES
(40, 10, 100, GETDATE(), 1, 1),
(35, 10, 100, GETDATE(), 2, 1),
(50, 15, 150, GETDATE(), 3, 1),
(25, 10, 80,  GETDATE(), 4, 1),
(60, 20, 200, GETDATE(), 5, 1),
(120,30, 300, GETDATE(), 6, 1),
(90, 24, 240, GETDATE(), 7, 1),
(20, 5,  60,  GETDATE(), 8, 1);
GO

INSERT INTO PUBROCK_TIPO_MOVIMIENTO_INVENTARIO_TB (DESCRIPCION, ID_ESTADO) VALUES
('Entrada por compra', 1),
('Salida por venta',   1),
('Ajuste de inventario', 1);
GO

INSERT INTO PUBROCK_MOVIMIENTO_INVENTARIO_TB (FECHA_MOVIMIENTO, CANTIDAD, OBSERVACION, ID_INVENTARIO, ID_TIPO_MOVIMIENTO, ID_ESTADO) VALUES
('2024-06-01', 50, 'Ingreso de nachos por compra a proveedor',        2, 1, 1),
('2024-06-01', -3, 'Salida por venta del dia',                        2, 2, 1),
('2024-06-02', -1, 'Ajuste por producto danado en cocina',            5, 3, 1);
GO

-- =====================================================================
-- 7. COMPRAS
-- =====================================================================
INSERT INTO PUBROCK_COMPRA_TB (FECHA_COMPRA, TOTAL_COMPRA, ID_PROVEEDOR, ID_EMPLEADO, ID_ESTADO) VALUES
('2024-05-28', 185000.00, 2, 1, 1),
('2024-05-30',  95000.00, 1, 1, 1);
GO

INSERT INTO PUBROCK_DETALLE_COMPRA_TB (CANTIDAD, COSTO_UNITARIO, SUBTOTAL, ID_COMPRA, ID_PRODUCTO, ID_ESTADO) VALUES
(50, 2200.00, 110000.00, 1, 4, 1),
(40, 1875.00,  75000.00, 1, 3, 1),
(120, 500.00,  60000.00, 2, 6, 1),
(90,  388.89,  35000.00, 2, 7, 1);
GO

-- =====================================================================
-- 8. VENTAS Y FACTURACION
-- =====================================================================
INSERT INTO PUBROCK_VENTA_TB (FECHA_VENTA, TOTAL_VENTA, ID_CLIENTE, ID_EMPLEADO, ID_ESTADO) VALUES
('2024-06-01', 13600.00, 1, 3, 1),
('2024-06-01',  9600.00, 2, 3, 1),
('2024-06-02', 11700.00, 3, 3, 1);
GO

INSERT INTO PUBROCK_DETALLE_VENTA_TB (CANTIDAD, PRECIO_UNITARIO, SUBTOTAL, ID_VENTA, ID_PRODUCTO, ID_ESTADO) VALUES
(1, 6800.00, 6800.00, 1, 3, 1),
(1, 5200.00, 5200.00, 1, 1, 1),
(1, 1800.00, 1800.00, 1, 5, 1),
(2, 4500.00, 9000.00, 2, 2, 1),
(1, 1500.00, 1500.00, 2, 6, 1),
(1, 8900.00, 8900.00, 3, 4, 1),
(1, 2800.00, 2800.00, 3, 7, 1);
GO

INSERT INTO PUBROCK_TIPO_PAGO_TB (DESCRIPCION, ID_ESTADO) VALUES
('Efectivo', 1),
('Tarjeta',  1),
('SINPE Movil', 1);
GO

INSERT INTO PUBROCK_FACTURA_TB (NUMERO_FACTURA, FECHA_FACTURA, MONTO_TOTAL, ID_VENTA, ID_TIPO_PAGO, ID_ESTADO) VALUES
('FE-0001-2024', '2024-06-01', 13600.00, 1, 2, 1),
('FE-0002-2024', '2024-06-01',  9600.00, 2, 1, 1),
('FE-0003-2024', '2024-06-02', 11700.00, 3, 3, 1);
GO

-- =====================================================================
-- 9. DEVOLUCIONES
-- =====================================================================
INSERT INTO PUBROCK_TIPO_DEVOLUCION_TB (DESCRIPCION, ID_ESTADO) VALUES
('Producto en mal estado', 1),
('Error en el pedido',      1);
GO

INSERT INTO PUBROCK_DEVOLUCION_TB (FECHA_DEVOLUCION, MOTIVO_GENERAL, ID_VENTA, ID_CLIENTE, ID_EMPLEADO, ID_TIPO_DEVOLUCION, ID_ESTADO) VALUES
('2024-06-02', 'Cliente reporto la bebida derramada al momento de servirla', 2, 2, 3, 2, 1);
GO

INSERT INTO PUBROCK_DETALLE_DEVOLUCION_TB (CANTIDAD, MONTO_DEVUELTO, ID_DEVOLUCION, ID_PRODUCTO, ID_ESTADO) VALUES
(1, 1500.00, 1, 6, 1);
GO

-- =====================================================================
-- 10. CAJA Y MOVIMIENTOS FINANCIEROS
-- =====================================================================
INSERT INTO PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB (DESCRIPCION, ID_ESTADO) VALUES
('Ingreso por venta', 1),
('Pago a proveedor',  1),
('Gasto operativo',   1);
GO

INSERT INTO PUBROCK_MOVIMIENTO_FINANCIERO_TB (FECHA_MOVIMIENTO, DESCRIPCION, MONTO, ID_TIPO_MOVIMIENTO_FINANCIERO, ID_VENTA, ID_EMPLEADO, ID_ESTADO) VALUES
('2024-06-01', 'Ingreso por venta #1', 13600.00, 1, 1, 3, 1),
('2024-05-30', 'Pago a Distribuidora La Central', 95000.00, 2, NULL, 1, 1),
('2024-06-01', 'Compra de servilletas y articulos de limpieza', 15000.00, 3, NULL, 1, 1);
GO

INSERT INTO PUBROCK_CAJA_TB (FECHA_APERTURA, MONTO_INICIAL, FECHA_CIERRE, MONTO_FINAL, DIFERENCIA, ID_EMPLEADO, ID_ESTADO) VALUES
('2024-06-01', 50000.00, '2024-06-01', 84900.00, 0.00, 4, 1);
GO

INSERT INTO PUBROCK_CIERRE_CAJA_TB (FECHA_CIERRE, TOTAL_VENTAS, TOTAL_INGRESOS, TOTAL_EGRESOS, BALANCE_FINAL, ID_EMPLEADO, ID_ESTADO) VALUES
('2024-06-01', 23200.00, 23200.00, 0.00, 73200.00, 4, 1);
GO

INSERT INTO PUBROCK_BITACORA_TB (FECHA_HORA, ACCION_REALIZADA, DESCRIPCION, CEDULA, ID_ESTADO) VALUES
('2024-06-01 08:00:00', 'INICIO_SESION', 'Inicio de sesion del gerente', '207890456', 1),
('2024-06-01 18:45:00', 'CIERRE_CAJA',   'Cierre de caja del turno diurno', '206780345', 1);
GO

-- =====================================================================
-- 11. IMPUESTOS
-- =====================================================================
INSERT INTO PUBROCK_IMPUESTO_TB (NOMBRE_IMPUESTO, PORCENTAJE, ID_ESTADO) VALUES
('IVA', 13.00, 1);
GO

INSERT INTO PUBROCK_VENTA_IMPUESTO_TB (ID_VENTA, ID_IMPUESTO, MONTO_IMPUESTO, ID_ESTADO) VALUES
(1, 1, 1768.00, 1),
(2, 1, 1248.00, 1),
(3, 1, 1521.00, 1);
GO

-- =====================================================================
-- 12. PROMOCIONES
-- =====================================================================
INSERT INTO PUBROCK_PROMOCION_TB (NOMBRE_PROMOCION, DESCRIPCION, PORCENTAJE_DESCUENTO, FECHA_INICIO, FECHA_FIN, ID_ESTADO) VALUES
('Happy Hour Cervezas', '20% de descuento en cervezas artesanales de 5pm a 7pm', 20.00, '2024-06-01', '2024-08-31', 1),
('2x1 Nachos Miercoles', 'Segundos nachos gratis todos los miercoles', 50.00, '2024-06-01', '2024-09-30', 1);
GO

INSERT INTO PUBROCK_PROMOCION_PRODUCTO_TB (ID_PROMOCION, ID_PRODUCTO, ID_ESTADO) VALUES
(1, 7, 1),
(2, 2, 1);
GO

-- =====================================================================
-- 13. REPORTES
-- =====================================================================
INSERT INTO PUBROCK_FORMATO_REPORTE_TB (DESCRIPCION, ID_ESTADO) VALUES
('PDF', 1),
('Excel', 1);
GO

INSERT INTO PUBROCK_TIPO_REPORTE_TB (DESCRIPCION, ID_ESTADO) VALUES
('Reporte de ventas', 1),
('Reporte de inventario', 1);
GO

INSERT INTO PUBROCK_REPORTE_TB (FECHA_GENERACION, FECHA_INICIO, FECHA_FIN, ID_TIPO_REPORTE, ID_EMPLEADO, ID_ESTADO, ID_FORMATO_REPORTE) VALUES
(GETDATE(), '2024-06-01', '2024-06-02', 1, 1, 1, 1),
(GETDATE(), '2024-06-01', '2024-06-02', 2, 1, 1, 2);
GO

-- =====================================================================
-- 14. MARKETING
-- =====================================================================
INSERT INTO PUBROCK_MARKETING_TB (TITULO, TIPO_CONTENIDO, DESCRIPCION, FECHA_INICIO, FECHA_FIN, PRECIO, ID_ESTADO) VALUES
('Lanzamiento Costillas BBQ', 'Publicacion Instagram', 'Campana de lanzamiento del nuevo plato de costillas BBQ', '2024-06-01', '2024-06-15', 45000.00, 1),
('Promo Happy Hour', 'Historia Instagram', 'Difusion de la promocion de cervezas artesanales', '2024-06-01', '2024-08-31', 20000.00, 1);
GO

-- =====================================================================
-- 15. MESAS, RESERVACIONES Y HORARIOS
-- =====================================================================
INSERT INTO PUBROCK_MESA_TB (NUMERO_MESA, CAPACIDAD, ID_ESTADO) VALUES
(1, 2, 1),
(2, 4, 1),
(3, 4, 1),
(4, 6, 1),
(5, 8, 1);
GO

INSERT INTO PUBROCK_RESERVACION_TB (FECHA_RESERVACION, HORA_INICIO, HORA_FIN, CANTIDAD_PERSONAS, OBSERVACIONES, ID_CLIENTE, ID_MESA, ID_ESTADO) VALUES
('2024-06-10', '19:00', '21:00', 4, 'Cumpleanos, solicitan mesa cerca de la ventana', 1, 3, 1),
('2024-06-12', '13:00', '14:30', 2, NULL, 2, 1, 1);
GO

INSERT INTO PUBROCK_HORARIO_TB (DIA_SEMANA, HORA_ENTRADA, HORA_SALIDA, ID_EMPLEADO, ID_ESTADO) VALUES
('Lunes',    '08:00', '17:00', 1, 1),
('Lunes',    '10:00', '20:00', 2, 1),
('Martes',   '10:00', '20:00', 3, 1),
('Martes',   '10:00', '20:00', 4, 1);
GO

-- =====================================================================
-- 16. ORDENES
-- =====================================================================
INSERT INTO PUBROCK_ESTADO_ORDEN_TB (DESCRIPCION, ID_ESTADO) VALUES
('Pendiente', 1),
('En preparacion', 1),
('Servido', 1),
('Cancelado', 1);
GO

INSERT INTO PUBROCK_ORDEN_TB (FECHA_ORDEN, OBSERVACIONES, ID_VENTA, ID_ESTADO_ORDEN, ID_ESTADO) VALUES
('2024-06-01', 'Sin cebolla en la hamburguesa', 1, 3, 1),
('2024-06-01', 'Nachos extra picantes', 2, 3, 1),
('2024-06-02', NULL, 3, 3, 1);
GO

INSERT INTO PUBROCK_DETALLE_ORDEN_TB (ID_ORDEN, ID_PRODUCTO, CANTIDAD, OBSERVACION, ID_ESTADO) VALUES
(1, 3, 1, 'Sin cebolla', 1),
(1, 1, 1, NULL, 1),
(1, 5, 1, NULL, 1),
(2, 2, 2, 'Extra picante', 1),
(2, 6, 1, NULL, 1),
(3, 4, 1, NULL, 1),
(3, 7, 1, NULL, 1);
GO

PRINT 'DATOS DE PRUEBA CARGADOS CORRECTAMENTE EN PUBROCK_CR.';
GO
