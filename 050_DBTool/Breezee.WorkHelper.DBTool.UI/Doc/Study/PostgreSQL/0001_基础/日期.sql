-- ��ȡ���ڻ�ʱ��
select current_date,/*��ȡϵͳ��ǰ����*/
	current_time,/*��ȡϵͳ��ǰʱ��(����ʱ��ʱ��)*/
	localtime,/*��ȡϵͳ��ǰʱ��(����ʱ����ʱ��)*/
	current_timestamp,/*��ȡϵͳ��ǰ���ں�ʱ��(����ʱ��ʱ��)*/
	localtimestamp,/*��ȡϵͳ��ǰ���ں�ʱ��(����ʱ����ʱ��)*/
	now(),/*��ȡϵͳ��ǰ���ں�ʱ��(����ʱ��ʱ��)*/
	extract(day from timestamp '2012-09-10 10:18:40') as extract_day,/*���·�����ȡ����*/
	extract(month from timestamp '2012-09-10 10:18:40') as extract_month,
	extract(year from timestamp '2012-09-10 10:18:40') as extract_year,
	extract(doy from timestamp '2012-09-10 10:18:40') as extract_doy,/*��ѯָ��������һ���еĵڼ���*/
	extract(dow from timestamp '2012-09-10 10:18:40') as extract_dow,/*��ѯָ��������һ���е����ڼ�*/
	extract(quarter from timestamp '2012-09-10 10:18:40') as extract_quarter,/*��ѯָ�������Ǹ���ĵڼ�����(1-4)*/
	date '2019-09-28' + integer '10' as date_integer,/*��10��*/
	date '2012-09-28' + interval '3 hour' as date_interval_hour,/*��3Сʱ*/
	date '2012-09-28' + time '06:00' as date_time,/*����+ʱ��*/
	timestamp '2012-09-28 02:00:00' + interval '10 hours' as timestamp_interval,/*ʱ���+ʱ��*/
	date '2012-11-01' - date '2012-09-10' as date_subtract_date,/*���ڲ�*/
	date '2012-09-28' - integer '10' date_subtract_integer,/*����-����*/
	15 * interval '2 day' as interval_double_day,
	50 * interval '2 second' as interval_double_second,
	interval '1 hour' / integer  '2' as interval_chu_integer


-- ����ת��
select to_char(now(), 'yyyy-mm-dd hh24:mi:ss') as to_char1,
 to_char(now(), 'yyyy-mm-dd') as to_char2,
 to_timestamp('2025-04-01 14:30:55', 'yyyy-mm-dd hh24:mi:ss') as to_timestamp1,
 cast('2023-04-01 12:00:00' as timestamp) as cast_timestamp,
 to_date('2025-04-01', 'yyyy-mm-dd') as to_date1,
 cast('2023-04-01' as date) as cast_date1,
 date '2023-04-01' as date_only,
 time '12:00:00' as time_only,
 date '2023-04-01' + time '12:00:00' date_time