/*Mysql�ķ�������
��ʽ��	������() OVER(PARTITION BY ����1 ORDER BY ����2 DESC) AS �б���1	
�кţ�	ROW_NUMBER() OVER(PARTITION BY VIN ORDER BY INVOICE_DATE, INVOICE_NO DESC) AS ORDER_NUM	
����	rank()	
����	dense_rank()	
����	count()	
���ֵ	max()	
��Сֵ	min()	
���	sum()	
ƽ��ֵ	avg()	
��һ��ֵ	first_value()	
���ֵ	last_value()	
ǰN��	lag()	
��N��	lead()	
*/
select instr(A.NEW_CAR_TYPE_CODE,':') >0
FROM t_car_type
;


