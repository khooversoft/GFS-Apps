CREATE PROCEDURE [dbo].[DeletePrincipalIdentity]
    @PrincipalId nvarchar(50)
AS
BEGIN
    DELETE FROM [AppDbo].[PrincipalIdentity]
    WHERE [PrincipalId] = @PrincipalId
END
