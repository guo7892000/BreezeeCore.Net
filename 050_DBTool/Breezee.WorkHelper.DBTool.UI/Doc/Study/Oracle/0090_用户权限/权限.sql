/*����������޸���ͼ����Ȩѡ������*/
--��ѯ��Ȩ��
SELECT * FROM USER_TAB_PRIVS T 
WHERE T.GRANTOR='MDM' AND T.grantee='UC'
ORDER BY T.TABLE_NAME;
select * from T_MDM_COMP_BRAND;
--��UC�û���ִ����ͼV_UC_BASE_ALLCARBRAND���ʱ������ʾ��ORA-01720�������ڡ�****.****"��Ȩѡ�
--ִ�����������
GRANT SELECT ON MDM.T_MDM_COMP_BRAND TO UC WITH GRANT OPTION;

-- Grant/Revoke object privileges 
grant select on T_PA_BU_DLR_OUT_STORE to DEVELOPERS_FOR_E3S;
grant select, insert, update, delete on T_PA_BU_DLR_OUT_STORE to MDS;
grant select, insert, update, delete on T_PA_BU_DLR_OUT_STORE to FI with grant option;
grant select, insert, update, delete, references, alter, index, debug on T_PA_BU_DLR_OUT_STORE to IFR;

