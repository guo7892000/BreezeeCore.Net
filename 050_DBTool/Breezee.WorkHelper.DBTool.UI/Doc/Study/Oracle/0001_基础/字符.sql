/*��ѯ���ַ��͸�ֵӦ��*/
declare v varchar2(20);
begin
 v:='��dҪ��һɱ��';
 dbms_output.put_line(substr(v, 2, 3));
end;

/*�ַ�����*/
select concat('�й�','����') "test",'�й�'||'����' from dual;

/*INITCAP(n)����
 ���ַ���n��ÿ����������ĸ��д������Сд(���ֵ��ʵĹ����ǰ��ո�����ĸ�ַ����������������ַ�����û���κ�����)�����磺*/
select initcap('�� �� �� ��') "test",initcap('my word') "test1",initcap('my�й�word') "test2" from dual;

/*INSTR(chr1,chr2,[n,[m]])����
��ȡ�ַ���chr2���ַ���chr1�г��ֵ�λ�á�n��m��ѡ,ʡ����Ĭ��Ϊ1��
n����ʼ���ҵ���ʼλ�ã���nΪ������β����ʼ������m�����ִ����ֵĴ��������磺*/
select instr('pplkoopijk','k',-1,1) "test",instr('pplkoopijk','k',1,2) from dual;

/*LENGTH(n)����
 �����ַ����ַ������ȡ�(��nΪnullʱ������nll�����صĳ��Ȱ�������Ŀո�)�����磺*/
select length('ppl ') "test",length(null) "test1",length(trim('ppl ')) "test" from dual;

/*LOWER(n)����
 ��nת��ΪСд;UPPER(n)����������: ��nת��Ϊ��д�����磺*/
select lower('KKKD') "test",UPPER('hello') from dual;

/*LPAD(chr1,n,[chr2])����
��chr1�������ַ�chr2��ʹ���ַ��ܳ���Ϊn��chr2��ѡ��Ĭ��Ϊ�ո񣻵�chr1�ַ������ȴ���nʱ�������߽�ȡchr1��n���ַ���ʾ��
RPAD(chr1,n,chr2)��������������chr1�ұ����chr2��ʹ�����ַ�������Ϊn..��chr1���ȴ���nʱ���������n���ַ������磺*/
select lpad('kkk',5) "test",lpad('kkkkk',4) "test1",lpad('kkk',6,'lll') "test2" from dual;

/*LTRIM(chr,[n])����
ȥ���ַ���chr��߰�����n�ַ����е��κ��ַ���ֱ������һ����������n�е��ַ�Ϊֹ�����磺*/
select ltrim('abcde','a') "test",ltrim('abcde','b') "test1",ltrim('abcdefg','cba') "test2",ltrim('  abcdefg') from dual;

/*REPLACE(chr,search_string,[,replacement_string])������
��chr������search_string�������滻Ϊreplacement_stringָ�����ַ�������search_stringΪnullʱ������chr��
��replacement_stringΪnullʱ������chr�н�ȡ��search_string���ֵ��ַ��������磺*/
SELECT REPLACE('abcdeef','e','oo') "test",REPLACE('abcdeef','ee','oo') "test1",REPLACE('abcdeef',NULL,'oo') "test2",REPLACE('abcdeef','ee',NULL) "test3" FROM dual;

/*SQL�е�SQL���ѵ����Ż������������žͺ���*/
SELECT 'INSERT INTO (a, b,c,d) VALUES(''aa'', 2, '''',3)' from dual;