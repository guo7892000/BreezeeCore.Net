/*������*/
  ALTER TABLE emp01 ADD eno NUMBER(4);

/*�޸��ж���*/
  ALTER TABLE emp01 MODIFY job VARCHAR2(15)  DEFAULT 'CLERK';

/*ɾ����*/
  ALTER TABLE emp01 DROP COLUMN dno;

/*�޸�����*/
  ALTER TABLE emp01 RENAME COLUMN eno TO empno;

/*�޸ı���*/
  RENAME emp01 TO employee;

/*��ɾ������������*/
ALTER TABLE MQ_PS_PREPARE DROP CONSTRAINT PK_MQ_PS_PREPARE;
ALTER TABLE MQ_PS_PREPARE ADD CONSTRAINT PK_MQ_PS_PREPARE PRIMARY KEY (OID);
/*��������SQL*/
COMMENT ON TABLE TEST1 IS '���Ա�';--�������ע��
COMMENT ON COLUMN T_AI_BU_INSURE_ORDER.LINK_NO IS '��ͬ����';--���м�ע��
ALTER TABLE TEST1 MODIFY PLANMONTH NUMBER;
ALTER TABLE TEST1 ADD LONG1 LONG;
ALTER TABLE TEST1 ADD DFD NUMBER(14,2) DEFAULT 0;
ALTER TABLE TEST1 ADD FDFED NUMBER DEFAULT 1;
ALTER TABLE TEST1 ADD CONSTRAINT PK_ID PRIMARY KEY (ID);
ALTER TABLE TEST1 ADD CONSTRAINT FK_D_ID FOREIGN KEY (D_ID) REFERENCES TEST2 (RID);
ALTER TABLE TEST1 ADD CONSTRAINT U_DFD UNIQUE (PLANMONTH);
ALTER TABLE TEST1 DROP COLUMN PLANMONTH;

-- Create table
create table T_PA_BU_DLR_OUT_STORE
(
  out_store_id               VARCHAR2(36) not null,
  out_store_code             VARCHAR2(50) not null,
  bill_type                  VARCHAR2(10) not null,
  relate_order_id            VARCHAR2(36),
  relate_order_code          VARCHAR2(50),
  cust_id                    VARCHAR2(50),
  cust_name                  VARCHAR2(200),
  vin                        VARCHAR2(17),
  out_store_date             DATE,
  picking_person_name        VARCHAR2(100),
  balance_month              VARCHAR2(6),
  out_store_status           VARCHAR2(10) default '0',
  applier_name               VARCHAR2(100),
  remark                     VARCHAR2(200),
  balance_status             VARCHAR2(2),
  balance_date               DATE,
  car_brand_code             VARCHAR2(50),
  car_license                VARCHAR2(50),
  relate_out_store_code      VARCHAR2(50),
  relate_out_store_link_code VARCHAR2(50),
  dlr_id                     VARCHAR2(36) not null,
  creator                    VARCHAR2(50) not null,
  created_date               TIMESTAMP(6) default systimestamp not null,
  modifier                   VARCHAR2(50) not null,
  last_updated_date          TIMESTAMP(6) default systimestamp not null,
  is_enable                  VARCHAR2(50) default '1' not null,
  sdp_user_id                VARCHAR2(36) default nvl(SYS_CONTEXT('SDP_CONTEXT','userid'),'88888') not null,
  sdp_org_id                 VARCHAR2(36) default nvl(SYS_CONTEXT('SDP_CONTEXT','orgid'),'2') not null,
  update_control_id          VARCHAR2(36) default sys_guid() not null,
  picking_person_id          VARCHAR2(100),
  relate_out_store_id        VARCHAR2(36),
  relate_out_store_link_id   VARCHAR2(36),
  bill_big_type              VARCHAR2(10),
  applier_id                 VARCHAR2(36),
  tax_rate                   NUMBER(14,4),
  is_new_car_acce_out        VARCHAR2(2) default 0,
  sale_order_code            VARCHAR2(50),
  car_type_code              VARCHAR2(50),
  use_dept_id                VARCHAR2(36),
  draw_type_code             VARCHAR2(10),
  is_account_check           VARCHAR2(2) default 0 not null,
  car_series_code            VARCHAR2(50),
  transfer_in_dlr_id         VARCHAR2(36),
  audit_flag                 VARCHAR2(2),
  is_archived                VARCHAR2(2),
  interface_update_date      TIMESTAMP(6)
);
-- Add comments to the table 
comment on table T_PA_BU_DLR_OUT_STORE
  is 'רӪ�걸�����˿ⵥ(DLR)';
-- Add comments to the columns 
comment on column T_PA_BU_DLR_OUT_STORE.out_store_id
  is 'רӪ�걸�����ⵥID';
comment on column T_PA_BU_DLR_OUT_STORE.out_store_code
  is '���ⵥ���';
comment on column T_PA_BU_DLR_OUT_STORE.bill_type
  is '��������  �������ͣ�1  ά�޳��� 2  �ۺ�Ʒ���۳��� 3  ����������� 4  �ڲ����ó��� 5  �������ó��� 6  �������ϳ��� 7  �����ڲ��������� 8  �����̿����� 9  �³���Ʒ���۳��⣻10  ����������۳��⣻11  �����������۳��⣻12  ��Ӧ�̷������⣻13 ���޳���  �����˿����ͣ�101 ά�޳����˻���102 �ۺ�Ʒ���۳����˻���103 ������������˻���104 �ڲ����ó����˻���105 �������ó����˻���109 �³���Ʒ���۳����˻���110  ����������۳����˻���111 �������������˿�';
comment on column T_PA_BU_DLR_OUT_STORE.relate_order_id
  is '��������ID  �ɹ��� �ۺ�Ʒ���۵� ������ε� �����ڲ����ε� ����̵㵥 �³���Ʒ���۵�';
comment on column T_PA_BU_DLR_OUT_STORE.relate_order_code
  is '�������ݱ���  �ɹ��� �ۺ�Ʒ���۵� ������ε� �����ڲ����ε� ����̵㵥 �³���Ʒ���۵�';
comment on column T_PA_BU_DLR_OUT_STORE.cust_id
  is '�ͻ�ID';
comment on column T_PA_BU_DLR_OUT_STORE.cust_name
  is '�ͻ�����  ά�޳��⣺�ɹ��Ŀͻ����� �ۺ�Ʒ���۳��⣺��Ʒ���۵��Ŀͻ����� ������γ��⣺Ŀ������ �������ó���:ά�޼�ʦ �ڲ����ó���:ʹ����Ա ���ϳ��⣺��ǰϵͳ��¼�� �����ڲ����γ��⣺�Է����� �̿����⣺��ǰϵͳ��¼��';
comment on column T_PA_BU_DLR_OUT_STORE.vin
  is 'VIN��';
comment on column T_PA_BU_DLR_OUT_STORE.out_store_date
  is '��������';
comment on column T_PA_BU_DLR_OUT_STORE.picking_person_name
  is '������Ա  ά�޳��⣬�ۺ�Ʒ���۳��⣬�³���Ʒ���۳��⣬�����������ɲ���';
comment on column T_PA_BU_DLR_OUT_STORE.balance_month
  is '�½��·�';
comment on column T_PA_BU_DLR_OUT_STORE.out_store_status
  is '���ⵥ״̬  0��ʾδ��� 1��ʾ������� 2��ʾ��ȫ���';
comment on column T_PA_BU_DLR_OUT_STORE.applier_name
  is '����������';
comment on column T_PA_BU_DLR_OUT_STORE.remark
  is '��ע';
comment on column T_PA_BU_DLR_OUT_STORE.balance_status
  is 'ά���ɹ�������д�����־��';
comment on column T_PA_BU_DLR_OUT_STORE.balance_date
  is '����ʱ��';
comment on column T_PA_BU_DLR_OUT_STORE.car_brand_code
  is '����Ʒ��';
comment on column T_PA_BU_DLR_OUT_STORE.car_license
  is '���ƺ�';
comment on column T_PA_BU_DLR_OUT_STORE.relate_out_store_code
  is '�������ⵥ�ţ���Գ����˻�������ԭ���ⵥ��';
comment on column T_PA_BU_DLR_OUT_STORE.relate_out_store_link_code
  is '��������Դ���ţ���Գ����˻������湤���š��ۺ�Ʒ���š��³���Ʒ����';
comment on column T_PA_BU_DLR_OUT_STORE.dlr_id
  is 'רӪ��ID';
comment on column T_PA_BU_DLR_OUT_STORE.creator
  is '������';
comment on column T_PA_BU_DLR_OUT_STORE.created_date
  is '����ʱ��';
comment on column T_PA_BU_DLR_OUT_STORE.modifier
  is '��������Ա';
comment on column T_PA_BU_DLR_OUT_STORE.last_updated_date
  is '������ʱ��';
comment on column T_PA_BU_DLR_OUT_STORE.is_enable
  is '�Ƿ����';
comment on column T_PA_BU_DLR_OUT_STORE.sdp_user_id
  is 'SDP�û�ID';
comment on column T_PA_BU_DLR_OUT_STORE.sdp_org_id
  is 'SDP��֯ID';
comment on column T_PA_BU_DLR_OUT_STORE.update_control_id
  is '���������ֶ�';
comment on column T_PA_BU_DLR_OUT_STORE.picking_person_id
  is '������ID';
comment on column T_PA_BU_DLR_OUT_STORE.relate_out_store_id
  is '��������ID ��Գ����˻�������ԭ��������ID';
comment on column T_PA_BU_DLR_OUT_STORE.relate_out_store_link_id
  is '��������Դ��ID ��Գ����˻�������ԭ������Դ������ID������ά�޹���ID���ۺ�Ʒ���۵�ID���³���Ʒ���۵�ID';
comment on column T_PA_BU_DLR_OUT_STORE.bill_big_type
  is '���ݴ��� 3������ 4�������˻�';
comment on column T_PA_BU_DLR_OUT_STORE.applier_id
  is '������ID';
comment on column T_PA_BU_DLR_OUT_STORE.tax_rate
  is '˰��';
comment on column T_PA_BU_DLR_OUT_STORE.is_new_car_acce_out
  is '�Ƿ��³���Ʒ����  0��1��';
comment on column T_PA_BU_DLR_OUT_STORE.sale_order_code
  is '���۶�����';
comment on column T_PA_BU_DLR_OUT_STORE.car_type_code
  is '���ͱ���';
comment on column T_PA_BU_DLR_OUT_STORE.use_dept_id
  is 'ʹ�ò���ID';
comment on column T_PA_BU_DLR_OUT_STORE.draw_type_code
  is '�ڲ���������  �ڲ���������:�칫��Ʒ���г���Ʒ�����߼�...';
comment on column T_PA_BU_DLR_OUT_STORE.is_account_check
  is '�Ƿ��Ѷ���  0δ����  1�Ѷ��� 2������';
comment on column T_PA_BU_DLR_OUT_STORE.car_series_code
  is '��ϵ����';
comment on column T_PA_BU_DLR_OUT_STORE.transfer_in_dlr_id
  is '����רӪ��ID';
comment on column T_PA_BU_DLR_OUT_STORE.audit_flag
  is '��˱�־';
comment on column T_PA_BU_DLR_OUT_STORE.is_archived
  is '�鵵��־';
-- Create/Recreate indexes 
create index IDX01_T_PA_BU_DLR_OUT_STORE on T_PA_BU_DLR_OUT_STORE (OUT_STORE_CODE);
create index IDX9_T_PA_BU_DLR_OUT_STORE on T_PA_BU_DLR_OUT_STORE (DLR_ID, RELATE_ORDER_CODE)
  nologging  local;
-- Create/Recreate primary, unique and foreign key constraints 
alter table T_PA_BU_DLR_OUT_STORE
  add constraint PK_T_PA_BU_DLR_OUT_STORE1 primary key (OUT_STORE_ID)
;
-- Grant/Revoke object privileges 
grant select on T_PA_BU_DLR_OUT_STORE to DEVELOPERS_FOR_E3S;
grant select, insert, update, delete on T_PA_BU_DLR_OUT_STORE to MDS;
grant select, insert, update, delete on T_PA_BU_DLR_OUT_STORE to FI with grant option;
grant select, insert, update, delete, references, alter, index, debug on T_PA_BU_DLR_OUT_STORE to IFR;
