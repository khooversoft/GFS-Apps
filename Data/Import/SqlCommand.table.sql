SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
drop  TABLE [dbo].[Misc_Tables]
go

CREATE TABLE [dbo].[Misc_Tables](
	[Table_ID] [varchar](20) NULL,
	[ID] [varchar](20) NULL,
	[Descr] [varchar](2000) NULL,
) ON [PRIMARY]
GO
