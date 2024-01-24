/*ɾ���洢����*/
--1��ɾ��CLR�洢����
IF ( OBJECT_ID('P_P_E3S_OWEQTY_RECEIPT', 'PC') IS NOT NULL ) 
	DROP PROCEDURE [dbo].[P_P_E3S_OWEQTY_RECEIPT];
--2��ɾ��SQL��CLR�洢����
IF ( OBJECT_ID('P_PA_E3S_OUTSTOCK_RECEIPT') IS NOT NULL ) 
	DROP PROCEDURE P_PA_E3S_OUTSTOCK_RECEIPT;
--3��ɾ��SQL��CLR�洢����
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IFR_UC_T_BASE_ASSESSMENT_ITEMProcedure]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[IFR_UC_T_BASE_ASSESSMENT_ITEMProcedure];
GO

/*�����洢����*/
/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_COMM_GET_FORM_CODE]') AND type in (N'P', N'PC'))
DROP PROCEDURE[dbo].[P_COMM_GET_FORM_CODE]
GO
*/
CREATE PROCEDURE [dbo].[P_COMM_GET_FORM_CODE]
(
       @V_ORG_ID VARCHAR(50),     --��֯ID
       @V_RULE_CODE VARCHAR(50),   --�������
       @V_RETURN_CODE VARCHAR(50) OUTPUT
)
AS 
/*******************************************************************************************
* �洢�������ƣ����ɵ���
* ���ߣ�       �ƹ���
* �������ڣ�   2013-12-15
* ��Ҫ����:  
*       ���õ��Ź����BAS_CODE_RULE��:�������������ɵ��ţ�������֯����һ�׹���
*       ����ʹ����Ϣ��BAS_CODE_USE�������ÿһ�����ͣ�һ����֯һ�����ݡ�
* �޸���ʷ��
*     V1.00 �����������ɹ���洢���� HGH 2013-11-01
*	  V1.01 �޸ģ������������͵�֧�֣�ʵ���������Ϊ��ֵ̬��
*	  V1.02 �޸ģ���������������Ϊ0ʱ���޷������������⡣hgh 2016-7-4
********************************************************************************************/
BEGIN
  DECLARE @V_CODE_PRE        VARCHAR(50), --��ʼ����ֵ
		@V_CODETIME_FORMAT    VARCHAR(50), --ʱ���ʽ
		@V_CODE_LENGTH        VARCHAR(50), --��ˮ�ų���
		@V_RESUME_PERIOD      VARCHAR(50), --�ݹ����ڸ�ʽ����ǰ��ʹ��
		@V_INITVALUE          VARCHAR(50), --��ʼ��ֵ
		@V_STEP               VARCHAR(50), --����ֵ
		@V_END_CODE      VARCHAR(50), --��׺����ֵ
		@V_ORDER_CODE_RULE_ID VARCHAR(50), --���Ź���ID
		@CONFIG_TYPE_CODE	  VARCHAR(50), --�������ͣ���ֵ����#�ŵ�����
		
		@V_CURRENT_VALUE      VARCHAR(50), --��ǰֵ
		@V_UPDATE_CONTROL_ID  VARCHAR(50), --��ǰֵ

		@V_CURR_FORMAT_VALUE VARCHAR(50), --�ϴε�ǰ
		@V_COUNT             VARCHAR(50), --����ֵ
		@V_NEW_FORMAT_VALUE VARCHAR(50), --�����ֵ

		@V_FIST_CODE VARCHAR(50),
		@V_DLR_CODE  VARCHAR(50),       /*�������*/
		@V_NEW_VALUE       VARCHAR(50),/*��ˮ���ַ�*/
		@I_NEW_VALUE        INTEGER, /*��ˮ�ŵ�����ֵ*/
		@ICOUNT             INTEGER, /*��¼��*/
		@V_INT              INTEGER,
		@V_LENGTH           INTEGER,
		@VALUE_NAME         VARCHAR(100),
		
		--��ȡ��������ֵ
		@V_TABLE_NAME  VARCHAR(50),
		@V_CODE_COLUMN_NAME VARCHAR(50),
		@V_PK_COLUMN_NAME VARCHAR(50),
		@V_OTHER_CONDITIONE VARCHAR(1000),
		
		/*��̬SQL��ز���*/
		@SQLSTRING NVARCHAR(4000),
		@PARMDEFINITION NVARCHAR(500),
		@FORM_CODE_SHORT_OUT VARCHAR(30),
		
		--������Ϣ 
		@V_NOW_DATE VARCHAR(10),
		@ERROR_MES VARCHAR(1000)	/*��������*/
		
  SET @V_COUNT = 0;
  SET @V_RETURN_CODE = '';
	
  /*��ѯ�õ������͵����ü�¼��*/
  SET @V_COUNT = 0
  SELECT @V_COUNT=COUNT(1) 
   FROM CFG_CODE_RULE T
   WHERE T.CODE_RULE_CODE=@V_RULE_CODE
		 AND T.IS_ENABLED='1';       
  /*���ñ������һ���ж�*/
  IF @V_COUNT <> 1
  BEGIN
    IF @V_COUNT = 0
		BEGIN
			SET @ERROR_MES='������CODE_RULE_CODEΪ['+@V_RULE_CODE+']�ĵ����������ݣ�'
		END
	ELSE
		BEGIN
			SET @ERROR_MES='CODE_RULE_CODEΪ['+@V_RULE_CODE+']�ĵ����������ݴ��ڶ�������ɾ���ظ������ݣ�'
		END
	--�׳�����
	RAISERROR(@ERROR_MES,1,10) WITH NOWAIT
	RETURN;
  END
  
  --��ȡ��������
  SELECT  @V_ORDER_CODE_RULE_ID=T.CODE_RULE_ID,
		  @V_CODE_PRE=T.PRE_CODE,
		  @V_END_CODE = T.END_CODE,
		  @V_CODETIME_FORMAT=T.DATE_FORMAT,
		  @V_CODE_LENGTH=T.AUTO_CODE_LENGH,
		  @V_RESUME_PERIOD=T.RESUME_TYPE,
		  @V_INITVALUE=T.INIT_VALUE,
		  @V_STEP=T.STEP_VALUE,
		  @CONFIG_TYPE_CODE = T.CONFIG_TYPE_CODE
 FROM CFG_CODE_RULE T
 WHERE T.CODE_RULE_CODE=@V_RULE_CODE
		 AND T.IS_ENABLED='1';
   
  IF @CONFIG_TYPE_CODE <> '0' --0����Զʹ��һ����ǰֵ������֯���޹�
  BEGIN  
		SELECT @V_COUNT=COUNT(1) 
		FROM CFG_CODE_CONFIG_TYPE T
		WHERE T.CONFIG_TYPE_CODE = @CONFIG_TYPE_CODE;       
		/*���ñ������һ���ж�*/
		IF @V_COUNT = 0
		BEGIN
			SET @ERROR_MES='CFG_CODE_CONFIG_TYPE������CONFIG_TYPE_CODEΪ['+@CONFIG_TYPE_CODE+']�������������ݣ�'
			--�׳�����	
			RAISERROR(@ERROR_MES,1,10) WITH NOWAIT
			RETURN;
		END 
  	  --��ȡ��������ֵ
	  SELECT @V_TABLE_NAME = TABLE_NAME ,
			@V_CODE_COLUMN_NAME =CODE_COLUMN_NAME ,
			@V_PK_COLUMN_NAME =PK_COLUMN_NAME ,
			@V_OTHER_CONDITIONE =OTHER_CONDITIONE  
	  FROM CFG_CODE_CONFIG_TYPE T
	  WHERE T.CONFIG_TYPE_CODE = @CONFIG_TYPE_CODE;
	  
	  	    
	  --���ö�̬SQL
	  SET @SQLSTRING = N'SELECT TOP 1 @FORM_CODE_SHORT ='+@V_CODE_COLUMN_NAME+' FROM '
		+@V_TABLE_NAME+' WHERE '+
		@V_PK_COLUMN_NAME+' = @ORG_ID'+ ISNULL(@V_OTHER_CONDITIONE,'');
	  SET @PARMDEFINITION = N'@ORG_ID VARCHAR(50), @FORM_CODE_SHORT VARCHAR(50) OUTPUT';

	  EXECUTE SP_EXECUTESQL @SQLSTRING, @PARMDEFINITION, @ORG_ID = @V_ORG_ID, @FORM_CODE_SHORT=@FORM_CODE_SHORT_OUT OUTPUT;
	  
	  /*����Ҳ��������¼�����״�*/
	  IF(@FORM_CODE_SHORT_OUT IS NULL )
	  BEGIN
		 SET @ERROR_MES=@V_PK_COLUMN_NAME + 'Ϊ['+@V_ORG_ID+']��'+@V_TABLE_NAME+'���¼�����ڻ�Ϊ�գ��������ɵ��ţ�';
		 RAISERROR(@ERROR_MES,1,10) WITH NOWAIT
		 RETURN;
	  END
	  /*�õ���̬�滻����֯����*/
	  SET @V_DLR_CODE = @FORM_CODE_SHORT_OUT;
	  /*���Ҹ������Ƿ���ڸù����ʹ��*/
	  SELECT @ICOUNT=COUNT(1) 
	  FROM CFG_CODE_USE T 
	  WHERE T.CODE_RULE_CODE=@V_RULE_CODE 
			AND T.REL_OBJECT_ID = @V_ORG_ID;
	  /*���ڶ��ʱ���״�*/
	  IF @ICOUNT > 1
	  BEGIN
		 SET @ERROR_MES=@V_DLR_CODE+'��'+@V_RULE_CODE+'��������ֻ��Ϊһ����Ŀǰ���ڶ����'
		 RAISERROR(@ERROR_MES,1,10) WITH NOWAIT
	  END
  END
  ELSE --��������Ϊ0����ֻ��һ����¼
	  BEGIN
		/*���Ҹ������Ƿ���ڸù����ʹ��*/
	  SELECT @ICOUNT=COUNT(1) 
	  FROM CFG_CODE_USE T 
	  WHERE T.CODE_RULE_CODE=@V_RULE_CODE;
	  /*���ڶ��ʱ���״�*/
	  IF @ICOUNT>1
	  BEGIN
		 SET @ERROR_MES=@V_RULE_CODE+'�ĵ�������ֻ��Ϊһ����Ŀǰ���ڶ����'
		 RAISERROR(@ERROR_MES,1,10) WITH NOWAIT
	  END
	  --����֯IDΪ��ʱ��Ĭ��ȫ��ȡ1��Ϊ��ID���ϡ�
	  SET @V_ORG_ID = '1' 
	  SET @V_DLR_CODE = ''
  END
  
  /*������ʱ����*/
  IF @ICOUNT=0 
  BEGIN    
	--����ʹ�ü�¼
	INSERT INTO CFG_CODE_USE /**/
	(
	  CODE_USE_ID,	
	  CODE_RULE_CODE,	
	  CURRENT_VALUE,
	  REL_OBJECT_ID,	
	  --DATE_FORMAT_VALUE,	
	  IS_ENABLED,	
	  IS_SYSTEM,
	  CREATOR_ID,	
	  CREATOR,
	  MODIFIER_ID,	
	  MODIFIER,	
	  UPDATE_CONTROL_ID,	
	  ORG_ID	
	)
	VALUES
	(
	  NEWID(),	/**/
	  @V_RULE_CODE,	/**/
	  0,	/*CURRENT_VALUE*/
	  @V_ORG_ID,
	  --V_CODETIME_FORMAT,	/*DATE_FORMAT_VALUE*/
	  '1',	/*IS_ENABLE*/
	  '1',	/*IS_SYSTEM*/
	  '1',	/*CREATOR_ID*/
	  '1',	/*CREATOR*/
	  '1',	/*MODIFIER_ID*/
	  '1',	/*MODIFIER*/
	  NEWID(),	/*UPDATE_CONTROL_ID*/
	  @V_ORG_ID	/*DLR_CODE*/
	)
   END
   
   --��ס��¼
   UPDATE CFG_CODE_USE WITH ( ROWLOCK )
   SET IS_ENABLED = IS_ENABLED
   WHERE CODE_RULE_CODE=@V_RULE_CODE 
		AND REL_OBJECT_ID = @V_ORG_ID 
   
   /*��ǰ���ڸ�ʽ*/
   SELECT @V_NOW_DATE=CONVERT(VARCHAR(8), GETDATE(), 112)
   
   /*��ǰֵ*/
   SELECT   @V_CURRENT_VALUE  = R.CURRENT_VALUE,
			@V_CURR_FORMAT_VALUE   = R.DATE_FORMAT_VALUE,
			@V_UPDATE_CONTROL_ID   = R.UPDATE_CONTROL_ID
   FROM CFG_CODE_USE R
   WHERE R.CODE_RULE_CODE=@V_RULE_CODE 
		AND R.REL_OBJECT_ID = @V_ORG_ID
   
  /*����ʱ���ʽ�жϵ���ֵ*/
  IF @V_CODETIME_FORMAT IS NULL
	SET @V_NEW_FORMAT_VALUE = '';
  ELSE
	  BEGIN
		--ע����λ���0��ʼ����λ���3��ʼ
		SELECT @V_NEW_FORMAT_VALUE=CASE  UPPER(@V_CODETIME_FORMAT)
		 WHEN 'YYYYMMDD' THEN @V_NOW_DATE
		 WHEN 'YYYYMM' THEN SUBSTRING(@V_NOW_DATE,0,7)
		 WHEN 'YYMMDD' THEN SUBSTRING(@V_NOW_DATE,3,6)
		 WHEN 'YYMM' THEN SUBSTRING(@V_NOW_DATE,3,4)
	  END
  --PRINT @V_NEW_FORMAT_VALUE
  --�����ǰ�����ڸ�ʽֵ�뵱ǰһ�£���V_NEW_INITVALUE +1��������ڳ�ʼ��ֵ
  IF @V_CURR_FORMAT_VALUE IS NULL OR @V_NEW_FORMAT_VALUE <> @V_CURR_FORMAT_VALUE
	  BEGIN
		SET @V_NEW_VALUE = CONVERT(INT,@V_INITVALUE)
	  END
  ELSE
	  BEGIN
		--��ǰֵ���ϵ���ֵ
		SET @V_NEW_VALUE = CONVERT(INT,@V_CURRENT_VALUE) + CONVERT(INT,@V_STEP)
	  END
	  
  
  /*����ˮ�Ÿ�ֵ*/
  SET @I_NEW_VALUE=@V_NEW_VALUE;
  --��������ˮ�Ų��ֵĴ���
  IF CHARINDEX('@',@V_CODE_PRE)>0
     --�Զ������ˮ�Ŵ���:����F_MDS_GET_WORD_SEQUENCE_NO��������ȡ
     SELECT @V_NEW_VALUE = DBO.F_SYS_GET_WORD_SEQUENCE_NO(@I_NEW_VALUE,@V_CODE_LENGTH)
  ELSE
	BEGIN
		/*���ֵ�ж�*/
		IF LEN(@V_NEW_VALUE) > CAST(@V_CODE_LENGTH AS INT) 
		BEGIN
		  RAISERROR('��������ʧ�ܣ������Ѿ�������ǰ�涨���������',1,10) WITH NOWAIT
		  RETURN
	    END
		--���Զ���ˮ�Ĵ�������������ֵǰ����0��
		SET @V_NEW_VALUE = RIGHT(REPLICATE('0',10)+LTRIM(@V_NEW_VALUE),@V_CODE_LENGTH)
    END
  
  PRINT @V_NEW_VALUE
  
  SET @V_FIST_CODE = ''
  IF @V_CODE_PRE IS NOT NULL
  BEGIN
	--���ֻ��*����ȡ�������
	IF @V_CODE_PRE = '*' OR @V_CODE_PRE ='#' OR @V_CODE_PRE = '@'
		/*��*��ֱ��ȡ�������*/
		SET @V_FIST_CODE = @V_DLR_CODE	
	ELSE
		/*����ֱ�ӽ�#���滻Ϊ�������*/
		SET @V_FIST_CODE = REPLACE(@V_CODE_PRE, '#', @V_DLR_CODE)
		SET @V_FIST_CODE = REPLACE(@V_FIST_CODE, '*', @V_DLR_CODE)
		SET @V_FIST_CODE = REPLACE(@V_FIST_CODE, '@', @V_DLR_CODE)
	END
  END
  --PRINT @V_END_CODE 
  /*���쵥��*/
  IF @V_END_CODE IS NOT NULL
	--�����׺��Ϊ��
	SET @V_RETURN_CODE = @V_FIST_CODE + @V_NEW_FORMAT_VALUE  + @V_END_CODE + @V_NEW_VALUE
  ELSE
	BEGIN
	--�����׺Ϊ��
	SET @V_RETURN_CODE = @V_FIST_CODE + @V_NEW_FORMAT_VALUE + @V_NEW_VALUE
	END;
  
  --PRINT @V_RETURN_CODE 
  /*��ѯ����*/
  SET @ICOUNT=0
  SELECT @ICOUNT=COUNT(1)
  FROM CFG_CODE_USE R
  WHERE R.REL_OBJECT_ID = @V_ORG_ID
	 AND R.CODE_RULE_CODE = @V_RULE_CODE
	 AND R.UPDATE_CONTROL_ID = @V_UPDATE_CONTROL_ID;
  
  IF @ICOUNT = 0
	BEGIN
	 SET @ERROR_MES='�������ɹ�����ִ��󣬸õ��ݺ����ݱ������������£�'
	 RAISERROR(@ERROR_MES,1,10) WITH NOWAIT
	END
  ELSE
	BEGIN
		/*�޸���������͵�ǰֵ*/
		UPDATE CFG_CODE_USE
		SET CURRENT_VALUE     = @I_NEW_VALUE,
			DATE_FORMAT_VALUE = CASE WHEN CURRENT_VALUE IS
											NULL THEN NULL ELSE @V_NEW_FORMAT_VALUE END,
			   UPDATE_CONTROL_ID = NEWID()
		 WHERE REL_OBJECT_ID = @V_ORG_ID
		   AND CODE_RULE_CODE = @V_RULE_CODE
		   AND UPDATE_CONTROL_ID = @V_UPDATE_CONTROL_ID
	 END
  --����ֵ���ò�ѯSQLʵ��
  SELECT @V_RETURN_CODE	 
END
GO

ALTER PROCEDURE [dbo].[P_COMM_GET_FORM_CODE]
(
       @V_ORG_ID VARCHAR(50),     --��֯ID
       @V_RULE_CODE VARCHAR(50),   --�������
       @V_RETURN_CODE VARCHAR(50) OUTPUT
)
AS 
/*******************************************************************************************
* �洢�������ƣ����ɵ���
* ���ߣ�       �ƹ���
* �������ڣ�   2013-12-15
* ��Ҫ����:  
*       ���õ��Ź����BAS_CODE_RULE��:�������������ɵ��ţ�������֯����һ�׹���
*       ����ʹ����Ϣ��BAS_CODE_USE�������ÿһ�����ͣ�һ����֯һ�����ݡ�
* �޸���ʷ��
*     V1.00 �����������ɹ���洢���� HGH 2013-11-01
*	  V1.01 �޸ģ������������͵�֧�֣�ʵ���������Ϊ��ֵ̬��
*	  V1.02 �޸ģ���������������Ϊ0ʱ���޷������������⡣hgh 2016-7-4
********************************************************************************************/
BEGIN
	DECLARE @V_CODE_PRE        VARCHAR(50), --��ʼ����ֵ
		@V_CODETIME_FORMAT    VARCHAR(50) --ʱ���ʽ
    /*�˴�ʡ��...*/
END
GO
