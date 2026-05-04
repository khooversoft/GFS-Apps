CREATE PROCEDURE [App].[UpdatePrincipalGroup]
    @GroupName NVARCHAR(50),
    @Description NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    UPDATE [AppDbo].[PrincipalGroup]
    SET [Description] = @Description
    WHERE [GroupName] = @GroupName;

END
