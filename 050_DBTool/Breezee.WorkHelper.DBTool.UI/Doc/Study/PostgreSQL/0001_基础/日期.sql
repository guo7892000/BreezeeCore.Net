SELECT CURRENT_DATE,/*��ȡϵͳ��������*/
	CURRENT_TIME,/*��ȡϵͳ��������(����ʱ��ʱ��)*/
	LOCALTIME,/*��ȡϵͳ��������(����ʱ����ʱ��)*/
	CURRENT_TIMESTAMP,/*��ȡϵͳ�������ں�ʱ��*/
	LOCALTIMESTAMP,/*��ȡϵͳ�������ں�ʱ��*/
	NOW(),/*��ȡϵͳ�������ں�ʱ��*/
	EXTRACT(DAY FROM TIMESTAMP '2012-09-10 10:18:40'),/*���·�����ȡ����*/
	EXTRACT(MONTH FROM TIMESTAMP '2012-09-10 10:18:40'),
	EXTRACT(YEAR FROM TIMESTAMP '2012-09-10 10:18:40'),
	EXTRACT(DOY FROM TIMESTAMP '2012-09-10 10:18:40'),/*��ѯָ��������һ���еĵڼ���*/
	EXTRACT(DOW FROM TIMESTAMP '2012-09-10 10:18:40'),/*��ѯָ��������һ���е����ڼ�*/
	EXTRACT(QUARTER FROM TIMESTAMP '2012-09-10 10:18:40'),/*��ѯָ�������Ǹ���ĵڼ�����(1-4)*/
	DATE '2019-09-28' + integer '10',
	DATE '2012-09-28' + interval '3 hour',
	DATE '2012-09-28' + time '06:00',
	TIMESTAMP '2012-09-28 02:00:00' + interval '10 hours',
	date '2012-11-01' - date '2012-09-10',
	DATE '2012-09-28' - integer '10',
	15 * interval '2 day',
	50 * interval '2 second',
	interval '1 hour' / integer  '2',



;