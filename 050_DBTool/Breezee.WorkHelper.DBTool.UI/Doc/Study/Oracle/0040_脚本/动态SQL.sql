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

