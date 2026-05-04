CREATE PROCEDURE [App].[GetGroupMembership]
    @GroupName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- First result set: group records
    SELECT  x.*
    FROM    [App].[PrincipalGroupView] x
    WHERE   x.[GroupName] = @GroupName;

    -- Second result set: identity records
    SELECT  p.*
    FROM    [App].[PrincipalIdentityView] p
            INNER JOIN [AppDbo].[GroupMembership] m ON m.[NameIdentifier] = p.[NameIdentifier]
    WHERE   m.[GroupName] = @GroupName;
END