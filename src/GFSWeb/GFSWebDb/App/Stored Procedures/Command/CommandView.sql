CREATE VIEW [App].[CommandView]
AS
    SELECT  x.CommandId
            ,x.Description
            ,x.Type
            ,x.Data
            ,x.Hash
            ,x.Disabled
    FROM    [AppDbo].[Command] x;
