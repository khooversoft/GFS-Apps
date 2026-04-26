CREATE PROCEDURE [App].[DeleteGroupMembership]
    @GroupName NVARCHAR(50),
    @NameIdentifier NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE [AppDbo].[GroupMembership]
    WHERE  [GroupName] = @GroupName
    AND    [NameIdentifier] = @NameIdentifier;
END