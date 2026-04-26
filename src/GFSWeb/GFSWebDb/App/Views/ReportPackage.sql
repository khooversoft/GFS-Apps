CREATE VIEW [App].[ReportPackage]
AS
    SELECT  x.[PackageId]
            ,x.[Description]
            ,x.[MenuId]
            ,x.[Data]
            ,x.[Disabled]
    FROM    [AppDbo].[ReportPackage] x;