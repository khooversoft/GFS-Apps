CREATE VIEW [App].[PrincipalRole]
AS
    SELECT  x.[NameIdentifier]
            ,r.[RoleCode]
            ,r.[Description]
    FROM    [AppDbo].[PrincipalIdentity] x
            INNER JOIN [AppDbo].[UserToRole] xr on x.[PrincipalId] = xr.[PrincipalId]
            INNER JOIN [AppDbo].[AppRole] r on xr.[RoleId] = r.[RoleId];
