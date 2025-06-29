/*UUID�����ӷ�*/
SELECT gen_random_uuid() as UUID,'A'||'B' AS "�����ַ�"
;

/*ʹ�õݹ��ѯ��ʵ�ֲ�νṹ���ݵĲ�ѯ��������֯�ṹ������Ŀ¼�ȡ��ݹ��ѯͨ����WITH RECURSIVE�Ӿ�һ��ʹ�á�
����ʹ�ö���ݹ顣*/
WITH RECURSIVE subordinates AS (
    -- ��ʼ����ѯ��ѡ��ֱ������
    SELECT id, name, manager_id
    FROM employees
    WHERE manager_id = 1  -- ���ض�Ա����ʼ������Ա��IDΪ1
    UNION ALL
    -- �ݹ鲿�֣�ѡ�����м������
    SELECT e.id, e.name, e.manager_id
    FROM employees e
    INNER JOIN subordinates s ON e.manager_id = s.id
),subordinatesB AS (
    -- ��ʼ����ѯ��ѡ��ֱ������
    SELECT id, name, manager_id
    FROM employees
    WHERE manager_id = 2  -- ���ض�Ա����ʼ������Ա��IDΪ2
    UNION ALL
    -- �ݹ鲿�֣�ѡ�����м������
    SELECT e.id, e.name, e.manager_id
    FROM employees e
    INNER JOIN subordinatesB s ON e.manager_id = s.id
)
SELECT * FROM subordinates
;

/*�ֶ����*/
SELECT get_no, STRING_AGG(category_name , ',') AS names
FROM t_ebom_db_material
GROUP BY get_no;

/*ʱ�䷶Χ��ѯ*/
SELECT * FROM subordinates
where 1=1
and a.created_date > to_date('#created_date_beg#', 'yyyy-mm-dd')
and a.created_date < to_date('#created_date_end#', 'yyyy-mm-dd') + interval '1 day'
;

/*���ֶ�Ϊ��ʱȡĬ��ֵ*/
select COALESCE(is_tax_machine, '0')
from t_mathine
;

/*����ת��*/
select (A.qty*A.price)::numeric(14,4) AS total_amount
from t_out_store_d A
;


