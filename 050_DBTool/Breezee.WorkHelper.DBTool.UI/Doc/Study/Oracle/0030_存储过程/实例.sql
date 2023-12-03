CREATE OR REPLACE PROCEDURE P_XX_NAME
(
       V_ORDER_CODE   IN VARCHAR2, --������������ݱ��
       V_ORDER_TYPE   IN VARCHAR2, --�����������������
       V_RET          OUT NUMBER   --���������0ʧ�ܣ�1�ɹ�
) IS
/***********************************************************************************************
*�������ƣ�xxxx
*�������ͣ��洢����
*�������ߣ�xxx
*�������ڣ�xxxx-xx-xx
*��Ҫ����: ��ʾ������Ϊʾ��
*�޸���ʷ��
*   xxxx-xx-xx������ by xxx
**********************************************************************************************/
  V_COUNT NUMBER; --����
  MyException EXCEPTION; --�Զ����쳣

  V_FACT_STORE NUMBER(20, 6) := 0; --С��6λ
  D_NOW_NOT_OUT_QTY NUMBER(14, 4) := 0;--����δ���������
  D_NOW_OUT_QTY NUMBER(14, 4) := 0;--���γ��������
	
  V_DLR_STORAGE_ID VARCHAR2(50); --רӪ����ID
  V_CHECK_MODEL VARCHAR2(10);

  --�α�1�������������ⵥ��ϸ�����α�
  CURSOR CUR_DEAL_LIST IS 
	SELECT A.* 
	FROM T_TMP_COSTW A;
  REC_DEAL_LIST CUR_DEAL_LIST%ROWTYPE;  --�α��¼��������

  --����רӪ��ID�ͱ���ID�����ұ������������ϸ�α꣨רӪ��ID������ID��
  CURSOR CUR_STORE_BAT(V_DLR_ID VARCHAR2,V_PART_ID VARCHAR2) IS
    SELECT A.DLR_STORAGE_D_ID,A.DLR_STORAGE_ID,
		       NVL(A.OUT_STORE_QTY,0) AS OUT_STORE_QTY,
           (A.PATCH_QTY - NVL(A.OUT_STORE_QTY,0)) AS NOT_OUT_QTY,
					 A.BATCH_NO,A.PATCH_BILL_TYPE,A.PATCH_DATE,A.PATCH_QTY,A.UP_DLR_ID
      FROM T_STORAGE_D A /*רӪ����ܿ���*/
     WHERE A.UP_DLR_ID = V_DLR_ID
       AND A.PART_ID = V_PART_ID
       AND A.PATCH_QTY > 0 --ֻ�����������
       ORDER BY A.PATCH_DATE; --������ʱ����������
  REC_STORE_BAT CUR_STORE_BAT%ROWTYPE;

BEGIN
  V_RET := 1;
	--�������ʱ��
	DELETE FROM T_TMP_COSTW;
    /*һ��ȷ��������Χ�α�*/
    INSERT INTO  T_TMP_COSTW  	/*רӪ����ܿ�ɱ���ʱ��*/
	(
		COST_ESCROW_ID,
		BILL_BIG_TYPE,
		BILL_TYPE
	)
	SELECT SYS_GUID(),	/*COST_ESCROW_ID:���ܿ�ɱ�ID*/
	    A.BILL_BIG_TYPE,--BILL_BIG_TYPE,
		A.BILL_TYPE--BILL_TYPE
	FROM T_PA_UP_ESCROW_IN_STORE A
	UNION ALL
	SELECT SYS_GUID(),	/*COST_ESCROW_ID:���ܿ�ɱ�ID*/
	     OA.BILL_BIG_TYPE,--BILL_BIG_TYPE,
		OA.BILL_TYPE--BILL_TYPE,
	FROM T_UP_OUT OA
	;
	
	--�ж���ʱ�����Ƿ�����������С�ڵ���0��
	SELECT COUNT(1) INTO V_COUNT
	FROM T_PA_UP_TMP_COST_ESCROW A
	WHERE A.NEED_QTY <= 0;

	IF V_COUNT>0 THEN --Ϊ��
		RAISE_APPLICATION_ERROR(-20999, '����xxxʧ�ܡ��뱣֤����ⵥ�е���������0��');
		RETURN;
	END IF;
	
	 /*�������������Ϣ*/
	INSERT INTO  T_STORAGE  /*רӪ�걸����棨DLR)*/
	(
		 DLR_STORAGE_ID,
		 PART_ID,
		 PART_NO,
		 WAREHOUSE_ID,
		 PLACE_ID,
		 STORE_QTY,
		 DLR_ID,
		 CREATOR,
		 CREATED_DATE,
		 MODIFIER,
		 LAST_UPDATED_DATE,
		 IS_ENABLE,
		 UPDATE_CONTROL_ID,
		 UP_DLR_ID,
		 WELL_QTY,
		 BADNESS_QTY,
		 IS_SYSTEM
	)
	SELECT
		SYS_GUID(),  /*DLR_STORAGE_ID:DLR_STORAGE*/
		A.PART_ID,  /*PART_ID:����ID*/
		A.PART_NO,  /*PART_NO:��������*/
		A.WAREHOUSE_ID,  /*WAREHOUSE_ID:�ֿ�ID*/
		A.PLACE_ID,  /*PLACE_ID:��λID*/
		0,  /*STORE_QTY:�������*/
		A.DLR_ID,  /*DLR_ID:רӪ��ID*/
		A.CREATOR,  /*CREATOR:������*/
		SYSDATE,  /*CREATED_DATE:����ʱ��*/
		A.CREATOR,  /*MODIFIER:��������Ա*/
		SYSDATE,  /*LAST_UPDATED_DATE:������ʱ��*/
		'1',  /*IS_ENABLE:�Ƿ���� 0:������,1:����*/
		SYS_GUID(),  /*UPDATE_CONTROL_ID:���������ֶ�*/
		A.UP_DLR_ID,
		0,
		0,
		'0'
	FROM T_PA_UP_TMP_COST_ESCROW A
	WHERE NOT EXISTS(SELECT 1 FROM T_STORAGE S
	                 WHERE S.UP_DLR_ID = A.UP_DLR_ID AND S.PART_ID = A.PART_ID 
									       AND A.WAREHOUSE_ID = S.WAREHOUSE_ID);
  
	/*�������*/                       
	UPDATE T_STORAGE A
	SET A.UPDATE_CONTROL_ID = SYS_GUID()
	WHERE EXISTS(SELECT 1 FROM T_PA_UP_TMP_COST_ESCROW T
	      WHERE T.PART_ID = A.PART_ID AND T.WAREHOUSE_ID = A.WAREHOUSE_ID AND T.UP_DLR_ID = A.UP_DLR_ID);
												 
  /*�������ÿ��������������ɱ�*/
  OPEN CUR_DEAL_LIST;
  LOOP
    FETCH CUR_DEAL_LIST
      INTO REC_DEAL_LIST;
    IF CUR_DEAL_LIST%FOUND THEN

      --�õ����ID
      SELECT DLR_STORAGE_ID INTO V_DLR_STORAGE_ID
      FROM T_STORAGE A
      WHERE A.PART_ID = REC_DEAL_LIST.PART_ID
         AND A.UP_DLR_ID = REC_DEAL_LIST.UP_DLR_ID
				 AND A.WAREHOUSE_ID = REC_DEAL_LIST.WAREHOUSE_ID;

      ---------------------���---------------------------------------------
      IF V_ORDER_TYPE = '1' THEN
        /*���¿����ϸ��*/
        UPDATE T_STORAGE A/*M003_�����ϸ*/
           SET WAREHOUSE_ID       = REC_DEAL_LIST.WAREHOUSE_ID /*�ֿ�����*/,
               PLACE_ID           = REC_DEAL_LIST.PLACE_ID /*��λ����*/,
               STORE_QTY          = NVL(STORE_QTY, 0) + REC_DEAL_LIST.NEED_QTY ,/*�������*/
               LAST_UPDATED_DATE  = SYSTIMESTAMP,
               MODIFIER           = REC_DEAL_LIST.CREATOR /*������Ա*/
         WHERE A.DLR_STORAGE_ID = V_DLR_STORAGE_ID;
         /*1.3�������ο����ϸ*/
        INSERT INTO  T_STORAGE_D   /*רӪ������ϸ��*/
        (
           DLR_STORAGE_D_ID,
           DLR_STORAGE_ID,
           DLR_ID,
           PART_ID,
           PART_NO,
           BATCH_NO,
           PATCH_DATE,
           PATCH_QTY,
           OUT_STORE_QTY,
           IS_ALL_OUT,
           CREATED_DATE,
           CREATOR,
           LAST_UPDATED_DATE,
           MODIFIER,
           UPDATE_CONTROL_ID,
					 UP_DLR_ID
        )
        SELECT SYS_GUID(),  /*DLR_STORAGE_D_ID:�����ϸID*/
          V_DLR_STORAGE_ID,  /*DLR_STORAGE_ID:���ID  T_STORAGE����ID*/
          A.WAREHOUSE_ID,  /*DLR_ID:רӪ��ID*/
          A.PART_ID,  /*PART_ID:����ID*/
          A.PART_NO,  /*PART_NO:��������*/
          B.IN_STORE_CODE,  /*BATCH_NO:���κ�  ����ⵥ��Ϊ���κ�*/
          B.IN_STORE_DATE,  /*PATCH_DATE:����ʱ��  ���ʱ��*/
          A.IN_STORE_QTY,  /*PATCH_QTY:����������  ����������=������=��Ʒ��+����Ʒ������Ϊ0ʱ���ѳ���*/
          0,--OUT_STORE_QTY
          '0',  /*IS_ALL_OUT:�Ƿ��ѳ���  0δ����1�ѳ���(����������Ϊ0ʱΪ1��*/
          SYSDATE,  /*CREATED_DATE:����ʱ��*/
          'COST',  /*CREATOR:������*/
          SYSDATE,  /*LAST_UPDATED_DATE:������ʱ�� ����޸�ʱ��*/
          'COST',  /*MODIFIER:��������Ա ����޸���*/
          SYS_GUID(),  /*UPDATE_CONTROL_ID:���������ֶ�*/
					A.UP_DLR_ID
        FROM T_PA_UP_ESCROW_IN_STORE_D A /*�����ɹ������ϸ��*/
        JOIN T_PA_UP_ESCROW_IN_STORE B ON A.IN_STORE_ID = B.IN_STORE_ID
        WHERE A.IN_STORE_CODE = V_ORDER_CODE
              AND A.PART_ID = REC_DEAL_LIST.PART_ID
              AND ROWNUM=1;
        
			END IF;

      --------------------����˻�------------------------------------------
      IF V_ORDER_TYPE = '2' THEN
        --��ѯʵ�ʿ�棬����ϸ��Ϊ׼
        SELECT NVL(PS.STORE_QTY, 0)
          INTO V_FACT_STORE
          FROM T_STORAGE PS
         WHERE PS.DLR_STORAGE_ID = V_DLR_STORAGE_ID;
        /*��ʵ�ʿ��(�����������������Ʒ��������������Ʒ����)������ʱ���޸ķ�����ϢΪ0��ֱ���˳�*/
        IF V_FACT_STORE - REC_DEAL_LIST.NEED_QTY < 0 THEN
          V_RET := 0;
          CLOSE CUR_DEAL_LIST;
          RAISE MyException; /*�׳���治�����*/
          EXIT;
        END IF;
        /*���¿����ϸ��*/
        UPDATE T_STORAGE A/*M003_�����ϸ*/
           SET STORE_QTY          = NVL(STORE_QTY, 0) - REC_DEAL_LIST.NEED_QTY /*�������*/,
               LAST_UPDATED_DATE  = SYSTIMESTAMP,
               MODIFIER           = REC_DEAL_LIST.CREATOR /*������Ա*/
         WHERE A.DLR_STORAGE_ID = V_DLR_STORAGE_ID;      
      
			END IF;

      --------------------����----------------------------------------------
      IF V_ORDER_TYPE = '3' THEN
        --��ѯʵ�ʿ�棬����ϸ��Ϊ׼
        SELECT NVL(PS.STORE_QTY, 0)
          INTO V_FACT_STORE
          FROM T_STORAGE PS
         WHERE PS.DLR_STORAGE_ID = V_DLR_STORAGE_ID;

        /*��ʵ�ʿ��(�����������������Ʒ��������������Ʒ����)������ʱ���޸ķ�����ϢΪ0��ֱ���˳�*/
        IF V_FACT_STORE - REC_DEAL_LIST.NEED_QTY < 0 THEN
          V_RET := 0;
          CLOSE CUR_DEAL_LIST;
          RAISE MyException; /*�׳���治�����*/
          EXIT;
        END IF;
        /*���¿����ϸ��*/
        UPDATE T_STORAGE A/*M003_�����ϸ*/
           SET STORE_QTY          = NVL(STORE_QTY, 0) - REC_DEAL_LIST.NEED_QTY /*�������*/,
               LAST_UPDATED_DATE  = SYSTIMESTAMP,
               MODIFIER           = REC_DEAL_LIST.CREATOR /*������Ա*/
         WHERE A.DLR_STORAGE_ID = V_DLR_STORAGE_ID;
					 
      END IF;

      ---------------------�����˻�----------------------------------
      IF V_ORDER_TYPE = '4' THEN
        /*���¿����ϸ��*/
        UPDATE T_STORAGE A/*M003_�����ϸ*/
           SET PLACE_ID         = REC_DEAL_LIST.PLACE_ID /*��λ����*/,
               STORE_QTY          = NVL(STORE_QTY, 0) + REC_DEAL_LIST.NEED_QTY /*�������*/,
               LAST_UPDATED_DATE  = SYSTIMESTAMP,
               MODIFIER           = REC_DEAL_LIST.CREATOR /*������Ա*/
         WHERE A.DLR_STORAGE_ID = V_DLR_STORAGE_ID;
        
        --δ�����˻���������ʼֵΪ���γ����˻�����
        --ע�������˻�ֻ����Ʒ��û�в���Ʒ
        D_NOW_NOT_OUT_QTY := REC_DEAL_LIST.NEED_QTY;
        --�α괦��
        OPEN CUR_OUT_BAT(REC_DEAL_LIST.DLR_ID,REC_DEAL_LIST.PART_ID,V_ORDER_CODE);
        LOOP
          FETCH CUR_OUT_BAT
            INTO REC_OUT_BAT;
          IF CUR_OUT_BAT%FOUND THEN
              --ȫ�����꣬���˳�
              IF D_NOW_NOT_OUT_QTY <= 0 THEN
                CLOSE CUR_OUT_BAT;
                EXIT;
              END IF;

              /*�жϿ����е������Ƿ��Ѵ���*/
              SELECT COUNT(1) INTO  V_COUNT
              FROM T_STORAGE_D A
              WHERE A.PART_NO = REC_DEAL_LIST.PART_NO
                 AND A.UP_DLR_ID = REC_DEAL_LIST.UP_DLR_ID
                 AND A.BATCH_NO = REC_OUT_BAT.BATCH_NO;
              --��Ʒ���ο�洦��
              IF V_COUNT > 0 THEN --�����δ���
                --�ѳ�������ȫ�����㱾���˻�����
                IF REC_STORE_BAT.OUT_STORE_QTY >= D_NOW_NOT_OUT_QTY THEN
									D_NOW_OUT_QTY := D_NOW_NOT_OUT_QTY;
                  --�޸��ѳ�������=ԭ�ѳ�������-�����˻�����
                   UPDATE T_STORAGE_D T
                     SET T.OUT_STORE_QTY = T.OUT_STORE_QTY - D_NOW_NOT_OUT_QTY
                   WHERE T.DLR_STORAGE_D_ID = REC_OUT_BAT.DLR_STORAGE_D_ID;
                   --�˳��α�
                   CLOSE CUR_OUT_BAT;
                   EXIT;
                 ELSIF REC_STORE_BAT.OUT_STORE_QTY>0 AND  REC_STORE_BAT.OUT_STORE_QTY < D_NOW_NOT_OUT_QTY THEN
                   D_NOW_OUT_QTY := REC_STORE_BAT.OUT_STORE_QTY;
									 --�޸��ѳ�������Ϊ0
                   UPDATE T_STORAGE_D T
                     SET T.OUT_STORE_QTY = 0
                   WHERE T.DLR_STORAGE_D_ID = REC_OUT_BAT.DLR_STORAGE_D_ID;
                 END IF;
              ELSE --���β�����
                --��ѯԭ������ⵥ������������μ�¼
                INSERT INTO  T_STORAGE_D   /*רӪ������ϸ��*/
                (
                   DLR_STORAGE_D_ID,
                   DLR_STORAGE_ID,
                   DLR_ID,
                   PART_ID,
                   PART_NO,
                   BATCH_NO,
                   PATCH_DATE,
                   PATCH_QTY,
                   IS_ALL_OUT,
                   CREATED_DATE,
                   CREATOR,
                   LAST_UPDATED_DATE,
                   MODIFIER,
                   UPDATE_CONTROL_ID,
                   OUT_STORE_QTY
                )
                SELECT SYS_GUID(),  /*DLR_STORAGE_D_ID:�����ϸID*/
                  REC_OUT_BAT.DLR_STORAGE_ID,  /*DLR_STORAGE_ID:���ID  T_STORAGE����ID*/
                  REC_DEAL_LIST.WAREHOUSE_ID,  /*DLR_ID:רӪ��ID*/
                  REC_OUT_BAT.PART_ID,  /*PART_ID:����ID*/
                  REC_OUT_BAT.PART_NO,  /*PART_NO:��������*/
                  REC_OUT_BAT.BATCH_NO,  /*BATCH_NO:���κ�  ����ⵥ��Ϊ���κ�*/
                  REC_OUT_BAT.PATCH_DATE,  /*PATCH_DATE:����ʱ��  ���ʱ��*/
                  REC_OUT_BAT.OUT_STORE_QTY,  /*PATCH_QTY:����������  ����������=������=��Ʒ��+����Ʒ������Ϊ0ʱ���ѳ���*/
                  '0',  /*IS_ALL_OUT:�Ƿ��ѳ���  0δ����1�ѳ���(����������Ϊ0ʱΪ1��*/
                  SYSDATE,  /*CREATED_DATE:����ʱ��*/
                  'COST',  /*CREATOR:������*/
                  SYSDATE,  /*LAST_UPDATED_DATE:������ʱ�� ����޸�ʱ��*/
                  'COST',  /*MODIFIER:��������Ա ����޸���*/
                  SYS_GUID(),  /*UPDATE_CONTROL_ID:���������ֶ�*/
                  0--OUT_STORE_QTY,
                FROM T_PA_UP_ESCROW_IN_STORE_D A
                LEFT JOIN T_PA_UP_ESCROW_IN_STORE B
                     ON A.IN_STORE_ID = B.IN_STORE_ID
                WHERE A.IN_STORE_CODE = REC_OUT_BAT.BATCH_NO
                      AND A.PART_ID = REC_OUT_BAT.PART_ID;
              END IF;
							
				--���������˻�������
				INSERT INTO  T_PA_UP_ESCROW_OUT_STORE_BAT  	/*���ܿ���ⵥ_����*/
				(
					OUT_STORE_BAT_ID,
					OUT_STORE_D_DLR_ID,
					OUT_STORE_ID,
					OUT_STORE_CODE,
					PART_ID,
					PART_NO,
					UNIT,
					BATCH_NO,
					PATCH_DATE,
					OUT_STORE_QTY,
					REMARK,
					DLR_ID,
					UP_DLR_ID,
					IS_ENABLE,
					CREATED_DATE,
					CREATOR,
					LAST_UPDATED_DATE,
					MODIFIER,
					UPDATE_CONTROL_ID,
					BILL_TYPE
				)
				VALUES
				(
					SYS_GUID(),	/*OUT_STORE_BAT_ID:רӪ�걸������������ϸID*/
					REC_DEAL_LIST.ORDER_D_ID,	/*OUT_STORE_D_DLR_ID:רӪ�걸�����ⵥ_��ϸID*/
					REC_DEAL_LIST.ORDER_ID,	/*OUT_STORE_ID:רӪ�걸�����ⵥ T_PA_BU_DLR_OUT_STORE.OUT_STORE_ID*/
					REC_DEAL_LIST.ORDER_CODE,	/*OUT_STORE_CODE:���ⵥ���*/
					REC_DEAL_LIST.PART_ID,	/*PART_ID:����ID*/
					REC_DEAL_LIST.PART_NO,	/*PART_NO:��������*/
					REC_DEAL_LIST.PART_UNIT,	/*UNIT:��λ*/
					REC_OUT_BAT.BATCH_NO,	/*BATCH_NO:���κ� ����ⵥ��Ϊ���κ�*/
					REC_OUT_BAT.PATCH_DATE,	/*PATCH_DATE:����ʱ�� ���ʱ��*/
					D_NOW_OUT_QTY,	/*OUT_STORE_QTY:��������*/
					'�Զ�����',	/*REMARK:��ע*/
					REC_OUT_BAT.UP_DLR_ID,	/*DLR_ID:רӪ��ID*/
					REC_OUT_BAT.UP_DLR_ID,	/*UP_DLR_ID:ͳ�ɵ�ID*/
					1,	/*IS_ENABLE:�Ƿ���� 0��Ч��1��Ч*/
					SYSDATE,	/*CREATED_DATE:����ʱ��*/
					REC_DEAL_LIST.CREATOR,	/*CREATOR:������*/
					SYSDATE,	/*LAST_UPDATED_DATE:������ʱ��*/
					REC_DEAL_LIST.CREATOR,	/*MODIFIER:��������Ա*/
					SYS_GUID(),	/*UPDATE_CONTROL_ID:���������ֶ�*/
					REC_OUT_BAT.BILL_TYPE	/*BILL_TYPE:��������*/
				);

          ELSE
            CLOSE CUR_OUT_BAT;
            EXIT;
          END IF;
        END LOOP; 
        
      END IF;

      /*����˻�����⣺�����α�ʵ�ָ������ο����ϸ��ע����ֻ��������*/
      IF V_ORDER_TYPE = '2' OR V_ORDER_TYPE = '3' THEN
        --δ������������ʼֵΪ���γ�������
        D_NOW_NOT_OUT_QTY := REC_DEAL_LIST.NEED_QTY;
        --�α괦��
        OPEN CUR_STORE_BAT(REC_DEAL_LIST.DLR_ID,REC_DEAL_LIST.PART_ID);
        LOOP
          FETCH CUR_STORE_BAT
            INTO REC_STORE_BAT;
          IF CUR_STORE_BAT%FOUND THEN
              --ȫ�����꣬���˳�
              IF D_NOW_NOT_OUT_QTY <= 0 THEN
                CLOSE CUR_STORE_BAT;
                EXIT;
              END IF;

              --���ο�洦��
              IF REC_STORE_BAT.NOT_OUT_QTY  > 0 THEN --�����οɳ���
                --ȫ������
                IF REC_STORE_BAT.NOT_OUT_QTY >= D_NOW_NOT_OUT_QTY THEN
								  D_NOW_OUT_QTY := D_NOW_NOT_OUT_QTY;
                  --����������ϸ���ѳ�������=��ǰ�������� + ����δ��������
                  UPDATE T_STORAGE_D T
                   SET T.OUT_STORE_QTY = NVL(T.OUT_STORE_QTY,0) + D_NOW_NOT_OUT_QTY,
                       T.IS_ALL_OUT = CASE WHEN REC_STORE_BAT.NOT_OUT_QTY = D_NOW_NOT_OUT_QTY THEN '1' ELSE '0' END
                   WHERE T.DLR_STORAGE_D_ID = REC_STORE_BAT.DLR_STORAGE_D_ID;
                  --����δ����������0
                  D_NOW_NOT_OUT_QTY:=0;
                ELSE
                  --��������
									D_NOW_OUT_QTY := REC_STORE_BAT.NOT_OUT_QTY;
                  --����������ϸ���ѳ�������=��ǰ�������� + ����������
                  UPDATE T_STORAGE_D T
                   SET T.OUT_STORE_QTY = NVL(T.OUT_STORE_QTY,0) + REC_STORE_BAT.NOT_OUT_QTY,
                       T.IS_ALL_OUT = '1' --������ȫ�������
                   WHERE T.DLR_STORAGE_D_ID = REC_STORE_BAT.DLR_STORAGE_D_ID;
                  --����δ�������� =  δ�������� - ������Ʒ����
                  D_NOW_NOT_OUT_QTY:=D_NOW_NOT_OUT_QTY-REC_STORE_BAT.NOT_OUT_QTY;
                END IF;
								--���������˻�������
								INSERT INTO  T_PA_UP_ESCROW_OUT_STORE_BAT  	/*���ܿ���ⵥ_����*/
								(
									OUT_STORE_BAT_ID,
									OUT_STORE_D_DLR_ID,
									OUT_STORE_ID,
									OUT_STORE_CODE,
									PART_ID,
									PART_NO,
									UNIT,
									BATCH_NO,
									PATCH_DATE,
									OUT_STORE_QTY,
									REMARK,
									DLR_ID,
									UP_DLR_ID,
									IS_ENABLE,
									CREATED_DATE,
									CREATOR,
									LAST_UPDATED_DATE,
									MODIFIER,
									UPDATE_CONTROL_ID,
									BILL_TYPE
								)
								VALUES
								(
									SYS_GUID(),	/*OUT_STORE_BAT_ID:רӪ�걸������������ϸID*/
									REC_DEAL_LIST.ORDER_D_ID,	/*OUT_STORE_D_DLR_ID:רӪ�걸�����ⵥ_��ϸID*/
									REC_DEAL_LIST.ORDER_ID,	/*OUT_STORE_ID:רӪ�걸�����ⵥ T_PA_BU_DLR_OUT_STORE.OUT_STORE_ID*/
									REC_DEAL_LIST.ORDER_CODE,	/*OUT_STORE_CODE:���ⵥ���*/
									REC_DEAL_LIST.PART_ID,	/*PART_ID:����ID*/
									REC_DEAL_LIST.PART_NO,	/*PART_NO:��������*/
									REC_DEAL_LIST.PART_UNIT,	/*UNIT:��λ*/
									REC_STORE_BAT.BATCH_NO,	/*BATCH_NO:���κ� ����ⵥ��Ϊ���κ�*/
									REC_STORE_BAT.PATCH_DATE,	/*PATCH_DATE:����ʱ�� ���ʱ��*/
									D_NOW_OUT_QTY,	/*OUT_STORE_QTY:��������*/
									'�Զ�����',	/*REMARK:��ע*/
									REC_STORE_BAT.UP_DLR_ID,	/*DLR_ID:רӪ��ID*/
									REC_STORE_BAT.UP_DLR_ID,	/*UP_DLR_ID:ͳ�ɵ�ID*/
									1,	/*IS_ENABLE:�Ƿ���� 0��Ч��1��Ч*/
									SYSDATE,	/*CREATED_DATE:����ʱ��*/
									REC_DEAL_LIST.CREATOR,	/*CREATOR:������*/
									SYSDATE,	/*LAST_UPDATED_DATE:������ʱ��*/
									REC_DEAL_LIST.CREATOR,	/*MODIFIER:��������Ա*/
									SYS_GUID(),	/*UPDATE_CONTROL_ID:���������ֶ�*/
									REC_STORE_BAT.PATCH_BILL_TYPE	/*BILL_TYPE:��������*/
								);
              END IF;
          ELSE
            CLOSE CUR_STORE_BAT;
            EXIT;
          END IF;
        END LOOP;
      END IF;

    ELSE
      CLOSE CUR_DEAL_LIST;
      EXIT;
    END IF;
  END LOOP;
EXCEPTION
  WHEN MyException THEN
    V_RET := 0;
    IF CUR_DEAL_LIST%ISOPEN = TRUE THEN
      CLOSE CUR_DEAL_LIST;
    END IF;
    RAISE_APPLICATION_ERROR(-20999, 'xxxxxʧ�ܣ�');
  WHEN OTHERS THEN
    V_RET := 0;
    IF CUR_DEAL_LIST%ISOPEN = TRUE THEN
      CLOSE CUR_DEAL_LIST;
    END IF;
    RAISE_APPLICATION_ERROR(-20999, '���ִ���,���¿��ʧ�ܣ�'||SQLERRM);
END;