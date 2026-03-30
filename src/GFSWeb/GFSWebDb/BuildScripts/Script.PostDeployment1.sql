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
MERGE [AppDbo].[PrincipalIdentity] AS target
USING (VALUES
    ('khoover@biz-bricks.com', 'Kelvin Hoover', 'khoover@biz-bricks.com', 'owner'),
    ('user2-entra', 'User2', 'User2@odysseygroup.com', 'reader'),
    ('user3-entra', 'User3', 'User3@odysseygroup.com', 'reader')
) AS src ([NameIdentifier], [UserName], [Email], [Role])
    ON target.[NameIdentifier] = src.[NameIdentifier]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([NameIdentifier], [UserName], [Email], [Role])
    VALUES (src.[NameIdentifier], src.[UserName], src.[Email], [Role]);


