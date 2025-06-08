using System;
using System.Data;
using System.Windows.Forms;
using Breezee.Core.Entity;
using Breezee.WorkHelper.DBTool.IBLL;
using Breezee.Core.Tool;
using Breezee.WorkHelper.DBTool.Entity;
using Breezee.Core.IOC;
using Breezee.Core.WinFormUI;
using System.IO;
using System.Reflection;
using Breezee.Core.Interface;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ߣ��ƹ���
    /// �������ڣ�2013-10-19
    /// ����˵�������ݿ���������
    /// </summary>
    public partial class FrmDBConfigSet : BaseForm
    {
        #region ����
        IDBConfigSet _IDBConfigSet;
        DataTable dtIsEnabel;
        DataTable dtDbType;
        bool _IsMoreSelected = false; //�Ƿ��ѡ��Ĭ��Ϊ��
        public bool In_IsMoreSelected
        {
            get { return _IsMoreSelected; }
            set { _IsMoreSelected = value; }
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="DataBaseType"></param>
        public FrmDBConfigSet()
        {
            InitializeComponent();
        }
        #endregion

        #region ҳ�����
        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmVersionInfo_Load(object sender, EventArgs e)
        {
            _IDBConfigSet = ContainerContext.Container.Resolve<IDBConfigSet>();

            SetTag();//����Tag
            //���ݿ�����
            dtDbType = DBToolUIHelper.GetBaseDataTypeTable();
            cbbDbType.BindTypeValueDropDownList(dtDbType, true, true);
            //״̬
            _dicString["1"] = "����";
            _dicString["0"] = "����";
            dtIsEnabel = _dicString.GetTextValueTable(false);
            cbbStatus.BindTypeValueDropDownList(dtIsEnabel.Copy(), true, true);
        }
        #endregion

        #region ����Tag����
        private void SetTag()
        {
            //��ѯ���
            FlexGridColumnDefinition fdc = new FlexGridColumnDefinition();
            fdc.AddColumn("ROWNO", "���", DataGridViewColumnTypeEnum.TextBox, true, 40, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn("IS_SELECTED", "ѡ��", DataGridViewColumnTypeEnum.CheckBox, _IsMoreSelected, 60, DataGridViewContentAlignment.MiddleCenter, true, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.DB_CONFIG_CODE, "���ñ���", DataGridViewColumnTypeEnum.TextBox, true, 80, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.DB_CONFIG_NAME, "��������", DataGridViewColumnTypeEnum.TextBox, true, 100, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.DB_TYPE, "���ݿ�����", DataGridViewColumnTypeEnum.ComboBox, true, 160, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.SERVER_IP, "������IP", DataGridViewColumnTypeEnum.TextBox, true, 260, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.DB_NAME, "���ݿ�����", DataGridViewColumnTypeEnum.TextBox, true, 160, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.IS_ENABLED, "״̬", DataGridViewColumnTypeEnum.ComboBox, true, 80, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.SORT_ID, "�����", DataGridViewColumnTypeEnum.TextBox, true, 80, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.REMARK, "��ע", DataGridViewColumnTypeEnum.TextBox, true, 160, DataGridViewContentAlignment.MiddleLeft, false, 800);
            fdc.AddColumn(DT_DBT_BD_DB_CONFIG.SqlString.CREATE_TIME, "��������", DataGridViewColumnTypeEnum.TextBox, true, 100, DataGridViewContentAlignment.MiddleLeft, false, 800);
            dgvQuery.Tag = fdc.GetGridTagString();
            dgvQuery.BindDataGridView(null, true);           
        }
        #endregion

        #region ��ѯ��ť�¼�
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            _dicQuery[IDBConfigSet.QueryDbConfig_InDicKey.DB_TYPE] = cbbDbType.SelectedValue.ToString();
            _dicQuery[IDBConfigSet.QueryDbConfig_InDicKey.DB_CONFIG_CODE] = txbConfigCode.Text.Trim();
            _dicQuery[IDBConfigSet.QueryDbConfig_InDicKey.DB_NAME] = txbDbName.Text.Trim();
            _dicQuery[IDBConfigSet.QueryDbConfig_InDicKey.IS_ENABLED] = cbbStatus.SelectedValue.ToString();
            DataTable dtQuery = _IDBConfigSet.QueryDbConfig(_dicQuery).SafeGetDictionaryTable();
            dgvQuery.BindDataGridView(dtQuery);

            //״̬
            var cmbBig = (DataGridViewComboBoxColumn)dgvQuery.Columns[DT_DBT_BD_DB_CONFIG.SqlString.IS_ENABLED];
            cmbBig.ValueMember = DT_BAS_VALUE.VALUE_CODE;
            cmbBig.DisplayMember = DT_BAS_VALUE.VALUE_NAME;
            cmbBig.DataSource = dtIsEnabel;
            //���ݿ�����
            cmbBig = (DataGridViewComboBoxColumn)dgvQuery.Columns[DT_DBT_BD_DB_CONFIG.SqlString.DB_TYPE];
            cmbBig.ValueMember = DT_BAS_VALUE.VALUE_CODE;
            cmbBig.DisplayMember = DT_BAS_VALUE.VALUE_NAME;
            cmbBig.DataSource = dtDbType;
        } 
        #endregion

        #region ѡ��ť�¼�
        private void tsbSelect_Click(object sender, EventArgs e)
        {
            try
            {
                dgvQuery.EndEdit();

                BindingSource bs = (BindingSource)dgvQuery.DataSource;
                if (bs == null)
                {
                    ShowInfo("û��ѡ��һ�����ݣ����Ȳ�ѯ��");
                    return;
                }
                bs.EndEdit();
                DataTable dtSelect;
                if (_IsMoreSelected)
                {
                    DataTable roleList = (DataTable)bs.DataSource;
                    dtSelect = roleList.Select("IS_SELECTED = 'True'").CopyToDataTable();
                }
                else
                {
                    DataRow dr = (bs.Current as DataRowView).Row;
                    dtSelect = dr.Table.Clone();
                    dtSelect.ImportRow(dr);
                }
                if (dtSelect.Rows.Count == 0)
                {
                    ShowInfo("������ѡ��һ�����ݣ�");
                    return;
                }
                Tag = dtSelect;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                ShowInfo(ex.Message);
            }
        }
        #endregion

        #region ������ť�¼�
        private void tsbNew_Click(object sender, EventArgs e)
        {
            FrmDBConfigSet_D frm = new FrmDBConfigSet_D();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                tsbQuery.PerformClick();
            }
        } 
        #endregion

        #region �༭��ť�¼�
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            DataRow drSelect = dgvQuery.GetCurrentRow();
            if (drSelect == null)
            {
                ShowErr("��ѡ��һ�����ݣ�");
                return;
            }
            FrmDBConfigSet_D frm = new FrmDBConfigSet_D(drSelect);
            if(frm.ShowDialog() == DialogResult.OK)
            {
                tsbQuery.PerformClick();
            }
        } 
        #endregion

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

        #region ����˫��
        private void dgvQuery_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }
        #endregion

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow drSelect = dgvQuery.GetCurrentRow();
            if (drSelect == null)
            {
                ShowErr("��ѡ��һ�����ݣ�");
                return;
            }
            if (MsgHelper.ShowYesNo("ȷ��Ҫɾ�������ã�") == DialogResult.Yes)
            {
                _dicString[IDBConfigSet.DeleteDbConfig_InDicKey.DB_CONFIG_ID] = drSelect[DT_DBT_BD_DB_CONFIG.SqlString.DB_CONFIG_ID].ToString();
                _IDBConfigSet.DeleteDbConfig(_dicString).SafeGetDictionary();
                ShowInfo("ɾ���ɹ���");
                tsbQuery.PerformClick();
            }
        }
    }
}