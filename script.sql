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