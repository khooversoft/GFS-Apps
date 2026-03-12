create  procedure [App].[AddOrUpdatePrincipalIdentity]
    @NameIdentifier NVARCHAR(50),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(50),
    @Disabled BIT = 0
AS
BEGIN
    MERGE [AppDbo].[PrincipalIdentity] AS target
    USING (VALUES (@NameIdentifier, @UserName, @Email, @Disabled)) AS src ([NameIdentifier], [UserName], [Email], [Disabled])
        ON target.[NameIdentifier] = src.[NameIdentifier]
    WHEN MATCHED THEN
        UPDATE SET
            [UserName] = src.[UserName],
            [Email] = src.[Email],
            [Disabled] = src.[Disabled]
    WHEN NOT MATCHED THEN
        INSERT ([NameIdentifier], [UserName], [Email], [Disabled])
        VALUES (src.[NameIdentifier], src.[UserName], src.[Email], src.[Disabled]);
END