CREATE VIEW [App].[ReportPackageView]
AS
    SELECT  x.[PackageId]
            ,x.[Description]
            ,x.[MenuId]
            ,x.[Data]
            ,x.[Disabled]
    FROM    [AppDbo].[ReportPackage] x
    WHERE   x.[Disabled] = 0;
