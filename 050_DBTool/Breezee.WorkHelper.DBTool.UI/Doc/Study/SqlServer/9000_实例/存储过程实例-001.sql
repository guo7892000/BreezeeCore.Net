/*�����洢��������*/
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
*	V1.7.06: ��CLR�洢����ת��ΪSQL�洢����(CLR����̫�鷳��) hgh 2012-12-05 
***********************************************************************************/ 
BEGIN
	DECLARE @curTemp CURSOR
	SET @curTemp = Cursor For SELECT In_Id 
		FROM  AA		  
	OPEN @curTemp
	FETCH NEXT FROM @curTemp INTO @In_Id 		  
	WHILE @@fetch_status = 0 
		BEGIN		
					  
			FETCH NEXT FROM @curTemp INTO @In_Id
		END 	
	close @curTemp
	deallocate @curTemp 	
END


