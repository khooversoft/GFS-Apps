CREATE VIEW [App].[AppRole]
AS
    SELECT  x.[RoleCode]
            ,x.[Description]
    FROM    [AppDbo].[AppRole] x;