CREATE PROCEDURE [App].[UpsertGroupMembership]
    @GroupName NVARCHAR(50),
    @NameIdentifier NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    MERGE [AppDbo].[GroupMembership] AS target
    USING (SELECT @GroupName AS [GroupName], @NameIdentifier AS [NameIdentifier]) AS source
        ON target.[GroupName] = source.[GroupName] AND target.[NameIdentifier] = source.[NameIdentifier]
    WHEN NOT MATCHED THEN
        INSERT ([GroupName], [NameIdentifier]) VALUES (source.[GroupName], source.[NameIdentifier]);
END