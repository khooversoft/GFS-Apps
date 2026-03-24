CREATE VIEW [App].[Menu]
AS
    SELECT  x.[MenuId]
            ,x.[Description]
    FROM    [AppDbo].[Menu] x;