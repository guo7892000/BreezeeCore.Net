/*ָ���ֶ�ָ��λ��Ϊ�̶�ֵ��SQL*/
SELECT * FROM dbo.PA_PART_LIST WHERE Charindex('A',PARTNO)=2 OR Charindex('1',PARTNO)=2;

/*��ȡ��ָ���������ȡ��ָ��λ���м�¼*/
SELECT * FROM (
select ROW_NUMBER() OVER(ORDER BY createtime) rowNum,*
 FROM UC_T_CUST_TYPE
  WHERE CHECKSTATE = '2'
 AND ISNULL(STORE_STATUS, 0) = '0'
  AND ISENABLE = '1') a
where rowNum BETWEEN 1 AND 10

/*Union��Union all ������
�����ݿ��У�union��union all�ؼ��ֶ��ǽ�����������ϲ�Ϊһ�����������ߴ�ʹ�ú�Ч������˵��������ͬ��
union�ڽ��б����Ӻ��ɸѡ���ظ��ļ�¼�������ڱ����Ӻ����������Ľ���������������㣬ɾ���ظ��ļ�¼�ٷ��ؽ����
��union allֻ�Ǽ򵥵Ľ���������ϲ���ͷ��ء���Ч����˵��union allҪ��union��ܶ࣬���ԣ��������ȷ�Ϻϲ�������������в������ظ������ݵĻ�����ô��ʹ��union all��
ʹ��union��ϲ�ѯ�Ľ����������������Ĺ���
1�����в�ѯ�е��������е�˳�������ͬ��
2���������ͱ������
*/

/*ͬһ�������Ŀ���ѯ*/
SELECT kcsx,stockupperlimit,kcxx,stocklowerlimit
from zyddb..j_bjml a,dms.dbo.pa_part_list b
where b.partno=a.bjbh;

/*ROW_NUMBER��ʹ��*/
WITH TABS AS  
(  
	SELECT ROW_NUMBER() OVER(PARTITION BY ORG_ID  ORDER BY EMP_ID  ) AS ROWNO,ORG_ID,EMP_ID FROM ORG_EMPLOYEE  
 )  
SELECT MAX(ROWS) AS 'Ա������',ORG_ID FROM TABS GROUP BY ORG_ID;

