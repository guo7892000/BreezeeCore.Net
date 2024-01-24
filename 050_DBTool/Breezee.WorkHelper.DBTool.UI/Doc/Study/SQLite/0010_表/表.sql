/*������*/
CREATE TABLE OrderTest(
	ID int not null,
	OrderCode varchar(30),
	Remark
);

/*ɾ����*/
DROP TABLE OrderTest;

/*���±������ֶ�*/
ALTER TABLE OrderTest ADD COLUMN OrderDate Date not null;

/*���±�ɾ���ֶ�*/
ALTER TABLE OrderTest DROP COLUMN OrderDate;

DROP TABLE IF EXISTS ORG_POSITION_LEVEL;

/*1�����������ͱ�(BAS_TYPE)*/
CREATE TABLE IF NOT EXISTS "BAS_TYPE" (
TYPE_ID varchar(36) PRIMARY KEY  NOT NULL ,/*����ID*/
TYPE_CLASS_BIG varchar(20),/*���ʹ���*/
TYPE_CLASS_SMALL varchar(20),/*����С��*/
TYPE_CODE varchar(50),/*���ͱ���*/
TYPE_NAME varchar(100),/*��������*/
IS_FROM_INTERFACE varchar(2),/*�Ƿ���Դ�ӿ�*/
MODIFY_MODE varchar(2),/*�޸ķ�ʽ*/
BELONG_OBJECT_TYPE varchar(2),/*������������*/
EXTEND_VALUE1 varchar(50),/*��չֵ1*/
EXTEND_VALUE2 varchar(50),/*��չֵ2*/
EXTEND_VALUE3 varchar(50),/*��չֵ3*/
EXTEND_VALUE4 varchar(50),/*��չֵ4*/
EXTEND_VALUE5 varchar(50),/*��չֵ5*/
TYPE_DESC varchar(200),/*����*/
SORT_ID int ,/*����ID*/
REMARK varchar(200),/*��ע*/
CREATE_TIME datetime  NOT NULL ,/*����ʱ��*/
CREATOR_ID varchar(36) NOT NULL ,/*������ID*/
CREATOR varchar(50),/*������*/
MODIFIER_ID varchar(36),/*�޸���ID*/
MODIFIER varchar(50),/*�޸���*/
LAST_UPDATED_TIME datetime  NOT NULL ,/*������ʱ��*/
IS_ENABLED varchar(2) NOT NULL  DEFAULT 1 ,/*�Ƿ���Ч*/
IS_SYSTEM varchar(2) NOT NULL  DEFAULT 0 ,/*ϵͳ��־*/
ORG_ID varchar(36) NOT NULL ,/*��֯ID*/
UPDATE_CONTROL_ID varchar(36) NOT NULL ,/*��������ID*/
TFLAG varchar(2) NOT NULL  DEFAULT 0 /*�����־*/
);

