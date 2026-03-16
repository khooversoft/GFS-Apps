CREATE PROCEDURE [App].[AddElimOperation]
    @ElimCode nvarchar(50) NOT NULL,
    @Description nvarchar(100) NOT NULL,
    @Data nvarchar(max) NULL = NULL,
    @Disabled BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF EXISTS (SELECT 1 FROM [AppDbo].[ElimOperation] WHERE [ElimCode] = @ElimCode)
    BEGIN
        RAISERROR('ElimOperation record already exists for the specified @ElimCode', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    INSERT [AppDbo].[ElimOperation] ([ElimCode], [Description], [Data], [Disabled])
    VALUES (@ElimCode, @Description, @Data, @Disabled);

    COMMIT TRAN;
END