/*������*/
CREATE TABLE OrderTest(
	ID int not null,
	OrderCode varchar(30),
	Remark
);

/*ɾ����*/
 DROP TABLE OrderTest;

/*���±������ֶ�*/
ALTER TABLE OrderTest ADD COLUMN OrderDate Date not null;

/*���±�ɾ���ֶ�*/
ALTER TABLE OrderTest DROP COLUMN OrderDate;

