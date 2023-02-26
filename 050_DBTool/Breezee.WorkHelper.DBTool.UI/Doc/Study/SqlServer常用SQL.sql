/*SQL�����*/
sys.objects��syscolumns��systypes��sys.default_constraints��sys.all_sql_modules

/*��ѯ����Ϣ*/
SELECT b.name OWER,a.name Table_Name,a.* 
FROM sys.objects a 
JOIN sys.schemas b ON a.schema_id=b.schema_id WHERE a.type='U' AND a.name='FI_MATAINGATHER_M'

/*��ִ̬��SQL������sp_executesql��ʾ��*/
EXECUTE sp_executesql 
          N'SELECT * FROM pa_part_list 
          WHERE partno = @level',
          N'@level varchar(50)',
          @level = '0009141430-A034';

/*ָ���ֶ�ָ��λ��Ϊ�̶�ֵ��SQL*/
SELECT * FROM dbo.PA_PART_LIST WHERE Charindex('A',PARTNO)=2 OR Charindex('1',PARTNO)=2

/*��������CLR��䣬������ʹ��VS 2005�����Ĵ洢����*/
EXEC sp_configure 'clr enabled' , 1 
RECONFIGURE WITH override

/*�������÷ֲ�ʽ������Ҫ��Distributed Transaction Coordinator��������*/
---�������̣������С�==>��net start msdtc��

-/*�������ǰ5������������ĸ���ձ�1��A�� 2��B�� 3��C�� 4��D�� 5��E�� 6��F�� 7��G�� 8��H�� 9��K�� 0��M����
 ��ѯ��������ڵ���ǰ��λ������Ϊ��ͷ�ĺ�ģ����ѯʾ��������ֻ����λΪ����*/ 
 SELECT * FROM PA_PART_LIST 
 WHERE 1=1 --AND Charindex('5',PARTNO)=1
 AND (Charindex('E',PARTNO)=1 OR Charindex('5',PARTNO)=1)
 AND (Charindex('B',PARTNO)=2 OR Charindex('2',PARTNO)=2) AND PARTNO LIKE '%112A%'

/*SQL Server������־�����ݿ��ļ�����*/
--�������ݿ��ļ��� 
DBCC SHRINKDATABASE(@dbName ) 
DUMP TRANSACTION @dbName WITH NO_LOG 
--�ض�������־�� 
BACKUP LOG @dbName WITH NO_LOG 


/*������־�ļ�*/ 
DBCC SHRINKFILE 
( 
{ 'file_name'  file_id } 
{ [ , EMPTYFILE ] 
 [ [ , target_size ] [ , { NOTRUNCATE  TRUNCATEONLY } ] ] 
} 
) 
[ WITH NO_INFOMSGS ] 

/*�޸ı�������*/
if Not Exists(Select 1 From sysobjects a,syscolumns b 
Where a.id = b.id And a.name = 'PA_STORAGE' And b.name = 'RDCQTY' And a.type = 'U')
ALTER TABLE PA_STORAGE ADD RDCQTY DECIMAL(14,2) /*RDC������*/

/*����ת��*/
--PIVOT �ṩ���﷨��һϵ�и��ӵ� SELECT...CASE �������ָ�����﷨���򵥡����߿ɶ��ԡ�
--�����Ŀ��ܻ��õ� PIVOT �������ǣ���Ҫ���ɽ����񱨱��Ի�������ʱ��
--A����ת��ʾ��(��Ҫɸѡ��������ĳ��ֵ��Ϊ�����У������е�ֵ���ݸ�ֵ�����ܵ�)��
USE AdventureWorks;
GO
SELECT VendorID, [164] AS Emp1, [198] AS Emp2, [223] AS Emp3, [231] AS Emp4, [233] AS Emp5
FROM 
(SELECT PurchaseOrderID, EmployeeID, VendorID
FROM PurchaseOrderHeader) p
PIVOT
(
COUNT (PurchaseOrderID)
FOR EmployeeID IN
( [164], [198], [223], [231], [233] )
) AS pvt
ORDER BY VendorID
---B����ת��
--UNPIVOT ���� PIVOT ִ�м�����ȫ�෴�Ĳ���������ת��Ϊ�С�
CREATE TABLE pvt (VendorID int, Emp1 int, Emp2 int,
Emp3 int, Emp4 int, Emp5 int)
GO
INSERT INTO pvt VALUES (1,4,3,5,4,4)
INSERT INTO pvt VALUES (2,4,1,5,5,5)
INSERT INTO pvt VALUES (3,4,3,5,4,4)
INSERT INTO pvt VALUES (4,4,2,5,5,4)
INSERT INTO pvt VALUES (5,5,1,5,5,5)
GO
--Unpivot the table.
SELECT VendorID, Employee, Orders
FROM 
   (SELECT VendorID, Emp1, Emp2, Emp3, Emp4, Emp5
   FROM pvt) p
UNPIVOT
   (Orders FOR Employee IN 
      (Emp1, Emp2, Emp3, Emp4, Emp5)
)AS unpvt
GO

/*��д��־ɾ������*/
TRUNCATE TABLE ����;

/*��ȡ��ָ���������ȡ��ָ��λ���м�¼*/
SELECT * FROM (
select ROW_NUMBER() OVER(ORDER BY createtime) rowNum,*
 FROM UC_T_CUST_TYPE
  WHERE CHECKSTATE = '2'
 AND ISNULL(STORE_STATUS, 0) = '0'
  AND ISENABLE = '1') a
where rowNum BETWEEN 1 AND 10

/*����ָ������������ֵ��¼*/
Set   IDENTITY_INSERT   Temp_Test   ON   
INSERT   INTO   Temp_Test(CHEKCID,CHEKCLISTNO,CheckModel )   
SELECT   CHEKCID,CHEKCLISTNO,CheckModel    
FROM dbo.PA_STORAGE_CHECK_M WHERE CHEKCLISTNO='PC100903'
Set   IDENTITY_INSERT   Temp_Test   Off

/*�����±�*/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OP_PLAN_Audit]') AND type in (N'U'))
CREATE TABLE [dbo].[OP_PLAN_Audit] (
[ID]  [BIGINT ] IDENTITY(1,1) NOT NULL,
[YEAR]  [int ] NULL,
[MONTH]  [int ] NULL,
[ISENABLE]  [VARCHAR ](1) NULL,
[ISSYSTEM]  [VARCHAR ](1) NULL,
[NETCODE]  [VARCHAR ](10) NULL,
CONSTRAINT [ OP_Audit_ID] PRIMARY KEY CLUSTERED  ( [ID] ASC ))  
GO
---����ͳ���õ��ı�PA_TEMP_IN_OUT_TOTAL������ڣ���ɾ�����ؽ�
  IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PA_TEMP_IN_OUT_TOTAL]') AND type in (N'U'))
    TRUNCATE TABLE PA_TEMP_IN_OUT_TOTAL;
 CREATE TABLE PA_TEMP_IN_OUT_TOTAL
 (
      PARTNO VARCHAR(50),/*�������*/
      PARTNAME VARCHAR(50),/*��������*/
      UNIT VARCHAR(50),/*��λ*/
      JPXSLR DECIMAL(14, 2)/*��Ʒ��������*/
 )

/*Union��Union all ������*/
�����ݿ��У�union��union all�ؼ��ֶ��ǽ�����������ϲ�Ϊһ�����������ߴ�ʹ�ú�Ч������˵��������ͬ��
union�ڽ��б����Ӻ��ɸѡ���ظ��ļ�¼�������ڱ����Ӻ����������Ľ���������������㣬ɾ���ظ��ļ�¼�ٷ��ؽ����
�磺
select * from test_union1
   union
select * from test_union2
      ���SQL������ʱ��ȡ��������Ľ������������ռ��������ɾ���ظ��ļ�¼����󷵻ؽ�������������������Ļ����ܻᵼ���ô��̽�������
    ��union allֻ�Ǽ򵥵Ľ���������ϲ���ͷ��ء�������������ص���������������ظ������ݣ���ô���صĽ�����ͻ�����ظ��������ˡ�
     ��Ч����˵��union allҪ��union��ܶ࣬���ԣ��������ȷ�Ϻϲ�������������в������ظ������ݵĻ�����ô��ʹ��union all�����£�
select * from test_union1
union all
select * from test_union2
ʹ�� union ��ϲ�ѯ�Ľ����������������Ĺ���
1�����в�ѯ�е��������е�˳�������ͬ��
2���������ͱ������

/*�ж���ʱ���Ƿ����*/
 if object_id('tempdb..#TEMP_TABLE') is not null 
 print '��ʱ��#TEMP_TABLE�Ѵ���!' 

/*ɾ��SQL Server��JOB���*/
DECLARE @JobID VARBINARY(100)
IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'P_AJUST_PA_MONTH_TOTAL')
BEGIN
SELECT @JobID=job_id FROM msdb.dbo.sysjobs_view WHERE name = N'P_AJUST_PA_MONTH_TOTAL'
EXEC msdb.dbo.sp_delete_job @job_id=@JobID, @delete_unused_schedule=1
END

/*��������Ʒ��*/
---����IDΪ2�ļ�¼��ɾ��������
IF EXISTS(SELECT * FROM SE_CARBRAND WHERE CARBRAND_ID=2)
DELETE FROM SE_CARBRAND WHERE CARBRAND_ID=2
GO
SET IDENTITY_INSERT SE_CARBRAND ON
INSERT INTO SE_CARBRAND(CARBRAND_ID ,
        CARBRAND_CODE ,
        BRANDNAME ,
        REMARK ,
        ORDERNO ,
        IS_HD ,
        CREATETIME ,
        CREATOR ,
        LAST_UPDATED_DATE ,
        MODIFIER ,
        ISENABLE ,
        ISSYSTEM ,
        NETCODE)
SELECT 2,--CARBRAND_ID ,
       2,--CARBRAND_CODE ,
        '����',--BRANDNAME ,
        '',--REMARK ,
        '2',--ORDERNO ,
        '1',--IS_HD ,
        GETDATE(),--CREATETIME ,
        'Sys',--CREATOR ,
        GETDATE(),--LAST_UPDATED_DATE ,
        'Sys',--MODIFIER ,
        '1',--ISENABLE ,
        '0',--ISSYSTEM ,
        NETCODE 
FROM dbo.BD_SYSINFO WHERE DLR_TYPE=1
SET IDENTITY_INSERT SE_CARBRAND OFF
GO

/*ֻ��ά�޳���ı������������*/
SELECT CAL_MONTH,CAST(SUM(REAL_PZ)/SUM(TOTAL_PZ)*100  AS DECIMAL(14,2)) PZ_RATE,
CASE WHEN SUM(PARTQTY)=0 THEN 0 else CAST((SUM(PARTQTY)-SUM(ISNULL(LP_QTY,0)))*100/SUM(PARTQTY) AS DECIMAL(14,2)) end QTY_RATE,
CASE WHEN SUM(PARTQTY*PRICE)=0 THEN 0 else CAST((SUM(PARTQTY*PRICE)-SUM(ISNULL(LP_QTY,0)*PRICE))*100/SUM(PARTQTY*PRICE) AS DECIMAL(14,2)) end MONEY_RATE
INTO #T_REPAIR_LP
FROM (
	SELECT MONTH(A.CREATETIME) CAL_MONTH,A.PARTNO,CASE WHEN ISNULL(B.QTY,0)=0 THEN 1 
		WHEN A.PARTQTY=B.QTY THEN 0 ELSE 0.5 END REAL_PZ,
		1 TOTAL_PZ,A.PARTQTY,B.QTY AS LP_QTY,A.PRICE
	FROM	(SELECT A.REPAIRPAPER_ID,A.CREATETIME,B.PARTNO,SUM(B.PARTQTY) PARTQTY,
						CAST(SUM(B.PRICE)/COUNT(*) AS DECIMAL(14,2)) PRICE
				 FROM dbo.SE_REPAIRPAPER A
				 JOIN dbo.SE_REPAIRPART B ON A.REPAIRPAPER_ID=B.REPAIRPAPER_ID
				 JOIN dbo.PA_PART_LIST C ON C.PARTNO=B.PARTNO
				 WHERE B.IS_OUT_STORAGE = '1' /*Ҫ�����*/
					 AND A.ISENABLE=1/*��Ч��*/
					 AND YEAR(A.CREATETIME)='#YEAR#'/*�Դ�������Ϊ׼*/
					 AND A.NETCODE='#NETCODE#' /*���������*/
					 AND B.REPAIR_TYPE='#REPAIR_TYPE#' 
					 #MUST_TYPE# 
				GROUP BY A.REPAIRPAPER_ID,A.CREATETIME,B.PARTNO
				)A
		LEFT JOIN (SELECT AA.REL_ID,BB.PARTNO,SUM(QTY) QTY
					FROM dbo.PA_LP_BOOK_M AA,dbo.PA_LP_BOOK_D BB 
					WHERE  AA.BOOK_M_ID=BB.BOOK_M_ID AND AA.REL_TYPE='1'/*ά�޳���*/
						AND	 AA.AUDIT_STATUS='2' /*���ͨ��*/
						AND YEAR(AA.AUDIT_DATE)='#YEAR#'/*���������Ϊ׼*/
						AND AA.NETCODE='#NETCODE#' /*���������*/
					GROUP BY AA.REL_ID,BB.PARTNO 
			  ) B ON B.REL_ID=A.REPAIRPAPER_ID AND B.PARTNO=A.PARTNO
	 
 ) AA
GROUP BY  CAL_MONTH 
SELECT  1 AS SORTNUM,'����(%)' CALYEAR ,SUM([1]) [1],SUM([2]) [2] ,SUM([3]) [3] ,SUM([4]) [4] ,SUM([5]) [5] ,SUM([6]) [6],
  SUM([7]) [7] ,SUM([8]) [8] ,SUM([9]) [9] ,SUM([10]) [10] ,SUM([11]) [11] ,SUM([12]) [12]
FROM #T_REPAIR_LP p PIVOT
( SUM(PZ_RATE) FOR CAL_MONTH IN ( [1], [2], [3], [4], [5], [6], [7],
                                            [8], [9], [10], [11], [12] ) ) AS pvt
UNION
SELECT  2 AS SORTNUM,'����(%)' CALYEAR ,SUM([1]) [1],SUM([2]) [2] ,SUM([3]) [3] ,SUM([4]) [4] ,SUM([5]) [5] ,SUM([6]) [6],
  SUM([7]) [7] ,SUM([8]) [8] ,SUM([9]) [9] ,SUM([10]) [10] ,SUM([11]) [11] ,SUM([12]) [12]
FROM #T_REPAIR_LP p PIVOT
( SUM(QTY_RATE) FOR CAL_MONTH IN ( [1], [2], [3], [4], [5], [6], [7],
                                            [8], [9], [10], [11], [12] ) ) AS pvt
UNION
SELECT  3,'���(%)' AS CALYEAR ,SUM([1]) [1],SUM([2]) [2] ,SUM([3]) [3] ,SUM([4]) [4] ,SUM([5]) [5] ,SUM([6]) [6],
  SUM([7]) [7] ,SUM([8]) [8] ,SUM([9]) [9] ,SUM([10]) [10] ,SUM([11]) [11] ,SUM([12]) [12]
FROM  #T_REPAIR_LP p PIVOT
( SUM(MONEY_RATE) FOR CAL_MONTH IN ( [1], [2], [3], [4], [5], [6], [7], [8],
                                     [9], [10], [11], [12] ) ) AS pvt
ORDER BY SORTNUM

/*��ѯ��*/
Select * From sysobjects a,syscolumns b 
Where a.id = b.id and b.name = 'LOCATIONCODE' And a.type = 'U'

/*ִ�ж�̬SQL������ֵ*/
DECLARE @IntVariable int;
DECLARE @SQLString nvarchar(500);
DECLARE @ParmDefinition nvarchar(500);
DECLARE @max_title varchar(30);

SET @IntVariable = 197;
SET @SQLString = N'SELECT @max_titleOUT = max(Title) 
   FROM AdventureWorks.HumanResources.Employee
   WHERE ManagerID = @level';
SET @ParmDefinition = N'@level tinyint, @max_titleOUT varchar(30) OUTPUT';

EXECUTE sp_executesql @SQLString, @ParmDefinition, @level = @IntVariable, @max_titleOUT=@max_title OUTPUT;
SELECT @max_title;
 
/*ɾ���洢����*/
--1��ɾ��CLR�洢����
IF ( OBJECT_ID('P_P_E3S_OWEQTY_RECEIPT', 'PC') IS NOT NULL ) 
	DROP PROCEDURE [dbo].[P_P_E3S_OWEQTY_RECEIPT] ; 
--2��ɾ��SQL��CLR�洢����
IF ( OBJECT_ID('P_PA_E3S_OUTSTOCK_RECEIPT') IS NOT NULL ) 
	DROP PROCEDURE P_PA_E3S_OUTSTOCK_RECEIPT
--3��ɾ��SQL��CLR�洢����
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IFR_UC_T_BASE_ASSESSMENT_ITEMProcedure]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[IFR_UC_T_BASE_ASSESSMENT_ITEMProcedure]

/*�α�Ƕ��*/
DECLARE @curTemp CURSOR
	SET @curTemp = Cursor For SELECT In_Id 
		FROM  AA		  
	OPEN @curTemp
	FETCH NEXT FROM @curTemp INTO @In_Id 		  
	WHILE @@fetch_status = 0 
		BEGIN		
			DECLARE @curButton CURSOR
			SET @curButton	= Cursor For SELECT BUTTON_NAME 
				FROM BBB
				WHERE In_Id=@In_Id
			
			 /*��ť�α�*/
			 OPEN @curButton
				FETCH NEXT FROM @curButton INTO @BUTTON_NAME  
				WHILE @@fetch_status = 0 
					BEGIN   
						/*���Ӱ�ť���¼*/
						
				FETCH NEXT FROM @curButton INTO @BUTTON_NAME
				
			END 
			close @curButton
			deallocate @curButton 			  
	FETCH NEXT FROM @curTemp INTO @In_Id
END 	
close @curTemp
deallocate @curTemp 

/*�����洢��������*/
/*ɾ���洢����*/
IF ( OBJECT_ID('IFR_UC_T_CAR_SERIESProcedure') IS NOT NULL ) 
	DROP PROCEDURE IFR_UC_T_CAR_SERIESProcedure
GO	
/*�����洢����*/
CREATE PROCEDURE IFR_UC_T_CAR_SERIESProcedure 
AS
/***********************************************************************************
* �洢��������: ���ֳ���ϵ����
* ��������: �ƹ���
* ��������: 2012-12-05   
* ʹ��ϵͳ: DMS
* ʹ��ģ��: ���ֳ�����
* ˵�������ֳ���ϵ����
* �޸���ʷ˵����
*	V1.7.06: ��CLR�洢����ת��ΪSQL�洢����(CLR����̫�鷳��) hgh 2012-12-05 
***********************************************************************************/ 
BEGIN
	DECLARE @curTemp CURSOR
	SET @curTemp = Cursor For SELECT In_Id 
		FROM  AA		  
	OPEN @curTemp
	FETCH NEXT FROM @curTemp INTO @In_Id 		  
	WHILE @@fetch_status = 0 
		BEGIN		
					  
			FETCH NEXT FROM @curTemp INTO @In_Id
		END 	
	close @curTemp
	deallocate @curTemp 	
END


/*�����洢��������(��д��־)*/
/*ɾ���洢����*/
IF ( OBJECT_ID('IFR_UC_T_CAR_SERIESProcedure') IS NOT NULL ) 
	DROP PROCEDURE IFR_UC_T_CAR_SERIESProcedure
GO	
/*�����洢����*/
CREATE PROCEDURE IFR_UC_T_CAR_SERIESProcedure 
AS
/***********************************************************************************
* �洢��������: ���ֳ���ϵ����
* ��������: �ƹ���
* ��������: 2012-12-05   
* ʹ��ϵͳ: DMS
* ʹ��ģ��: ���ֳ�����
* ˵�������ֳ���ϵ����
* �޸���ʷ˵����
*	V1.7.06: ��CLR�洢����ת��ΪSQL�洢����(CLR����̫�鷳��) hgh 2012-12-05 
***********************************************************************************/ 
BEGIN
	/*��������*/
	DECLARE @NETCODE VARCHAR(10),
			@FUN_NAME VARCHAR(200) ,
			@IF_NO VARCHAR(50) ,    
			@IF_NAME VARCHAR(200),  
			@IF_TYPE VARCHAR(1),    
			@MESSAGE VARCHAR(MAX),
			@curTemp CURSOR
    
	/*�ӿ���Ϣ*/  
	SELECT  @FUN_NAME = '�������', 
			@IF_NO = 'DMS_SE_E3SS_006',
			@IF_NAME = '�г���̨KPI�����ϴ�',
			@IF_TYPE = 'N',
			@MESSAGE = ''
    /*ȡһ������*/	
	SELECT @NETCODE=NETCODE FROM dbo.BD_SYSINFO WHERE DLR_TYPE='1'
	/*�����α�*/ 
	SET @curTemp = Cursor For SELECT In_Id FROM  AA
	OPEN @curTemp
	FETCH NEXT FROM @curTemp INTO @In_Id 		  
	WHILE @@fetch_status = 0 
		BEGIN
			BEGIN TRY		
				/*�α����߼�����:���������:GOTO Final_Deal*/
				/**�˴���******/
				
				/*д�봦��ɹ���Ϣ*/
				SELECT  @MESSAGE = 'E3S���ⵥ' +@E3S_OUT_CODE+ '���ճɹ���',@IF_TYPE = 'N'
			END TRY
			BEGIN CATCH 
				IF @@trancount > 0 
					BEGIN
						ROLLBACK TRAN
					END
					SELECT  @MESSAGE = ERROR_MESSAGE()+'. Error Line: '+ CAST(ERROR_LINE() AS VARCHAR(10)),@IF_TYPE = 'E'
					SET @MESSAGE = 'E3S���ⵥ: '+ CAST(@E3S_OUT_CODE AS VARCHAR(50)) + '����ʧ�ܣ�'+ @MESSAGE
			END CATCH
			/*�������մ����ǩ*/
			Final_Deal:
			/*���½ӿڱ��¼Ϊ�Ѵ���״̬1*/        
			UPDATE dbo.IFR_PA_E3S_OUTSTOCK SET MQSENDSTATUS='1'
			WHERE NETCODE=@NETCODE AND OUTSTORAGENO=@E3S_OUT_CODE
			/*�ύ����*/
			IF @@trancount > 0 
			BEGIN
				COMMIT TRAN
			END      
			--д��־    
			EXEC PROC_LOG_MESSAGE @FUN_NAME_ = @FUN_NAME,@IF_NO_ = @IF_NO, @IF_NAME_ = @IF_NAME, @SCENS_ = '',
				   @DIRECT_ = 'SR', @IF_TYPE_ = @IF_TYPE,@REMARK_ = @MESSAGE		  
			/*������һ���α��¼*/
			FETCH NEXT FROM @curTemp INTO @In_Id
		END	
	--�ر��α�	        	
	close @curTemp
	deallocate @curTemp 	
END

/*SQLSERVER 2005���������ݿ����͵Ķ�Ӧ��ϵ*/
SELECT * FROM msdb.dbo.MSdatatype_mappings

/*SQL�в������*/
SELECT ROW_NUMBER() OVER (ORDER BY a.name)  REQ,a.name table_name  
FROM sys.objects a  
JOIN sys.schemas b ON a.schema_id=b.schema_id WHERE a.type='U'

/*�޸ı����˵��*/
IF EXISTS(SELECT * FROM sys.extended_properties A
JOIN sys.objects B ON a.major_id=b.object_id
LEFT JOIN sys.all_columns C ON A.minor_id=C.column_id AND B.object_id = C.object_id
WHERE B.name='IFS_BAS_USER' AND (C.name IS NULL OR C.name='MQSENDSTATUS'))
BEGIN
	EXEC sys.sp_dropextendedproperty @name = N'MS_Description',
    @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE',@level1name = N'IFS_BAS_USER',  
    @level2type = N'COLUMN', @level2name = N'MQSENDSTATUS' 
END
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'�����־��0δ�ϴ�(Ĭ��)��1���ϴ�',
    @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE',@level1name = N'IFS_BAS_USER',  
    @level2type = N'COLUMN', @level2name = N'MQSENDSTATUS'  

/*ɾ���ظ�ֵ��һ��*/
SELECT * FROM PA_LP_DEAL 
WHERE DEAL_ID IN (
select MIN(DEAL_ID) 
from   PA_LP_DEAL 
WHERE BOOK_D_ID IN( select BOOK_D_ID from   PA_LP_DEAL 
WHERE DEAL_TYPE='1' GROUP BY BOOK_D_ID 
HAVING  COUNT (BOOK_D_ID) > 1
  ) 
GROUP BY BOOK_D_ID
)

/*ͬһ�������Ŀ���ѯ*/
SELECT kcsx,stockupperlimit,kcxx,stocklowerlimit
from zyddb..j_bjml a,dms.dbo.pa_part_list b
where b.partno=a.bjbh

/*ROW_NUMBER��ʹ��*/
WITH TABS AS  
(  
	SELECT ROW_NUMBER() OVER(PARTITION BY ORG_ID  ORDER BY EMP_ID  ) AS ROWNO,ORG_ID,EMP_ID FROM ORG_EMPLOYEE  
 )  
SELECT MAX(ROWS) AS 'Ա������',ORG_ID FROM TABS GROUP BY ORG_ID 

/*SQL Server�н���������ƴ��Ϊһ�����ݣ�һ���ַ�����*/
SQL Server�н���������ƴ��Ϊһ�����ݣ�һ���ַ����� (2013-11-06 18:06:38)ת�ب�
��ǩ�� it	���ࣺ רҵ
����һ: ʹ��T-SQL
DECLARE @Users NVARCHAR(MAX)
SET @Users = ''

SELECT @Users = @Users + ',' + UserName FROM dbo.[User]
WHERE RoleID = 1

SELECT @Users

ת���ԣ�http://www.fengfly.com/plus/view-172336-1.html
 
 
������:ʹ��for xml path('') ��stuff
--ʹ�� �����ӡ�for xml path('')��stuff�ϲ���ʾ�������ݵ�һ����   
  
--ע   
--1�������п��Բ��ð����ھۺϺ����ж�ֱ����ʾ������������val��   
--2��for xml path('') Ӧ��Ӧ������������棬�̶�����xml��   
--3��for xml path('root')�е�path���������ɵ�xml����ڵ㡣   
--4���ֶ������Ǳ�������Ϊxml���ӽڵ㣬����û������(�ֶ�+'')����û�б������ֶν�ֱ����ʾ��[value] +','������,�ָ�������(aa,bb,)��   
--5�����ںϲ�����������ʾΪһ������ʱʹ��������   
  
--���ɲ��Ա������������   
create table tb(id int, value varchar(10))  
insert into tb values(1, 'aa')  
insert into tb values(1, 'bb')  
insert into tb values(2, 'aaa')  
insert into tb values(2, 'bbb')  
insert into tb values(2, 'ccc')  
go  
  
--��һ����ʾ   
select id, [val]=(  
select [value] +',' from tb as b where b.id = a.id for xml path('')) from tb as a  
--��һ����ʾ���   
--1 aa,bb,   
--1 aa,bb,   
--2 aaa,bbb,ccc,   
--2 aaa,bbb,ccc,   
--2 aaa,bbb,ccc,   
  
  
--�ڶ�����ʾ   
select id, [val]=(  
select [value] +',' from tb as b where b.id = a.id for xml path('')) from tb as a  
group by id  
--�ڶ�����ʾ���   
--1 aa,bb,   
--2 aaa,bbb,ccc,   
  
--��������ʾ   
select id, [val]=stuff((  
select ','+[value] from tb as b where b.id = a.id for xml path('')),1,1,'') from tb as a  
group by id  
--��������ʾ���   
--1 aa,bb   
--2 aaa,bbb,ccc   
  
  
--����Ӧ��   
--AMD_GiftNew�л�ȡ���еĹ���ԱID   
--select adminIds = stuff((select ','+cast(UserId as varchar) from MM_Users where RoleId = 1 and flag =0 for xml path('')),1,1,'')   
--����Ӧ����ʾ���   
--3,27
 
ת���ԣ�http://blog.csdn.net/kula_dkj/article/details/8568599

 
select PPID, [val]=stuff(( 
 select CAST(SKUCD AS VARCHAR) + ',' from PPSKUTbl as b where b.PPID = a.PPID for xml path('')),1,1,'') from PPSKUTbl as a where PPID = @PPID group by PPID

/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/


/**/