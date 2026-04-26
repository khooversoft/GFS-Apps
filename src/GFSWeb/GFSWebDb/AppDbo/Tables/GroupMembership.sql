CREATE TABLE [AppDbo].[GroupMembership]
(
    [GroupName]         NVARCHAR (50) NOT NULL,
    [NameIdentifier]    NVARCHAR (50) NOT NULL,
    CONSTRAINT [FK_GroupMembership_GroupName] FOREIGN KEY ([GroupName]) REFERENCES [AppDbo].[PrincipalGroup] ([GroupName]) ON DELETE CASCADE,
    CONSTRAINT [FK_GroupMembership_NameIdentifier] FOREIGN KEY ([NameIdentifier]) REFERENCES [AppDbo].[PrincipalIdentity] ([NameIdentifier]) ON DELETE CASCADE
)
GO

CREATE CLUSTERED INDEX [PK_GroupMembership] ON [AppDbo].[GroupMembership] ([GroupName], [NameIdentifier]);
GO