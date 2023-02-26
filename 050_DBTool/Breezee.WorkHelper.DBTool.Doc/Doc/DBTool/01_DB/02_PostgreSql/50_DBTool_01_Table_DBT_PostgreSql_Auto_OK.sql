/***********************************************************************************
* �ű�����: �����޸ı�
* ��������: 
* ��������: 2021/8/10 
* ʹ��ģ�飺
* ʹ�ð汾: 
* ˵    ����
      1 �������ݿ����ã�DBT_BD_DB_CONFIG��
***********************************************************************************/
/*1�����������ݿ�����(DBT_BD_DB_CONFIG)*/
CREATE TABLE IF NOT EXISTS DBT_BD_DB_CONFIG (
DB_CONFIG_ID character varying(36) NOT NULL ,/*���ݿ�����ID*/
DB_CONFIG_CODE character varying(50),/*���ݿ����ñ���*/
DB_CONFIG_NAME character varying(100),/*���ݿ���������*/
DB_TYPE character varying(2),/*���ݿ�����*/
SERVER_IP character varying(400),/*������IP*/
PORT_NO character varying(20),/*�˿ں�*/
SCHEMA_NAME character varying(30),/*�ܹ�����*/
DB_NAME character varying(30),/*���ݿ�����*/
USER_NAME character varying(50),/*�û���*/
USER_PASSWORD character varying(50),/*�û�����*/
LOGIN_TYPE character varying(2),/*��¼����*/
TYPE_DESC character varying(200),/*����*/
SORT_ID int ,/*����ID*/
REMARK character varying(200),/*��ע*/
CREATE_TIME date  NOT NULL ,/*����ʱ��*/
CREATOR_ID character varying(36) NOT NULL ,/*������ID*/
CREATOR character varying(50),/*������*/
MODIFIER_ID character varying(36),/*�޸���ID*/
MODIFIER character varying(50),/*�޸���*/
LAST_UPDATED_TIME date  NOT NULL ,/*������ʱ��*/
IS_ENABLED character varying(2) NOT NULL  DEFAULT 1 ,/*�Ƿ���Ч*/
IS_SYSTEM character varying(2) NOT NULL  DEFAULT 0 ,/*ϵͳ��־*/
ORG_ID character varying(36) NOT NULL ,/*��֯ID*/
UPDATE_CONTROL_ID character varying(36) NOT NULL ,/*��������ID*/
TFLAG character varying(2) NOT NULL  DEFAULT 0 ,/*�����־*/
 PRIMARY KEY (DB_CONFIG_ID)
);
COMMENT ON TABLE DBT_BD_DB_CONFIG IS '���ݿ����ã�';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.DB_CONFIG_ID IS '���ݿ�����ID:����ID';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.DB_CONFIG_CODE IS '���ݿ����ñ���:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.DB_CONFIG_NAME IS '���ݿ���������:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.DB_TYPE IS '���ݿ�����:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.SERVER_IP IS '������IP:��Oracle���ͣ�ΪTNS����,SQLite�Ǵ��ļ�·��';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.PORT_NO IS '�˿ں�:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.SCHEMA_NAME IS '�ܹ�����:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.DB_NAME IS '���ݿ�����:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.USER_NAME IS '�û���:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.USER_PASSWORD IS '�û�����:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.LOGIN_TYPE IS '��¼����:���SqlServer����Windows��¼��SQL��¼���֡�';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.TYPE_DESC IS '����:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.SORT_ID IS '����ID:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.REMARK IS '��ע:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.CREATE_TIME IS '����ʱ��:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.CREATOR_ID IS '������ID:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.CREATOR IS '������:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.MODIFIER_ID IS '�޸���ID:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.MODIFIER IS '�޸���:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.LAST_UPDATED_TIME IS '������ʱ��:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.IS_ENABLED IS '�Ƿ���Ч:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.IS_SYSTEM IS 'ϵͳ��־:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.ORG_ID IS '��֯ID:��ʾ���������Ĺ�˾';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.UPDATE_CONTROL_ID IS '��������ID:';
 COMMENT ON COLUMN DBT_BD_DB_CONFIG.TFLAG IS '�����־:0���ϴ���1δ�ϴ���2�ɹ��ϴ���3�ϴ�����';

