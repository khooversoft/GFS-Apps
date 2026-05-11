CREATE PROCEDURE [App].[UpsertPrincipalGroup]
    @GroupName NVARCHAR(50),
    @Description NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    MERGE INTO [AppDbo].[PrincipalGroup] AS Target
    USING (SELECT @GroupName AS GroupName) AS Source
    ON Target.GroupName = Source.GroupName
    WHEN MATCHED THEN
        UPDATE SET [Description] = @Description
    WHEN NOT MATCHED THEN
        INSERT (GroupName, [Description]) VALUES (@GroupName, @Description);

END
