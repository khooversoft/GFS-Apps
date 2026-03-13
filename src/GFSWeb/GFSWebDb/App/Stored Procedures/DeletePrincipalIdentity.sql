CREATE PROCEDURE [App].[DeletePrincipalIdentity]
    @NameIdentifier nvarchar(50)
AS
BEGIN
    DELETE FROM [AppDbo].[PrincipalIdentity]
    WHERE [NameIdentifier] = @NameIdentifier
END
