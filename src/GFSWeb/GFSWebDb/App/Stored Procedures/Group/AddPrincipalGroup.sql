CREATE PROCEDURE [App].[AddPrincipalGroup]
    @GroupName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    INSERT INTO [AppDbo].[PrincipalGroup] ([GroupName]) VALUES (@GroupName);
END