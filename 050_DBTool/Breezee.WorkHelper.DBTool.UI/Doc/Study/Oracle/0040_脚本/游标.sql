/*����*/
v_Dms_NetCode eabuc.t_org_dealer.dealerno%Type;
Cursor curBalance is
    Select * from eabuc.PS_U_T_MEMU_CERTIFICATE;
  RecBalance curBalance%RowType;
begin
  open curBalance;
  loop
    Fetch curBalance
      into RecBalance;
    Exit when curBalance%NotFound;
	  begin
		/*�߼������˴���*/

		  commit;
		WHEN OTHERS THEN
			/*����ع�*/
			Rollback; 
			/*�׳�ϵͳ����*/
			RAISE_APPLICATION_ERROR(-20999, sqlerrm);
	 end;
  end loop;
  close curBalance;
end;

/*Oracle�ľ�̬�α��붯̬�α꣺
1����̬�α꣺��ʽ�α����ʽ�α��Ϊ��̬�α꣬��Ϊ��ʹ������֮ǰ���α�Ķ����Ѿ���ɣ������ٸ��ġ�
���壺
	Cursor �α���(����1������2......) is  ��ѯ���
����ʱ��
	for ������ in �α��� loop

	end loop;

2����̬�α꣺�α�������ʱû���趨���ڴ�ʱ���Զ�������޸ġ�
���壺
	TYPE �α���� IS REF CURSOR;
	�α��� �α����;
����ʱ��
	open �α��� for ��̬SQL���;
	loop
		exit when �α���%NOTFOUND;
		fetch �α���
		  into ����1������2������3������4;
*/



