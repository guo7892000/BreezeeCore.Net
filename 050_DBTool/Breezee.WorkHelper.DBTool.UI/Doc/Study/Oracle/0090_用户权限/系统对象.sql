/*��ѯ�洢��������*/
select * from dba_source a where a.name='SP_MDM_PRO_SEND_TOUCCUST';

/*�鿴oracle�����ļ����·��*/
select * from v$datafile;

/*��ѯ�汾*/
select * from v$version

/*���ݱ����*/
create table userinfo_bak as select * from userinfo;

/*��ѯ����Ķ���*/
SELECT A.OWNER, A.OBJECT_NAME
  FROM ALL_OBJECTS A
 WHERE A.OBJECT_TYPE IN ('PROCEDURE', 'TRIGGER', 'VIEW')
   AND A.OWNER IN ('UC')
   AND A.LAST_DDL_TIME > SYSDATE - 2
   AND A.STATUS = 'VALID'
UNION
SELECT A.OWNER, A.OBJECT_NAME
  FROM ALL_OBJECTS A
 WHERE A.OBJECT_TYPE IN ('PROCEDURE', 'TRIGGER', 'VIEW')
   AND A.OWNER IN ('IFR', 'IFS')
   AND A.LAST_DDL_TIME > SYSDATE - 2
   AND A.STATUS = 'VALID';

/*��ѯ����ס�ı�*/
SELECT /*+ rule */ lpad(' ',decode(l.xidusn ,0,3,0))||l.oracle_username User_name, 
o.owner,o.object_name,o.object_type,s.sid,s.serial# 
FROM v$locked_object l,dba_objects o,v$session s 
WHERE l.object_id=o.object_id 
AND l.session_id=s.sid 
ORDER BY o.object_id,xidusn DESC;


