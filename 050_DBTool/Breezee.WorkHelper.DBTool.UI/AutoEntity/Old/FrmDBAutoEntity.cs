using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Breezee.WorkHelper.DBTool.Entity;
using Breezee.Core.WinFormUI;
using Breezee.Core.Entity;
using Breezee.WorkHelper.DBTool.IBLL;
using Breezee.Core.Tool;
using Breezee.Core.Interface;
using Breezee.Core.IOC;
using System.IO;
using Breezee.AutoSQLExecutor.Core;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// ����ʵ��
    /// ���Խ����ͨ��
    /// </summary>
	public partial class FrmDBAutoEntity: BaseForm
	{
        #region ����
        private DbServerInfo _dbServer;
        private List<EntityInfo> entityList;
        private AutoEntityBiz _entiyBiz;
        private DataSet dataTables = null;
        private TreeViewEventArgs treeNode;
        private List<EntityInfo> list = new List<EntityInfo>();
        private IDBConfigSet _IDBConfigSet;
        //
        string _sTeamplateEntity = "0";
        string _sStringColumnList = "1";
        string _sStringColumn = "2";
        string _sSimpleEntity = "3";

        //
        private IDataAccess _dataAccess;
        #endregion

        #region ���캯��
        public FrmDBAutoEntity()
        {
            InitializeComponent();
        } 
        #endregion

        #region �����¼�
        private void FrmAutoEntity_Load(object sender, EventArgs e)
        {
            Text = "����ʵ�����ļ�";
            ctxmBuilt.Visible = false;
            txtAccess.Text = "public";
            txtClassPreString.Text = "DT_";
            //�������ݿ����ӿؼ�
            _IDBConfigSet = ContainerContext.Container.Resolve<IDBConfigSet>();
            DataTable dtConn = _IDBConfigSet.QueryDbConfig(_dicQuery).SafeGetDictionaryTable();
            uC_DbConnection1.SetDbConnComboBoxSource(dtConn);
            uC_DbConnection1.IsDbNameNotNull = false;

            #region ��������
            //��¼����
            _dicString.Add(_sTeamplateEntity, "ģ��");
            _dicString.Add(_sStringColumnList, "�ַ������ͼ���"); 
            _dicString.Add(_sStringColumn, "�ַ�����");
            _dicString.Add(_sSimpleEntity, "��ʵ��");
            cbbEntityType.BindTypeValueDropDownList(_dicString.GetTextValueTable(false), false, true);
            cbbEntityType.SelectedIndex = 0;
            #endregion

            //����ģ���ַ���Ϣ
            //rtbParamList.AppendText(File.ReadAllText(UIHelper.GetSystemFullPath(DBTGlobalValue.AutoEntity.Param),System.Text.Encoding.Default));
            //rtbTable.AppendText(File.ReadAllText(UIHelper.GetSystemFullPath(DBTGlobalValue.AutoEntity.Table), System.Text.Encoding.Default));
            //rtbColumn.AppendText(File.ReadAllText(UIHelper.GetSystemFullPath(DBTGlobalValue.AutoEntity.ColumnProp), System.Text.Encoding.Default));
            //�����������Ϣ
            tvDataBaseInfo.Font = new Font("������", 10f, FontStyle.Bold);
            dgvTableInfo.ReadOnly = true;
            dgvTableInfo.AutoGenerateColumns = false;
            dgvTableInfo.AllowUserToAddRows = false;
            dgvTableInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEntityInfo.ReadOnly = true;
            dgvEntityInfo.AutoGenerateColumns = false;
            dgvEntityInfo.AllowUserToAddRows = false;
        } 
        #endregion

        #region �������Ӱ�ť�¼�
        private void btnLinkServer_Click(object sender, EventArgs e)
        {
            try
            {
                _dbServer = uC_DbConnection1.GetDbServerInfo();
                if (_dbServer == null)
                {
                    return;
                }

                _entiyBiz = new AutoEntityBiz(_dbServer.DatabaseType, _dbServer);
                _dataAccess = AutoSQLExecutor.Common.AutoSQLExecutors.Connect(_dbServer);
                //�������ݿ�
                LoadTreeView(_dbServer, _entiyBiz);

                dgvEntityInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                tabControl1.SelectedTab = tpgEntity;
            }
            catch (Exception exception)
            {
                ShowErr(exception.Message);
            }
        } 
        #endregion

        #region �رհ�ť�¼�
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        } 
        #endregion

        #region ѡ��·����ť�¼�
        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dialog.SelectedPath;
            }
        } 
        #endregion

        #region ����ı�����
        public bool checkTxt()
        {
            if ((((txtAccess.Text.Trim() == null) || (txtAccess.Text.Trim() == "")) || ((txtNameSpace.Text.Trim() == null) || (txtNameSpace.Text.Trim() == ""))) || (txtClassEndString.Text.Trim() == null))
            {
                ShowErr("�������η��������ռ䲻��Ϊ�գ�");
                return false;
            }
            return true;
        } 
        #endregion

        #region ����������
        private void LoadTreeView(DbServerInfo server, AutoEntityBiz entiyBiz)
        {
            try
            {
                string collectionName = DBSchemaString.Databases;
                string[] restrictionValues = null;
                switch (server.DatabaseType)
                {
                    case DataBaseType.SqlServer:
                        collectionName = DBSchemaString.Databases;
                        break;
                    case DataBaseType.Oracle:
                        collectionName = DBSchemaString.Users;
                        break;
                    case DataBaseType.MySql:
                        collectionName = DBSchemaString.Databases;
                        break;
                    case DataBaseType.SQLite:
                        collectionName = DBSchemaString.Tables;//��SQLite��ֻ��Ψһ��Main���ݿ⣬û�����ݿ��嵥��
                        break;
                    case DataBaseType.PostgreSql:
                        collectionName = DBSchemaString.Databases;
                        break;
                    default:
                        break;
                }
                //�õ��ܹ�
                DataTable schema = entiyBiz.GetSchema(collectionName, null);
                //DataTable schema = _dataAccess.GetSchema(collectionName, null);
                tvDataBaseInfo.Nodes.Clear();
                TreeNode node = new TreeNode();
                node.ToolTipText = "������ѡ���ڰ�����Ҽ�";
                node.Text = server.ServerName;
                for (int i = 0; i < schema.Rows.Count; i++)
                {
                    TreeNode node2 = new TreeNode();
                    string str2 = schema.Rows[i][0].ToString();
                    if (server.DatabaseType == DataBaseType.MySql)
                    {
                        str2 = schema.Rows[i][1].ToString();
                    }
                    server.Database = str2;
                    if (server.DatabaseType == DataBaseType.Oracle)
                    {
                        restrictionValues = new string[] { str2 };
                    }
                    else if (server.DatabaseType == DataBaseType.MySql)
                    {
                        string[] strArray2 = new string[2];
                        strArray2[1] = str2;
                        restrictionValues = strArray2;
                    }
                    //�õ����嵥
                    DataTable table2 = entiyBiz.GetSchema("Tables", restrictionValues);
                    //DataTable table2 = _dataAccess.GetSchemaTables();
                    DataView dv = table2.DefaultView;
                    table2.DefaultView.Sort = "TABLE_NAME ASC";
                    for (int j = 0; j < dv.Count; j++)
                    {
                        string str3 = "";
                        if (server.DatabaseType == DataBaseType.Oracle)
                        {
                            str3 = dv[j][1].ToString();
                        }
                        else
                        {
                            str3 = dv[j][2].ToString();
                        }
                        TreeNode node3 = new TreeNode();
                        node3.ToolTipText = "������ѡ���ڰ�����Ҽ�";
                        node3.Text = str3;
                        node2.Nodes.Add(node3);
                    }
                    node2.ToolTipText = "������ѡ���ڰ�����Ҽ�";
                    node2.Text = str2;
                    node.Nodes.Add(node2);
                    //����SQLite����ֹѭ��
                    if (server.DatabaseType == DataBaseType.SQLite)
                    {
                        break;
                    }
                }
                tvDataBaseInfo.Nodes.Add(node);

                //չ��Ĭ�ϵ����ݿ����
                string strDefuatExpandDBName = server.Database;
                foreach (TreeNode tnode in tvDataBaseInfo.Nodes)
                {
                    ExpandDefaultDB(tnode, strDefuatExpandDBName);
                }
            }
            catch (Exception exception)
            {
                ShowErr(exception.Message);
            }
        } 
        #endregion

        #region չ��Ĭ�����ݿ����ͼ����
        /// <summary>
        /// �ҵ�Ĭ�����ݿⲢչ��
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="strDefuatExpandDBName"></param>
        private void ExpandDefaultDB(TreeNode tn, string strDefuatExpandDBName)
        {
            if (tn.Text == strDefuatExpandDBName)
            {
                tn.ExpandAll();
                ExpandParentNode(tn);
            }
            else
            {
                foreach (TreeNode node in tn.Nodes)
                {
                    ExpandDefaultDB(node, strDefuatExpandDBName);
                }
            }
        }

        /// <summary>
        /// չ�����ڵ�
        /// </summary>
        /// <param name="node"></param>
        private void ExpandParentNode(TreeNode node)
        {
            if (node.Parent != null)
            {
                node.Parent.Expand();
                ExpandParentNode(node.Parent);
            }
        } 
        #endregion

        #region �����������Ҽ��˵��¼�
        private void tsmiStable_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTableInfo.DataSource != null)
                {
                    DataSet dataSet = new DataSet();
                    DataTable dataSource = (DataTable)dgvTableInfo.DataSource;
                    dataSet.Tables.Add(dataSource);
                    if (txtPath.Text == "")
                    {
                        ShowInfo("�������ô��·��!");
                    }
                    else if (checkTxt() && (treeNode.Node.Level == 2))
                    {
                        string sEntityType = cbbEntityType.SelectedValue.ToString();
                        if (sEntityType == _sStringColumnList)
                        {
                            entityList = _entiyBiz.BuildCustomEntity(dataSet, txtNameSpace.Text.Trim(), txtAccess.Text.Trim(), txtClassPreString.Text.Trim(), txtClassEndString.Text.Trim(), txtPath.Text.Trim());
                        }
                        else if (sEntityType == _sStringColumn)
                        {
                            entityList = _entiyBiz.BuildTableColumnNameEntity(dataSet, txtNameSpace.Text.Trim(), txtAccess.Text.Trim(), txtClassPreString.Text.Trim(), txtClassEndString.Text.Trim(), txtPath.Text.Trim());
                        }
                        else
                        {
                            entityList = _entiyBiz.BuildEntity(dataSet, txtNameSpace.Text.Trim(), txtAccess.Text.Trim(), txtClassPreString.Text.Trim(), txtClassEndString.Text.Trim(), txtPath.Text.Trim());
                        }
                        //
                        if (entityList.Count > 0)
                        {
                            dgvEntityInfo.DataSource = entityList;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ShowErr(exception.Message);
            }
        } 
        #endregion

        #region ȫ���������Ҽ��˵��¼�
        private void tsmiFtable_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPath.Text == "")
                {
                    ShowInfo("�������ô��·��!");
                }
                else if (checkTxt() && (treeNode.Node.Level == 1))
                {
                    string sEntityType = cbbEntityType.SelectedValue.ToString();
                    if (sEntityType == _sStringColumnList)
                    {
                        entityList = _entiyBiz.BuildCustomEntity(dataTables, txtNameSpace.Text.Trim(), txtAccess.Text.Trim(), txtClassPreString.Text.Trim(), txtClassEndString.Text.Trim(), txtPath.Text.Trim());
                    }
                    else if (sEntityType == _sStringColumn)
                    {
                        entityList = _entiyBiz.BuildTableColumnNameEntity(dataTables, txtNameSpace.Text.Trim(), txtAccess.Text.Trim(), txtClassPreString.Text.Trim(), txtClassEndString.Text.Trim(), txtPath.Text.Trim());
                    }
                    else
                    {
                        entityList = _entiyBiz.BuildEntity(dataTables, txtNameSpace.Text.Trim(), txtAccess.Text.Trim(), txtClassPreString.Text.Trim(), txtClassEndString.Text.Trim(), txtPath.Text.Trim());
                    }
                    
                    if (entityList.Count > 0)
                    {
                        dgvEntityInfo.DataSource = entityList;
                    }
                }
            }
            catch (Exception exception)
            {
                ShowErr(exception.Message);
            }
        } 
        #endregion

        #region ѡ�������¼�
        private void tvDataBaseInfo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e != null)
            {
                treeNode = e;
            }
        } 
        #endregion

        #region ��������¼�
        private void tvDataBaseInfo_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (treeNode != null)
                {
                    if ((treeNode.Node.Level == 1) || (treeNode.Node.Level == 2))
                    {
                        if (treeNode.Node.IsSelected && (treeNode.Node.Level == 1))
                        {
                            if (treeNode.Node.Nodes.Count > 0)
                            {
                                tsmiStable.Visible = false;
                                tsmiFtable.Visible = true;
                            }
                            else
                            {
                                ctxmBuilt.Visible = false;
                            }
                        }
                        else
                        {
                            tsmiFtable.Visible = false;
                            tsmiStable.Visible = true;
                        }
                    }
                    else
                    {
                        ctxmBuilt.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                ShowErr(exception.Message);
            }
        } 
        #endregion

        #region ����굥���¼�
        private void tvDataBaseInfo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DataTable table = null;
            try
            {
                if (e.Node.Level == 2)
                {
                    _dbServer.Database = e.Node.Parent.Text;
                    table = _entiyBiz.GetMemorySTable(e.Node.Parent.Text, e.Node.Text, _dbServer);
                }
                else if ((e.Node.Level == 1) && (e.Node.Nodes.Count > 0))
                {
                    _dbServer.Database = e.Node.Text;
                    DataTable schema = null;
                    if (_dbServer.DatabaseType == DataBaseType.Oracle) 
                    {
                        schema = _entiyBiz.GetSchema("Tables", new string[] { _dbServer.Database });
                    }
                    else
                    {
                        schema = _entiyBiz.GetSchema("Tables", null);
                    }
                    dataTables = new DataSet();
                    for (int i = 0; i < schema.Rows.Count; i++)
                    {
                        string tblName = "";
                        if (_dbServer.DatabaseType == DataBaseType.Oracle)
                        {
                            tblName = schema.Rows[i][1].ToString();
                        }
                        else
                        {
                            tblName = schema.Rows[i][2].ToString();
                        }
                        dataTables.Tables.Add(_entiyBiz.GetMemorySTable(_dbServer.Database, tblName, _dbServer));
                    }
                }
                if (table != null)
                {
                    dgvTableInfo.DataSource = table;
                }
            }
            catch (Exception exception)
            {
                ShowErr(exception.Message);
            }
        } 
        #endregion

    
	}
}