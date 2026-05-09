CREATE VIEW [App].[CommandView]
AS
    SELECT  x.CommandId
            ,x.Description
            ,x.Data
            ,x.Disabled
    FROM    [AppDbo].[Command] x;
