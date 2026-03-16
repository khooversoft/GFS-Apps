SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
drop table [dbo].[Eliminations]
go

CREATE TABLE [dbo].[Eliminations](
	[ID] [int] NOT NULL,
	[ShortName] [varchar](50) NULL,
	[Def] [varchar](50) NULL,
	[ElimMapNbr] [varchar](5) NULL,
	[ElimMapName] [varchar](50) NULL,
	[ContractPrefix] [varchar](12) NULL,
	[offsetBC] [varchar](2) NULL,
	[offsetPC] [varchar](10) NULL,
	[offsetPCdac] [varchar](10) NULL,
	[forcePC] [varchar](10) NULL,
	[forceTP] [varchar](5) NULL,
	[UWyr] [varchar](1) NULL,
	[StatLedger] [varchar](1) NULL,
	[Notes] [varchar](100) NULL,
	[TrueUpCo] [varchar](4) NULL,
	[TrueUpAorC] [varchar](1) NULL,
	[TrueUpMap] [varchar](5) NULL,
	[TrueUpPC] [varchar](10) NULL,
	[TrueUpBC] [varchar](2) NULL,
	[TrueUpTP] [varchar](5) NULL,
	[TrueUpContract] [varchar](15) NULL,
	[Flex1] [varchar](255) NULL,
	[Flex2] [varchar](255) NULL,
	[Flex3] [varchar](255) NULL,
	[DateTimeStamp] [datetime2](7) NULL,
 CONSTRAINT [PK_Eliminations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
