/*ע��Oracle�Ķ����������ȣ�С�ڵ���30*/

/* ������ֵʹ��:=  */
/* ִ�з���Ϊ/  */

/*��������*/
V_FACT_STORE NUMBER(20,6) := 0; --����V_FACT_STORE��С�����ͣ�С��λ��Ϊ6λ��Ĭ��ֵΪ0
V_DLR_STORAGE_ID VARCHAR2(50); --����V_DLR_STORAGE_ID���ַ����ͣ�����Ϊ50
V_COUNT NUMBER; --����V_COUNT������
MyException EXCEPTION; --����MyException���쳣����
V_PUR_ASSIGN_TYPE T_GROUP_D.PUR_ASSIGN_TYPE%TYPE; /*����V_PUR_ASSIGN_TYPE����T_GROUP_D���PUR_ASSIGN_TYPE�����ͼ�����һ��*/
/*�α�����*/
CURSOR CUR_DEAL_LIST IS SELECT A.*  FROM T_TMP_COSTW A;
/*�α��б���*/
REC_DEAL_LIST CUR_DEAL_LIST%ROWTYPE;  --����REC_DEAL_LIST���α�CUR_DEAL_LIST��������

/*IF�ж����*/
IF v_gender = '��' AND v_age >= 30 THEN
	dbms_output.put_line('������������ڵ���30��');
ELSIF v_gender = '��' AND v_age < 30
	dbms_output.put_line('����������С��30��');
ELSIF v_gender = 'Ů' AND v_age >= 30 THEN
	dbms_output.put_line('��Ů��������ڵ���30��');
ELSE
	dbms_output.put_line('��Ů������С��30��');
END IF;

/*DECODE���൱��if-then-else�߼�*/
DECODE(value,if1,then1,if2,then2,if3,then3,...,else);

