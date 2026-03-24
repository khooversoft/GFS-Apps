CREATE PROCEDURE [App].[UpdateReportPackage]
    @PackageId nvarchar(50),
    @Description nvarchar(100),
    @MenuId nvarchar(50),
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
        SET [Description] = @Description
            ,[MenuId] = @MenuId
            ,[Data] = @Data
            ,[Disabled] = @Disabled
    WHERE [PackageId] = @PackageId;

    COMMIT TRAN;
END