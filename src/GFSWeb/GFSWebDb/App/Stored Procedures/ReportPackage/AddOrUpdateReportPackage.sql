CREATE PROCEDURE [App].[UpsertReportPackage]
    @PackageId nvarchar(50),
    @Description nvarchar(100),
    @MenuId nvarchar(50),
    @Data nvarchar(max),
    @Disabled BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    MERGE [AppDbo].[ReportPackage] AS target
    USING (SELECT
                @PackageId AS [PackageId],
                @Description AS [Description],
                @MenuId AS [MenuId],
                @Data AS [Data],
                @Disabled AS [Disabled]) AS source
    ON target.[PackageId] = source.[PackageId]
    WHEN MATCHED THEN
        UPDATE SET
            [Description] = source.[Description],
            [MenuId] = source.[MenuId],
            [Data] = source.[Data],
            [Disabled] = source.[Disabled]
    WHEN NOT MATCHED THEN
        INSERT ([PackageId], [Description], [MenuId], [Data], [Disabled])
        VALUES (source.[PackageId], source.[Description], source.[MenuId], source.[Data], source.[Disabled]);
END