CREATE PROCEDURE [App].[GetUserUsage]
    @NameIdentifier    NVARCHAR (50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SELECT TOP 5
        NameIdentifier,
        PackageId,
        LastAccessed,
        Favorite
    FROM [AppDbo].[PackageUsage]
    WHERE NameIdentifier = @NameIdentifier
    ORDER BY 
        CASE WHEN Favorite = 1 THEN 0 ELSE 1 END,
        LastAccessed DESC;

END