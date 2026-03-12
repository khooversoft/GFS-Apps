CREATE TABLE [AppDbo].[AppRole] (
    [RoleId]      INT            IDENTITY (1, 1) NOT NULL,
    [RoleCode]    NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_AppRoles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);


GO

CREATE UNIQUE INDEX [IX_AppRole_RoleCode] ON [AppDbo].[AppRole] ([RoleCode])
GO