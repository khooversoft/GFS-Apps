CREATE TABLE [AppDbo].[Command]
(
    [CommandId]         NVARCHAR (50) NOT NULL PRIMARY KEY, -- HASH of Data
    [Description]       NVARCHAR (100) NOT NULL,
    [Data]              NVARCHAR (MAX) NOT NULL,
    [Disabled]          BIT CONSTRAINT [Command_Disabled] DEFAULT ((0))
)
GO