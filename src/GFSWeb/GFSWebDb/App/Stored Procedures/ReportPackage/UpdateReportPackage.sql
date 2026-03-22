CREATE PROCEDURE [App].[UpdateReportPackage]
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

    IF NOT EXISTS (SELECT 1 FROM [AppDbo].[ReportPackage] WHERE [PackageId] = @PackageId)
    BEGIN
        RAISERROR('ReportPackage record does not exists for the specified @PackageId', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    UPDATE [AppDbo].[ReportPackage]
        SET [SortKey] = @SortKey
            ,[Description] = @Description
            ,[ParentPackageId] = @ParentPackageId
            ,[Data] = @Data
            ,[Disabled] = @Disabled
    WHERE [PackageId] = @PackageId;

    COMMIT TRAN;
END