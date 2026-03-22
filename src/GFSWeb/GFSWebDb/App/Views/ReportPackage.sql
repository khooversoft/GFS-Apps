CREATE VIEW [App].[ReportPackage]
AS
    SELECT  x.[PackageId]
            ,x.[Description]
            ,x.[Data]
            ,x.[Disabled]
            ,x.[DateTimeStamp]
            ,x.[UserStamp]
    FROM    [AppDbo].[ReportPackage] x;