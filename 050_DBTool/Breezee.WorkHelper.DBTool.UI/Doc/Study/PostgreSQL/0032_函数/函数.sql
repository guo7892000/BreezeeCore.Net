CREATE OR REPLACE  FUNCTION F_SYS_GET_WORD_SEQUENCE_NO
(
  V_SEQUENCE_NO INT, /**��1���ʮ������ˮ�ţ����磺10**/
  V_SEQUENCE_LENGTH INT /**��ˮ�ų��ȣ�����4**/
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
RETURNS VARCHAR(50) AS $$
  /**�Զ�����ˮ�ŵ���ر���**/
DECLARE 
  V_MY_DEFINE_VALUE VARCHAR(50); /**�Զ������ˮ��**/
  V_MY_DEFINE_WORD_LIST VARCHAR(50); /**�Զ����ַ���**/
  N_MY_DEFINE_WORD_LIST_LEN INT;  /**�Զ����ַ�������**/

  I_AFTER_DIVIDE_VALUE INT; /**�����ֵ**/
  N_MOD_VALUE           INT; /**����ֵ**/
  V_LOOP_NUM            INT; /**ѭ���е�ֵ**/
BEGIN  
  /**��Ч���ж�**/
  IF V_SEQUENCE_NO IS NULL OR V_SEQUENCE_LENGTH IS NULL THEN
    /**�������ˮ�š���ˮ�ų��ȶ�����Ϊ��**/
    RETURN '';
  END IF;

  IF V_SEQUENCE_LENGTH <=0 THEN
    /**�������ˮ�ų��ȱ������0**/
    RETURN '';
  END IF;

  /**������ʼ��**/
  V_MY_DEFINE_WORD_LIST := '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  I_AFTER_DIVIDE_VALUE := 0;
  N_MY_DEFINE_WORD_LIST_LEN := LEN(V_MY_DEFINE_WORD_LIST); /**�Զ����ݼ�����**/
  V_MY_DEFINE_VALUE := ''; /**���ص�ֵ**/
  I_AFTER_DIVIDE_VALUE := V_SEQUENCE_NO; /**ֵΪ�������ˮ��**/
  
  V_LOOP_NUM := 1;
  /**ѭ��ת��Ϊ��ˮ��**/
  WHILE V_LOOP_NUM <= V_SEQUENCE_LENGTH LOOP
	  /**�õ�����ֵ**/
		N_MOD_VALUE := I_AFTER_DIVIDE_VALUE%N_MY_DEFINE_WORD_LIST_LEN;
		/**�õ������ֵ**/
		I_AFTER_DIVIDE_VALUE := FLOOR(I_AFTER_DIVIDE_VALUE/N_MY_DEFINE_WORD_LIST_LEN); /**����Ҫ����ȡ��**/
		/**����**/
		V_MY_DEFINE_VALUE :=  SUBSTRING(V_MY_DEFINE_WORD_LIST,N_MOD_VALUE + 1, 1) + V_MY_DEFINE_VALUE;
		IF I_AFTER_DIVIDE_VALUE < N_MY_DEFINE_WORD_LIST_LEN THEN /**������С�ڽ�����ʱ�˳�**/
			 /**�õ�ǰһλ**/
			 V_MY_DEFINE_VALUE :=  SUBSTRING(V_MY_DEFINE_WORD_LIST,I_AFTER_DIVIDE_VALUE + 1, 1) + V_MY_DEFINE_VALUE;
			 EXIT; /**�˳�ѭ��**/
		END IF;
		V_LOOP_NUM := V_LOOP_NUM + 1;
	END LOOP;
  
  V_MY_DEFINE_VALUE := RIGHT(REPLICATE('0',10)+LTRIM(V_MY_DEFINE_VALUE),V_SEQUENCE_LENGTH);
  V_MY_DEFINE_VALUE := SUBSTRING(V_MY_DEFINE_VALUE,LEN(V_MY_DEFINE_VALUE)-V_SEQUENCE_LENGTH,V_SEQUENCE_LENGTH+1);

  /**����ֵ**/
  RETURN V_MY_DEFINE_VALUE;
END;
$$
language 'plpgsql';

