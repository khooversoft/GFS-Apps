/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/


-- Ensure required application roles exist
--MERGE [AppDbo].[ElimOperation] AS target
--USING (VALUES
--    ('R1-100', 'Elimination for Canada R1'),
--    ('G1-110', 'Elimination for George R2')
--) AS src ([ElimCode], [Description])
--    ON target.[ElimCode] = src.[ElimCode]
--WHEN NOT MATCHED BY TARGET THEN
--    INSERT ([ElimCode], [Description])
--    VALUES (src.[ElimCode], src.[Description]);

-- Ensure required application roles exist
MERGE [AppDbo].[PrincipalIdentity] AS target
USING (VALUES
    ('user1-entra', 'User1', 'User1@odysseygroup.com'),
    ('user2-entra', 'User2', 'User2@odysseygroup.com'),
    ('user3-entra', 'User3', 'User3@odysseygroup.com')
) AS src ([NameIdentifier], [UserName], [Email])
    ON target.[NameIdentifier] = src.[NameIdentifier]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([NameIdentifier], [UserName], [Email])
    VALUES (src.[NameIdentifier], src.[UserName], src.[Email]);


