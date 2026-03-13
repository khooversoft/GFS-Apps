create view [App].[PrincipalIdentity]
AS
    SELECT  x.[PrincipalId]
            ,x.[NameIdentifier]
            ,x.[UserName]
            ,x.[Email]
            ,x.[Disabled]
            ,x.[Role]
            ,x.[Parker]
            ,x.[ParkerPost]
    FROM    [AppDbo].[PrincipalIdentity] x;