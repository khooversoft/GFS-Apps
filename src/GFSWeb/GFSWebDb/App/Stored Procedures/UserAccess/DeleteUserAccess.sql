CREATE PROCEDURE [App].[DeleteUserAccess]
    @UserId int
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE FROM [AppDbo].[UserAccess]
    WHERE [UserId] = @UserId;
END