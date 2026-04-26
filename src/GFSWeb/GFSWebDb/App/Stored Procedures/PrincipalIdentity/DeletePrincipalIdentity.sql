CREATE PROCEDURE [App].[DeletePrincipalIdentity]
    @NameIdentifier nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE FROM [AppDbo].[PrincipalIdentity]
    WHERE [NameIdentifier] = @NameIdentifier
END
