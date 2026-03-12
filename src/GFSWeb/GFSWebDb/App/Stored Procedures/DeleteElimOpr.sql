CREATE PROCEDURE [dbo].[DeleteElimOpr]
    @ElimCode nvarchar(50)
AS
BEGIN
    DELETE FROM [AppDbo].[ElimOperation]
    WHERE [ElimCode] = @ElimCode
END