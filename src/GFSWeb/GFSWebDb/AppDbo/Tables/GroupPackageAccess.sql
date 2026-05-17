CREATE TABLE [AppDbo].[GroupPackageAccess] (
    [PackageId]         NVARCHAR (50) NOT NULL,
    [GroupName]         NVARCHAR (50) NOT NULL,
    [Role]            NVARCHAR (50) NOT NULL DEFAULT('reader') CHECK ([Role] IN ('reader', 'contributor', 'owner')),
    CONSTRAINT [FK_GroupPackageAccess_Package] FOREIGN KEY ([PackageId]) REFERENCES [AppDbo].[ReportPackage] ([PackageId]) ON DELETE CASCADE,
    CONSTRAINT [FK_GroupPackageAccess_GroupName] FOREIGN KEY ([GroupName]) REFERENCES [AppDbo].[PrincipalGroup] ([GroupName]) ON DELETE CASCADE
);
GO

CREATE UNIQUE CLUSTERED INDEX [PK_GroupAccess] ON [AppDbo].[GroupPackageAccess] ([PackageId], [GroupName]);
GO

CREATE INDEX [IX_GroupAccess_GroupName] ON [AppDbo].[GroupPackageAccess] ([GroupName]);
GO
