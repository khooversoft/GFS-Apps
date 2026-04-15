CREATE PROCEDURE [App].[AddOrUpdateUserAccess]
    @PrincipalNameIdentity NVARCHAR(50),
    @Table_ID NVARCHAR(20),
    @ID VARCHAR(20),
    @Descr NVARCHAR(2000),
    @Field1 NVARCHAR(255) NULL,
    @Field2 NVARCHAR(255) NULL,
    @Field3 NVARCHAR(255) NULL,
    @Field4 NVARCHAR(255) NULL,
    @Field5 NVARCHAR(255) NULL,
    @Field6 NVARCHAR(255) NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @PrincipalNameIdentity IS NULL
    BEGIN
        RAISERROR('@PrincipalNameIdentity is required', 16, 1);
        RETURN;
    END

    IF @Table_ID IS NULL
    BEGIN
        RAISERROR('@Table_ID is required', 16, 1);
        RETURN;
    END

    IF @Field1 IS NULL
    BEGIN
        RAISERROR('@Field1 is required', 16, 1);
        RETURN;
    END

    BEGIN TRAN;

    -- Check if @PrincipalNameIdentity is an [UserName] and if so, get the corresponding NameIdentifier
    DECLARE @Identity VARCHAR(50) = (SELECT TOP 1 [NameIdentifier] FROM [AppDbo].[PrincipalIdentity] WHERE [UserName] = @PrincipalNameIdentity);
    IF(@Identity IS NOT NULL) SET @PrincipalNameIdentity = @Identity;

    IF(NOT EXISTS (SELECT 1 FROM [AppDbo].[PrincipalIdentity] WHERE [NameIdentifier] = @PrincipalNameIdentity))
    BEGIN
        RAISERROR('PrincipalIdentity with NameIdentifier %s does not exist', 16, 1, @PrincipalNameIdentity);
        ROLLBACK TRAN;
        RETURN;
    END

    MERGE [AppDbo].[UserAccess] AS target
    USING (
        SELECT
            @PrincipalNameIdentity  AS [PrincipalNameIdentity],
            @Table_ID               AS [Table_ID],
            @Field1                 AS [Field1]
    ) AS source
    ON  target.[PrincipalNameIdentity] = source.[PrincipalNameIdentity]
    AND target.[Table_ID]              = source.[Table_ID]
    AND target.[Field1]                = source.[Field1]
    WHEN MATCHED THEN
        UPDATE SET
            [ID]            = @ID,
            [Descr]         = @Descr,
            [Field2]        = @Field2,
            [Field3]        = @Field3,
            [Field4]        = @Field4,
            [Field5]        = @Field5,
            [Field6]        = @Field6
    WHEN NOT MATCHED THEN
        INSERT (
            [PrincipalNameIdentity], [Table_ID], [ID], [Descr],
            [Field1], [Field2], [Field3], [Field4], [Field5], [Field6]
        )
        VALUES (
            @PrincipalNameIdentity, @Table_ID, @ID, @Descr,
            @Field1, @Field2, @Field3, @Field4, @Field5, @Field6
        );

    COMMIT TRAN;
END