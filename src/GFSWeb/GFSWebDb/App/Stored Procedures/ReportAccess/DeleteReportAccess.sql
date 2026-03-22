CREATE PROCEDURE [App].[DeleteReportAccess]
    @PackageId VARCHAR(50),
    @NameIdentifier VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF NOT EXISTS (SELECT 1 FROM [AppDbo].[ReportAccess] WHERE [PackageId] = @PackageId AND [NameIdentifier] = @NameIdentifier)
    BEGIN
        RAISERROR('Report access record does not exists for the specified PackageID & NameIdentifier', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    DELETE FROM [AppDbo].[ReportAccess]
    WHERE [PackageId] = @PackageId
    AND [NameIdentifier] = @NameIdentifier;

    COMMIT TRAN;
END