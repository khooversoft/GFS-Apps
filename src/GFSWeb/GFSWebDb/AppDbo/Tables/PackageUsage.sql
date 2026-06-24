CREATE TABLE [AppDbo].[PackageUsage]
(
    [NameIdentifier]    NVARCHAR (50) NOT NULL,
    [PackageId]         NVARCHAR (50) NOT NULL,
    [Favorite]          BIT NOT NULL DEFAULT(0),
    [LastAccessed]      DateTime NOT NULL DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT [FK_PackageUsage_NameIdentifier] FOREIGN KEY ([NameIdentifier]) REFERENCES [AppDbo].[PrincipalIdentity] ([NameIdentifier]) ON DELETE CASCADE,
    CONSTRAINT [FK_PackageUsage_Package] FOREIGN KEY ([PackageId]) REFERENCES [AppDbo].[ReportPackage] ([PackageId]) ON DELETE CASCADE,
)
GO

CREATE UNIQUE CLUSTERED INDEX [PK_PackageUsage] ON [AppDbo].[PackageUsage] ([NameIdentifier], [PackageId]);
GO