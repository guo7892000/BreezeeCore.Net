--�鿴����
select * from user_jobs;
select * from all_jobs;
--�鿴�������е����񣨲��Ƽ�ʹ�ã��ٶ�������
select * from dba_jobs_running;

/*���JOB*/
DECLARE JOBID NUMBER;
BEGIN
  SELECT MAX(JOB)+1 INTO JOBID FROM ALL_JOBS;
  DBMS_JOB.SUBMIT(JOBID, 'P_PA_DELETE_OVERDUE_LACK_AUTO;', SYSDATE,'TRUNC(SYSDATE+1)');
  COMMIT;
END;

/*�޸�JOB*/
declare jobID number;
begin
  select JOB into jobID from all_jobs where what='P_SE_IR_DAY_ONCE;';
  dbms_job.change(jobID ,'P_SE_IR_DAY_ONCE;', sysdate+1,'sysdate+1/24' );
  commit;
end;

/*JOB����������*/
begin
 dbms_job.remove(41); --ɾ��JO
 dbms_job.broken(25,true); --ֹͣjob
 dbms_job.run(25); --����job
 dbms_job.what(v_job,'sp_fact_charge_code;'); --�޸�What����
 dbms_job.next_date(v_job,sysdate); --�޸�ĳ��job�� �޸���һ������ʱ��
end;

