-- PL/SQL -> Help -> Support Info���ɲ鿴TNS��Ϣ
-- ��ѯ�汾
SELECT version FROM v$instance;

/*
regexp_like(search_string ,pattern[,match_option])
����˵����
search_string��������ֵ
pattern��������ʽԪ�ַ����ɵ�ƥ��ģʽ,����������512�ֽ���
match_option����һ���ı����������û����øú�����ƥ����Ϊ������ʹ�õ�ѡ���У�
c ����Сд���У�Ĭ��ֵ
i ����Сд������
n ������ʹ��ԭ�㣨.��ƥ���κ������ַ�
m ������Դ�ַ���Ϊ����ַ����Դ�
*/
AND regexp_like(B.NAME_CN,#{NAME_CN,jdbcType=VARCHAR},'i')
