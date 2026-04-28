CREATE PROCEDURE [dbo].[GetPrincipalGroups]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  x.[GroupName]
    FROM    [AppDbo].[PrincipalGroup] x;

END