CREATE PROCEDURE [App].[DeleteGroupPackageAccess]
    @PackageId NVARCHAR(50),
    @GroupName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE  [AppDbo].[GroupPackageAccess]
    WHERE   [PackageId] = @PackageId
    AND     [GroupName] = @GroupName;
END