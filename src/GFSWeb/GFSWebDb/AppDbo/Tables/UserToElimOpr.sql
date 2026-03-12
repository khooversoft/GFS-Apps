CREATE TABLE [AppDbo].[UserToElimOpr] (
    [PrincipalId] INT NOT NULL,
    [ElimOprId]   INT NOT NULL,
    CONSTRAINT [PK_UserToElimOpr] PRIMARY KEY CLUSTERED ([PrincipalId] ASC, [ElimOprId] ASC),
    CONSTRAINT [FK_UserToElim_ElimOpr] FOREIGN KEY ([ElimOprId]) REFERENCES [AppDbo].[ElimOperation] ([ElimOprId]),
    CONSTRAINT [FK_UserToElim_Principal] FOREIGN KEY ([PrincipalId]) REFERENCES [AppDbo].[PrincipalIdentity] ([PrincipalId])
);

