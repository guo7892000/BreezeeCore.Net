CREATE OR REPLACE FUNCTION F_MDS_GET_WORD_SEQUENCE_NO
(
  V_SEQUENCE_NO INTEGER, ---��1���ʮ������ˮ�ţ����磺12
  V_SEQUENCE_LENGTH INTEGER --��ˮ�ų��ȣ�����4
)
/*******************************************************************************************
* �������ƣ���ȡ����ĸ����ˮ��
* �������ߣ��ƹ���
* �������ڣ�2014-11-12
* ��������: ������ˮ�ż���ˮ�ų��ȣ����ؿ��ܰ�����ĸ����ˮ�š�
*       ��������ˮ�ű�ԭ��ֻ�����ֵĻ��һЩ��
* �����ʷ(��ʽ���汾��\���α�����ݼ���\�޸���\�޸�����)��
*     2014-11-12������ HGH
*     2015-08-03����ȫ��д�˸��㷨�����֮ǰ���ظ����ŵ����� HUANGGH 
********************************************************************************************/
RETURN VARCHAR2
IS
	--�Զ�����ˮ�ŵ���ر���
	V_MY_DEFINE_VALUE  VARCHAR2(50); --�Զ������ˮ��
	V_MY_DEFINE_WORD_LIST VARCHAR2(50):= '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'; --�Զ����ַ���
	N_MY_DEFINE_WORD_LIST_LEN NUMBER;  --�Զ����ַ�������

	I_AFTER_DIVIDE_VALUE INTEGER:=0; --�����ֵ
	N_MOD_VALUE           NUMBER; --����ֵ
	V_LOOP_NUM            NUMBER; --ѭ���е�ֵ
BEGIN
	--��Ч���ж�
	IF V_SEQUENCE_NO IS NULL OR V_SEQUENCE_LENGTH IS NULL THEN
	RAISE_APPLICATION_ERROR(-20999,'�������ˮ�š���ˮ�ų��ȶ�����Ϊ�գ�');
	END IF;

	IF V_SEQUENCE_LENGTH <=0 THEN
	RAISE_APPLICATION_ERROR(-20999,'�������ˮ�ų��ȱ������0��');
	END IF;

	--������ʼ��
	N_MY_DEFINE_WORD_LIST_LEN := LENGTH(V_MY_DEFINE_WORD_LIST);--�Զ����ݼ�����
	V_MY_DEFINE_VALUE:=''; --���ص�ֵ
	I_AFTER_DIVIDE_VALUE:=V_SEQUENCE_NO; --ֵΪ�������ˮ��

	--ѭ��ת��Ϊ��ˮ��
	FOR V_LOOP_NUM IN 1 .. V_SEQUENCE_LENGTH LOOP
		--�õ�����ֵ������ȡ�Զ����ַ����еڼ����ַ�
		N_MOD_VALUE := MOD(I_AFTER_DIVIDE_VALUE,N_MY_DEFINE_WORD_LIST_LEN);
		--�õ������ֵ
		I_AFTER_DIVIDE_VALUE :=FLOOR(I_AFTER_DIVIDE_VALUE/N_MY_DEFINE_WORD_LIST_LEN);--����Ҫ����ȡ��
		--ƴ�ӣ���Ϊ�ַ���һ���ַ�Ϊ0��������Ե�һ����ˮ�Ż�����ȡһλ�����⣺���ȵõ������������
		V_MY_DEFINE_VALUE:=  SUBSTR(V_MY_DEFINE_WORD_LIST,N_MOD_VALUE + 1, 1) || V_MY_DEFINE_VALUE;
		IF I_AFTER_DIVIDE_VALUE < N_MY_DEFINE_WORD_LIST_LEN THEN --������С�ڽ�����ʱ�˳�
				--�õ�ǰһλ
				V_MY_DEFINE_VALUE:=  SUBSTR(V_MY_DEFINE_WORD_LIST,I_AFTER_DIVIDE_VALUE + 1, 1) || V_MY_DEFINE_VALUE;
				EXIT; --�˳�ѭ��
		END IF;
	END LOOP;
	/*��λ������ʱ����߲�0*/
	V_MY_DEFINE_VALUE :=LPAD(V_MY_DEFINE_VALUE,V_SEQUENCE_LENGTH,'0');	
	/*��������ȡ������ʵ����ˮ��λ��������ˮ�ų��ȣ���ô���ص�λ���Ͷ��ˣ����������������������ؽ�ȡ���ֵ���ˮ���ǻ��ظ��ģ����������ȡ������Ҳ���󣡣�*/
    V_MY_DEFINE_VALUE := SUBSTR(V_MY_DEFINE_VALUE,LENGTH(V_MY_DEFINE_VALUE)-V_SEQUENCE_LENGTH,V_SEQUENCE_LENGTH);

	--����ֵ
	RETURN(V_MY_DEFINE_VALUE);
END;
/
