CREATE PROCEDURE [App].[DeletePrincipalGroup]
    @GroupName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE [AppDbo].[PrincipalGroup]
    WHERE  [GroupName] = @GroupName;
END