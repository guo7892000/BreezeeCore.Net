/*��ѯǰʮ��*/
SELECT a 
FROM t1 WHERE a=10 AND B=1 
ORDER BY a 
LIMIT 10
;

/*���鶯̬��������ȡһ����ȫ��ϸ������������Ϊ�����У�Ȼ����ݷ���������ͬ����ȡ�̶�ֵ���൱�ں��Ը÷������������Ǿ���ֵ*/
SELECT case when t.AUTH_STATUS='3' then t.AUTH_PASS_DATE else t.AUTH_APPLY_DATE end,count(1) 
from t_au_bu_auth_apply t
#where t.AUTH_STATUS='3'
group by case when t.AUTH_STATUS='3' then t.AUTH_PASS_DATE else t.AUTH_APPLY_DATE end
;

/*ʹ��With�﷨��ʵ������ͳ����ϸ���á��ȸ��������õ�������С���ظ������ı��ٽ����������������ע��WITH�е���ʱ������������̫�󣬻�Ӱ�����ܡ�*/
with m_tb as (SELECT * 
from t_au_bu_auth_apply t
where t.AUTH_STATUS='3'
and t.AUTH_APPLY_DATE>'2023-01-01')
select AUTH_APPLY_DATE,AUTH_STATUS from m_tb;

/*��̬SQL:�������в�ȷ����������һ������ͳ�ƽ��*/
SET @EE='';
select @EE := CONCAT(@EE,'sum(if(OLD_CUST_NAME= \'',OLD_CUST_NAME,'\',IS_BIND,0)) as `',OLD_CUST_NAME, '`,') AS aa FROM (SELECT DISTINCT OLD_CUST_NAME FROM t_au_car_bind_log where OLD_CUST_NAME is not null) A ;
SET @QQ = CONCAT('select ifnull(VIN,\'TOTAL\')as userid,',@EE,' sum(IS_BIND) as TOTAL from t_au_car_bind_log where OLD_CUST_NAME is not null group by VIN WITH ROLLUP');
#SELECT @QQ;
PREPARE stmt FROM @QQ;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;


