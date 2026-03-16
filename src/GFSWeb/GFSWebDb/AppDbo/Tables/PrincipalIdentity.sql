CREATE TABLE [AppDbo].[PrincipalIdentity] (
    [NameIdentifier] NVARCHAR (50)  NOT NULL PRIMARY KEY,
    [UserName]       NVARCHAR (100) NOT NULL,
    [Email]          NVARCHAR (50)  NOT NULL,
    [Disabled]       BIT            NOT NULL DEFAULT(0),
    [Role]           NVARCHAR (50)  NOT NULL DEFAULT('reader') CHECK ([Role] IN ('reader', 'contributor', 'owner')),
    [Parker]         BIT            NOT NULL DEFAULT(0),
    [ParkerPost]     BIT            NOT NULL DEFAULT(0),
	[DateTimeStamp]  [datetime2](7)  NOT NULL DEFAULT (SYSUTCDATETIME()),
    [UserStamp]      NVARCHAR(50) NOT NULL DEFAULT (SUSER_SNAME())
);
GO

CREATE UNIQUE INDEX [IX_PrincipalIdentity_UserName] ON [AppDbo].[PrincipalIdentity] ([UserName])
GO

CREATE UNIQUE INDEX [IX_PrincipalIdentity_Email] ON [AppDbo].[PrincipalIdentity] ([Email])
GO

