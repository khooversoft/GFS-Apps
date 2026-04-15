CREATE PROCEDURE [App].[AddUserAccess]
    @PrincipalNameIdentity NVARCHAR(50),
    @Table_ID NVARCHAR(20),
    @ID VARCHAR(20),
    @Descr NVARCHAR(2000),
    @Field1 NVARCHAR(255),
    @Field2 NVARCHAR(255),
    @Field3 NVARCHAR(255),
    @Field4 NVARCHAR(255),
    @Field5 NVARCHAR(255),
    @Field6 NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    INSERT INTO [AppDbo].[UserAccess] (
        [PrincipalNameIdentity], [Table_ID], [ID], [Descr],
        [Field1], [Field2], [Field3], [Field4], [Field5], [Field6]
    )
    VALUES (
        @PrincipalNameIdentity, @Table_ID, @ID, @Descr,
        @Field1, @Field2, @Field3, @Field4, @Field5, @Field6
    );

END