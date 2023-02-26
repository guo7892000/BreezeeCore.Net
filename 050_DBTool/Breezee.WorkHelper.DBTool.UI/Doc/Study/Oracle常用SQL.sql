/*��ʽת��*/
TO_CHAR('#DATE#','YYYY-MM-DD')
TO_DATE('#DATE#','YYYY-MM-DD')

/*ע��Oracle�Ķ����������ȣ�С�ڵ���30*/

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

/*�������У���10000��ʼ��ÿ������1��*/
CREATE SEQUENCE SEQT_PORTAL_RPT_ID MINVALUE 1 MAXVALUE 999999999999999999999999999 START WITH 10000 INCREMENT BY 1 CACHE 20;

/*��������SQL*/
COMMENT ON TABLE TEST1 IS '���Ա�';--�������ע��
COMMENT ON COLUMN T_AI_BU_INSURE_ORDER.LINK_NO IS '��ͬ����';--���м�ע��
ALTER TABLE TEST1 MODIFY PLANMONTH NUMBER;
ALTER TABLE TEST1 ADD LONG1 LONG;
ALTER TABLE TEST1 ADD DFD NUMBER(14,2) DEFAULT 0;
ALTER TABLE TEST1 ADD FDFED NUMBER DEFAULT 1;
ALTER TABLE TEST1 ADD CONSTRAINT PK_ID PRIMARY KEY (ID);
ALTER TABLE TEST1 ADD CONSTRAINT FK_D_ID FOREIGN KEY (D_ID) REFERENCES TEST2 (RID);
ALTER TABLE TEST1 ADD CONSTRAINT U_DFD UNIQUE (PLANMONTH);
ALTER TABLE TEST1 DROP COLUMN PLANMONTH;

/*�����к�ע��*/
1���������ע�ͣ�COMMENT ON TABLE ���� IS '��ע��';
2�����м�ע�ͣ�COMMENT ON COLUMN ��.�� IS '��ע��';
3����ȡ��ע�ͣ�SELECT * FROM USER_TAB_COMMENTS WHERE COMMENTS IS NOT NULL;
4����ȡ��ע�ͣ�SELECT * FROM USER_COL_COMMNENTS WHERE COMMENTS IS NOT NULL AND TABLE_NAME='����';

/*oracle �����ȡ��һ����¼*/
select * from (select * from table order by a )C
where rownum=1;

/*��ѯ����ס�ı�*/
SELECT /*+ rule */ lpad(' ',decode(l.xidusn ,0,3,0))||l.oracle_username User_name, 
o.owner,o.object_name,o.object_type,s.sid,s.serial# 
FROM v$locked_object l,dba_objects o,v$session s 
WHERE l.object_id=o.object_id 
AND l.session_id=s.sid 
ORDER BY o.object_id,xidusn DESC;

/*����������޸���ͼ����Ȩѡ������*/
--��ѯ��Ȩ��
SELECT * FROM USER_TAB_PRIVS T 
WHERE T.GRANTOR='MDM' AND T.grantee='UC'
ORDER BY T.TABLE_NAME;
select * from T_MDM_COMP_BRAND;
--��UC�û���ִ����ͼV_UC_BASE_ALLCARBRAND���ʱ������ʾ��ORA-01720�������ڡ�****.****"��Ȩѡ�
--ִ�����������
GRANT SELECT ON MDM.T_MDM_COMP_BRAND TO UC WITH GRANT OPTION;

/*��������*/
CREATE INDEX IDX1_T_UC_BU_BUY_CONTRACT ON T_UC_BU_BUY_CONTRACT(OPRATE_TIME) TABLESPACE UC_IDX_TBS;

/*����ʱ��ε����������Զ���*/
SELECT TO_CHAR(ADD_MONTHS(TO_DATE('20150525', 'YYYYMMDD'), 1 - LEVEL),
                 'YYYYMM') MONTHS
    FROM DUAL
  CONNECT BY LEVEL <= 12;

/*����ظ������л�ȡĳ������*/
SELECT *
  FROM (SELECT *
          FROM (SELECT A.*,
                       ROW_NUMBER() OVER(PARTITION BY A.BUYUPNO ORDER BY A.BUYUPNO) RN
                  FROM T_CAR_STORE@MD_DL_EABUC A) X
         WHERE X.RN = 1) A
  LEFT JOIN (SELECT *
               FROM (SELECT B.*,
                            ROW_NUMBER() OVER(PARTITION BY B.BUYUPNO ORDER BY B.BUYUPNO) RN
                       FROM T_MEMU_CONTRACT_M@MD_DL_EABUC B
                      WHERE B.CONTRACT_TYPE IN ('0', '1')) X
              WHERE X.RN = 1) B
    ON A.BUYUPNO = B.BUYUPNO
  LEFT JOIN T_CAR_CATTYPESET@MD_DL_EABUC C
    ON A.CATTYPESETID = C.CATTYPESETID
;

---���Ե�������
DECLARE V_CODE VARCHAR2(50); 
        v_num NUMBER;
BEGIN
	for v_num in 1 .. 100 LOOP
		p_mds_get_form_code('','2057',V_CODE);
		dbms_output.put_line(V_CODE);
	END LOOP;
END;  

/*����ʱ��Σ��ꡢ�¡��գ������������Զ���*/
SELECT DECODE('2',/*ָ��ʱ�����ͣ�1�꣬2�·ݣ�3��*/
  '1',
  ADD_MONTHS(TO_DATE('20160525', 'YYYYMMDD'), (LEVEL-1)*12),
  '2',
  ADD_MONTHS(TO_DATE('20150525', 'YYYYMMDD'), LEVEL-1),
  '3',
  TO_DATE('20150522', 'YYYYMMDD') + LEVEL-1)
FROM DUAL
CONNECT BY LEVEL <= DECODE('2',/*ָ��ʱ�����ͣ�1�꣬2�·ݣ�3��*/
  '1',
  TO_CHAR(TO_DATE('20160625', 'YYYYMMDD'),'YYYY')-TO_CHAR(TO_DATE('20160125', 'YYYYMMDD'),'YYYY')+1,
  '2',
  FLOOR(MONTHS_BETWEEN(TO_DATE('20150625', 'YYYYMMDD'), TO_DATE('20150501', 'YYYYMMDD')))+1,
  '3',
  TRUNC(TO_DATE('20150523', 'YYYYMMDD'))-TRUNC(TO_DATE('20150522', 'YYYYMMDD'))+1
	)
;

/*������*/
  ALTER TABLE emp01 ADD eno NUMBER(4);

/*�޸��ж���*/
  ALTER TABLE emp01 MODIFY job VARCHAR2(15)  DEFAULT 'CLERK';

/*ɾ����*/
  ALTER TABLE emp01 DROP COLUMN dno;

/*�޸�����*/
  ALTER TABLE emp01 RENAME COLUMN eno TO empno;

/*�޸ı���*/
  RENAME emp01 TO employee;

/*��ɾ������������*/
ALTER TABLE MQ_PS_PREPARE DROP CONSTRAINT PK_MQ_PS_PREPARE;
ALTER TABLE MQ_PS_PREPARE ADD CONSTRAINT PK_MQ_PS_PREPARE PRIMARY KEY (OID);

/*��������������*/
CREATE OR REPLACE TRIGGER TR_MQSEND_TEMP_RECORDID
BEFORE INSERT ON MQSEND_TEMP
FOR EACH ROW
BEGIN
SELECT SE_MQSEND_TEMP.NEXTVAL INTO :NEW.RECORDID FROM DUAL;
END;

/*��ͼ����ͬ��ʲ���Ȩ*/
GRANT SELECT ON UC.V_UC_QUERY_STORE_CAR TO MDS_ROLE;  
CREATE OR REPLACE SYNONYM MDS.V_UC_QUERY_STORE_CAR FOR UC.V_UC_QUERY_STORE_CAR;  
GRANT SELECT ON UC.V_UC_QUERY_STORE_CAR TO MDS WITH GRANT OPTION;

/*�ж�Ӱ����*/
UPDATE TB_NETYKRUNLOG SET NETYKSPNAME='P_AUTO_QJ' WHERE  NUM=1 AND NETYKSPNAME IS NULL;
IF SQL%ROWCOUNT=1 THEN
 --dong something
 v:='��dҪ��һɱ��';
 dbms_output.put_line(substr(v, 2, 3));
END IF;

/*��̬ɾ����SQL��*/
declare  cnt  number;
begin
  ---ɾ����COC_CAL_GoodReturn
  select count(1) into cnt from user_objects where upper(object_name) = 'COC_CAL_GOODRETURN' and upper(object_type) = 'TABLE'; 
  if cnt = 1 then 
    begin 
      execute immediate 'DROP TABLE COC_CAL_GOODRETURN';
    end;
  end if;
END;

/*��̬�޸ı�������ע��*/
declare cnt number; 
begin 
 ---�޸�רӪ���������DEALER_CODE_O,������ע��
 select count(1) into cnt from user_tab_columns where upper(table_name) = 'COC_BAS_DEALER' and upper(column_name) = 'DEALER_CODE_O'; 
 if cnt = 0 then 
   begin 
      execute immediate 'ALTER TABLE COC_BAS_DEALER ADD (DEALER_CODE_O varchar2(10))'; 
      execute immediate 'COMMENT ON column COC_BAS_DEALER.DEALER_CODE_O IS ' || '''רӪ�����'''; 
   end; 
 end if;
END;

/*�ϲ��������*/
MERGE INTO T_PA_BU_PUR_ORDER_D U
USING (SELECT SUM(NVL(B.IN_QTY, 0)) AS T_IN_QTY, B.PART_NO
             FROM T_PA_BU_IN_ORDER A
             LEFT JOIN T_PA_BU_IN_ORDER_D B
               ON A.IN_ORDER_ID = B.IN_ORDER_ID
            WHERE B.SOURCE_ORDER_CODE = V_ORDER_NO /*��������:�⹺����*/
              and A.net_code = NetCode
            GROUP BY B.PART_NO
	  ) F
ON (U.PART_NO = F.PART_NO)
    WHEN MATCHED THEN
      UPDATE
         SET U.IN_STOCK_QTY      = F.T_IN_QTY,
             U.LAST_UPDATED_DATE = systimestamp
       WHERE POD.PUR_ORDER_ID =
             (select pur_order_id
                from T_PA_BU_PUR_ORDER
               where pur_order_code = V_ORDER_NO
                 and net_code = NetCode); /*�ɹ���ID*/

/*��ĳһ�������Ե�����ʾ*/
SELECT A.PART_BRAND_CODE,
       LISTAGG(A.CAR_BRAND_CODE, ',') WITHIN GROUP(ORDER BY A.PART_BRAND_CODE) AS CAR_BRAND_CODE
  FROM T_PA_DB_PART_BRAND_DETAIL A
 WHERE A.IS_ENABLE = '1'
 GROUP BY A.PART_BRAND_CODE;

/*��ͼ��Ȩ�봴��ͬ���*/
grant select on uc.v_uc_carprice to mds ;
create or replace synonym mds.v_uc_carprice for uc.v_uc_carprice;

/*ȡ������*/
select to_char(sysdate,'yyyy'), to_char(sysdate, 'mm'), to_char(sysdate, 'dd') from dual; 

/*��ѯ���ַ��͸�ֵӦ��*/
declare v varchar2(20);
begin
 v:='��dҪ��һɱ��';
 dbms_output.put_line(substr(v, 2, 3));
end;

/*���JOB*/
DECLARE JOBID NUMBER;
BEGIN
  SELECT MAX(JOB)+1 INTO JOBID FROM ALL_JOBS;
  DBMS_JOB.SUBMIT(JOBID, 'P_PA_DELETE_OVERDUE_LACK_AUTO;', SYSDATE,'TRUNC(SYSDATE+1)');
  COMMIT;
END;

/*�޸�JOB*/
declare jobID number;
begin
  select JOB into jobID from all_jobs where what='P_SE_IR_DAY_ONCE;';
  dbms_job.change(jobID ,'P_SE_IR_DAY_ONCE;', sysdate+1,'sysdate+1/24' );
  commit;
end;

/*JOB����������*/
begin
 dbms_job.remove(41); --ɾ��JO
 dbms_job.broken(25,true); --ֹͣjob
 dbms_job.run(25); --����job
 dbms_job.what(v_job,'sp_fact_charge_code;'); --�޸�What����
 dbms_job.next_date(v_job,sysdate); --�޸�ĳ��job�� �޸���һ������ʱ��
end;

--�鿴����
select * from user_jobs;select * from all_jobs;
--�鿴�������е����񣨲��Ƽ�ʹ�ã��ٶ�������
select * from dba_jobs_running;


/*����*/
v_Dms_NetCode eabuc.t_org_dealer.dealerno%Type;
Cursor curBalance is
    Select * from eabuc.PS_U_T_MEMU_CERTIFICATE;
  RecBalance curBalance%RowType;
begin
  open curBalance;
  loop
    Fetch curBalance
      into RecBalance;
    Exit when curBalance%NotFound;
	  begin
		/*�߼������˴���*/

		  commit;
		WHEN OTHERS THEN
			/*����ع�*/
			Rollback; 
			/*�׳�ϵͳ����*/
			RAISE_APPLICATION_ERROR(-20999, sqlerrm);
	 end;
  end loop;
  close curBalance;
end;

/*�ַ�����*/
select concat('�й�','����') "test",'�й�'||'����' from dual;

/*INITCAP(n)����*/ 
 ���ַ���n��ÿ����������ĸ��д������Сд(���ֵ��ʵĹ����ǰ��ո�����ĸ�ַ����������������ַ�����û���κ�����)�����磺
select initcap('�� �� �� ��') "test",initcap('my word') "test1",initcap('my�й�word') "test2" from dual;

/*INSTR(chr1,chr2,[n,[m]])����*/
��ȡ�ַ���chr2���ַ���chr1�г��ֵ�λ�á�n��m��ѡ,ʡ����Ĭ��Ϊ1��n����ʼ���ҵ���ʼλ�ã���nΪ������β����ʼ������m�����ִ����ֵĴ��������磺
select instr('pplkoopijk','k',-1,1) "test",instr('pplkoopijk','k',1,2) from dual;

/*LENGTH(n)����*/
 �����ַ����ַ������ȡ�(��nΪnullʱ������nll�����صĳ��Ȱ�������Ŀո�)�����磺
select length('ppl ') "test",length(null) "test1",length(trim('ppl ')) "test" from dual;

/*LOWER(n)����*/
 ��nת��ΪСд;UPPER(n)����������: ��nת��Ϊ��д�����磺
select lower('KKKD') "test",UPPER('hello') from dual;

/*LPAD(chr1,n,[chr2])����*/
��chr1�������ַ�chr2��ʹ���ַ��ܳ���Ϊn��chr2��ѡ��Ĭ��Ϊ�ո񣻵�chr1�ַ������ȴ���nʱ�������߽�ȡchr1��n���ַ���ʾ��RPAD(chr1,n,chr2)��������������chr1�ұ����chr2��ʹ�����ַ�������Ϊn..��chr1���ȴ���nʱ���������n���ַ������磺
select lpad('kkk',5) "test",lpad('kkkkk',4) "test1",lpad('kkk',6,'lll') "test2" from dual;

/*LTRIM(chr,[n])����*/
ȥ���ַ���chr��߰�����n�ַ����е��κ��ַ���ֱ������һ����������n�е��ַ�Ϊֹ�����磺
select ltrim('abcde','a') "test",ltrim('abcde','b') "test1",ltrim('abcdefg','cba') "test2",ltrim('  abcdefg') from dual;

/*REPLACE(chr,search_string,[,replacement_string])����*/
��chr������search_string�������滻Ϊreplacement_stringָ�����ַ�������search_stringΪnullʱ������chr����replacement_stringΪnullʱ������chr�н�ȡ��search_string���ֵ��ַ��������磺
SELECT REPLACE('abcdeef','e','oo') "test",REPLACE('abcdeef','ee','oo') "test1",REPLACE('abcdeef',NULL,'oo') "test2",REPLACE('abcdeef','ee',NULL) "test3" FROM dual;

/*��ѯ�汾*/
select * from v$version

/*���ݱ����*/
create table userinfo_bak as select * from userinfo;

/*�䶯����ʱ����ֵ*/
select
trunc(sysdate)+(interval '1' second), --��1��(1/24/60/60)
trunc(sysdate)+(interval '1' minute), --��1����(1/24/60)
trunc(sysdate)+(interval '1' hour), --��1Сʱ(1/24)
trunc(sysdate)+(INTERVAL '1' DAY),  --��1��(1)
trunc(sysdate)+(INTERVAL '1' MONTH), --��1��
trunc(sysdate)+(INTERVAL '1' YEAR), --��1��
trunc(sysdate)+(interval '01:02:03' hour to second), --��ָ��Сʱ����
trunc(sysdate)+(interval '01:02' minute to second), --��ָ�����ӵ���
trunc(sysdate)+(interval '01:02' hour to minute), --��ָ��Сʱ������
trunc(sysdate)+(interval '2 01:02' day to minute) --��ָ������������
from dual;

/*ʹ����ʱ���ѯSQL*/
WITH tempKPI AS (select TO_NUMBER(SUBSTR(KPI.YEAR_MONTH, -2)) AS MONTHS,
       KPI.PART_PZ_RATE /*����Ʒ��������*/,
       KPI.NEED_VARIETY_ALL /*�ر���Ʒ��������*/,
       KPI.LP_RELIEVE_TIME_ALL /*ȱ��ƽ�����ʱ��*/,
       KPI.STORE_RATE /*����*/,
       KPI.BO_RATE /*����BO��*/,
       KPI.ORDER_RATE /*����������*/,
       KPI.REACH_RATE /*����������*/,
       KPI.EO_RATE /*����������EO*/
 FROM T_PA_BU_PART_MONTH_KPI KPI
 WHERE KPI.NET_CODE= 'H2904'--'#NET_CODE#'
 AND SUBSTR(KPI.YEAR_MONTH,0,4) = '2011'--'#YYYY#%'
 ) 
select '', decode(months,1,PART_PZ_RATE,0)
from tempKPI


/*��ȡ����*/
select sysdate ��ʱ����,
to_char(sysdate-1,'d') ���ڼ�,
round(sysdate) ���0������,
round(sysdate,'day') ���������,
round(sysdate,'month') ����³�,
round(sysdate,'q') �����������, 
round(sysdate,'year') ���������� from dual;

/*ʱ���ֵ*/
select 
round(to_number(end_date-start_date)) "���ŵ�ʱ�䣨����Ϊ��λ��", 
round(to_number(end_date-start_date)*24) "���ŵ�ʱ�䣨��СʱΪ��λ��", 
round(to_number(end_date-start_date)*1440) "���ŵ�ʱ�䣨�Է���Ϊ��λ��"
from dual;

/*SQL�е�SQL���ѵ����Ż������������žͺ���*/
SELECT 'INSERT INTO (a, b,c,d) VALUES(''aa'', 2, '''',3)' from dual;

/*ת��Ϊ�����ַ�*/
select to_char(systimestamp,'yyyy-MM-dd HH24:mi:ss.ff3') last_updated_date,TO_TIMESTAMP('2012-07' || '-01 00:00:00:000000','YYYY-MM-DD hh24:mi:ss:ff3')
from t_se_bu_tool_store

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
--ɾ����: 
delete from users;


/*ִ�д洢����*/
begin
   p_se_if_to_other_system('SE_TOOL_MAINTAIN_002','D2305','MD23051207001',null,null,null,null);
end;

/*ɾ���ظ���JOB*/
declare jobID number;
CURSOR CUR_UC IS
      select what,max(job) max_job  from all_jobs 
      where what in (select what from (select what,count(1) icount
                        from all_jobs
                        group by what
                        having count(1)>1)
      )
      --and what='P_DC_IS_NO_CONSUME;'
      group by what;
      REC_UC  CUR_UC%ROWTYPE;
BEGIN
  OPEN CUR_UC;
  LOOP
    FETCH CUR_UC
      INTO REC_UC;
    IF CUR_UC%FOUND THEN
        declare CURSOR CUR_UC2 IS select job from all_jobs where what=REC_UC.what;
         REC_UC2 CUR_UC2%ROWTYPE;
        begin 
          OPEN CUR_UC2;
          LOOP
            FETCH CUR_UC2
              INTO REC_UC2; 
          IF CUR_UC2%FOUND THEN
            if REC_UC2.job<>REC_UC.max_job then
               begin
                 dbms_job.remove(REC_UC2.job);
                 commit;
               end;
            end if; 
          ELSE
            CLOSE CUR_UC2;
            EXIT;
          END IF;  
          END LOOP;
        end;
    ELSE
      CLOSE CUR_UC;
      EXIT;
    END IF;  
  END LOOP; 
end;   

/*��̬SQL����ֵ*/
--׼������
create table EE(REPORT_M_ID INTEGER,YEAR VARCHAR(20),YEAR_MONTH VARCHAR(20),START_DATE VARCHAR(20), ORG_CODE VARCHAR(20),ORDER_NO INTEGER,SON_ORG_CODE VARCHAR2(20));
INSERT INTO EE SELECT 1,'2012','201206','2012-07-01','DLR1',1,'a,b,c,d' from dual;
INSERT INTO EE SELECT 2,'2012','201206','2012-07-01','DLR1',2,'a,c' from dual;
--��̬SQLʹ��
declare 
      V_SON_ORG_CODE VARCHAR2(200);
      V_SON_ORG_CODE1 VARCHAR2(200);
      vStrTempSql VARCHAR2(2000);
      V_REPORT_FREQUENCY VARCHAR2(2);
      --�¼�
      V_CONN_REAL VARCHAR2(200);
BEGIN
  V_REPORT_FREQUENCY := 'Y';
  select decode(V_REPORT_FREQUENCY,'Y',' AND CC.YEAR=TT.YEAR ',
         	'M',' AND CC.START_DATE=TT.START_DATE ',
          ' AND CC.YEAR_MONTH=TT.YEAR_MONTH ') into V_CONN_REAL from dual;     
  vStrTempSql :='SELECT TT.SON_ORG_CODE,(SELECT SON_ORG_CODE FROM EE CC
    WHERE CC.ORG_CODE = TT.ORG_CODE ' || V_CONN_REAL || ' AND TT.ORDER_NO-1=CC.ORDER_NO ) SON_ORG_CODE1 
     FROM EE TT WHERE REPORT_M_ID=2 ';
    dbms_output.put_line(vStrTempSql);
    execute immediate vStrTempSql into V_SON_ORG_CODE,V_SON_ORG_CODE1;             
    dbms_output.put_line(V_SON_ORG_CODE);
    dbms_output.put_line(V_SON_ORG_CODE1);          
END; 

/*��̬SQL���ַ��Ĵ���*/
declare strSql varchar2(8000);
begin
  strSql:='UPDATE T_FI_BU_GATHERING_ORDER_D T 
            SET T.VOUCHER_STATUE = V_VOUCHERSTATE
       WHERE NET_CODE = V_NETCODE
       AND T.GATHERING_ORDER_D_ID IN(V_ORDERID)
       AND T.IS_ENABLE=''1'' and IS_SYSTEM=''0''';
  dbms_output.put_line(strSql);     
end;

/*��ѯ�洢��������*/
select * from dba_source a where a.name='SP_MDM_PRO_SEND_TOUCCUST'

/*��ת��*/
WITH tempKPI AS (select TO_NUMBER(SUBSTR(KPI.YEAR_MONTH, -2)) AS MONTHS,
       KPI.PART_PZ_RATE /*����Ʒ��������*/,
       KPI.NEED_VARIETY_ALL /*�ر���Ʒ��������*/
 FROM T_PA_BU_PART_MONTH_KPI KPI
 WHERE KPI.NET_CODE= '#NET_CODE#'
 AND SUBSTR(KPI.YEAR_MONTH,0,4) = '#YYYY#'
 ) 
select '����������(%)' ""CAL_TYPE"", 
sum(decode(months,1,PART_PZ_RATE,0)) ""1"",sum(decode(months,2,PART_PZ_RATE,0)) ""2"",sum(decode(months,3,PART_PZ_RATE,0)) ""3"",
sum(decode(months,4,PART_PZ_RATE,0)) ""4"",sum(decode(months,5,PART_PZ_RATE,0)) ""5"",sum(decode(months,6,PART_PZ_RATE,0)) ""6"",
sum(decode(months,7,PART_PZ_RATE,0)) ""7"",sum(decode(months,8,PART_PZ_RATE,0)) ""8"",sum(decode(months,9,PART_PZ_RATE,0)) ""9"",
sum(decode(months,10,PART_PZ_RATE,0)) ""10"",sum(decode(months,11,PART_PZ_RATE,0)) ""11"",sum(decode(months,12,PART_PZ_RATE,0)) ""12""
from tempKPI
union 
select '�ر���Ʒ��������(%)' ""CAL_TYPE"", 
sum(decode(months,1,NEED_VARIETY_ALL,0)) ""1"",sum(decode(months,2,NEED_VARIETY_ALL,0)) ""2"",sum(decode(months,3,NEED_VARIETY_ALL,0)) ""3"",
sum(decode(months,4,NEED_VARIETY_ALL,0)) ""4"",sum(decode(months,5,NEED_VARIETY_ALL,0)) ""5"",sum(decode(months,6,NEED_VARIETY_ALL,0)) ""6"",
sum(decode(months,7,NEED_VARIETY_ALL,0)) ""7"",sum(decode(months,8,NEED_VARIETY_ALL,0)) ""8"",sum(decode(months,9,NEED_VARIETY_ALL,0)) ""9"",
sum(decode(months,10,NEED_VARIETY_ALL,0)) ""10"",sum(decode(months,11,NEED_VARIETY_ALL,0)) ""11"",sum(decode(months,12,NEED_VARIETY_ALL,0)) ""12""
from tempKPI;

/*�鿴oracle�����ļ����·��*/
select * from v$datafile;


--ʵ�����������еĴ�����
CREATE OR REPLACE TRIGGER TRG_PK_PS_QJ_SH_BAK
 BEFORE INSERT ON PS_QJ_SH_BAK
 FOR EACH ROW
BEGIN
 SELECT SEQ_PK_PS_QJ_SH_BAK.NEXTVAL INTO :NEW.RECORDNO FROM DUAL;
END;

--��ȡ����ĸ����ˮ��
CREATE OR REPLACE FUNCTION F_MDS_GET_WORD_SEQUENCE_NO
(
  V_SEQUENCE_NO INTEGER, ---��1���ʮ������ˮ�ţ����磺10
  V_SEQUENCE_LENGTH INTEGER --��ˮ�ų��ȣ�����4
)
/*******************************************************************************************
* �������ƣ���ȡ����ĸ����ˮ��
* �������ߣ��ƹ���
* �������ڣ�2014-11-12
* ��������:  ������ˮ�ż���ˮ�ų��ȣ����ؿ��ܰ�����ĸ����ˮ�š�
*       ��������ˮ�ű�ԭ��ֻ�����ֵĻ��һЩ��
* �����ʷ(��ʽ���汾��\���α�����ݼ���\�޸���\�޸�����)��
*     V1.00������ HGH 2014-11-12
*     V1.01����ȫ��д�˸��㷨�����֮ǰ���ظ����ŵ����� HUANGGH 2015-8-3
********************************************************************************************/
RETURN VARCHAR2
IS
  --�Զ�����ˮ�ŵ���ر���
  V_MY_DEFINE_VALUE  VARCHAR2(50); --�Զ������ˮ��
  V_MY_DEFINE_WORD_LIST VARCHAR2(50):= '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'; --�Զ����ַ���
  N_MY_DEFINE_WORD_LIST_LEN NUMBER;  --�Զ����ַ�������

  I_AFTER_DIVIDE_VALUE INTEGER:=0; --�����ֵ
	N_MOD_VALUE           NUMBER; --����ֵ
	V_LOOP_NUM            NUMBER; --ѭ���е�ֵ
BEGIN
  --��Ч���ж�
  IF V_SEQUENCE_NO IS NULL OR V_SEQUENCE_LENGTH IS NULL THEN
    RAISE_APPLICATION_ERROR(-20999,'�������ˮ�š���ˮ�ų��ȶ�����Ϊ�գ�');
  END IF;

  IF V_SEQUENCE_LENGTH <=0 THEN
    RAISE_APPLICATION_ERROR(-20999,'�������ˮ�ų��ȱ������0��');
  END IF;

  --������ʼ��
  N_MY_DEFINE_WORD_LIST_LEN := LENGTH(V_MY_DEFINE_WORD_LIST);--�Զ����ݼ�����
  V_MY_DEFINE_VALUE:=''; --���ص�ֵ
	I_AFTER_DIVIDE_VALUE:=V_SEQUENCE_NO; --ֵΪ�������ˮ��

  --ѭ��ת��Ϊ��ˮ��
	FOR V_LOOP_NUM IN 1 .. V_SEQUENCE_LENGTH LOOP
		  --�õ�����ֵ
			N_MOD_VALUE := MOD(I_AFTER_DIVIDE_VALUE,N_MY_DEFINE_WORD_LIST_LEN);
			--�õ������ֵ
			I_AFTER_DIVIDE_VALUE :=FLOOR(I_AFTER_DIVIDE_VALUE/N_MY_DEFINE_WORD_LIST_LEN);--����Ҫ����ȡ��
			--����
			V_MY_DEFINE_VALUE:=  SUBSTR(V_MY_DEFINE_WORD_LIST,N_MOD_VALUE + 1, 1) || V_MY_DEFINE_VALUE;
			IF I_AFTER_DIVIDE_VALUE < N_MY_DEFINE_WORD_LIST_LEN THEN --������С�ڽ�����ʱ�˳�
				 --�õ�ǰһλ
				 V_MY_DEFINE_VALUE:=  SUBSTR(V_MY_DEFINE_WORD_LIST,I_AFTER_DIVIDE_VALUE + 1, 1) || V_MY_DEFINE_VALUE;
				 EXIT; --�˳�ѭ��
			END IF;
	END LOOP;
	
	V_MY_DEFINE_VALUE :=LPAD(V_MY_DEFINE_VALUE,V_SEQUENCE_LENGTH,'0');	
  V_MY_DEFINE_VALUE := SUBSTR(V_MY_DEFINE_VALUE,LENGTH(V_MY_DEFINE_VALUE)-V_SEQUENCE_LENGTH,V_SEQUENCE_LENGTH);

  --����ֵ
  RETURN(V_MY_DEFINE_VALUE);
END;

