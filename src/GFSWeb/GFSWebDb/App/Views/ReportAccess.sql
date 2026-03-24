CREATE VIEW [App].[ReportAccess]
AS
    SELECT  x.[PackageId]
            ,x.[NameIdentifier]
            ,x.[Access]
    FROM    [AppDbo].[ReportAccess] x;