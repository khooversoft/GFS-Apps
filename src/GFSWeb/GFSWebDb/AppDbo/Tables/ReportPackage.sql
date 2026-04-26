CREATE TABLE [AppDbo].[ReportPackage] (
    [PackageId]         NVARCHAR (50) NOT NULL PRIMARY KEY,
    [Description]       NVARCHAR (100) NOT NULL,
    [MenuId]            NVARCHAR (50) NOT NULL,
    [Data]              NVARCHAR (MAX) NOT NULL,
    [Disabled]          BIT CONSTRAINT [ReportPackage_Disabled] DEFAULT ((0))
    CONSTRAINT [FK_ReportPackage_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [AppDbo].[Menu] ([MenuId]) ON DELETE CASCADE,
);
GO

CREATE INDEX [IX_ReportPackage_MenuId] ON [AppDbo].[ReportPackage] ([MenuId])
GO

