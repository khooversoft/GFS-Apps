create  procedure [App].[UpdatePrincipalIdentity]
    @NameIdentifier NVARCHAR(50),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(50),
    @Disabled BIT = 0,
    @Role NVARCHAR(50),
    @Parker BIT,
    @ParkerPost BIT,
    @UserID_SAP NVARCHAR(50),
    @FirstName NVARCHAR(50),
    @NickName NVARCHAR(50),
    @MiddleName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Location NVARCHAR(50),
    @ParkPost NVARCHAR(50),
    @PostEmail NVARCHAR(50),
    @CCEmail1 NVARCHAR(50),
    @CCEmail2 NVARCHAR(50),
    @Elim NVARCHAR(50),
    @Co_Update NVARCHAR(50),
    @Co_View NVARCHAR(50),
    @CC_NodeID NVARCHAR(50),
    @Flex1 NVARCHAR(255),
    @Flex2 NVARCHAR(255),
    @Flex3 NVARCHAR(255),
    @PostEmail2 NVARCHAR(50)
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
        [ParkerPost] = @ParkerPost,
        [UserID_SAP] = @UserID_SAP,
        [FirstName] = @FirstName,
        [NickName] = @NickName,
        [MiddleName] = @MiddleName,
        [LastName] = @LastName,
        [Location] = @Location,
        [ParkPost] = @ParkPost,
        [PostEmail] = @PostEmail,
        [CCEmail1] = @CCEmail1,
        [CCEmail2] = @CCEmail2,
        [Elim] = @Elim,
        [Co_Update] = @Co_Update,
        [Co_View] = @Co_View,
        [CC_NodeID] = @CC_NodeID,
        [Flex1] = @Flex1,
        [Flex2] = @Flex2,
        [Flex3] = @Flex3,
        [PostEmail2] = @PostEmail2
    WHERE [NameIdentifier] = @NameIdentifier;

    COMMIT TRAN;
END