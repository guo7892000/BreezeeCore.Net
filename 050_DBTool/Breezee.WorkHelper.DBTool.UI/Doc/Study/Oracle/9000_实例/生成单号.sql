CREATE OR REPLACE PROCEDURE P_COMM_GET_FORM_CODE
(
       V_ORG_ID      VARCHAR2,     --��֯ID(�ɿգ���Բ�ͬ���ݹ������ʹ���ͬ�Ĺ���ID��
       V_RULE_CODE VARCHAR2,     --�������
       V_RETURN_CODE    OUT VARCHAR2  --���ص���
)
/*******************************************************************************************
* �洢�������ƣ����ɵ���
* ���ߣ�       �ƹ���
* �������ڣ�   2013-12-15
* ��Ҫ����:
*       ���õ��Ź����BAS_CODE_RULE��:�������������ɵ��ţ�������֯����һ�׹���
*       ����ʹ����Ϣ��BAS_CODE_USE�������ÿһ�����ͣ�һ����֯һ�����ݡ�
* �޸���ʷ��
*   2013-11-01 �����������ɹ���洢���� HGH 
*
********************************************************************************************/
IS
  /*���Ź����*/
  V_CODE_STRING_BEGIN  CFG_CODE_RULE.PRE_CODE%TYPE; --ǰ׺����ֵ
  V_CODETIME_FORMAT    CFG_CODE_RULE.DATE_FORMAT%TYPE; --ʱ���ʽ
  V_CODE_LENGTH        CFG_CODE_RULE.AUTO_CODE_LENGH%TYPE; --��ˮ�ų���
  V_RESUME_PERIOD      CFG_CODE_RULE.RESUME_TYPE%TYPE; --�ݹ����ڸ�ʽ����ǰ��ʹ��
  V_INIT_VALUE         CFG_CODE_RULE.INIT_VALUE%TYPE; --��ʼ��ֵ
  V_STEP               CFG_CODE_RULE.STEP_VALUE%TYPE; --����ֵ
  V_CODE_STRING_END    CFG_CODE_RULE.END_CODE%TYPE; --��׺����ֵ
  V_ORDER_CODE_RULE_ID CFG_CODE_RULE.CODE_RULE_ID%TYPE; --���Ź���ID
  V_CONFIG_TYPE_CODE   CFG_CODE_RULE.CONFIG_TYPE_CODE%TYPE; --���ݹ������ͱ���
 /*����ʹ����Ϣ��*/
  V_CURRENT_VALUE      CFG_CODE_USE.CURRENT_VALUE%TYPE; --��ǰֵ
  V_UPDATE_CONTROL_ID  CFG_CODE_USE.UPDATE_CONTROL_ID%TYPE; --��������ID
  /*���ݹ������ͱ�*/
  V_CONFIG_TYPE_NAME   CFG_CODE_CONFIG_TYPE.CONFIG_TYPE_NAME%TYPE; --������������
  V_TABLE_NAME         CFG_CODE_CONFIG_TYPE.TABLE_NAME%TYPE; --����
  V_CODE_COLUMN_NAME   CFG_CODE_CONFIG_TYPE.CODE_COLUMN_NAME%TYPE; --����
  V_PK_COLUMN_NAME     CFG_CODE_CONFIG_TYPE.PK_COLUMN_NAME%TYPE; --��������
  V_OTHER_CONDITION    CFG_CODE_CONFIG_TYPE.OTHER_CONDITIONE%TYPE; --��������
  /*����*/
  V_CURR_FORMAT_VALUE VARCHAR2(20); --�ϴε�ǰ
  V_COUNT             NUMBER; --����ֵ
  V_NEW_FORMAT_VALUE VARCHAR2(20); --�����ֵ
  V_SOURCE_ID        VARCHAR2(50); --ԴID���Ե��ݹ������ͱ��룺0ΪORDER_CODE_RULE_ID,1ΪDLR_ID

  V_FIST_CODE        VARCHAR2(50);
  V_DLR_CODE         VARCHAR2(50);        /*�������*/
  V_NEW_VALUE        VARCHAR2(50);/*��ˮ���ַ�*/
  I_NEW_VALUE        INTEGER; /*��ˮ�ŵ�����ֵ*/
  ICOUNT             INTEGER; /*��¼��*/
  --V_INT              NUMBER;
  --V_LENGTH           NUMBER;
  --VALUE_NAME         VARCHAR2(100);
  V_TEMP_SQL         VARCHAR2(2000);

  /*���ݹ���ʹ�ñ��¼�α꣺ע��������¼*/
  CURSOR CUR_IF_TAB IS
    SELECT *
      FROM CFG_CODE_USE R
     WHERE R.CODE_RULE_CODE = V_RULE_CODE
       AND R.ORG_ID = V_SOURCE_ID
     FOR UPDATE;
  /*�α����*/
  REC_IF_TAB CUR_IF_TAB%ROWTYPE;
BEGIN
  /*1����ʼ������*/
  V_COUNT     := 0;
  V_RETURN_CODE := '';

  /*2������Ψһ���ж�*/
  /*��ѯ�õ������͵����ü�¼��*/
  SELECT COUNT(1) INTO V_COUNT
   FROM CFG_CODE_RULE T
   WHERE T.CODE_RULE_CODE = V_RULE_CODE
         AND T.IS_ENABLED = '1';
  /*���ñ������һ���ж�*/
  IF V_COUNT <> 1 THEN
    RAISE_APPLICATION_ERROR(-20999,'��������Ч�ĸõ�������,���߸õ��������ж����������ݣ�');
  END IF;

  /*3��ȡ�������еĶ������*/
  /*��ȡ����������Ϣ*/
  SELECT  T.CODE_RULE_ID,
          T.PRE_CODE,
          NVL(T.DATE_FORMAT,'0'), /*����û��ʱ���ʽ�ģ���һ��Ĭ��ֵ0����������޸�*/
          T.AUTO_CODE_LENGH,
          T.RESUME_TYPE,
          T.INIT_VALUE,
          T.STEP_VALUE,
          T.END_CODE,
          T.CONFIG_TYPE_CODE
     INTO V_ORDER_CODE_RULE_ID,
          V_CODE_STRING_BEGIN,
          V_CODETIME_FORMAT,
          V_CODE_LENGTH,
          V_RESUME_PERIOD,
          V_INIT_VALUE,
          V_STEP,
          V_CODE_STRING_END,
          V_CONFIG_TYPE_CODE
 FROM CFG_CODE_RULE T
 WHERE T.CODE_RULE_CODE = V_RULE_CODE
         AND T.IS_ENABLED = '1';
  /*���ݲ�ͬ���ݹ������ͱ�������ͬ����*/
  --V_COUNT := 0;
  IF(V_CONFIG_TYPE_CODE IS NULL OR V_CONFIG_TYPE_CODE=0) THEN
    /*3.1 û�����ã���ȡ����IDΪԴID*/
    V_SOURCE_ID:=V_ORDER_CODE_RULE_ID;
  ELSE
    /*3.2 �����ã���ѯ�������ͱ����Ƿ����*/
    SELECT COUNT(1) INTO V_COUNT
     FROM CFG_CODE_CONFIG_TYPE T
     WHERE T.CONFIG_TYPE_CODE = V_CONFIG_TYPE_CODE;
    /*�жϼ�¼�Ƿ����*/
    IF V_COUNT=0 THEN
      /*�����ڵ����ͣ��״�*/
       RAISE_APPLICATION_ERROR(-20999,'Ŀǰ��֧�ָù������ͣ�'||V_CONFIG_TYPE_CODE||'��');
    END IF;
    /*ȡ����������*/
    SELECT T.TABLE_NAME,
       T.CODE_COLUMN_NAME,
       T.PK_COLUMN_NAME,
       T.OTHER_CONDITIONE,
       T.CONFIG_TYPE_NAME
    INTO V_TABLE_NAME,
       V_CODE_COLUMN_NAME,
       V_PK_COLUMN_NAME,
       V_OTHER_CONDITION,
       V_CONFIG_TYPE_NAME
    FROM CFG_CODE_CONFIG_TYPE T
   WHERE T.CONFIG_TYPE_CODE = V_CONFIG_TYPE_CODE;
    /*ִ�ж�̬SQL��ѯ���*/
    /*���ݴ���ֵ��ѯ�Ƿ��м�¼*/
    V_TEMP_SQL:='SELECT COUNT(1) FROM '|| V_TABLE_NAME
    ||' WHERE '|| V_PK_COLUMN_NAME ||'='''||V_ORG_ID||'''';
    IF(V_OTHER_CONDITION IS NOT NULL) THEN
      V_TEMP_SQL:=V_TEMP_SQL||' AND '||V_OTHER_CONDITION;
    END IF;
    EXECUTE IMMEDIATE V_TEMP_SQL INTO V_COUNT;
    /*ԴID�ͱ���ǿ��ж�*/
    IF (V_COUNT = 0) THEN
       RAISE_APPLICATION_ERROR(-20999,V_PK_COLUMN_NAME||'Ϊ'||V_ORG_ID||'��'||V_CONFIG_TYPE_NAME||'('||V_TABLE_NAME||')���¼�����ڣ�');
    END IF;
    /*�м�¼�����ѯ����*/
    V_TEMP_SQL:='SELECT ' || V_CODE_COLUMN_NAME || ','||V_PK_COLUMN_NAME||' FROM '|| V_TABLE_NAME
    ||' WHERE '|| V_PK_COLUMN_NAME ||'='''||V_ORG_ID||'''';
    IF(V_OTHER_CONDITION IS NOT NULL) THEN
      V_TEMP_SQL:=V_TEMP_SQL||' AND '||V_OTHER_CONDITION;
    END IF;
    EXECUTE IMMEDIATE V_TEMP_SQL INTO V_DLR_CODE,V_SOURCE_ID;
    /*����ǿ��ж�*/
    IF (V_DLR_CODE IS NULL) THEN
       RAISE_APPLICATION_ERROR(-20999,V_PK_COLUMN_NAME||'Ϊ'||V_SOURCE_ID||'��'||V_CONFIG_TYPE_NAME||'('||V_TABLE_NAME||')���¼��'||V_CODE_COLUMN_NAME||'Ϊ�գ���������');
    END IF;
  END IF;

  /*4����ȡ���õĵ�ǰʹ��ֵ*/
  /*���Ҹ������Ƿ���ڸù����ʹ��*/
  SELECT COUNT(1) INTO ICOUNT
  FROM CFG_CODE_USE T
  WHERE T.CODE_RULE_CODE = V_RULE_CODE
        AND T.ORG_ID = V_SOURCE_ID;
  /*���ڶ��ʱ���״�*/
  IF ICOUNT>1 THEN
     RAISE_APPLICATION_ERROR(-20999,'רӪ��Ը����͵�ʹ������ֻ��Ϊһ����Ŀǰ���ڶ����');
  END IF;
  /*������ʱ����*/
  IF ICOUNT=0 THEN
    INSERT INTO CFG_CODE_USE  /**/
    (
      CODE_USE_ID,
      CODE_RULE_CODE,
      CURRENT_VALUE,
      DATE_FORMAT_VALUE,
      IS_ENABLED,
      IS_SYSTEM,
      CREATOR,
      MODIFIER,
      UPDATE_CONTROL_ID,
      ORG_ID
    )
    VALUES
    (
      SYS_GUID(),  /*ORDER_CODE_USE_ID*/
      V_RULE_CODE,  /*ORDER_CODE_RULE_ID*/
      0,  /*CURRENT_VALUE*/
      V_CODETIME_FORMAT,  /*CURR_FORMAT_VALUE*/
      '1',  /*IS_ENABLE*/
      '1',  /*IS_SYSTEM*/
      '1',  /*CREATOR*/
      '1',  /*MODIFIER*/
      SYS_GUID(),  /*UPDATE_CONTROL_ID*/
      V_SOURCE_ID  /*��������ID*/
    );
    END IF;
  /*���α꣬ȡ��ֵ*/
  OPEN CUR_IF_TAB;
  LOOP
    FETCH CUR_IF_TAB
      INTO REC_IF_TAB; --��ȡ��Ҫ������·���Ϣ���α����
    EXIT WHEN CUR_IF_TAB%NOTFOUND;
    BEGIN
      V_CURRENT_VALUE      := REC_IF_TAB.CURRENT_VALUE;
      V_CURR_FORMAT_VALUE  := REC_IF_TAB.DATE_FORMAT_VALUE;
      V_UPDATE_CONTROL_ID  := REC_IF_TAB.UPDATE_CONTROL_ID;
    END;
  END LOOP;
  CLOSE CUR_IF_TAB;

  /*5���������õõ��µ���*/
  /*����ʱ���ʽ�жϵ���ֵ*/
  IF (NVL(V_CODETIME_FORMAT,'0')='0') THEN
    V_NEW_FORMAT_VALUE := '';
  ELSE
    V_NEW_FORMAT_VALUE := TO_CHAR(SYSDATE, V_CODETIME_FORMAT);
  END IF;

  --�����ǰ�����ڸ�ʽֵ�뵱ǰһ�£���V_NEW_INITVALUE +1��������ڳ�ʼ��ֵ
  IF NVL(V_NEW_FORMAT_VALUE,'0') != V_CURR_FORMAT_VALUE THEN
    IF V_INIT_VALUE = 0 THEN
       V_INIT_VALUE :=1; --Ĭ�ϴ�1��ʼ
    END IF;
    V_NEW_VALUE := TO_NUMBER(V_INIT_VALUE);
  ELSE
      /*�����ǰֵ�����е�ֵС�����״���ֹ���ݿ������ʱ�䲻�Ե��µ��Ŵ���*/
      IF(NVL(V_NEW_FORMAT_VALUE,'0')<V_CURR_FORMAT_VALUE) THEN
        RAISE_APPLICATION_ERROR(-20999,'�������ɵ�ʱ���ʽ����С���ϴ�����ֵ��');
      END IF;
      --��ǰֵ���ϵ���ֵ
      V_NEW_VALUE := TO_NUMBER(V_CURRENT_VALUE) + TO_NUMBER(V_STEP);
  END IF;

  /*����ˮ�Ÿ�ֵ*/
  I_NEW_VALUE:=V_NEW_VALUE;
  --��������ˮ�Ų��ֵĴ���
  SELECT INSTR(V_CODE_STRING_BEGIN,'@') INTO ICOUNT FROM��DUAL;
  IF ICOUNT>0 THEN
    --�Զ������ˮ�Ŵ���:����F_MDS_GET_WORD_SEQUENCE_NO��������ȡ
    V_NEW_VALUE:= F_SYS_GET_WORD_SEQUENCE_NO(I_NEW_VALUE,V_CODE_LENGTH);
  ELSE
    /*���ֵ�ж�*/
    IF LENGTH(TO_CHAR(V_NEW_VALUE)) > TO_NUMBER(V_CODE_LENGTH) THEN
      RAISE_APPLICATION_ERROR(-20999,'��������ʧ�ܣ������Ѿ�������ǰ�涨���������');
    END IF;
    --���Զ���ˮ�Ĵ�������������ֵǰ����0��
     V_NEW_VALUE := LPAD(V_NEW_VALUE, V_CODE_LENGTH, '0');
  END IF;

  --���滻�ַ��Ĵ���
  V_FIST_CODE := '';
  IF V_CODE_STRING_BEGIN IS NOT NULL THEN
    --���ֻ��*����ȡ�������
    CASE V_CODE_STRING_BEGIN
      WHEN '*' THEN
        /*��*��ֱ��ȡ�������*/
        V_FIST_CODE := V_DLR_CODE;
      WHEN '#' THEN
        /*#��ת��ΪASCII��*/
        /*V_LENGTH :=LENGTH(V_DLR_CODE);
        V_INT:=0;
        FOR V_INT   IN 1..V_LENGTH LOOP
            VALUE_NAME :=VALUE_NAME||ASCII(SUBSTR(V_DLR_CODE,V_INT,1));
        END LOOP;
        V_FIST_CODE := VALUE_NAME;*/
        V_FIST_CODE := V_DLR_CODE;
      WHEN '@' THEN
        /*@��Ϊ��ϵͳ����ȡǰ׺����Ŀǰϵͳ���ñ�û��������룬�Ժ����޸�ʵ��*/
        /*SELECT MAX(V.PARAMETER_VALUE) INTO VALUE_NAME
        FROM     T_DB_SYS_INFO V
        WHERE V. = V_DLR_CODE
              AND V.PARAMETER_CODE = 'SYS_COMM_0000';
        V_FIST_CODE := VALUE_NAME;*/
        V_FIST_CODE := V_DLR_CODE;
      ELSE
        /*����ֱ�ӽ�#��*��@���滻Ϊ�������*/
        V_FIST_CODE := REPLACE(V_CODE_STRING_BEGIN, '#', V_DLR_CODE);
        V_FIST_CODE := REPLACE(V_FIST_CODE, '*', V_DLR_CODE);
        V_FIST_CODE := REPLACE(V_FIST_CODE, '@', V_DLR_CODE);
    END CASE;
  END IF;

  /*���쵥��*/
  IF V_CODE_STRING_END IS NOT NULL THEN
    --�����׺��Ϊ��
    V_RETURN_CODE := V_FIST_CODE || V_NEW_FORMAT_VALUE || V_CODE_STRING_END || TO_CHAR(V_NEW_VALUE);
  ELSE
    --�����׺Ϊ��
    V_RETURN_CODE := V_FIST_CODE || V_NEW_FORMAT_VALUE || TO_CHAR(V_NEW_VALUE);
  END IF;

  /*6������ʹ�����ñ�*/
  /*��ѯ����*/
  ICOUNT:=0;
  SELECT COUNT(*) INTO ICOUNT
  FROM CFG_CODE_USE R
  WHERE R.ORG_ID = V_SOURCE_ID
     AND R.CODE_RULE_CODE = V_RULE_CODE
     AND R.UPDATE_CONTROL_ID = V_UPDATE_CONTROL_ID;
  IF ICOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20999,'�������ɹ�����ִ��󣬸õ��ݺ����ݱ������������£�');
  ELSE
    /*�޸���������͵�ǰֵ*/
    UPDATE CFG_CODE_USE R
       SET R.CURRENT_VALUE     = I_NEW_VALUE,
           R.DATE_FORMAT_VALUE = DECODE(R.DATE_FORMAT_VALUE,
                                        NULL,
                                        NULL,
                                        NVL(V_NEW_FORMAT_VALUE,'0')),
           R.UPDATE_CONTROL_ID = SYS_GUID()
     WHERE R.ORG_ID = V_SOURCE_ID
       AND R.CODE_RULE_CODE = V_RULE_CODE
       AND R.UPDATE_CONTROL_ID = V_UPDATE_CONTROL_ID;

     if SQL%ROWCOUNT = 0 then
       RAISE_APPLICATION_ERROR(-20999,'�������Ų����쳣��');
      end if;
  END IF;
  EXCEPTION
    /*�쳣����*/
    WHEN OTHERS THEN
      RAISE_APPLICATION_ERROR(-20999,SQLERRM);
END;
/
