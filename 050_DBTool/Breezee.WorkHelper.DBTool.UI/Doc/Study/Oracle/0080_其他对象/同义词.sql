/*��ͼ����ͬ��ʲ���Ȩ*/
GRANT SELECT ON UC.V_UC_QUERY_STORE_CAR TO MDS_ROLE;  
CREATE OR REPLACE SYNONYM MDS.V_UC_QUERY_STORE_CAR FOR UC.V_UC_QUERY_STORE_CAR;  
GRANT SELECT ON UC.V_UC_QUERY_STORE_CAR TO MDS WITH GRANT OPTION;

/*��ͼ��Ȩ�봴��ͬ���*/
grant select on uc.v_uc_carprice to mds ;
create or replace synonym mds.v_uc_carprice for uc.v_uc_carprice;

