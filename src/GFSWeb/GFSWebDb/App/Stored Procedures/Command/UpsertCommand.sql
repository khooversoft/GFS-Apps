CREATE PROCEDURE [App].[UpsertCommand]
    @CommandId NVARCHAR(50),
    @Description NVARCHAR(100),
    @Type NVARCHAR(50),
    @Data NVARCHAR(MAX),
    @Hash NVARCHAR(50),
    @Disabled BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO [AppDbo].[Command] AS Target
    USING (SELECT @CommandId AS CommandId) AS Source
    ON Target.CommandId = Source.CommandId
    WHEN MATCHED THEN 
        UPDATE SET Description = @Description, [Type] = @Type, Data = @Data, Hash = @Hash, Disabled = @Disabled
    WHEN NOT MATCHED THEN 
        INSERT (CommandId, Description, [Type], Data, Hash, Disabled) VALUES
            (@CommandId, @Description, @Type, @Data, @Hash, @Disabled);
END
