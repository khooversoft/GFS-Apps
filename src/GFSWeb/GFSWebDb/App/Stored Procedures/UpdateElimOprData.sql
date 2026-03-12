CREATE PROCEDURE [App].[UpdateElimOprData]
    @ElimOprId nvarchar(50),
    @Data nvarchar(1000) NULL
AS
BEGIN
    UPDATE [AppDbo].[ElimOperation]
    SET [Data] = @Data
    WHERE [ElimOprId] = @ElimOprId

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR ('No elimination operation found for ElimOprId: %s', 16, 1, @ElimOprId);
    END
END