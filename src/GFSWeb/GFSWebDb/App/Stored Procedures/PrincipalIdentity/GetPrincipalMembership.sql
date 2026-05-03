CREATE PROCEDURE [App].[GetPrincipalMembership]
    @NameIdentifier NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  x.[GroupName]
            ,x.[Description]
    FROM    [AppDbo].[PrincipalGroup] x
            INNER JOIN [AppDbo].[GroupMembership] m ON x.[GroupName] = m.[GroupName]
            INNER JOIN [App].[PrincipalIdentityView] p ON m.[NameIdentifier] = p.[NameIdentifier]
    WHERE   p.[NameIdentifier] = @NameIdentifier;
END