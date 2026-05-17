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

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('ReportPackage with PackageId %s does not exist.', 16, 1, @PackageId);
        RETURN;
    END
END