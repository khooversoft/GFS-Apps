create view [App].[PrincipalIdentity]
AS
    SELECT  x.[PrincipalId]
            ,x.[NameIdentifier]
            ,x.[UserName]
            ,x.[Email]
            ,x.[Disabled]
    FROM    [AppDbo].[PrincipalIdentity] x;