/*ɾ������*/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_SYS_GET_WORD_SEQUENCE_NO]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[F_SYS_GET_WORD_SEQUENCE_NO]
GO

/*��������*/
CREATE FUNCTION [dbo].[F_SYS_GET_WORD_SEQUENCE_NO]
(
  @V_SEQUENCE_NO INT, ---��1���ʮ������ˮ�ţ����磺10
  @V_SEQUENCE_LENGTH INT --��ˮ�ų��ȣ�����4
)
/*******************************************************************************************
* �������ƣ���ȡ����ĸ����ˮ��
* �������ߣ��ƹ���
* �������ڣ�2014-11-12
* ��������:  ������ˮ�ż���ˮ�ų��ȣ����ؿ��ܰ�����ĸ����ˮ�š�
*       ��������ˮ�ű�ԭ��ֻ�����ֵĻ��һЩ��
* �����ʷ(��ʽ���汾��\���α�����ݼ���\�޸���\�޸�����)��
*     V1.00������ HGH 2015-10-2
********************************************************************************************/
RETURNS VARCHAR(50)
AS
BEGIN
  --�Զ�����ˮ�ŵ���ر���
  DECLARE @V_MY_DEFINE_VALUE VARCHAR(50), --�Զ������ˮ��
  @V_MY_DEFINE_WORD_LIST VARCHAR(50), --�Զ����ַ���
  @N_MY_DEFINE_WORD_LIST_LEN INT,  --�Զ����ַ�������

  @I_AFTER_DIVIDE_VALUE INT, --�����ֵ
  @N_MOD_VALUE           INT, --����ֵ
  @V_LOOP_NUM            INT --ѭ���е�ֵ
  
  --��Ч���ж�
  IF @V_SEQUENCE_NO IS NULL OR @V_SEQUENCE_LENGTH IS NULL
  BEGIN
    --�������ˮ�š���ˮ�ų��ȶ�����Ϊ��
    RETURN ''
  END

  IF @V_SEQUENCE_LENGTH <=0 
  BEGIN
    --�������ˮ�ų��ȱ������0
    RETURN ''
  END

  --������ʼ��
  SET @V_MY_DEFINE_WORD_LIST = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'
  SET @I_AFTER_DIVIDE_VALUE = 0
  SET @N_MY_DEFINE_WORD_LIST_LEN = LEN(@V_MY_DEFINE_WORD_LIST)--�Զ����ݼ�����
  SET @V_MY_DEFINE_VALUE = '' --���ص�ֵ
  SET @I_AFTER_DIVIDE_VALUE = @V_SEQUENCE_NO --ֵΪ�������ˮ��
  
  SET @V_LOOP_NUM = 1
  --ѭ��ת��Ϊ��ˮ��
  WHILE @V_LOOP_NUM <= @V_SEQUENCE_LENGTH
  BEGIN
	  --�õ�����ֵ
		SET @N_MOD_VALUE = @I_AFTER_DIVIDE_VALUE%@N_MY_DEFINE_WORD_LIST_LEN
		--�õ������ֵ
		SET @I_AFTER_DIVIDE_VALUE = FLOOR(@I_AFTER_DIVIDE_VALUE/@N_MY_DEFINE_WORD_LIST_LEN) --����Ҫ����ȡ��
		--����
		SET @V_MY_DEFINE_VALUE =  SUBSTRING(@V_MY_DEFINE_WORD_LIST,@N_MOD_VALUE + 1, 1) + @V_MY_DEFINE_VALUE;
		IF @I_AFTER_DIVIDE_VALUE < @N_MY_DEFINE_WORD_LIST_LEN  --������С�ڽ�����ʱ�˳�
		BEGIN
			 --�õ�ǰһλ
			 SET @V_MY_DEFINE_VALUE =  SUBSTRING(@V_MY_DEFINE_WORD_LIST,@I_AFTER_DIVIDE_VALUE + 1, 1) + @V_MY_DEFINE_VALUE
			 BREAK; --�˳�ѭ��
		END
		SET @V_LOOP_NUM = @V_LOOP_NUM + 1
	END 
  
  SET @V_MY_DEFINE_VALUE = RIGHT(REPLICATE('0',10)+LTRIM(@V_MY_DEFINE_VALUE),@V_SEQUENCE_LENGTH)
  SET @V_MY_DEFINE_VALUE = SUBSTRING(@V_MY_DEFINE_VALUE,LEN(@V_MY_DEFINE_VALUE)-@V_SEQUENCE_LENGTH,@V_SEQUENCE_LENGTH+1)

  --����ֵ
  RETURN @V_MY_DEFINE_VALUE
END
GO

/*�޸ĺ���*/
ALTER FUNCTION [dbo].[F_SYS_GET_WORD_SEQUENCE_NO]
(
  @V_SEQUENCE_NO INT, ---��1���ʮ������ˮ�ţ����磺10
  @V_SEQUENCE_LENGTH INT --��ˮ�ų��ȣ�����4
)
/*******************************************************************************************
* �������ƣ���ȡ����ĸ����ˮ��
* �������ߣ��ƹ���
* �������ڣ�2014-11-12
* ��������:  ������ˮ�ż���ˮ�ų��ȣ����ؿ��ܰ�����ĸ����ˮ�š�
*       ��������ˮ�ű�ԭ��ֻ�����ֵĻ��һЩ��
* �����ʷ(��ʽ���汾��\���α�����ݼ���\�޸���\�޸�����)��
*     V1.00������ HGH 2015-10-2
********************************************************************************************/
RETURNS VARCHAR(50)
AS
BEGIN
  --�Զ�����ˮ�ŵ���ر���
  DECLARE @V_MY_DEFINE_VALUE VARCHAR(50), --�Զ������ˮ��
  @V_MY_DEFINE_WORD_LIST VARCHAR(50) --�Զ����ַ���
  /*�˴�ʡ��...*/

  --����ֵ
  RETURN @V_MY_DEFINE_VALUE
END
GO