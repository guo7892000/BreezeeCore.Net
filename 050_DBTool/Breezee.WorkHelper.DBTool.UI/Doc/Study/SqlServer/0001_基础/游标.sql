/*�α�Ƕ��*/
DECLARE @curTemp CURSOR
	SET @curTemp = Cursor For SELECT In_Id 
		FROM  AA		  
	OPEN @curTemp
	FETCH NEXT FROM @curTemp INTO @In_Id 		  
	WHILE @@fetch_status = 0 
		BEGIN		
			DECLARE @curButton CURSOR
			SET @curButton	= Cursor For SELECT BUTTON_NAME 
				FROM BBB
				WHERE In_Id=@In_Id
			
			 /*��ť�α�*/
			 OPEN @curButton
				FETCH NEXT FROM @curButton INTO @BUTTON_NAME  
				WHILE @@fetch_status = 0 
					BEGIN   
						/*���Ӱ�ť���¼*/
						
				FETCH NEXT FROM @curButton INTO @BUTTON_NAME
				
			END 
			close @curButton
			deallocate @curButton 			  
	FETCH NEXT FROM @curTemp INTO @In_Id
END 	
close @curTemp
deallocate @curTemp 


