CREATE VIEW [App].[PackageRoleView]
AS
    SELECT  x.[PackageId]
            ,x.[Description]
            ,x.[MenuId]
            ,x.[Data]
            ,x.[Disabled]
            ,pa.[Role] AS [AccessRole]
            ,p.[Role] as [PrincipalRole]
            ,CASE
                WHEN 'owner'       IN (pa.[Role], p.[Role]) THEN 'owner'
                WHEN 'contributor' IN (pa.[Role], p.[Role]) THEN 'contributor'
                ELSE 'reader'
             END AS [EffectiveRole]
    FROM    [AppDbo].[ReportPackage] x
            INNER JOIN [AppDbo].[GroupPackageAccess] pa ON x.[PackageId] = pa.[PackageId]
            INNER JOIN [AppDbo].[GroupMembership] m ON pa.[GroupName] = m.[GroupName]
            INNER JOIN [AppDbo].[PrincipalIdentity] p ON m.[NameIdentifier] = p.[NameIdentifier]
    WHERE   x.[Disabled] = 0;