CREATE TABLE [AppDbo].[ElimOperation] (
    [ElimOprId]   INT            IDENTITY (1, 1) NOT NULL,
    [ElimCode]    NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (100) NOT NULL,
    [Data]        NVARCHAR (MAX) NULL,
    [Disabled]    BIT            CONSTRAINT [ElimOperation_Disabled] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ElimOperationx] PRIMARY KEY CLUSTERED ([ElimOprId] ASC)
);


GO

CREATE UNIQUE INDEX [IX_ElimOperation_ElimCode] ON [AppDbo].[ElimOperation] ([ElimCode])
GO