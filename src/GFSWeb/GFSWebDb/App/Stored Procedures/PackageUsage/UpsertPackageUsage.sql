CREATE PROCEDURE [App].[UpsertPackageUsage]
    @NameIdentifier    NVARCHAR (50),
    @PackageId         NVARCHAR (50),
    @Favorite          BIT NULL = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    MERGE INTO [AppDbo].[PackageUsage] AS target
    USING (SELECT @NameIdentifier AS NameIdentifier, @PackageId AS PackageId, @Favorite AS Favorite) AS source
    ON target.NameIdentifier = source.NameIdentifier AND target.PackageId = source.PackageId
    WHEN MATCHED THEN
        UPDATE SET
            LastAccessed = GETUTCDATE(),
            Favorite = CASE WHEN @Favorite IS NULL THEN target.Favorite ELSE @Favorite END
    WHEN NOT MATCHED THEN
        INSERT (NameIdentifier, PackageId, LastAccessed, Favorite)
        VALUES (source.NameIdentifier, source.PackageId, GETUTCDATE(), source.Favorite);
END