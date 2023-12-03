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


/*��ĳһ�������Ե�����ʾ*/
SELECT A.PART_BRAND_CODE,
       LISTAGG(A.CAR_BRAND_CODE, ',') WITHIN GROUP(ORDER BY A.PART_BRAND_CODE) AS CAR_BRAND_CODE
  FROM T_PA_DB_PART_BRAND_DETAIL A
 WHERE A.IS_ENABLE = '1'
 GROUP BY A.PART_BRAND_CODE;

