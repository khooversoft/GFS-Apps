CREATE PROCEDURE [App].[AddOrUpdateElimOpr]
    @ElimCode nvarchar(50),
    @Description nvarchar(100)
AS
BEGIN
    MERGE [AppDbo].[ElimOperation] AS target
    USING (VALUES (@ElimCode, @Description)) AS src ([ElimCode], [Description])
        ON target.[ElimCode] = src.[ElimCode]
    WHEN MATCHED THEN
        UPDATE SET
            [Description] = src.[Description]
    WHEN NOT MATCHED THEN
        INSERT ([ElimCode], [Description])
        VALUES (src.[ElimCode], src.[Description]);
END