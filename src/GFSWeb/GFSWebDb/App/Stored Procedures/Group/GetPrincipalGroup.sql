CREATE PROCEDURE [App].[GetPrincipalGroup]
    @GroupName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT  x.*
    FROM    [App].[PrincipalGroupView] x
    WHERE   x.[GroupName] = @GroupName;
END