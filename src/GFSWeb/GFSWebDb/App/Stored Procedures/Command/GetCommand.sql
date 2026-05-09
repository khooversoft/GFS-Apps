CREATE PROCEDURE [App].[GetCommand]
    @CommandId NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @CommandId IS NULL
    BEGIN
         SELECT  x.*
        FROM    [App].[CommandView] x;
        RETURN;
    END

    SELECT  x.*
    FROM    [App].[CommandView] x
    WHERE   x.CommandId = @CommandId;

END