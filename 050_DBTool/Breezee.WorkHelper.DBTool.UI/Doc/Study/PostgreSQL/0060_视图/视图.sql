
CREATE OR REPLACE VIEW V_COM_PROVICE_CITY
AS
SELECT 
/*******************************************
* �������ƣ�ʡ�ݳ�������Ϣ��ͼ
* �������ͣ���ͼ
* �������ߣ��ƹ���
* �������ڣ�2016-6-12
* �޸���ʷ��
*	V1.0��hgh ���� 2016-6-12
*******************************************/
   A.PROVINCE_ID,
   A.PROVINCE_CODE,
   A.PROVINCE_NAME,
   B.CITY_ID,
   B.CITY_CODE,
   B.CITY_NAME,
   C.COUNTY_ID,
   C.COUNTY_CODE,
   C.COUNTY_NAME
FROM  BAS_PROVINCE A
JOIN BAS_CITY B
	ON A.PROVINCE_ID = B.PROVINCE_ID
JOIN BAS_COUNTY C
	ON B.CITY_ID = C.CITY_ID;


