/*ע��mysql��''��null�ǲ���ȵ�*/
if(TRIM(ifnull(B.FINANCE_TYPE,''))='','4',B.FINANCE_TYPE)
;

/*�������ѯ����ϲ���һ��*/
SELECT GROUP_CONCAT(dlr.DLR_ID) 
FROM  t_usc_mdmn_db_base_dlr_info dlr
;


