CREATE PROCEDURE [App].[GetCommand]
    @CommandId NVARCHAR(50) = NULL,
    @Hash NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @CommandId IS NOT NULL
    BEGIN
        SELECT  x.*
        FROM    [App].[CommandView] x
        WHERE   x.CommandId = @CommandId;

        RETURN;
    END

    IF @Hash is NOT NULL
    BEGIN
        SELECT  x.*
        FROM    [App].[CommandView] x
        WHERE   x.[Hash] = @Hash;

        RETURN;
    END

    SELECT  x.*
    FROM    [App].[CommandView] x;
END