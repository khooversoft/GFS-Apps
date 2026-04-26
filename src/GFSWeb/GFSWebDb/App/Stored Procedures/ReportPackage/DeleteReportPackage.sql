CREATE PROCEDURE [App].[DeleteElimOpr]
    @PackageId nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE FROM [AppDbo].[ReportPackage]
    WHERE [PackageId] = @PackageId
END