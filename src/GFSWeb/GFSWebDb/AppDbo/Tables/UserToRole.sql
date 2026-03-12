CREATE TABLE [AppDbo].[UserToRole] (
    [PrincipalId] INT NOT NULL,
    [RoleId]      INT NOT NULL,
    CONSTRAINT [PK_UserToRole] PRIMARY KEY CLUSTERED ([PrincipalId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_UserToRole_AppRole] FOREIGN KEY ([RoleId]) REFERENCES [AppDbo].[AppRole] ([RoleId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserToRole_PrincipalIdentity] FOREIGN KEY ([PrincipalId]) REFERENCES [AppDbo].[PrincipalIdentity] ([PrincipalId]) ON DELETE CASCADE ON UPDATE CASCADE
);

