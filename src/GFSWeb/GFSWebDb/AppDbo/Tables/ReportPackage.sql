CREATE TABLE [AppDbo].[ReportPackage] (
    [PackageId]         NVARCHAR (50) NOT NULL PRIMARY KEY,
    [SortKey]           NVARCHAR (50) NOT NULL,
    [Description]       NVARCHAR (100) NOT NULL,
    [ParentPackageId]   NVARCHAR (50) NOT NULL,
    [Data]              NVARCHAR (MAX) NOT NULL,
    [Disabled]          BIT CONSTRAINT [ReportPackage_Disabled] DEFAULT ((0)),
	[DateTimeStamp] [datetime2](7)  DEFAULT (SYSUTCDATETIME()),
    [UserStamp]      NVARCHAR(50) DEFAULT (SUSER_SNAME())
);
GO
