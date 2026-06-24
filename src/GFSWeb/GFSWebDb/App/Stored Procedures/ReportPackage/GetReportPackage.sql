CREATE PROCEDURE [App].[GetReportPackage]
    @PackageId NVARCHAR(50) NULL = NULL,
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

    -- System contributors and owners can see all packages
    IF @role IN ('contributor', 'owner')
    BEGIN
        SELECT  rp.[PackageId]
                ,rp.[Description]
                ,rp.[MenuId]
                ,rp.[Data]
                ,rp.[Disabled]
                ,ISNULL(pu.[Favorite], 0) AS [IsFavorite]
        FROM    [App].[ReportPackageView] rp
                    LEFT JOIN [AppDbo].[PackageUsage] pu ON pu.[PackageId] = rp.[PackageId]
        AND     pu.[NameIdentifier] = @NameIdentifier
        WHERE   (@PackageId IS NULL OR rp.[PackageId] = @PackageId);
        RETURN;
    END

    -- Regular users can only see packages accessible via their group memberships
    SELECT  DISTINCT
            rp.[PackageId]
            ,rp.[Description]
            ,rp.[MenuId]
            ,rp.[Data]
            ,rp.[Disabled]
            ,ISNULL(pu.[Favorite], 0) AS [IsFavorite]
    FROM    [App].[ReportPackageView] rp
                INNER JOIN [AppDbo].[GroupPackageAccess] gpa ON gpa.[PackageId] = rp.[PackageId]
                INNER JOIN [AppDbo].[GroupMembership] m ON m.[GroupName] = gpa.[GroupName]
                LEFT JOIN  [AppDbo].[PackageUsage] pu ON pu.[PackageId] = rp.[PackageId]
    AND     pu.[NameIdentifier] = @NameIdentifier
    WHERE   m.[NameIdentifier] = @NameIdentifier
    AND     (@PackageId IS NULL OR rp.[PackageId] = @PackageId);
END