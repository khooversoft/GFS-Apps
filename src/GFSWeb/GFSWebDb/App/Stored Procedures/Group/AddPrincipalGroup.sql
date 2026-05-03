CREATE PROCEDURE [App].[AddPrincipalGroup]
    @GroupName NVARCHAR(50),
    @Description NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    INSERT INTO [AppDbo].[PrincipalGroup] ([GroupName], [Description]) VALUES
    (@GroupName, @Description);
END