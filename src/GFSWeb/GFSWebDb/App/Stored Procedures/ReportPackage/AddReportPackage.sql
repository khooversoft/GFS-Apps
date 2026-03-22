CREATE PROCEDURE [App].[AddReportPackage]
    @PackageId nvarchar(50),
    @SortKey nvarchar(50),
    @Description nvarchar(100),
    @ParentPackageId nvarchar(50),
    @Data nvarchar(max),
    @Disabled BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF EXISTS (SELECT 1 FROM [AppDbo].[ReportPackage] WHERE [PackageId] = @PackageId)
    BEGIN
        RAISERROR('ReportPackage record already exists for the specified @PackageId', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    INSERT [AppDbo].[ReportPackage] ([PackageId], [SortKey], [Description], [ParentPackageId], [Data], [Disabled])
    VALUES (@PackageId, @SortKey, @Description, @ParentPackageId, @Data, @Disabled);

    COMMIT TRAN;
END