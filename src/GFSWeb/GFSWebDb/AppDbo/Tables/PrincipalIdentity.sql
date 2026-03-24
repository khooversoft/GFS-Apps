CREATE TABLE [AppDbo].[PrincipalIdentity] (
    [NameIdentifier]	NVARCHAR (50)	NOT NULL PRIMARY KEY,
    [UserName]			NVARCHAR (100)	NOT NULL,
    [Email]				NVARCHAR (50)	NOT NULL,
    [Disabled]			BIT				NOT NULL DEFAULT(0),
    [Role]				NVARCHAR (50)	NOT NULL DEFAULT('reader') CHECK ([Role] IN ('reader', 'contributor', 'owner')),
    [Parker]			BIT				NOT NULL DEFAULT(0),
    [ParkerPost]		BIT				NOT NULL DEFAULT(0),
	[UserID_SAP]		NVARCHAR (50)	NULL,
	[FirstName]			NVARCHAR (50)	NULL,
	[NickName]			NVARCHAR (50)	NULL,
	[MiddleName]		NVARCHAR (50)	NULL,
	[LastName]			NVARCHAR (50)	NULL,
	[Location]			NVARCHAR (50)	NULL,
	[ParkPost]			NVARCHAR (50)	NULL,
	[PostEmail]			NVARCHAR (50)	NULL,
	[CCEmail1]			NVARCHAR (50)	NULL,
	[CCEmail2]			NVARCHAR (50)	NULL,
	[Elim]				NVARCHAR (50)	NULL,
	[Co_Update]			NVARCHAR (50)	NULL,
	[Co_View]			NVARCHAR (50)	NULL,
	[CC_NodeID]			NVARCHAR (50)	NULL,
	[Flex1]				NVARCHAR (255)	NULL,
	[Flex2]				NVARCHAR (255)	NULL,
	[Flex3]				NVARCHAR (255)	NULL,
	[PostEmail2]		NVARCHAR (50)	NULL,
	[DateTimeStamp]		[datetime2](7)	NOT NULL DEFAULT (SYSUTCDATETIME()),
    [UserStamp]			NVARCHAR(50)	NOT NULL DEFAULT (SUSER_SNAME())
);
GO

CREATE UNIQUE INDEX [IX_PrincipalIdentity_UserName] ON [AppDbo].[PrincipalIdentity] ([UserName])
GO

CREATE UNIQUE INDEX [IX_PrincipalIdentity_Email] ON [AppDbo].[PrincipalIdentity] ([Email])
GO

