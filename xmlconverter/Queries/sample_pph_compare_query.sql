
--stored procedure to check if records from tables of two databases matches or not
create procedure sp_findDifferences @tableName nvarchar(max)
as 
begin
Declare @xmltable nvarchar(max)

SET @xmltable = 'select ''In DB_XML_DATA '+ @tableName+' table but not in DB_DATA_SET '+@tableName+' table''
				as Unmatched_records_SRC,T1.* from (select * from DB_XML_DATA.dbo.'+@tableName+' 
				Except
				select * from DB_DATA_SET.dbo.'+@tableName+') as T1
				union all
				select ''In DB_DATA_SET '+@tableName+' table but not in DB_XML_DATA '+@tableName+' table''
				as Unmatched_records_SRC,T2.* from(select * from DB_DATA_SET.dbo.'+@tableName+'
				Except
				select * from DB_XML_DATA.dbo.'+@tableName+') as T2'

exec(@xmltable)

end

--checks the table data from DB_DATA_SET and DB_XML_DATA and returns non matching rows or empty if 
--everything matches

exec sp_findDifferences @tableName='Person'
exec sp_findDifferences @tableName='Address'
exec sp_findDifferences @tableName='Language'
exec sp_findDifferences @tableName='Registration'
	exec sp_findDifferences @tableName='Caution'
	exec sp_findDifferences @tableName='Condition'
	exec sp_findDifferences @tableName='Endorsement'
	exec sp_findDifferences @tableName='Notation'
	exec sp_findDifferences @tableName='Qualification'
	exec sp_findDifferences @tableName='Reprimand'
	exec sp_findDifferences @tableName='Restriction'
	exec sp_findDifferences @tableName='Specialty'
	exec sp_findDifferences @tableName='Undertaking'


--drop proc sp_findDifferences
