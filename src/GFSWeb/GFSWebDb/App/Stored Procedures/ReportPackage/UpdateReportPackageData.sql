CREATE PROCEDURE [App].[UpdateReportPackageData]
    @PackageId nvarchar(50),
    @Data nvarchar(max)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF NOT EXISTS (SELECT 1 FROM [AppDbo].[ReportPackage] WHERE [PackageId] = @PackageId)
    BEGIN
        RAISERROR('ReportPackage record does not exist for the specified @@PackageId', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    UPDATE  [AppDbo].[ReportPackage]
    SET     [Data] = @Data
    WHERE   [PackageId] = @PackageId;

    COMMIT TRAN;
END