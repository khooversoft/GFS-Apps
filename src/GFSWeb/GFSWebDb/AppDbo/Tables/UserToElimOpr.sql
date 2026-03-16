CREATE TABLE [AppDbo].[UserToElimOpr] (
    [ElimCode] NVARCHAR (50)  NOT NULL,
    [NameIdentifier] NVARCHAR (50)  NOT NULL,
    CONSTRAINT [FK_UserToElim_ElimOpr] FOREIGN KEY ([ElimCode]) REFERENCES [AppDbo].[ElimOperation] ([ElimCode]),
    CONSTRAINT [FK_UserToElim_Principal] FOREIGN KEY ([NameIdentifier]) REFERENCES [AppDbo].[PrincipalIdentity] ([NameIdentifier])
);
GO

CREATE CLUSTERED INDEX [PK_UserToElimOpr] ON [AppDbo].[UserToElimOpr] ([ElimCode], [NameIdentifier]);
GO

CREATE INDEX [IX_UserToElimOpr_NameIdentifier] ON [AppDbo].[UserToElimOpr] ([NameIdentifier]);
GO
