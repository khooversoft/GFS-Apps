CREATE PROCEDURE [App].[GetPrincipalMembership]
    @GroupName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  x.[GroupName]
    FROM    [AppDbo].[GroupMembership] x
            INNER JOIN [AppDbo].[PrincipalIdentity] p ON x.[NameIdentifier] = p.[NameIdentifier]
    WHERE   [GroupName] = @GroupName;
END