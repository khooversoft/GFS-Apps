CREATE TABLE [AppDbo].[ReportAccess] (
    [PackageId]         NVARCHAR (50) NOT NULL,
    [NameIdentifier]    NVARCHAR (50) NOT NULL,
    [Access]            NVARCHAR (50) NOT NULL DEFAULT('reader') CHECK ([Access] IN ('reader', 'contributor', 'owner')),
    CONSTRAINT [FK_ReportAccess_Package] FOREIGN KEY ([PackageId]) REFERENCES [AppDbo].[ReportPackage] ([PackageId]),
    CONSTRAINT [FK_ReportAccess_Principal] FOREIGN KEY ([NameIdentifier]) REFERENCES [AppDbo].[PrincipalIdentity] ([NameIdentifier])
);
GO

CREATE CLUSTERED INDEX [PK_ReportAccess] ON [AppDbo].[ReportAccess] ([PackageId], [NameIdentifier]);
GO

CREATE INDEX [IX_ReportAccess_NameIdentifier] ON [AppDbo].[ReportAccess] ([NameIdentifier]);
GO
