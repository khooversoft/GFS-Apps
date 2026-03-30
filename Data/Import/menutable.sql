SELECT *
FROM Misc_Tables
WHERE Table_ID = 'Menu'
order by ID

select *
from Misc_Tables
where table_id = 'menu'


-- begin tran
--commit tran

update Misc_Tables
set id = '3'
where table_id = 'menu'
and id = '4'

begin tran
delete Misc_Tables where Table_Id = 'Menu'

INSERT INTO Misc_Tables (Table_ID, ID, Descr, Field1)
VALUES
    ('Menu', 0, 'Elimination GLSUs', 'top'),
    ('Menu', 1, 'Recons', 'top'),
    ('Menu', 2, 'Elim True-up JEs', 'top'),
    ('Menu', 3, 'Special Functions', 'top'),
    ('Menu', 4, 'GLSUs', 'top'),
    ('Menu', 5, 'Reports', 'top'),
    ('Menu', 6, 'More Reports', 'top'),
    ('Menu', 7, 'Writes', 'top'),
    ('Menu', 8, 'Tables', 'top'),
    ('Menu', 9, 'User Manuals', 'top');

