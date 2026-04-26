CREATE PROCEDURE [App].[DeleteMenu]
    @MenuId nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE FROM [AppDbo].[Menu]
    WHERE [MenuId] = @MenuId
END
