CREATE PROCEDURE [App].[GetMenuForPrincipalIdentity]
    @NameIdentifier NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @role NVARCHAR(20) = (SELECT [Role] FROM [AppDbo].[PrincipalIdentity] WHERE [NameIdentifier] = @NameIdentifier);

    IF @role IS NULL
    BEGIN
        RAISERROR('PrincipalIdentity with NameIdentifier %s does not exist.', 16, 1, @NameIdentifier);
        RETURN;
    END

    IF @role IN ('contributor', 'owner')
    BEGIN
        SELECT  m.[MenuId]
                ,m.[Description] as [MenuDescription]
                ,x.[PackageId]
                ,x.[Description]
                ,x.[Data]
        FROM    [AppDbo].[ReportPackage] x
                INNER JOIN [AppDbo].[Menu] m ON x.[MenuId] = m.[MenuId]
        WHERE   x.[Disabled] = 0;
        RETURN;
    END

    SELECT  m.[MenuId]
            ,m.[Description] as [MenuDescription]
            ,x.[PackageId]
            ,x.[Description]
            ,x.[Data]
    FROM    [AppDbo].[ReportPackage] x
            INNER JOIN [AppDbo].[Menu] m ON x.[MenuId] = m.[MenuId]
            INNER JOIN [AppDbo].[GroupPackageAccess] a ON x.[PackageId] = a.[PackageId]
            INNER JOIN [AppDbo].[GroupMembership] gm ON a.[GroupName] = gm.[GroupName]
            INNER JOIN [AppDbo].[PrincipalIdentity] p ON gm.[NameIdentifier] = p.[NameIdentifier]
    WHERE   p.[NameIdentifier] = @NameIdentifier
    AND     x.[Disabled] = 0;
END
