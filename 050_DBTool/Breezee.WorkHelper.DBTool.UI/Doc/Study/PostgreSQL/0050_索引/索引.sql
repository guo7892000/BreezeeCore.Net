-- ����һ������
CREATE INDEX idx_t_pbom_db_material_material_name ON public.t_pbom_db_material (material_name);

-- ����Ψһ����
CREATE UNIQUE INDEX uk_t_pbom_db_material_material_code ON public.t_pbom_db_material (material_code);

--ɾ������������һ��������Ψһ������
DROP INDEX public.t_pbom_db_material_material_name_idx;


-- �����������ڴ�����ʱֱ�Ӷ�������
CREATE TABLE employees (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100),
    department VARCHAR(100)
);
-- �������������������ʹ�õ����� PRIMARY KEY Լ��
CREATE TABLE employees (
    id INT,
    name VARCHAR(100),
    department VARCHAR(100)
);
ALTER TABLE employees ADD PRIMARY KEY (id);
-- ����������ʹ�ø�������
CREATE TABLE employee_department (
    employee_id INT,
    department_id INT,
    PRIMARY KEY (employee_id, department_id)
);

-- ɾ������
ALTER TABLE your_table_name DROP CONSTRAINT constraint_name;
