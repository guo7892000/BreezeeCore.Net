DECLARE @TABLE_NAME VARCHAR(100),/*����*/
		@COLUMNS_NAME VARCHAR(100),/*����*/
		@EXTEND_TEXT VARCHAR(8000), /*��չ����ֵ*/
		@TABLE_ID INT,/*��ID*/
		@COLUMNS_ID INT,/*����ID*/
		@curTemp CURSOR
BEGIN		
	SET @curTemp = Cursor For SELECT TABLE_NAME,COLUMNS_NAME,EXTEND_TEXT 
		FROM (/*�������޸ı�����չ���Է�Χ���ж������UNION*/
			  #EXTEND_LIST#
			  ) AA		  
	OPEN @curTemp
	FETCH NEXT FROM @curTemp INTO @TABLE_NAME,@COLUMNS_NAME,@EXTEND_TEXT 		  
	WHILE @@fetch_status = 0 
		BEGIN		
			/*��*/
			SELECT @TABLE_ID=object_id FROM sys.objects WHERE type='U' AND name=@TABLE_NAME
			IF @TABLE_ID IS NULL
				BEGIN
					PRINT @TABLE_NAME+'������!\n'
					GOTO Final_Deal
				END
			/*����*/
			IF ISNULL(@COLUMNS_NAME,'')<>''
				BEGIN
					SELECT @COLUMNS_ID=column_id FROM sys.all_columns WHERE object_id=@TABLE_ID AND name=@COLUMNS_NAME
					IF @COLUMNS_ID IS NULL
					BEGIN
						PRINT @TABLE_NAME+'���'+@COLUMNS_NAME+'�в�����!\n'
						GOTO Final_Deal
					END
					IF EXISTS(SELECT 1 FROM sys.extended_properties WHERE major_id=@TABLE_ID AND minor_id=@COLUMNS_ID)
					BEGIN
						EXEC sys.sp_dropextendedproperty @name = N'MS_Description',
							@level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE',@level1name = @TABLE_NAME,  
							@level2type = N'COLUMN', @level2name = @COLUMNS_NAME
					END
					EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = @EXTEND_TEXT,
						@level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE',@level1name = @TABLE_NAME,  
						@level2type = N'COLUMN', @level2name = @COLUMNS_NAME
				END
			ELSE
				BEGIN
					IF EXISTS(SELECT 1 FROM sys.extended_properties WHERE major_id=@TABLE_ID AND minor_id=0)
					BEGIN
						EXEC sys.sp_dropextendedproperty @name = N'MS_Description',
							@level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE',@level1name = @TABLE_NAME,  
							@level2type = null, @level2name = null
					END
					EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = @EXTEND_TEXT,
						@level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE',@level1name = @TABLE_NAME,  
						@level2type = null, @level2name = null
				END
			/*�������մ����ǩ*/
			Final_Deal:
			/*ȡ��һ������*/
			FETCH NEXT FROM @curTemp INTO @TABLE_NAME,@COLUMNS_NAME,@EXTEND_TEXT 
		END 
END 	
close @curTemp
deallocate @curTemp 
GO
