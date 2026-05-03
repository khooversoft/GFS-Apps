CREATE PROCEDURE [App].[GetPrincipalGroups]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  x.*
    FROM    [App].[PrincipalGroupView] x;

END