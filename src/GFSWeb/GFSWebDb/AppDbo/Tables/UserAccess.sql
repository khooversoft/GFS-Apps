CREATE TABLE [AppDbo].[UserAccess]
(
    [UserId] INT IDENTITY(1, 1) PRIMARY KEY,
	[PrincipalNameIdentity] NVARCHAR(50) NOT NULL,
    [Table_ID] NVARCHAR(20) NOT NULL,
	[ID] [varchar](20) NOT NULL,
	[Descr] NVARCHAR(2000) NOT NULL,
	[Field1] NVARCHAR(255) NULL,
	[Field2] NVARCHAR(255) NULL,
	[Field3] NVARCHAR(255) NULL,
	[Field4] NVARCHAR(255) NULL,
	[Field5] NVARCHAR(255) NULL,
	[Field6] NVARCHAR(255) NULL,
	[DateTimeStamp] [datetime2](7)  DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT [FK_UserAccess_PrincipalNameIdentity] FOREIGN KEY ([PrincipalNameIdentity]) REFERENCES [AppDbo].[PrincipalIdentity] ([Email]) ON DELETE CASCADE
)
GO

CREATE UNIQUE INDEX [IX_UserAccess_PrincipalNameIdentity] ON [AppDbo].[UserAccess] ([PrincipalNameIdentity], [Table_ID], [Field1])
GO

CREATE INDEX [IX2_UserAccess_PrincipalNameIdentity] ON [AppDbo].[UserAccess] ([PrincipalNameIdentity])
GO
