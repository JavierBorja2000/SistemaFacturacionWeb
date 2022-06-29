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