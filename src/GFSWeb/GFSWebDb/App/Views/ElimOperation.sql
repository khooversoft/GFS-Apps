CREATE VIEW [App].[ElimOperation]
AS
    SELECT  x.[ElimCode]
            ,x.[Description]
            ,x.[Data]
            ,x.[Disabled]
            ,x.[DateTimeStamp]
            ,x.[UserStamp]
    FROM    [AppDbo].[ElimOperation] x;