CREATE TABLE [AppDbo].[Command]
(
    [CommandId]         NVARCHAR (50) NOT NULL PRIMARY KEY,
    [Description]       NVARCHAR (100) NOT NULL,
    [Type]              NVARCHAR (50) NOT NULL,
    [Data]              NVARCHAR (MAX) NOT NULL,
    [Hash]              NVARCHAR (50) NOT NULL,  -- HASH of Data
    [Disabled]          BIT CONSTRAINT [Command_Disabled] DEFAULT ((0))
)
GO