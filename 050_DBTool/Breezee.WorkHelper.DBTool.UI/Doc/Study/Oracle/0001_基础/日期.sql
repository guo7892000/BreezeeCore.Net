/*ת��Ϊ�����ַ�*/
select to_char(systimestamp,'yyyy-MM-dd HH24:mi:ss.ff3') last_updated_date,TO_TIMESTAMP('2012-07' || '-01 00:00:00:000000','YYYY-MM-DD hh24:mi:ss:ff3')
from t_se_bu_tool_store;
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

/*ȡ������*/
select to_char(sysdate,'yyyy'), to_char(sysdate, 'mm'), to_char(sysdate, 'dd') from dual; 

/*��ʽת��*/
TO_CHAR('#DATE#','YYYY-MM-DD')
TO_DATE('#DATE#','YYYY-MM-DD')

/*����ʱ��ε����������Զ���*/
SELECT TO_CHAR(ADD_MONTHS(TO_DATE('20150525', 'YYYYMMDD'), 1 - LEVEL),
                 'YYYYMM') MONTHS
    FROM DUAL
  CONNECT BY LEVEL <= 12;


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