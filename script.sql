-- SP DE LOGIN
IF OBJECT_ID('sp_VerificarLogin', 'P') IS NOT NULL
DROP PROC sp_VerificarLogin
GO

CREATE PROCEDURE [dbo].[sp_VerificarLogin]
    @email nvarchar(MAX),
    @password nvarchar(MAX)
AS
BEGIN
    SELECT *
    FROM Usuarios
    WHERE Email = @email AND Password = @password
END
GO


-- VISTAS REPORTE PRODUCTO

--sin parametros condicion
IF EXISTS(
	SELECT 1
	FROM sys.views
	WHERE name = 'V_ReporteProducto_CampoVacio' and type = 'v'
)

DROP VIEW V_ReporteProducto_CampoVacio
GO

CREATE VIEW V_ReporteProducto_CampoVacio
AS SELECT F.Fecha Fecha ,D.Codigo_producto Codigo, P.Nombre Nombre, SUM(D.Precio * D.Cantidad) Total
			FROM Facturas F Inner Join Detalle_Facturas D
			On F.Numero_factura = D.Numero_factura Inner Join Productos P
			On D.Codigo_producto = P.Codigo_producto
			WHERE F.Anulada = 'N'
			Group by P.Nombre, F.Fecha, D.Codigo_producto,D.Precio, D.Cantidad
			
GO
--con parametros condicion
IF EXISTS(
	SELECT 1
	FROM sys.views
	WHERE name = 'V_ReporteProducto_CampoLLeno' and type = 'v'
)

DROP VIEW V_ReporteProducto_CampoLLeno
GO

CREATE VIEW V_ReporteProducto_CampoLLeno
AS SELECT F.Fecha Fecha ,D.Codigo_producto Codigo, P.Nombre Nombre,  SUM(D.Precio * D.Cantidad) Total
			FROM Facturas F Inner Join Detalle_Facturas D
			On F.Numero_factura = D.Numero_factura Inner Join Productos P
			On D.Codigo_producto = P.Codigo_producto
			WHERE F.Anulada = 'N'
			Group by P.Nombre, F.Fecha, D.Codigo_producto
			
			
GO

-- SP DE reporte por producto
IF OBJECT_ID('sp_ReporteProducto', 'P') IS NOT NULL
DROP PROC sp_ReporteProducto
GO

CREATE PROCEDURE [dbo].[sp_ReporteProducto]
		@Fecha1 date,
		@Fecha2 date,
		@Nombre nvarchar(MAX)

AS
BEGIN

IF(@Fecha1 = '') and (@Nombre = '')
		SELECT * FROM V_ReporteProducto_CampoVacio v	
		Order By v.Fecha
			
ELSE IF(@Fecha1 = '')
		SELECT * FROM V_ReporteProducto_CampoLLeno v
		WHERE v.Nombre = @Nombre
		Order By v.Fecha
			
ELSE IF(@Nombre = '')
		SELECT * FROM V_ReporteProducto_CampoLLeno v
			WHERE v.Fecha BETWEEN @Fecha1 AND @Fecha2 
			Order By v.Fecha
ELSE
SELECT * FROM V_ReporteProducto_CampoLLeno v
			WHERE v.Fecha BETWEEN @Fecha1 AND @Fecha2 AND v.Nombre = @Nombre
			Order By v.Fecha
END
GO

-- VISTA REPORTE CLIENTE

IF EXISTS(
	SELECT 1
	FROM sys.views
	WHERE name = 'V_ReporteCliente' and type = 'v'
)

DROP VIEW V_ReporteCliente
GO

CREATE VIEW V_ReporteCliente
AS SELECT F.Fecha Fecha , C.Nombres Nombres,C.Apellidos Apellidos,  SUM(F.Total_factura) Total
			FROM Facturas F Inner Join Clientes C
			On F.Codigo_cliente = C.Codigo_cliente
			WHERE F.Anulada = 'N'
			Group by F.Fecha, C.Nombres,C.Apellidos
GO

-- SP DE reporte por cliente
IF OBJECT_ID('sp_ReporteCliente', 'P') IS NOT NULL
DROP PROC sp_ReporteCliente
GO

CREATE PROCEDURE [dbo].[sp_ReporteCliente]
		@Fecha1 date,
		@Fecha2 date,
		@Nombre_Completo nvarchar(MAX)

AS
BEGIN

IF(@Fecha1 = '') and (@Nombre_Completo = '')
		SELECT * FROM V_ReporteCliente v	
		Order By v.Fecha
			
ELSE IF(@Fecha1 = '')
		SELECT * FROM V_ReporteCliente v
		WHERE CONCAT(v.Nombres,' ', v.Apellidos) = @Nombre_Completo
		Order By v.Fecha
			
ELSE IF(@Nombre_Completo = '')
		SELECT * FROM V_ReporteCliente v
			WHERE v.Fecha BETWEEN @Fecha1 AND @Fecha2 
			Order By v.Fecha
ELSE
SELECT * FROM V_ReporteCliente v
			WHERE v.Fecha BETWEEN @Fecha1 AND @Fecha2 AND CONCAT(v.Nombres,' ', v.Apellidos) = @Nombre_Completo
			Order By v.Fecha
END
GO

--VISTA DE REPORTE FACTURAS

--Con parametros condicion
IF EXISTS(
	SELECT 1
	FROM sys.views
	WHERE name = 'V_ReporteFacturas' and type = 'v'
)

DROP VIEW V_ReporteFacturas
GO

CREATE VIEW V_ReporteFacturas
AS SELECT F.Fecha Fecha, COUNT(DISTINCT F.Numero_factura) Cantidad_Facturas, SUM(DF.Cantidad*DF.Precio) Total_Facturado, SUM(DF.Cantidad) Cantidad_productos
	FROM Facturas F Inner Join Detalle_Facturas DF
	On F.Numero_factura = DF.Numero_factura
	WHERE F.Anulada = 'N' 
	GROUP BY Fecha
GO

-- SP de reporte por fechas
IF OBJECT_ID('sp_ReporteFacturas', 'P') IS NOT NULL
DROP PROC sp_ReporteFacturas
GO

CREATE PROCEDURE [dbo].[sp_ReporteFacturas]
		@Fecha1 date,
		@Fecha2 date
AS
BEGIN

IF(@Fecha1 = '') 
		SELECT * FROM V_ReporteFacturas v	
		Order By v.Fecha
ELSE
	SELECT * FROM V_ReporteFacturas v
			WHERE v.Fecha BETWEEN @Fecha1 AND @Fecha2 
			Order By v.Fecha
END
GO



-- SP DE ANULAR FACTURA
IF OBJECT_ID('sp_AnularFactura', 'P') IS NOT NULL
DROP PROC sp_AnularFactura
GO

CREATE PROCEDURE [dbo].[sp_AnularFactura]
    @numero_factura int
AS
BEGIN
    UPDATE Facturas
    SET Anulada = 'A'
    WHERE Numero_factura = @numero_factura;

    UPDATE P
    SET Existencia = Existencia + D.Cantidad
    FROM Productos P
		INNER JOIN Detalle_Facturas D ON D.Codigo_producto = P.Codigo_producto
    WHERE Numero_factura = @numero_factura;
END
GO

-- SP DE NO ANULAR FACTURA
IF OBJECT_ID('sp_DeshacerAnularFactura', 'P') IS NOT NULL
DROP PROC sp_DeshacerAnularFactura
GO

CREATE PROCEDURE [dbo].[sp_DeshacerAnularFactura]
    @numero_factura int
AS
BEGIN
    UPDATE Facturas
    SET Anulada = 'N'
    WHERE Numero_factura = @numero_factura;

    UPDATE P
    SET Existencia = Existencia - D.Cantidad
    FROM Productos P
		INNER JOIN Detalle_Facturas D ON D.Codigo_producto = P.Codigo_producto
    WHERE Numero_factura = @numero_factura;
END
GO

-- SP DE CREAR FACTURA
IF OBJECT_ID('sp_CrearFactura', 'P') IS NOT NULL
DROP PROC sp_CrearFactura
GO

CREATE PROCEDURE [dbo].[sp_CrearFactura]
    @numero_factura int,
	@codigo_producto int,
	@cantidad int,
	@precio real
AS
BEGIN
    INSERT INTO Detalle_Facturas(Numero_factura, Codigo_producto, Cantidad, Precio) 
	VALUES (@numero_factura, @codigo_producto, @cantidad, @precio)

	UPDATE P
    SET Existencia = Existencia - D.Cantidad
    FROM Productos P
		INNER JOIN Detalle_Facturas D ON D.Codigo_producto = P.Codigo_producto
    WHERE Numero_factura = @numero_factura and D.Codigo_producto = @codigo_producto;
END
GO

-- SP DE EDITAR FACTURA
IF OBJECT_ID('sp_EditarFactura', 'P') IS NOT NULL
DROP PROC sp_EditarFactura
GO

CREATE PROCEDURE [dbo].[sp_EditarFactura]
    @numero_factura int,
	@codigo_producto int,
	@cantidad int,
	@precio real
AS
BEGIN
	IF EXISTS(SELECT Cantidad
						FROM Detalle_Facturas
						WHERE Numero_factura = @numero_factura 
							AND Codigo_producto = @codigo_producto)
	BEGIN
		IF @cantidad = 0
		BEGIN
			UPDATE P
			SET Existencia = Existencia + D.Cantidad
			FROM Productos P
				INNER JOIN Detalle_Facturas D ON D.Codigo_producto = P.Codigo_producto
			WHERE Numero_factura = @numero_factura and D.Codigo_producto = @codigo_producto

			DELETE FROM Detalle_Facturas
			WHERE Numero_factura = @numero_factura 
			AND Codigo_producto = @codigo_producto
		END
		ELSE
		BEGIN
			DECLARE @diferencia int

			SET @diferencia = (SELECT Cantidad
								FROM Detalle_Facturas
								WHERE Numero_factura = @numero_factura 
									AND Codigo_producto = @codigo_producto) - @cantidad

			UPDATE Detalle_Facturas
			SET Numero_factura = @numero_factura,
				Cantidad = @cantidad
			WHERE Numero_factura = @numero_factura 
									AND Codigo_producto = @codigo_producto;
			
			UPDATE P
			SET Existencia = Existencia + @diferencia
			FROM Productos P
				INNER JOIN Detalle_Facturas D ON D.Codigo_producto = P.Codigo_producto
			WHERE Numero_factura = @numero_factura and D.Codigo_producto = @codigo_producto
		END
	END
	ELSE
	BEGIN
		INSERT INTO Detalle_Facturas(Numero_factura, Codigo_producto, Cantidad, Precio) 
		VALUES (@numero_factura, @codigo_producto, @cantidad, @precio)

		UPDATE P
		SET Existencia = Existencia - D.Cantidad
		FROM Productos P
			INNER JOIN Detalle_Facturas D ON D.Codigo_producto = P.Codigo_producto
		WHERE Numero_factura = @numero_factura and D.Codigo_producto = @codigo_producto
    END
END
GO

