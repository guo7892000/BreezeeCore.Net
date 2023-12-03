/*oracle 10g ������ɾ����ռ䣬�û�����*/
--��һ����������ռ䣨Ҫ��������ʱ��ռ�����ݱ�ռ䣩 
--1��������ʱ��ռ䣺 
--��֤Ŀ¼��E:/oracle/product/10.2.0/oradata/hui������
create temporary tablespace hui_temp 
tempfile 'E:/oracle/product/10.2.0/oradata/hui/hui_temp01.dbf' 
size 100m 
autoextend on 
next 100m maxsize 500m 
extent management local; 

---
--2���������ݱ�ռ䣺 
CREATE TABLESPACE test 
DATAFILE 'E:/oracle/product/10.2.0/oradata/hui/hui_data01.DBF' 
SIZE 1000M AUTOEXTEND ON NEXT 100M MAXSIZE UNLIMITED LOGGING PERMANENT EXTENT 
MANAGEMENT LOCAL AUTOALLOCATE BLOCKSIZE 8K SEGMENT SPACE MANAGEMENT MANUAL FLASHBACK ON; 
---
--�ڶ����������û� 
create user hui identified by hui 
default tablespace test 
temporary tablespace hui_temp; 
--����testΪ�û�����testpasswordΪ���� 
--��󣺸��û�����Ȩ�� 
grant connect,resource to hui; 

--����ͺ��������ݿ�û��ʲô�����ˣ���ͨsql 
--�磺 
create table test_user( 
first_name varchar2(15), 
last_name varchar2(20) 
); 

--ɾ���û���test 
drop user test cascade 
--ɾ����ռ䣺 
DROP TABLESPACE hui_temp INCLUDING CONTENTS AND DATAFILES; 
--ɾ��������: 
delete from users;