/*��������*/
INSERT INTO OrderTest(ID,OrderCode,Remark)
SELECT 1,'','Test';

/*��������*/
UPDATE OrderTest
SET Remark = 'Test'
WHERE ID = '1';

/*ɾ������*/
DELETE FROM OrderTest
WHERE ID = '1';

