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

    UPDATE [AppDbo].[ReportPackage]
        SET [Description] = @Description
            ,[MenuId] = @MenuId
            ,[Data] = @Data
            ,[Disabled] = @Disabled
    WHERE [PackageId] = @PackageId;
END