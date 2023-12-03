DATE_FORMAT(DATE_ADD(NOW(),INTERVAL -1 DAY),'%Y%m%d');

/*����һ��ʹ��DATE()����*/
SELECT DATE(my_datetime_column) FROM my_table;
/*��������ʹ��CAST()����*/
SELECT CAST(my_datetime_column AS DATE) FROM my_table;
/*��������ʹ��DATE_FORMAT()����*/
SELECT DATE_FORMAT(my_datetime_column, '%Y-%m-%d') FROM my_table;

date_add( DATE_FORMAT(#{param.modifyDateEnd},'%Y-%m-%d'), interval 1 day);

select DATEDIFF('2008-11-30','2008-11-29'),
if(ifnull(LI.IS_LIMIT, '0')= '0', '0', '1'),
DATE_FORMAT(C.BIND_DATE,'%Y-%m-%d %H:%i:%s'),
DATE_ADD(DATE_FORMAT(now(),'%Y-%m-%d'), INTERVAL 1 DAY)
;


