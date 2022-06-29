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
AS SELECT F.Fecha Fecha ,D.Codigo_producto Codigo, P.Nombre Nombre, D.Total_factura Total
			FROM Facturas F Inner Join Detalle_Facturas D
			On F.Numero_factura = D.Numero_factura Inner Join Productos P
			On D.Codigo_producto = P.Codigo_producto
			WHERE F.Anulada = 'N'
			Group by P.Nombre, F.Fecha, D.Codigo_producto,D.Total_factura
			
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
AS SELECT F.Fecha Fecha ,D.Codigo_producto Codigo, P.Nombre Nombre,  SUM(D.Total_factura) Total
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

-- VISTAS REPORTE CLIENTE

--con parametros condicion
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
		@Nombre nvarchar(MAX)

AS
BEGIN

IF(@Fecha1 = '') and (@Nombre = '')
		SELECT * FROM V_ReporteCliente v	
		Order By v.Fecha
			
ELSE IF(@Fecha1 = '')
		SELECT * FROM V_ReporteCliente v
		WHERE v.Nombres = @Nombre
		Order By v.Fecha
			
ELSE IF(@Nombre = '')
		SELECT * FROM V_ReporteCliente v
			WHERE v.Fecha BETWEEN @Fecha1 AND @Fecha2 
			Order By v.Fecha
ELSE
SELECT * FROM V_ReporteCliente v
			WHERE v.Fecha BETWEEN @Fecha1 AND @Fecha2 AND v.Nombres = @Nombre
			Order By v.Fecha
END
GO
