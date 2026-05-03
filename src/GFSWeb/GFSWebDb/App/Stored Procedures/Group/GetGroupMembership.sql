CREATE PROCEDURE [App].[GetGroupMembership]
    @GroupName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  x.*
            ,p.*
    FROM    [App].[PrincipalGroupView] x
            INNER JOIN [AppDbo].[GroupMembership] m ON x.[GroupName] = m.[GroupName]
            INNER JOIN [App].[PrincipalIdentityView] p ON m.[NameIdentifier] = p.[NameIdentifier]
END