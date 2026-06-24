CREATE PROCEDURE [App].[ClearUserUsage]
    @NameIdentifier    NVARCHAR (50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE [AppDbo].[PackageUsage]
    WHERE NameIdentifier = @NameIdentifier;

END