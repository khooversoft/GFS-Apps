CREATE PROCEDURE [App].[UpdateMenu]
    @MenuId nvarchar(50),
    @Description nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF NOT EXISTS (SELECT 1 FROM [AppDbo].[Menu] WHERE [MenuId] = @MenuId)
    BEGIN
        RAISERROR('Menu record does not exist for the specified @MenuId', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    UPDATE [AppDbo].[Menu]
        SET [Description] = @Description
    WHERE [MenuId] = @MenuId;

    COMMIT TRAN;
END
