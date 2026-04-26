CREATE PROCEDURE [App].[ImportFixup]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Setup GroupMembership records
    MERGE [AppDbo].[GroupMembership] AS target
    USING (VALUES
        ('AGorman@OdysseyGroup.com', 'Corporate'),
        ('AMorelle@OdysseyRe.com', 'Corporate'),
        ('arajah@newlinegroup.com', 'Corporate'),
        ('AMorelle@OdysseyRe.com', 'Latm'),
        ('DRayner@NewlineGroup.com', 'Branches'),
        ('GDaskalakis@HudsonInsGroup.com', 'Branches')
    ) AS src ([NameIdentifier], [GroupName])
        ON target.[NameIdentifier] = src.[NameIdentifier] AND target.[GroupName] = src.[GroupName]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([GroupName], [NameIdentifier])
        VALUES (src.[GroupName], src.[NameIdentifier]);


    -- Setup GroupPackageAccess records
    MERGE [AppDbo].[GroupPackageAccess] AS target
    USING (VALUES
        ('Corporate', 'G11', 'owner'),
        ('Corporate', 'G12', 'reader'),
        ('Latm', 'G26', 'reader')
    ) AS src ([GroupName], [PackageId], [Access])
        ON target.[GroupName] = src.[GroupName] AND target.[PackageId] = src.[PackageId]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([PackageId], [GroupName])
        VALUES (src.[PackageId], src.[GroupName]);

END