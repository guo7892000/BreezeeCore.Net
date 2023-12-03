/*SQL�����*/
sys.objects��syscolumns��systypes��sys.default_constraints��sys.all_sql_modules
;

/*��ѯ����Ϣ*/
SELECT b.name OWER,a.name Table_Name,a.* 
FROM sys.objects a 
JOIN sys.schemas b ON a.schema_id=b.schema_id WHERE a.type='U' AND a.name='FI_MATAINGATHER_M'
;

/*��ѯ��*/
Select * From sysobjects a,syscolumns b 
Where a.id = b.id and b.name = 'LOCATIONCODE' And a.type = 'U';

/*��������CLR��䣬������ʹ��VS 2005�����Ĵ洢����*/
EXEC sp_configure 'clr enabled' , 1 
RECONFIGURE WITH override
;

/*SQL Server������־�����ݿ��ļ�����*/
--�������ݿ��ļ��� 
DBCC SHRINKDATABASE(@dbName ) 
DUMP TRANSACTION @dbName WITH NO_LOG;
--�ض�������־�� 
BACKUP LOG @dbName WITH NO_LOG;

/*������־�ļ�*/ 
DBCC SHRINKFILE 
( 
{ 'file_name'  file_id } 
{ [ , EMPTYFILE ] 
 [ [ , target_size ] [ , { NOTRUNCATE  TRUNCATEONLY } ] ] 
} 
) 
[ WITH NO_INFOMSGS ] 

