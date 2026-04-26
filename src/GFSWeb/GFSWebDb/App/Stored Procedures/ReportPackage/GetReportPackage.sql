CREATE PROCEDURE [App].[GetReportPackage]
    @PackageId NVARCHAR(50),
    @NameIdentifier NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @role NVARCHAR(20) = (
        SELECT [Role] FROM [AppDbo].[PrincipalIdentity] WHERE [NameIdentifier] = @NameIdentifier
    );

    IF @role IS NULL
    BEGIN
        RAISERROR('PrincipalIdentity with NameIdentifier %s does not exist.', 16, 1, @NameIdentifier);
        RETURN;
    END

    SELECT DISTINCT
            x.[PackageId]
            ,x.[Description]
            ,x.[MenuId]
            ,x.[Data]
            ,x.[Disabled]
    FROM    [AppDbo].[ReportPackage] x
    WHERE   x.[PackageId] = @PackageId
    AND     (
                @role IN ('contributor', 'owner')
                OR (
                    x.[Disabled] = 0
                    AND EXISTS (
                        SELECT 1
                        FROM   [AppDbo].[GroupPackageAccess] a
                               INNER JOIN [AppDbo].[GroupMembership] gm ON a.[GroupName] = gm.[GroupName]
                        WHERE  a.[PackageId] = x.[PackageId]
                        AND    gm.[NameIdentifier] = @NameIdentifier
                    )
                )
            );
END