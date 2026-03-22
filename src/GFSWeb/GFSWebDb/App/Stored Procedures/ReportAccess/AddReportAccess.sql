CREATE PROCEDURE [App].[AddReportAccess]
    @PackageId VARCHAR(50),
    @NameIdentifier VARCHAR(50),
    @Access VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF EXISTS (SELECT 1 FROM [AppDbo].[ReportAccess] WHERE [PackageId] = @PackageId AND [NameIdentifier] = @NameIdentifier)
    BEGIN
        RAISERROR('Report access record already exists for the specified PackageID & NameIdentifier', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    INSERT INTO [AppDbo].[ReportAccess] ([PackageId], [NameIdentifier], [Access])
    VALUES (@PackageId, @NameIdentifier, @Access);

    COMMIT TRAN;
END