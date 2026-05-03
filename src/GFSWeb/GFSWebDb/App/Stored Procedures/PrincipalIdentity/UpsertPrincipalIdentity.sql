create  procedure [App].[UpsertPrincipalIdentity]
    @NameIdentifier NVARCHAR(50),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(50),
    @Disabled BIT = 0,
    @Role NVARCHAR(50),
    @Parker BIT,
    @ParkerPost BIT,
    @UserID_SAP NVARCHAR(50) NULL,
    @FirstName NVARCHAR(50) NULL,
    @NickName NVARCHAR(50) NULL,
    @MiddleName NVARCHAR(50) NULL,
    @LastName NVARCHAR(50) NULL,
    @Location NVARCHAR(50) NULL,
    @ParkPost NVARCHAR(50) NULL,
    @PostEmail NVARCHAR(50) NULL,
    @CCEmail1 NVARCHAR(50) NULL,
    @CCEmail2 NVARCHAR(50) NULL,
    @Elim NVARCHAR(50) NULL,
    @Co_Update NVARCHAR(50) NULL,
    @Co_View NVARCHAR(50) NULL,
    @CC_NodeID NVARCHAR(50) NULL,
    @Flex1 NVARCHAR(255) NULL,
    @Flex2 NVARCHAR(255) NULL,
    @Flex3 NVARCHAR(255) NULL,
    @PostEmail2 NVARCHAR(50) NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    MERGE [AppDbo].[PrincipalIdentity] AS target
    USING (SELECT @NameIdentifier AS [NameIdentifier]) AS source
    ON target.[NameIdentifier] = source.[NameIdentifier]
    WHEN MATCHED THEN
        UPDATE SET
            [UserName]      = @UserName,
            [Email]         = @Email,
            [Disabled]      = @Disabled,
            [Role]          = @Role,
            [Parker]        = @Parker,
            [ParkerPost]    = @ParkerPost,
            [UserID_SAP]    = @UserID_SAP,
            [FirstName]     = @FirstName,
            [NickName]      = @NickName,
            [MiddleName]    = @MiddleName,
            [LastName]      = @LastName,
            [Location]      = @Location,
            [ParkPost]      = @ParkPost,
            [PostEmail]     = @PostEmail,
            [CCEmail1]      = @CCEmail1,
            [CCEmail2]      = @CCEmail2,
            [Elim]          = @Elim,
            [Co_Update]     = @Co_Update,
            [Co_View]       = @Co_View,
            [CC_NodeID]     = @CC_NodeID,
            [Flex1]         = @Flex1,
            [Flex2]         = @Flex2,
            [Flex3]         = @Flex3,
            [PostEmail2]    = @PostEmail2
    WHEN NOT MATCHED THEN
        INSERT (
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