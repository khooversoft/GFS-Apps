CREATE VIEW [App].[UserAccess]
AS
    SELECT
            x.[UserId],
            x.[PrincipalNameIdentity],
            x.[Table_ID],
            x.[ID],
            x.[Descr],
            x.[Field1],
            x.[Field2],
            x.[Field3],
            x.[Field4],
            x.[Field5],
            x.[Field6],
            x.[DateTimeStamp]
    FROM    [AppDbo].[UserAccess] x;
