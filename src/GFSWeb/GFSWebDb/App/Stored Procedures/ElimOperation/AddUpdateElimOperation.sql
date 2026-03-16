CREATE PROCEDURE [App].[AddUpdateElimOperation]
    @ElimCode nvarchar(50) NOT NULL,
    @Description nvarchar(100) NOT NULL,
    @Data nvarchar(max) NULL = NULL,
    @Disabled BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    MERGE [AppDbo].[ElimOperation] AS target
    USING (
        SELECT
            @ElimCode AS [ElimCode],
            @Description AS [Description],
            @Data AS [Data],
            @Disabled AS [Disabled]
    ) AS source
    ON target.[ElimCode] = source.[ElimCode]
    WHEN MATCHED THEN
        UPDATE
        SET target.[Description] = source.[Description],
            target.[Data] = source.[Data],
            target.[Disabled] = source.[Disabled]
    WHEN NOT MATCHED THEN
        INSERT ([ElimCode], [Description], [Data], [Disabled])
        VALUES (source.[ElimCode], source.[Description], source.[Data], source.[Disabled]);

    COMMIT TRAN;
END