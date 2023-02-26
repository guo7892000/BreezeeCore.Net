using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Breezee.WorkHelper.DBTool.Entity;
using Breezee.Core.Entity;
using Breezee.Core.Interface;
using Breezee.Core.Tool;
using Breezee.Core.WinFormUI;
using Breezee.AutoSQLExecutor.Core;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ߣ��ƹ���
    /// �������ڣ�2013-10-19
    /// ����˵�����鿴��ά��Ĭ��ֵ���ų���
    /// </summary>
    public partial class FrmKeyValueSet : BaseForm
    {
        #region ����
        private DataBaseType _DataBaseType = DataBaseType.SqlServer; //Ĭ�����ݿ�����
        private string _strType = "TYPE"; //����
        private string _strKey = "KEY";
        private string _strValue = "VALUE";
        private string _strTitle = "";
        private string _FileName = "";
        private string _strDefalutTitleName = "��Ĭ��ֵ�ļ�ֵά��";
        private string _strExcludeTitleName = "�ų���ά��";
        private string _strOrcleName = "Oracle";
        private string _strSqlServerName = "SQL Server";
        private DataBaseType _DbTyp = DataBaseType.Oracle;
        private AutoSqlColumnSetType _ascst = AutoSqlColumnSetType.Default;
        private KeyValue _kv;
        #endregion

        public FrmKeyValueSet()
        {
            
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="DataBaseType"></param>
        public FrmKeyValueSet(DataBaseType dbt, AutoSqlColumnSetType ascst)
        {
            InitializeComponent();
            _DataBaseType = dbt;
            _kv = new KeyValue(dbt, ascst);
            _FileName = _kv.GetFileName();
            _DbTyp = dbt;
            _ascst = ascst;
            #region ��������
            if (dbt == DataBaseType.Oracle)
            {
                if (ascst == AutoSqlColumnSetType.Default)
                {
                    _strTitle = _strOrcleName + _strDefalutTitleName;
                }
                else
                {
                    _strTitle = _strOrcleName + _strExcludeTitleName;
                }
            }
            else
            {
                if (ascst == AutoSqlColumnSetType.Default)
                {
                    _strTitle = _strSqlServerName + _strDefalutTitleName;
                }
                else
                {
                    _strTitle = _strSqlServerName + _strExcludeTitleName;
                }
            }
            this.Text = _strTitle; 
            #endregion
        }

        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmVersionInfo_Load(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = _kv.LoadXMLFile();
            bs.Sort = "TYPE";
            dgvKey.DataSource = bs;
            dgvKey.Columns[_strType].Visible = false;
            dgvKey.Columns[_strKey].Width = 200;
            dgvKey.Columns[_strValue].Width = 200;
            if (_ascst == AutoSqlColumnSetType.Exclude)
            {
                dgvKey.Columns[_strValue].Visible = false;
            }
            
            //�û�����
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(((int)SqlType.Insert).ToString(), "����");
            dic.Add(((int)SqlType.Update).ToString(), "�޸�");
            dic.Add(((int)SqlType.Query).ToString(), "��ѯ");
            dic.Add(((int)SqlType.Delete).ToString(), "ɾ��");
            if (_ascst == AutoSqlColumnSetType.Default)
            {
                dic.Add(((int)SqlType.Parameter).ToString(), "����");
            }
            //��ʼ����������
            DataTable dtDataSetType = dic.GetTextValueTable(false);
            DataGridViewComboBoxColumn cmbUserType = new DataGridViewComboBoxColumn();
            cmbUserType.DisplayIndex = 0;
            cmbUserType.HeaderText = "��������";
            cmbUserType.DataPropertyName = "TYPE";//
            cmbUserType.ValueMember = "VALUE";
            cmbUserType.DisplayMember = "TEXT";
            cmbUserType.DataSource = dtDataSetType.Copy();
            dgvKey.Columns.Insert(1, cmbUserType);
            
        }

        /// <summary>
        /// ���水ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            groupBox1.Focus();
            BindingSource bs=(BindingSource)dgvKey.DataSource;
            bs.EndEdit();
            DataTable dt = (DataTable)bs.DataSource;
            DataTable dtSave = dt.GetChanges();
            if (dtSave==null || dtSave.Rows.Count == 0)
            {
                ShowInfo("û���޸����ݣ����ñ��棡");
                return;
            }
            _kv.SaveXMLFile(dtSave, _FileName);
            ShowInfo("����ɹ���");
        }

        #region �رհ�ť�¼�
        /// <summary>
        /// �رհ�ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        } 
        #endregion

        #region �������ݳ����¼�
        /// <summary>
        /// �������ݳ����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvKey_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ShowInfo(e.Exception.Message);
        } 
        #endregion

    }
}