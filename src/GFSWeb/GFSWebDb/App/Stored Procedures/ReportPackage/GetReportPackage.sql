CREATE PROCEDURE [App].[GetReportPackage]
    @PackageId NVARCHAR(50) NULL = NULL,
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

    IF @role IN ('contributor', 'owner')
    BEGIN
        IF @PackageId IS NULL
        BEGIN
            SELECT  x.*
            FROM    [App].[ReportPackageView] x;
            RETURN;
        END

        SELECT  x.*
        FROM    [App].[ReportPackageView] x
        WHERE   x.[PackageId] = @PackageId;
        RETURN;
    END

    IF @PackageId IS NULL
    BEGIN
        SELECT  DISTINCT x.*
        FROM    [App].[PackageRoleView] x
        WHERE   x.[EffectiveRole] in ('contributor', 'owner');
        RETURN;
    END

    SELECT  DISTINCT x.*
    FROM    [App].[PackageRoleView] x
    WHERE   x.[EffectiveRole] in ('contributor', 'owner')
    AND     x.[PackageId] = @PackageId;
END