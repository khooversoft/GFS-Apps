CREATE PROCEDURE [App].[UpdateElimOperationData]
    @ElimCode nvarchar(50),
    @Data nvarchar(max)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF NOT EXISTS (SELECT 1 FROM [AppDbo].[ElimOperation] WHERE [ElimCode] = @ElimCode)
    BEGIN
        RAISERROR('ElimOperation record does not exist for the specified @ElimCode', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    UPDATE  [AppDbo].[ElimOperation]
    SET     [Data] = @Data
    WHERE   [ElimCode] = @ElimCode;

    COMMIT TRAN;
END