CREATE PROCEDURE [App].[UpdateMenu]
    @MenuId NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT OFF; -- needed so @@ROWCOUNT reflects UPDATE rows
    SET XACT_ABORT ON;

    UPDATE [AppDbo].[Menu]
        SET [Description] = @Description
    WHERE [MenuId] = @MenuId;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Menu record does not exist for the specified @MenuId', 16, 1);
        RETURN;
    END
END
