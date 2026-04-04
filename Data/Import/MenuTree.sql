create view MenuTreeView
AS
-- Build the TreeView data for trvList in Elim (Elim/Elim-W access)
SELECT * FROM (
    -- Top-level menu
    SELECT 'A' AS SortKey, ID, Descr, Field1 AS Parent
    FROM Misc_Tables
    WHERE Table_ID = 'Menu'

    UNION ALL

    -- Elimination processes
    SELECT 'B' AS SortKey, 'E' + RIGHT('00' + CAST(ID AS VARCHAR), 2) AS ID,
           'E' + RIGHT('00' + CAST(ID AS VARCHAR), 2) + ' - ' + ShortName + ' (' + Def + ')' AS Descr,
           '0' AS Parent
    FROM Eliminations

    UNION ALL

    -- Special "Run all Elims" node
    SELECT 'B' AS SortKey, 'E99' AS ID, 'E99 - Run all Elims' AS Descr, '0' AS Parent

    UNION ALL

    -- Recon processes
    SELECT 'C' AS SortKey, 'F' + RIGHT('00' + CAST(ID AS VARCHAR), 2) AS ID,
           'F' + RIGHT('00' + CAST(ID AS VARCHAR), 2) + ' - ' + ShortName + ' (' + Def + ')' AS Descr,
           '1' AS Parent
    FROM Eliminations

    UNION ALL

    -- Special recon nodes
    SELECT 'C' AS SortKey, 'F98' AS ID, 'F98 - Run all Doc Crcy Recons' AS Descr, '1' AS Parent
    UNION ALL
    SELECT 'C' AS SortKey, 'F99' AS ID, 'F99 - Run all Recons - LC2 (USD) Crcy' AS Descr, '1' AS Parent

    UNION ALL

    -- True-up contracts
    SELECT 'D' AS SortKey, 'P' + RIGHT('00' + CAST(ID AS VARCHAR), 2) AS ID,
           'P' + RIGHT('00' + CAST(ID AS VARCHAR), 2) + ' - ' + ShortName + ' (' + Def + ')' AS Descr,
           '2' AS Parent
    FROM Eliminations
    WHERE TrueUpContract <> 'NONE'

    UNION ALL

    -- Special functions
    SELECT 'E' AS SortKey, 'S' + ID AS ID, 'S' + ID + ' - ' + Descr AS Descr, '3' AS Parent
    FROM Misc_Tables
    WHERE Table_ID = 'SpecialFunctions'

    UNION ALL

    -- GLSUs
    SELECT 'F' AS SortKey, ID, ID + ' - ' + Descr AS Descr, '4' AS Parent
    FROM Misc_Tables
    WHERE Table_ID = 'GLSUs'

    UNION ALL

    -- Reports
    SELECT 'G' AS SortKey, ID, ID + ' - ' + Descr AS Descr, '5' AS Parent
    FROM Misc_Tables
    WHERE Table_ID = 'Reports'

    UNION ALL

    -- More Reports
    SELECT 'H' AS SortKey, ID, ID + ' - ' + Descr AS Descr, '6' AS Parent
    FROM Misc_Tables
    WHERE Table_ID = 'More Reports'

    UNION ALL

    -- Writes (only for Elim-W access, optional)
    SELECT 'I' AS SortKey, ID, ID + ' - ' + Descr AS Descr, '7' AS Parent
    FROM Misc_Tables
    WHERE Table_ID = 'Writes'

    UNION ALL

    -- Tables
    SELECT 'J' AS SortKey, ID, ID + ' - ' + Descr AS Descr, '8' AS Parent
    FROM Misc_Tables
    WHERE Table_ID = 'Tables'

    UNION ALL

    -- User Manuals
    SELECT 'K' AS SortKey, ID, ID + ' - ' + Descr AS Descr, '9' AS Parent
    FROM Misc_Tables
    WHERE Table_ID = 'UserManuals'
) a
