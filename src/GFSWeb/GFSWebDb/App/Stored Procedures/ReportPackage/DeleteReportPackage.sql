CREATE PROCEDURE [App].[DeleteElimOpr]
    @PackageId nvarchar(50)
AS
BEGIN
    DELETE FROM [AppDbo].[ReportPackage]
    WHERE [PackageId] = @PackageId
END