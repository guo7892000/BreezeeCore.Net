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
CREATE TABLE IF NOT EXISTS `DBT_BD_DB_CONFIG` (
DB_CONFIG_ID varchar(36) NOT NULL  COMMENT '���ݿ�����ID:����ID',/*���ݿ�����ID*/
DB_CONFIG_CODE varchar(50) COMMENT '���ݿ����ñ���:',/*���ݿ����ñ���*/
DB_CONFIG_NAME varchar(100) COMMENT '���ݿ���������:',/*���ݿ���������*/
DB_TYPE varchar(2) COMMENT '���ݿ�����:',/*���ݿ�����*/
SERVER_IP varchar(400) COMMENT '������IP:��Oracle���ͣ�ΪTNS����,SQLite�Ǵ��ļ�·��',/*������IP*/
PORT_NO varchar(20) COMMENT '�˿ں�:',/*�˿ں�*/
SCHEMA_NAME varchar(30) COMMENT '�ܹ�����:',/*�ܹ�����*/
DB_NAME varchar(30) COMMENT '���ݿ�����:',/*���ݿ�����*/
USER_NAME varchar(50) COMMENT '�û���:',/*�û���*/
USER_PASSWORD varchar(50) COMMENT '�û�����:',/*�û�����*/
LOGIN_TYPE varchar(2) COMMENT '��¼����:���SqlServer����Windows��¼��SQL��¼���֡�',/*��¼����*/
TYPE_DESC varchar(200) COMMENT '����:',/*����*/
SORT_ID int  COMMENT '����ID:',/*����ID*/
REMARK varchar(200) COMMENT '��ע:',/*��ע*/
CREATE_TIME datetime  NOT NULL  COMMENT '����ʱ��:',/*����ʱ��*/
CREATOR_ID varchar(36) NOT NULL  COMMENT '������ID:',/*������ID*/
CREATOR varchar(50) COMMENT '������:',/*������*/
MODIFIER_ID varchar(36) COMMENT '�޸���ID:',/*�޸���ID*/
MODIFIER varchar(50) COMMENT '�޸���:',/*�޸���*/
LAST_UPDATED_TIME datetime  NOT NULL  COMMENT '������ʱ��:',/*������ʱ��*/
IS_ENABLED varchar(2) NOT NULL  DEFAULT 1  COMMENT '�Ƿ���Ч:',/*�Ƿ���Ч*/
IS_SYSTEM varchar(2) NOT NULL  DEFAULT 0  COMMENT 'ϵͳ��־:',/*ϵͳ��־*/
ORG_ID varchar(36) NOT NULL  COMMENT '��֯ID:��ʾ���������Ĺ�˾',/*��֯ID*/
UPDATE_CONTROL_ID varchar(36) NOT NULL  COMMENT '��������ID:',/*��������ID*/
TFLAG varchar(2) NOT NULL  DEFAULT 0  COMMENT '�����־:0���ϴ���1δ�ϴ���2�ɹ��ϴ���3�ϴ�����',/*�����־*/
 primary key (DB_CONFIG_ID) 
);
ALTER TABLE `DBT_BD_DB_CONFIG` COMMENT '���ݿ����ã�';

