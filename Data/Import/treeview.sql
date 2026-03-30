-- select x.*, i.Id, i.parent
-- from MenuTreeView x
--     inner join MenuTreeView i on cast(cast(i.parent as int) + 1 as nvarchar(10)) = x.id
-- where i.parent <> 'top'
-- order by x.sortkey, x.id

select *
from MenuTreeView
where  parent = '9'
order by sortkey, id

