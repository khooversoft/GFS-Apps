CREATE PROCEDURE [App].[AddPrincipalIdentity]
    @NameIdentifier NVARCHAR(50),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(50),
    @Disabled BIT,
    @Role NVARCHAR(50),
    @Parker BIT,
    @ParkerPost BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF EXISTS (SELECT 1 FROM [AppDbo].[PrincipalIdentity] WHERE [NameIdentifier] = @NameIdentifier)
    BEGIN
        RAISERROR('PrincipalIdentity record already exists for the specified NameIdentifier', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    INSERT INTO [AppDbo].[PrincipalIdentity] ([NameIdentifier], [UserName], [Email], [Disabled], [Role], [Parker], [ParkerPost])
    VALUES (@NameIdentifier, @UserName, @Email, @Disabled, @Role, @Parker, @ParkerPost);
    COMMIT TRAN;
END