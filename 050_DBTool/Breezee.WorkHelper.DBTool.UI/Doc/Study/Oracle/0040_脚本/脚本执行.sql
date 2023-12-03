/*�ű���ִ���ַ�Ϊ�����ҵ�б�ˣ�����//��*/

/*�ж�Ӱ����*/
UPDATE TB_NETYKRUNLOG SET NETYKSPNAME='P_AUTO_QJ' WHERE  NUM=1 AND NETYKSPNAME IS NULL;
IF SQL%ROWCOUNT=1 THEN
 --dong something
 v:='��dҪ��һɱ��';
 dbms_output.put_line(substr(v, 2, 3));
END IF;
/
/*��̬ɾ����SQL��*/
declare  cnt  number;
begin
  ---ɾ����COC_CAL_GoodReturn
  select count(1) into cnt from user_objects where upper(object_name) = 'COC_CAL_GOODRETURN' and upper(object_type) = 'TABLE'; 
  if cnt = 1 then 
    begin 
      execute immediate 'DROP TABLE COC_CAL_GOODRETURN';
    end;
  end if;
END;
/
/*��̬�޸ı�������ע��*/
declare cnt number; 
begin 
 ---�޸�רӪ���������DEALER_CODE_O,������ע��
 select count(1) into cnt from user_tab_columns where upper(table_name) = 'COC_BAS_DEALER' and upper(column_name) = 'DEALER_CODE_O'; 
 if cnt = 0 then 
   begin 
      execute immediate 'ALTER TABLE COC_BAS_DEALER ADD (DEALER_CODE_O varchar2(10))'; 
      execute immediate 'COMMENT ON column COC_BAS_DEALER.DEALER_CODE_O IS ' || '''רӪ�����'''; 
   end; 
 end if;
END;


/*ɾ���ظ���JOB*/
declare jobID number;
CURSOR CUR_UC IS
      select what,max(job) max_job  from all_jobs 
      where what in (select what from (select what,count(1) icount
                        from all_jobs
                        group by what
                        having count(1)>1)
      )
      --and what='P_DC_IS_NO_CONSUME;'
      group by what;
      REC_UC  CUR_UC%ROWTYPE;
BEGIN
  OPEN CUR_UC;
  LOOP
    FETCH CUR_UC
      INTO REC_UC;
    IF CUR_UC%FOUND THEN
        declare CURSOR CUR_UC2 IS select job from all_jobs where what=REC_UC.what;
         REC_UC2 CUR_UC2%ROWTYPE;
        begin 
          OPEN CUR_UC2;
          LOOP
            FETCH CUR_UC2
              INTO REC_UC2; 
          IF CUR_UC2%FOUND THEN
            if REC_UC2.job<>REC_UC.max_job then
               begin
                 dbms_job.remove(REC_UC2.job);
                 commit;
               end;
            end if; 
          ELSE
            CLOSE CUR_UC2;
            EXIT;
          END IF;  
          END LOOP;
        end;
    ELSE
      CLOSE CUR_UC;
      EXIT;
    END IF;  
  END LOOP; 
end;   

