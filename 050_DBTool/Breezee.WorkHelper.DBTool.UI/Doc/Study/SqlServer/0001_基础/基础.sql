/*�������÷ֲ�ʽ������Ҫ��Distributed Transaction Coordinator��������*/
---�������̣������С�==>��net start msdtc��

/*�������ǰ5������������ĸ���ձ�1��A�� 2��B�� 3��C�� 4��D�� 5��E�� 6��F�� 7��G�� 8��H�� 9��K�� 0��M����
 ��ѯ��������ڵ���ǰ��λ������Ϊ��ͷ�ĺ�ģ����ѯʾ��������ֻ����λΪ����*/ 
 SELECT * FROM PA_PART_LIST 
 WHERE 1=1 --AND Charindex('5',PARTNO)=1
 AND (Charindex('E',PARTNO)=1 OR Charindex('5',PARTNO)=1)
 AND (Charindex('B',PARTNO)=2 OR Charindex('2',PARTNO)=2) AND PARTNO LIKE '%112A%';

