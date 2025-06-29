DROP TABLE IF EXISTS ORG_REL_EMPLOYEE_POSITION;

/*1�����������ͱ�(BAS_TYPE)*/
CREATE TABLE IF NOT EXISTS BAS_TYPE (
TYPE_ID character varying(36) NOT NULL ,/*����ID*/
TYPE_CLASS_BIG character varying(20),/*���ʹ���*/
TYPE_CLASS_SMALL character varying(20),/*����С��*/
TYPE_CODE character varying(50),/*���ͱ���*/
TYPE_NAME character varying(100),/*��������*/
IS_FROM_INTERFACE character varying(2),/*�Ƿ���Դ�ӿ�*/
MODIFY_MODE character varying(2),/*�޸ķ�ʽ*/
BELONG_OBJECT_TYPE character varying(2),/*������������*/
EXTEND_VALUE1 character varying(50),/*��չֵ1*/
EXTEND_VALUE2 character varying(50),/*��չֵ2*/
EXTEND_VALUE3 character varying(50),/*��չֵ3*/
EXTEND_VALUE4 character varying(50),/*��չֵ4*/
EXTEND_VALUE5 character varying(50),/*��չֵ5*/
TYPE_DESC character varying(200),/*����*/
SORT_ID int ,/*����ID*/
REMARK character varying(200),/*��ע*/
CREATE_TIME date  NOT NULL  DEFAULT now() ,/*����ʱ��*/
CREATOR_ID character varying(36) NOT NULL ,/*������ID*/
CREATOR character varying(50),/*������*/
MODIFIER_ID character varying(36),/*�޸���ID*/
MODIFIER character varying(50),/*�޸���*/
LAST_UPDATED_TIME date  NOT NULL  DEFAULT now() ,/*������ʱ��*/
IS_ENABLED character varying(2) NOT NULL  DEFAULT 1 ,/*�Ƿ���Ч*/
IS_SYSTEM character varying(2) NOT NULL  DEFAULT 0 ,/*ϵͳ��־*/
ORG_ID character varying(36) NOT NULL ,/*��֯ID*/
UPDATE_CONTROL_ID character varying(36) NOT NULL ,/*��������ID*/
TFLAG character varying(2) NOT NULL  DEFAULT 0 ,/*�����־*/
 PRIMARY KEY (TYPE_ID)
);
COMMENT ON TABLE BAS_TYPE IS '���ͱ�';
 COMMENT ON COLUMN BAS_TYPE.TYPE_ID IS '����ID:����ID';
 COMMENT ON COLUMN BAS_TYPE.TYPE_CLASS_BIG IS '���ʹ���:ֻ�ڲ�ѯͳ����ʹ��';
 COMMENT ON COLUMN BAS_TYPE.TYPE_CLASS_SMALL IS '����С��:ֻ�ڲ�ѯͳ����ʹ��';
 COMMENT ON COLUMN BAS_TYPE.TYPE_CODE IS '���ͱ���:';
 COMMENT ON COLUMN BAS_TYPE.TYPE_NAME IS '��������:';
 COMMENT ON COLUMN BAS_TYPE.IS_FROM_INTERFACE IS '�Ƿ���Դ�ӿ�:0��1��';
 COMMENT ON COLUMN BAS_TYPE.MODIFY_MODE IS '�޸ķ�ʽ:���Ƿ���������0���ɣ�1����';
 COMMENT ON COLUMN BAS_TYPE.BELONG_OBJECT_TYPE IS '������������:1ϵͳ���壬2�ͻ�����';
 COMMENT ON COLUMN BAS_TYPE.EXTEND_VALUE1 IS '��չֵ1:';
 COMMENT ON COLUMN BAS_TYPE.EXTEND_VALUE2 IS '��չֵ2:';
 COMMENT ON COLUMN BAS_TYPE.EXTEND_VALUE3 IS '��չֵ3:';
 COMMENT ON COLUMN BAS_TYPE.EXTEND_VALUE4 IS '��չֵ4:';
 COMMENT ON COLUMN BAS_TYPE.EXTEND_VALUE5 IS '��չֵ5:';
 COMMENT ON COLUMN BAS_TYPE.TYPE_DESC IS '����:';
 COMMENT ON COLUMN BAS_TYPE.SORT_ID IS '����ID:';
 COMMENT ON COLUMN BAS_TYPE.REMARK IS '��ע:';
 COMMENT ON COLUMN BAS_TYPE.CREATE_TIME IS '����ʱ��:';
 COMMENT ON COLUMN BAS_TYPE.CREATOR_ID IS '������ID:';
 COMMENT ON COLUMN BAS_TYPE.CREATOR IS '������:';
 COMMENT ON COLUMN BAS_TYPE.MODIFIER_ID IS '�޸���ID:';
 COMMENT ON COLUMN BAS_TYPE.MODIFIER IS '�޸���:';
 COMMENT ON COLUMN BAS_TYPE.LAST_UPDATED_TIME IS '������ʱ��:';
 COMMENT ON COLUMN BAS_TYPE.IS_ENABLED IS '�Ƿ���Ч:';
 COMMENT ON COLUMN BAS_TYPE.IS_SYSTEM IS 'ϵͳ��־:';
 COMMENT ON COLUMN BAS_TYPE.ORG_ID IS '��֯ID:��ʾ���������Ĺ�˾';
 COMMENT ON COLUMN BAS_TYPE.UPDATE_CONTROL_ID IS '��������ID:';
 COMMENT ON COLUMN BAS_TYPE.TFLAG IS '�����־:0���ϴ���1δ�ϴ���2�ɹ��ϴ���3�ϴ�����';

/*�����޸ı�������*/
/*������*/
ALTER TABLE t_test_db_supplier ADD supply_addr varchar(500) NULL;
COMMENT ON COLUMN t_test_db_supplier.supply_addr IS '��Ӧ�̵�ַ';

/*�޸��г���*/
ALTER TABLE t_test_db_supplier ALTER COLUMN supplier_full_name TYPE varchar(300) USING supplier_full_name::varchar(300);
/*�����зǿ�*/
ALTER TABLE t_test_db_supplier ALTER COLUMN supplier_full_name SET NOT NULL;
/*�޸���Ĭ��ֵ*/
ALTER TABLE t_test_db_supplier ALTER COLUMN supplier_full_name SET DEFAULT 2;

/*ɾ����*/
ALTER TABLE t_test_db_supplier DROP COLUMN sort_id;
/*��������*/
ALTER TABLE t_test_db_supplier RENAME COLUMN supply_addr TO supply_addr1;

/*������Ĭ��ֵ*/
ALTER TABLE table_name ALTER COLUMN column_name SET DEFAULT 'new_value';
/*ȥ���е�Ĭ��ֵ*/
ALTER TABLE table_name ALTER COLUMN column_name DROP DEFAULT;

/*�����зǿ�*/
ALTER TABLE table_name ALTER COLUMN column_name SET NOT NULL;
/*ȥ���еķǿ�Լ��*/
ALTER TABLE your_table_name ALTER COLUMN column_name DROP NOT NULL;


