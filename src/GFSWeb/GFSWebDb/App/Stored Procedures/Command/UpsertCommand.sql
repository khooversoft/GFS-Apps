CREATE PROCEDURE [App].[UpsertCommand]
    @CommandId NVARCHAR(50),
    @Description NVARCHAR(100),
    @Data NVARCHAR(MAX),
    @Disabled BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO [AppDbo].[Command] AS Target
    USING (SELECT @CommandId AS CommandId) AS Source
    ON Target.CommandId = Source.CommandId
    WHEN MATCHED THEN 
        UPDATE SET Description = @Description, Data = @Data, Disabled = @Disabled
    WHEN NOT MATCHED THEN 
        INSERT (CommandId, Description, Data, Disabled) VALUES
            (@CommandId, @Description, @Data, @Disabled);
END
