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