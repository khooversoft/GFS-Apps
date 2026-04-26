CREATE PROCEDURE [App].[UpdateReportPackageData]
    @PackageId nvarchar(50),
    @Data nvarchar(max)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    UPDATE  [AppDbo].[ReportPackage]
    SET     [Data] = @Data
    WHERE   [PackageId] = @PackageId;
END