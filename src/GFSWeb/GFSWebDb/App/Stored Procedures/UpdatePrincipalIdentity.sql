create  procedure [App].[UpdatePrincipalIdentity]
    @NameIdentifier NVARCHAR(50),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(50),
    @Disabled BIT = 0,
    @Role NVARCHAR(50),
    @Parker BIT,
    @ParkerPost BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRAN;

    IF NOT EXISTS (SELECT 1 FROM [AppDbo].[PrincipalIdentity] WHERE [NameIdentifier] = @NameIdentifier)
    BEGIN
        RAISERROR('PrincipalIdentity record does not exist for the specified NameIdentifier', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    UPDATE [AppDbo].[PrincipalIdentity]
    SET
        [UserName] = @UserName,
        [Email] = @Email,
        [Disabled] = @Disabled,
        [Role] = @Role,
        [Parker] = @Parker,
        [ParkerPost] = @ParkerPost
    WHERE [NameIdentifier] = @NameIdentifier;
    COMMIT TRAN;
END