SELECT date('now'), --���ص�ǰ����
	time('now'), --���ص�ǰʱ��
	datetime('now'), --���ص�ǰ���ں�ʱ��
	strftime('%Y-%m-%d %H:%M:%S', 'now'), --�����ں�ʱ���ʽ��Ϊ�ַ���
	julianday('now'), --���شӹ�Ԫǰ4713��1��1�տ�ʼ��������Ҳ��Ϊ�����ա�
	date('now', '+1 month'), --����ǰ�������һ����
	julianday(date1) - julianday(date2) --�����������ڻ�ʱ��֮��Ĳ���
;