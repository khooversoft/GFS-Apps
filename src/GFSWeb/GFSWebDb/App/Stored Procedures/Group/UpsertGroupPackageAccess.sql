CREATE PROCEDURE [App].[UpsertGroupPackageAccess]
    @PackageId NVARCHAR(50),
    @GroupName NVARCHAR(50),
    @Access NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    MERGE [AppDbo].[GroupPackageAccess] AS target
    USING (SELECT @PackageId AS [PackageId], @GroupName AS [GroupName], @Access AS [Access]) AS source
        ON target.[PackageId] = source.[PackageId] AND target.[GroupName] = source.[GroupName]
    WHEN NOT MATCHED THEN
        INSERT ([PackageId], [GroupName], [Access]) VALUES (source.[PackageId], source.[GroupName], source.[Access])
    WHEN MATCHED THEN
        UPDATE SET [Access] = source.[Access];
END