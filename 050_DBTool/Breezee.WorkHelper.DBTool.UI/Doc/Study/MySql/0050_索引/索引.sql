/*��Mysql��һ�������������������IDX_��ͷ����һ�м�������*/
alter table T_TABLE add  key IDX_DLR3 (DLR_ID,CREATED_DATE,ACCOUNT_INFO_CODE);

/*��Mysql��Ψһ�����������������UK_��ͷ����һ�м�������*/
alter table T_TABLE add unique key UK_ACCOUNT_INFO_CODE2 (ACCOUNT_INFO_CODE,MODULE_CODE);

