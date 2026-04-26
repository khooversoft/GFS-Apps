CREATE PROCEDURE [App].[AddPrincipalIdentity]
    @NameIdentifier NVARCHAR(50),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(50),
    @Disabled BIT,
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

    INSERT INTO [AppDbo].[PrincipalIdentity] (
        [NameIdentifier], [UserName], [Email], [Disabled], [Role], [Parker], [ParkerPost],
        [UserID_SAP], [FirstName], [NickName], [MiddleName], [LastName], [Location], [ParkPost], [PostEmail],
        [CCEmail1], [CCEmail2], [Elim], [Co_Update], [Co_View], [CC_NodeID], [Flex1], [Flex2], [Flex3], [PostEmail2]
    )
    VALUES (
        @NameIdentifier, @UserName, @Email, @Disabled, @Role, @Parker, @ParkerPost,
        @UserID_SAP, @FirstName, @NickName, @MiddleName, @LastName, @Location, @ParkPost, @PostEmail,
        @CCEmail1, @CCEmail2, @Elim, @Co_Update, @Co_View, @CC_NodeID, @Flex1, @Flex2, @Flex3, @PostEmail2
    );

END