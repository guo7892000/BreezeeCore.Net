--SQLite���������﷨���£�
CREATE TRIGGER trigger_name [BEFORE/AFTER] trigger_type ON table_name
[FOR EACH ROW] [WHEN condition]
BEGIN
 --����������
END;

/*���У���������ĺ������£�
trigger_name�����������ƣ����ϱ�־���������淶�����ܺ����йؼ����ظ���
trigger_type�����������ͣ�������INSERT��UPDATE����DELETE��
table_name�������������ı�����
FOR EACH ROW��������������ÿһ�����ݣ������������ÿһ�н��в�����
WHEN condition������Ҫִ�еĲ�����������
BEGIN/END���������ڵĲ����� BEGIN �� END ��ָ����Χ��

������������
���趨������ʱ����Ҫѡ�񴥷������ͣ������Ĵ��������Ͱ�����
INSERT����һ��INSERT���ִ��ǰ/�󴥷���
UPDATE����һ��UPDATE���ִ��ǰ/�󴥷���
DELETE����һ��DELETE���ִ��ǰ/�󴥷���
INSTEAD OF����������ͼ�����ڶ���һ����ͼ����Ϊ��
*/

create trigger student_log after delete
on student
begin
	--�����old��ʾ����֮ǰ�� Ҳ����Ϊnew����ʾ����֮��
	insert into delete_log values(old.ID, datetime('now')); -- ɾ��֮�������delete_log����һ������
end;


DROP TRIGGER trigger_name;