CREATE PROCEDURE [App].[GetReportPackage]
    @PackageId NVARCHAR(50),
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
        SELECT  x.[PackageId]
                ,x.[Description]
                ,x.[MenuId]
                ,x.[Data]
                ,x.[Disabled]
        FROM    [AppDbo].[ReportPackage] x
        WHERE   x.[PackageId] = @PackageId;
        RETURN;
    END

    SELECT  x.[PackageId]
            ,x.[Description]
            ,x.[MenuId]
            ,x.[Data]
            ,x.[Disabled]
    FROM    [AppDbo].[ReportPackage] x
            INNER JOIN [AppDbo].[ReportAccess] a ON x.[PackageId] = a.[PackageId]
    WHERE   x.[PackageId] = @PackageId
    AND     a.[NameIdentifier] = @NameIdentifier;
END