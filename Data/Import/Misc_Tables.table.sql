-- DROP TABLE [dbo].[Misc_Tables]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Misc_Tables](
	[Table_ID] [varchar](20) NULL,
	[ID] [varchar](20) NULL,
	[Descr] [varchar](2000) NULL,
	[Field1] [varchar](255) NULL,
	[Field2] [varchar](255) NULL,
	[Field3] [varchar](255) NULL,
	[Field4] [varchar](255) NULL,
	[Field5] [varchar](255) NULL,
	[Field6] [varchar](255) NULL,
	[DateTimeStamp] [datetime2](7) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Misc_Tables] ADD  DEFAULT (getdate()) FOR [DateTimeStamp]
GO
