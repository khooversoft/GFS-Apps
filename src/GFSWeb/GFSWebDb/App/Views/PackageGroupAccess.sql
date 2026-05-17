CREATE VIEW [App].[PackageGroupAccess]
AS
    SELECT  a.[PackageId]
            ,gm.[NameIdentifier]
    FROM    [AppDbo].[GroupPackageAccess] a
            INNER JOIN [AppDbo].[GroupMembership] gm ON a.[GroupName] = gm.[GroupName];
