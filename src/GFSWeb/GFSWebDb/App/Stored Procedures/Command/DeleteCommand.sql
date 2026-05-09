CREATE PROCEDURE [App].[DeleteCommand]
    @CommandId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [AppDbo].[Command]
    WHERE CommandId = @CommandId;

END
