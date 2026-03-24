CREATE PROCEDURE [App].[AddOrUpdateMenu]
    @MenuId nvarchar(50),
    @Description nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    MERGE [AppDbo].[Menu] AS target
    USING (SELECT @MenuId AS [MenuId], @Description AS [Description] ) AS source
    ON target.[MenuId] = source.[MenuId]
    WHEN MATCHED THEN
        UPDATE SET [Description] = source.[Description]
    WHEN NOT MATCHED THEN
        INSERT ([MenuId], [Description])
        VALUES (source.[MenuId], source.[Description]);

    COMMIT TRAN;
END