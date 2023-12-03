/*�����洢��������(��д��־)*/
/*ɾ���洢����*/
IF ( OBJECT_ID('IFR_UC_T_CAR_SERIESProcedure') IS NOT NULL ) 
	DROP PROCEDURE IFR_UC_T_CAR_SERIESProcedure
GO	
/*�����洢����*/
CREATE PROCEDURE IFR_UC_T_CAR_SERIESProcedure 
AS
/***********************************************************************************
* �洢��������: ���ֳ���ϵ����
* ��������: �ƹ���
* ��������: 2012-12-05   
* ʹ��ϵͳ: DMS
* ʹ��ģ��: ���ֳ�����
* ˵�������ֳ���ϵ����
* �޸���ʷ˵����
*	2012-12-05: ��CLR�洢����ת��ΪSQL�洢����(CLR����̫�鷳��) hgh  
***********************************************************************************/ 
BEGIN
	/*��������*/
	DECLARE @NETCODE VARCHAR(10),
			@FUN_NAME VARCHAR(200) ,
			@IF_NO VARCHAR(50) ,    
			@IF_NAME VARCHAR(200),  
			@IF_TYPE VARCHAR(1),    
			@MESSAGE VARCHAR(MAX),
			@curTemp CURSOR
    
	/*�ӿ���Ϣ*/  
	SELECT  @FUN_NAME = '�������', 
			@IF_NO = 'DMS_SE_E3SS_006',
			@IF_NAME = '�г���̨KPI�����ϴ�',
			@IF_TYPE = 'N',
			@MESSAGE = ''
    /*ȡһ������*/	
	SELECT @NETCODE=NETCODE FROM dbo.BD_SYSINFO WHERE DLR_TYPE='1'
	/*�����α�*/ 
	SET @curTemp = Cursor For SELECT In_Id FROM  AA
	OPEN @curTemp
	FETCH NEXT FROM @curTemp INTO @In_Id 		  
	WHILE @@fetch_status = 0 
		BEGIN
			BEGIN TRY		
				/*�α����߼�����:���������:GOTO Final_Deal*/
				/**�˴���******/
				
				/*д�봦��ɹ���Ϣ*/
				SELECT  @MESSAGE = 'E3S���ⵥ' +@E3S_OUT_CODE+ '���ճɹ���',@IF_TYPE = 'N'
			END TRY
			BEGIN CATCH 
				IF @@trancount > 0 
					BEGIN
						ROLLBACK TRAN
					END
					SELECT  @MESSAGE = ERROR_MESSAGE()+'. Error Line: '+ CAST(ERROR_LINE() AS VARCHAR(10)),@IF_TYPE = 'E'
					SET @MESSAGE = 'E3S���ⵥ: '+ CAST(@E3S_OUT_CODE AS VARCHAR(50)) + '����ʧ�ܣ�'+ @MESSAGE
			END CATCH
			/*�������մ����ǩ*/
			Final_Deal:
			/*���½ӿڱ��¼Ϊ�Ѵ���״̬1*/        
			UPDATE dbo.IFR_PA_E3S_OUTSTOCK SET MQSENDSTATUS='1'
			WHERE NETCODE=@NETCODE AND OUTSTORAGENO=@E3S_OUT_CODE
			/*�ύ����*/
			IF @@trancount > 0 
			BEGIN
				COMMIT TRAN
			END      
			--д��־    
			EXEC PROC_LOG_MESSAGE @FUN_NAME_ = @FUN_NAME,@IF_NO_ = @IF_NO, @IF_NAME_ = @IF_NAME, @SCENS_ = '',
				   @DIRECT_ = 'SR', @IF_TYPE_ = @IF_TYPE,@REMARK_ = @MESSAGE		  
			/*������һ���α��¼*/
			FETCH NEXT FROM @curTemp INTO @In_Id
		END	
	--�ر��α�	        	
	close @curTemp
	deallocate @curTemp 	
END