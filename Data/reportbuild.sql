/*
Script Name: ADO150826_ElimR19_Update.sql

Description: Update Elim S43 and F43 to add missing contracts

Recurrence: none

Runtime:
*/
use CorpAcct_Apps
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET IMPLICIT_TRANSACTIONS OFF
SET NOCOUNT ON

declare @lvsProcName		varchar(100) = 'ADO150826_ElimR19_Update.sql';
declare @lvsInServer		varchar(100) = QuoteName('GFSdb'); -- GFSDB, GFSDBUAT, GFSDBDEV
declare @lvsTargetDb		varchar(100) = 'CorpAcct_Apps'; -- CorpAcct_Apps/FX_Recon
declare @lvsConnectedDb		varchar(100);
declare @lvsDbName			nvarchar(100);
declare @lvstableName		nvarchar(100);
declare @lvsActMsg			varchar(300);
declare @lvsLogString 		varchar(max);
declare @lvnUpdCnt			int = 0; -- row counts
declare @lvnDelCnt			int = 0; -- row counts

begin
	set @lvsConnectedDb = DB_NAME();
		if UPPER(@lvsTargetDb) <> UPPER(@lvsConnectedDb)
		begin
			SET @lvsActMsg = 'You are not currently connected to '+ @lvsConnectedDb+'. Please verify connection and try again.';
			print @lvsActMsg;
			return;
		end;
begin tran
	begin try

		delete from Misc_Tables where Table_ID = 'SQL-R19' or ID in ('R19','R19a','R19b','R19c','R19d','R19e');

		set @lvsActMsg = 'delete from Misc_Tables for Elim R19'
		set @lvnDelCnt = @@ROWCOUNT;
		set @lvsLogString = CAST(@lvnDelCnt as varchar(10)) + ' rows deleted from Misc_Tables for Elim R19';

		print @lvsLogString;
		EXEC dbo.ProcLog	
				@P_LogText = @lvsLogString,
				@P_ProcName = @lvsProcName;

		Insert into Misc_Tables (Table_ID,ID,Descr,Field1,Field2,Field3,Field4,Field5,Field6) values 
		('SQL-R19','103','insert into TransX (KeyID, Co, GL, PC, TP, TTY, Amt1, Amt2, Amt3, Amt4, Amt5, Flex6) (select KeyID, Co, GL, PC, TP, TTY, ROUND(SUM(COALESCE(Amt1,0)),2) as Amt1, ROUND(SUM(COALESCE(Amt2,0)),2) as Amt2, ROUND(SUM(COALESCE(Amt3,0)),2) as Amt3, ROUND(SUM(COALESCE(Amt4,0)),2) as Amt4, ROUND(SUM(COALESCE(Amt5,0)),2) as Amt5, ''pass1'' as Flex6  from TransX where KeyID = ''[batchID]'' group by KeyID, Co, GL, PC,TP, TTY)','','','','','',''),
		('SQL-R19','104','delete from TransX where KeyID = ''[batchID]'' and Flex6 is NULL','','','','','',''),
		('SQL-R19','105','delete from TransX where KeyID = ''[batchID]'' and Amt1 = 0 and Amt2 = 0 and Amt3 = 0 and Amt4 = 0 and Amt5 = 0','','','','','',''),
		('SQL-R19','110','update TransX set GLdesc = c.Descr from TransX t inner join COA c on t.GL = c.GL where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','111','update TransX set TPname = c.Name from TransX t inner join TPs c on t.TP = c.TP where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','112','update TransX set Hierarchy = p.Division from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''Rpt'' inner join ElimSelect e on e.FromVal = p.Division and e.ElimID = ''Co:'' + m.Field1 where KeyID = ''[batchID]'' and t.Hierarchy is NULL','','','','','',''),
		('SQL-R19','113','update TransX set Hierarchy = p.Region from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''Rpt'' inner join ElimSelect e on e.FromVal = p.Region and e.ElimID = ''Co:'' + m.Field1 where KeyID = ''[batchID]'' and t.Hierarchy is NULL','','','','','',''),
		('SQL-R19','114','update TransX set Hierarchy = p.Location from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''Rpt'' inner join ElimSelect e on e.FromVal = p.Location and e.ElimID = ''Co:'' + m.Field1 where KeyID = ''[batchID]'' and t.Hierarchy is NULL','','','','','',''),
		('SQL-R19','115','update TransX set Hierarchy = p.Zone from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''Rpt'' inner join ElimSelect e on e.FromVal = p.Zone and e.ElimID = ''Co:'' + m.Field1 where KeyID = ''[batchID]'' and t.Hierarchy is NULL','','','','','',''),
		('SQL-R19','116','update TransX set Hierarchy = p.Hierarchy from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''Rpt'' inner join ElimSelect e on e.FromVal = p.Hierarchy and e.ElimID = ''Co:'' + m.Field1 where KeyID = ''[batchID]'' and t.Hierarchy is NULL','','','','','',''),
		('SQL-R19','121','update TransX set Hierarchy = p.Division from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect2'' and p.Division like m.Field2 and m.Field2 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','122','update TransX set Hierarchy = p.Region from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect2'' and p.Region like m.Field2 and m.Field2 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','123','update TransX set Hierarchy = p.Location from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect2'' and p.Location like m.Field2 and m.Field2 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','124','update TransX set Hierarchy = p.Zone from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect2'' and p.Zone like m.Field2 and m.Field2 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','125','update TransX set Hierarchy = p.Hierarchy from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect2'' and p.Hierarchy like m.Field2 and m.Field2 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','131','update TransX set Hierarchy = p.Division from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect1'' and t.Co = m.Field2 and p.Division like m.Field3 and m.Field3 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','132','update TransX set Hierarchy = p.Region from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect1'' and t.Co = m.Field2 and p.Region like m.Field3 and m.Field3 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','133','update TransX set Hierarchy = p.Location from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect1'' and t.Co = m.Field2 and p.Location like m.Field3 and m.Field3 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','134','update TransX set Hierarchy = p.Zone from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect1'' and t.Co = m.Field2 and p.Zone like m.Field3 and m.Field3 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','135','update TransX set Hierarchy = p.Hierarchy from TransX t inner join PCTRS p on t.PC = p.PCTR inner join Misc_Tables m on m.Descr = ''[CoSelect]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect1'' and t.Co = m.Field2 and p.Hierarchy like m.Field3 and m.Field3 <> '''' where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','141','update TransX set Flex1 = m.Field1 from TransX t inner join Misc_Tables m on m.Field2 = t.Co and (t.Hierarchy like m.Field3 or m.Field3 = '''') where KeyID = ''[batchID]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect1'' and m.Descr = ''[CoSelect]''','','','','','',''),
		('SQL-R19','142','update TransX set Flex1 = m.Field1 from TransX t inner join Misc_Tables m on m.Field2 = t.Hierarchy or m.Field2 = '''' where KeyID = ''[batchID]'' and m.Table_ID = ''Flux'' and m.ID = ''TabSelect2'' and m.Descr = ''[CoSelect]'' and Flex1 is NULL','','','','','',''),
		('SQL-R19','143','update TransX set Flex1 = Co where KeyID = ''[batchID]'' and ''[CoSelect]'' like ''% by Co''','','','','','',''),
		('SQL-R19','145','update TransX set Flex2 = m.Descr from TransX t inner join Misc_Tables m on t.GL >= m.Field1 and t.GL <= m.Field2 where KeyID = ''[batchID]'' and m.Table_ID = ''Flux'' and m.ID = ''GLgrp''','','','','','',''),
		('SQL-R19','151','insert into TransX (KeyID, Co, GL, GLdesc, TP, TPname, Flex2, Hierarchy, Flex1, PC, TTY, Amt1, Amt2, Amt3, Amt4,Amt5, Flex5) (select KeyID, Co, GL, GLdesc, TP, TPname, Flex2, Hierarchy, Flex1, PC, TTY, ROUND(SUM(COALESCE(Amt1,0)),2) as Amt1, ROUND(SUM(COALESCE(Amt2,0)),2) as Amt2, ROUND(SUM(COALESCE(Amt3,0)),2) as Amt3, ROUND(SUM(COALESCE(Amt4,0)),2) as Amt4, ROUND(SUM(COALESCE(Amt5,0)),2) as Amt5, ''pass2'' as Flex5  from TransX where KeyID = ''[batchID]'' group by KeyID, Co, GL, GLdesc, TP, TPname, Flex2, Hierarchy, Flex1, PC, TTY)','','','','','',''),
		('SQL-R19','152','delete from TransX where KeyID = ''[batchID]'' and Flex5 is NULL','','','','','',''),
		('SQL-R19','153','delete from TransX where KeyID = ''[batchID]'' and Amt1 = 0 and Amt2 = 0 and Amt3 = 0 and Amt4 = 0 and Amt5 = 0','','','','','',''),
		('SQL-R19','161','update TransX set Flex4 = p.hierarchy, PCDesc = p.Descr from TransX t inner join PCTRS p on t.PC = p.PCTR where KeyID = ''[batchID]''','','','','','',''),
		('SQL-R19','162','update TransX set Flex3 = c.Descr from TransX t inner join TTYs c on t.TTY = c.TTY where KeyID = ''[batchID]''','','','','','','');

        set @lvsActMsg = 'Inserting into Misc_Tables for Elim R19 logic'
		set @lvnUpdCnt = @@ROWCOUNT;
		set @lvsLogString = CAST(@lvnUpdCnt as varchar(10)) + ' rows inserted into Misc_Tables for Elim R19 logic';

		print @lvsLogString;
		EXEC dbo.ProcLog	
				@P_LogText = @lvsLogString,
				@P_ProcName = @lvsProcName;

		Insert into Misc_Tables (Table_ID,ID,Descr,Field1,Field2,Field3,Field4,Field5,Field6) values 
		('Reports','R19','Balance Sheet Flux','BS Flux','','','','',''),
		('GLSUtemplate','R19','R19 Template.xlsx','','','','','',''),
		('Inputs','R19','Co','Flux','','','','',''),
		('Inputs','R19','Crcy','FuncUSD','','','','',''),
		('ReconExcelHdrs1','R19','Co,GL,Gldesc,TP,TPname,Flex2,Hierarchy,Flex1,TTY,Flex3,PC,PCDesc,Flex4,Amt1,Amt2,Amt3,Amt4,Amt5','','','','','',''),
		('ReconExcelHdrs2','R19','Co,GL,GLdesc,TP,TPname,GLgrp,Hierarchy,TabName,TTY,TTY Desc,PC,PC Desc,Segment,CurrQtrCurrYr,PrQtrCurrYr,CurrQtrPrYr,PrQtrPrYr,2PrQtrCurrYr','','','','','',''),
		('ReconExcelComma','R19','N:R','','','','','',''),
		('ReconExcelEndColm','R19','R','','','','','',''),
		('ReconExcelPivot','R19','n','','','','','',''),
		('Data1RangeNames','R19','SelectAll','A1','Q','','','',''),
		('Data1RangeNames','R19','Account','B2','B','','','',''),
		('Data1RangeNames','R19','GLgrp','F2','F','','','',''),
		('Data1RangeNames','R19','TabName','H2','H','','','',''),
		('Data1RangeNames','R19','TransType','I2','I','','','',''),
		('Data1RangeNames','R19','CQCY','N2','N','','','',''),
		('Data1RangeNames','R19','PQCY','O2','O','','','',''),
		('Data1RangeNames','R19','CQPY','P2','P','','','',''),
		('Data1RangeNames','R19','PQPY','Q2','Q','','','',''),
		('Data1RangeNames','R19','PPQCY','R2','R','','','',''),
		('Data1AsOfStamp','R19','U1','','','','','',''),
		('Data1AsAtStamp','R19','X1','','','','','',''),
		('FieldSelect','R19a','COCODE','a','','','','',''),
		('FieldSelect','R19a','GL','b','','','','',''),
		('FieldSelect','R19a','PROFITCENTER','c','','','','',''),
		('FieldSelect','R19a','TRADINGPARTNER','d','','','','',''),
		('FieldSelect','R19a','TTY','e','','','','',''),
		('OutSelect','R19a','COCODE','a','','','','',''),
		('OutSelect','R19a','ACCOUNT','b','','','','',''),
		('OutSelect','R19a','PROFITCENTER','c','','','','',''),
		('OutSelect','R19a','TRADINGPARTNER','d','','','','',''),
		('OutSelect','R19a','TTY','e','','','','',''),
		('AmtSelect','R19a','ITD','','','','','',''),
		('CrcySelect','R19a','[input]','','','','','',''),
		('LedgerSelect','R19a','UG','a','','','','',''),
		('SQLselect','R19a','insert into TransX (KeyID, YearPeriod, Yr, Period, Co, GL, PC, TP, TTY, Amt1) values (''[batchID]'',''[Yr][Mo]'',[Yr],[Mo],','','','','','',''),
		('FieldSelect','R19b','COCODE','a','','','','',''),
		('FieldSelect','R19b','GL','b','','','','',''),
		('FieldSelect','R19b','PROFITCENTER','c','','','','',''),
		('FieldSelect','R19b','TRADINGPARTNER','d','','','','',''),
		('FieldSelect','R19b','TTY','e','','','','',''),
		('OutSelect','R19b','COCODE','a','','','','',''),
		('OutSelect','R19b','ACCOUNT','b','','','','',''),
		('OutSelect','R19b','PROFITCENTER','c','','','','',''),
		('OutSelect','R19b','TRADINGPARTNER','d','','','','',''),
		('OutSelect','R19b','TTY','e','','','','',''),
		('AmtSelect','R19b','ITD','','','','','',''),
		('CrcySelect','R19b','[input]','','','','','',''),
		('LedgerSelect','R19b','UG','a','','','','',''),
		('SQLselect','R19b','insert into TransX (KeyID, YearPeriod, Yr, Period, Co, GL, PC, TP, TTY, Amt2) values (''[batchID]'',''[Yr][PQ]'',[Yr],[PQ],','','','','','',''),
		('FieldSelect','R19c','COCODE','a','','','','',''),
		('FieldSelect','R19c','GL','b','','','','',''),
		('FieldSelect','R19c','PROFITCENTER','c','','','','',''),
		('FieldSelect','R19c','TRADINGPARTNER','d','','','','',''),
		('FieldSelect','R19c','TTY','e','','','','',''),
		('OutSelect','R19c','COCODE','a','','','','',''),
		('OutSelect','R19c','ACCOUNT','b','','','','',''),
		('OutSelect','R19c','PROFITCENTER','c','','','','',''),
		('OutSelect','R19c','TRADINGPARTNER','d','','','','',''),
		('OutSelect','R19c','TTY','e','','','','',''),
		('AmtSelect','R19c','ITD','','','','','',''),
		('CrcySelect','R19c','[input]','','','','','',''),
		('LedgerSelect','R19c','UG','a','','','','',''),
		('SQLselect','R19c','insert into TransX (KeyID, YearPeriod, Yr, Period, Co, GL, PC, TP, TTY, Amt3) values (''[batchID]'',''[PY][Mo]'',[PY],[Mo],','','','','','',''),
		('FieldSelect','R19d','COCODE','a','','','','',''),
		('FieldSelect','R19d','GL','b','','','','',''),
		('FieldSelect','R19d','PROFITCENTER','c','','','','',''),
		('FieldSelect','R19d','TRADINGPARTNER','d','','','','',''),
		('FieldSelect','R19d','TTY','e','','','','',''),
		('OutSelect','R19d','COCODE','a','','','','',''),
		('OutSelect','R19d','ACCOUNT','b','','','','',''),
		('OutSelect','R19d','PROFITCENTER','c','','','','',''),
		('OutSelect','R19d','TRADINGPARTNER','d','','','','',''),
		('OutSelect','R19d','TTY','e','','','','',''),
		('AmtSelect','R19d','ITD','','','','','',''),
		('CrcySelect','R19d','[input]','','','','','',''),
		('LedgerSelect','R19d','UG','a','','','','',''),
		('SQLselect','R19d','insert into TransX (KeyID, YearPeriod, Yr, Period, Co, GL, PC, TP, TTY, Amt4) values (''[batchID]'',''[PY][PQ]'',[PY],[PQ],','','','','','',''),
		('FieldSelect','R19e','COCODE','a','','','','',''),
		('FieldSelect','R19e','GL','b','','','','',''),
		('FieldSelect','R19e','PROFITCENTER','c','','','','',''),
		('FieldSelect','R19e','TRADINGPARTNER','d','','','','',''),
		('FieldSelect','R19e','TTY','e','','','','',''),
		('OutSelect','R19e','COCODE','a','','','','',''),
		('OutSelect','R19e','ACCOUNT','b','','','','',''),
		('OutSelect','R19e','PROFITCENTER','c','','','','',''),
		('OutSelect','R19e','TRADINGPARTNER','d','','','','',''),
		('OutSelect','R19e','TTY','e','','','','',''),
		('AmtSelect','R19e','ITD','','','','','',''),
		('CrcySelect','R19e','[input]','','','','','',''),
		('LedgerSelect','R19e','UG','a','','','','',''),
		('SQLselect','R19e','insert into TransX (KeyID, YearPeriod, Yr, Period, Co, GL, PC, TP, TTY, Amt5) values (''[batchID]'',''[Yr][2PQ]'',[Yr],[2PQ],','','','','','','');

		set @lvsActMsg = 'Inserting into Misc_Tables for Elim R19 output'
		set @lvnUpdCnt = @@ROWCOUNT;
		set @lvsLogString = CAST(@lvnUpdCnt as varchar(10)) + ' rows inserted into Misc_Tables for Elim R19 output';

		print @lvsLogString;
		EXEC dbo.ProcLog	
				@P_LogText = @lvsLogString,
				@P_ProcName = @lvsProcName;


		commit
	end try
	begin catch
		print ERROR_MESSAGE();	
		print @lvsActMsg;
	
		EXEC dbo.ErrorLogInsert
		if @@TRANCOUNT > 0 rollback;
	end catch
end;
go
