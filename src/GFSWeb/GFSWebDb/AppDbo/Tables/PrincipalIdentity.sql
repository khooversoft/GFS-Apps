CREATE TABLE [AppDbo].[PrincipalIdentity] (
    [PrincipalId]    INT            IDENTITY (1, 1) NOT NULL,
    [NameIdentifier] NVARCHAR (50)  NOT NULL,
    [UserName]       NVARCHAR (100) NOT NULL,
    [Email]          NVARCHAR (50)  NOT NULL,
    [Disabled]       BIT            NOT NULL DEFAULT(0),
    CONSTRAINT [PK_PrincipalIdentity] PRIMARY KEY CLUSTERED ([PrincipalId] ASC)
);
GO

CREATE UNIQUE INDEX [IX_PrincipalIdentity_NameIdentifier] ON [AppDbo].[PrincipalIdentity] ([NameIdentifier]);
GO

CREATE UNIQUE INDEX [IX_PrincipalIdentity_UserName] ON [AppDbo].[PrincipalIdentity] ([UserName])
GO

CREATE UNIQUE INDEX [IX_PrincipalIdentity_Email] ON [AppDbo].[PrincipalIdentity] ([Email])
GO

