/*����������޸���ͼ����Ȩѡ������*/
--��ѯ��Ȩ��
SELECT * FROM USER_TAB_PRIVS T 
WHERE T.GRANTOR='MDM' AND T.grantee='UC'
ORDER BY T.TABLE_NAME;
select * from T_MDM_COMP_BRAND;
--��UC�û���ִ����ͼV_UC_BASE_ALLCARBRAND���ʱ������ʾ��ORA-01720�������ڡ�****.****"��Ȩѡ�
--ִ�����������
GRANT SELECT ON MDM.T_MDM_COMP_BRAND TO UC WITH GRANT OPTION;

