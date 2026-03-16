CREATE TABLE [AppDbo].[ElimOperation] (
    [ElimCode]    NVARCHAR (50)  NOT NULL PRIMARY KEY,
    [Description] NVARCHAR (100) NOT NULL,
    [Data]        NVARCHAR (MAX) NULL,
    [Disabled]    BIT            CONSTRAINT [ElimOperation_Disabled] DEFAULT ((0)) NOT NULL,
	[DateTimeStamp] [datetime2](7)  NOT NULL DEFAULT (SYSUTCDATETIME()),
    [UserStamp]      NVARCHAR(50) NOT NULL DEFAULT (SUSER_SNAME())
);
GO
