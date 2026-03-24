CREATE PROCEDURE [App].[DeleteMenu]
    @MenuId nvarchar(50)
AS
BEGIN
    DELETE FROM [AppDbo].[Menu]
    WHERE [MenuId] = @MenuId
END
