/*Mariadb�����ִ�Сд*/
/*��ѯ�������ݿ�*/
SHOW DATABASES;
/*��ѯ��ǰ���ݿ�*/
SELECT DATABASE();

/*��ѯ��*/
USE aprilspring;
SHOW TABLES
WHERE TABLES_IN_APRILSPRING = LOWER('bas_type');
SHOW TABLES FROM schemaname;
/*��ѯ���������*/
SHOW COLUMNS FROM BAS_TYPE 
WHERE FIELD='TYPE_ID'; /*ָ����*/
/*��ѯ���������*/
DESCRIBE bas_type;
/**/

/*
information_schema���ݿ���MySQL�Դ��ģ����ṩ�˷������ݿ�Ԫ���ݵķ�ʽ��ʲô��Ԫ�����أ�Ԫ�����ǹ������ݵ����ݣ�
�����ݿ�����������е��������ͣ������Ȩ�޵ȡ���Щʱ�����ڱ�������Ϣ������������������ݴʵ䡱�͡�ϵͳĿ¼����
��MySQL�У��� information_schema ������һ�����ݿ⣬ȷ��˵����Ϣ���ݿ⡣���б����Ź���MySQL��������ά��������
�������ݿ����Ϣ�������ݿ��������ݿ�ı��������������������Ȩ �޵ȡ���INFORMATION_SCHEMA�У�������ֻ����
����ʵ��������ͼ�������ǻ�������ˣ��㽫�޷�������֮��ص��κ��ļ���
information_schema���ݿ��˵��:

SCHEMATA���ṩ�˵�ǰmysqlʵ�����������ݿ����Ϣ����show databases�Ľ��ȡ֮�˱�

TABLES���ṩ�˹������ݿ��еı����Ϣ��������ͼ������ϸ������ĳ���������ĸ�schema�������ͣ������棬����ʱ�����Ϣ��
	��show tables from schemaname�Ľ��ȡ֮�˱�

COLUMNS���ṩ�˱��е�����Ϣ����ϸ������ĳ�ű���������Լ�ÿ���е���Ϣ��
	��show columns from schemaname.tablename�Ľ��ȡ֮�˱�

STATISTICS���ṩ�˹��ڱ���������Ϣ����show index from schemaname.tablename�Ľ��ȡ֮�˱�

USER_PRIVILEGES���û�Ȩ�ޣ��������˹���ȫ��Ȩ�޵���Ϣ������ϢԴ��mysql.user��Ȩ���ǷǱ�׼��

SCHEMA_PRIVILEGES������Ȩ�ޣ��������˹��ڷ��������ݿ⣩Ȩ�޵���Ϣ������Ϣ����mysql.db��Ȩ���ǷǱ�׼��

TABLE_PRIVILEGES����Ȩ�ޣ��������˹��ڱ�Ȩ�޵���Ϣ������ϢԴ��mysql.tables_priv��Ȩ���ǷǱ�׼��

COLUMN_PRIVILEGES����Ȩ�ޣ��������˹�����Ȩ�޵���Ϣ������ϢԴ��mysql.columns_priv��Ȩ���ǷǱ�׼��

CHARACTER_SETS���ַ��������ṩ��mysqlʵ�������ַ�������Ϣ����SHOW CHARACTER SET�����ȡ֮�˱�

COLLATIONS���ṩ�˹��ڸ��ַ����Ķ�����Ϣ��

COLLATION_CHARACTER_SET_APPLICABILITY��ָ���˿�����У�Ե��ַ�������Щ�е�Ч��SHOW COLLATION��ǰ������ʾ�ֶΡ�

TABLE_CONSTRAINTS�������˴���Լ���ı��Լ����Լ�����͡�

KEY_COLUMN_USAGE�������˾���Լ���ļ��С�

ROUTINES���ṩ�˹��ڴ洢�ӳ��򣨴洢����ͺ���������Ϣ����ʱ��ROUTINES�������Զ��庯����UDF����
��Ϊ��mysql.proc name������ָ���˶�Ӧ��INFORMATION_SCHEMA.ROUTINES���mysql.proc���С�

VIEWS�������˹������ݿ��е���ͼ����Ϣ����Ҫ��show viewsȨ�ޣ������޷��鿴��ͼ��Ϣ��

TRIGGERS���ṩ�˹��ڴ����������Ϣ��������superȨ�޲��ܲ鿴�ñ�
*/