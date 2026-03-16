SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
drop table [dbo].[ElimSelect];
go

-- CREATE TABLE [dbo].[ElimSelect](
-- 	[ElimID] [varchar](20) NOT NULL,
-- 	[Pass] [int] NOT NULL,
-- 	[SubSeq] [int] NOT NULL,
-- 	[FieldName] [varchar](20) NULL,
-- 	[FieldNbr] [int] NOT NULL,
-- 	[IncExcl] [varchar](1) NULL,
-- 	[Oper] [varchar](2) NULL,
-- 	[FromVal] [varchar](255) NULL,
-- 	[ThruVal] [varchar](255) NULL,
-- 	[GLSU] [varchar](1) NULL,
-- 	[DateTimeStamp] [datetime2](7) NULL,
-- )
GO

CREATE TABLE [dbo].[ElimSelect](
	[ElimID] [varchar](20) NOT NULL,
	[Pass] [int] NOT NULL,
	[SubSeq] [int] NOT NULL,
	[FieldName] [varchar](20) NULL,
	[FieldNbr] [int] NOT NULL,
	[IncExcl] [varchar](1) NULL,
	[Oper] [varchar](2) NULL,
	[FromVal] [varchar](255) NULL,
	[ThruVal] [varchar](255) NULL,
	[GLSU] [varchar](1) NULL,
	[DateTimeStamp] [datetime2](7) NULL,
 CONSTRAINT [PK_ElimSelect] PRIMARY KEY CLUSTERED 
(
	[ElimID] ASC,
	[Pass] ASC,
	[FieldNbr] ASC,
	[SubSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
