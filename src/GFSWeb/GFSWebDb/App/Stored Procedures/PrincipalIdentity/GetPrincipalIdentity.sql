CREATE PROCEDURE [App].[GetPrincipalIdentity]
	@NameIdentifier NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

	IF (@NameIdentifier IS NOT NULL)
	BEGIN
		SELECT	x.*
		FROM    [App].[PrincipalIdentityView] x
		WHERE   x.[NameIdentifier] = @NameIdentifier;
		RETURN;
	END

	SELECT	x.*
	FROM    [App].[PrincipalIdentityView] x;
END