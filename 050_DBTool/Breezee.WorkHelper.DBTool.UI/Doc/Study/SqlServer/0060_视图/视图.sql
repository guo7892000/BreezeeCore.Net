/*ɾ����ͼ*/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_SYS_AND_MENU]'))
DROP VIEW [dbo].[V_SYS_AND_MENU]
GO

/*������ͼ*/
CREATE VIEW [dbo].[V_SYS_AND_MENU] 
AS
SELECT
/*******************************************
* �������ƣ�ϵͳ�˵���ͼ
* �������ͣ���ͼ
* �������ߣ��ƹ���
* �������ڣ�2014-7-25
* �޸���ʷ��
*	V1.0��hgh ���� 2014-7-25
*******************************************/
	  MU.MENU_ID ,/*�˵�ID*/
	  MU.MENU_CODE ,/*�˵�����*/
	  MU.MENU_NAME ,/*�˵�����*/
	  CASE WHEN MU.PARENT_MENU_ID IS NULL THEN MU.SYS_ID
			ELSE MU.PARENT_MENU_ID
	  END AS PARENT_MENU_ID ,/*���˵�ID*/
	  MU.MENU_TYPE ,/*�˵�����*/
	  MU.SYS_ID ,/*ϵͳID*/
	  MU.SORT_ID, /*�����*/
	  MU.IS_ENABLED,
	  /*ֻ�в˵��е��ֶ�*/
	  MU.FORM_ID ,
	  MU.MENU_DESC ,
	  MU.MENU_ACTION ,
	  MU.MENU_PARM ,
	  MU.MENU_ICON ,
	  V.MENU_FULL_PATH,
	  MU.DLL_ID ,
	  MU.DLL_NAME ,
	  MU.FORM_FULL_PATH ,
	  MU.FORM_TYPE ,
	  MU.REMARK ,
	  MU.CREATE_TIME ,
	  MU.CREATOR_ID ,
	  MU.CREATOR ,
	  MU.MODIFIER_ID ,
	  MU.MODIFIER ,
	  MU.LAST_UPDATED_TIME ,
	  MU.IS_SYSTEM ,
	  MU.ORG_ID ,
	  MU.UPDATE_CONTROL_ID ,
	  MU.RIGHT_TYPE,
	  MU.NOT_RIGHT_BUTTON_DISPLAY_TYPE
FROM    SYS_MENU MU
LEFT JOIN V_SYS_MENU_FULL_PATH V
	ON MU.MENU_ID = V.LAST_MENU_ID
WHERE MU.IS_ENABLED = '1';
GO

/*�޸���ͼ��ע����ͼһ��Ҫ���ڣ�����ᱨ��*/
ALTER VIEW [dbo].[V_SYS_AND_MENU] 
AS
SELECT
/*******************************************
* �������ƣ�ϵͳ�˵���ͼ
* �������ͣ���ͼ
* �������ߣ��ƹ���
* �������ڣ�2014-7-25
* �޸���ʷ��
*	V1.0��hgh ���� 2014-7-25
*******************************************/
	MU.MENU_ID ,/*�˵�ID*/
	MU.MENU_CODE /*�˵�����*/
FROM    SYS_MENU MU
LEFT JOIN V_SYS_MENU_FULL_PATH V
	ON MU.MENU_ID = V.LAST_MENU_ID
WHERE MU.IS_ENABLED = '1';
GO

