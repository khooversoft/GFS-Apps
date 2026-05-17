CREATE PROCEDURE [App].[GetPackageAccess]
    @PackageId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Return the list of groups with access to the specified package, along with their access levels
    SELECT  x.[PackageId]
            ,x.[GroupName]
            ,x.[Role]
    FROM    [AppDbo].[GroupPackageAccess] x
    WHERE   x.[PackageId] = @PackageId; 
END