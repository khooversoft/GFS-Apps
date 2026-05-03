CREATE VIEW [App].[PrincipalGroupView]
AS
    SELECT  x.[GroupName]
            ,x.[Description]
    FROM    [AppDbo].[PrincipalGroup] x;