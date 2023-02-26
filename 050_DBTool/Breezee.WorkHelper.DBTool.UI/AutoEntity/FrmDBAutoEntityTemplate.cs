using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Breezee.WorkHelper.DBTool.Entity;
using Breezee.Core.Entity;
using Breezee.WorkHelper.DBTool.IBLL;
using Breezee.Core.Tool;
using Breezee.Core.Interface;
using Breezee.Core.IOC;
using System.IO;
using System.Text;
using Setting = Breezee.WorkHelper.DBTool.UI.Properties.Settings;
using Breezee.Core.WinFormUI;
using Breezee.AutoSQLExecutor.Core;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// ����ʵ��
    /// ���Խ����ͨ��
    /// </summary>
	public partial class FrmDBAutoEntityTemplate : BaseForm
	{
        #region ����
        private DbServerInfo _dbServer;
        private TreeNode _selectedNode;
        private List<EntityInfo> list = new List<EntityInfo>();
        private IDBConfigSet _IDBConfigSet;
        private BindingSource bsTable = new BindingSource();
        //
        //private ICustomDataAccess _customDataAccess = ContainerContext.Container.Resolve<ICustomDataAccess>();
        private IDataAccess _dataAccess;
        string sTip = "�������һ���ڵ���ٰ�����Ҽ�ѡ������";
        String sGenPath = "GenPath";
        DataTable _dtGen;
        //
        MiniXmlCommon _miniXml;
        List<string> _lstCol;
        DataTable _dtSet;
        #endregion

        #region ���캯��
        public FrmDBAutoEntityTemplate()
        {
            InitializeComponent();
        } 
        #endregion

        #region �����¼�
        private void FrmAutoEntity_Load(object sender, EventArgs e)
        {
            Text = "����ʵ�����ļ�";
            ctxmBuilt.Visible = false;
            //��ȡ��������
            txtPath.Text = Setting.Default.AutoEntity_Path;
            txtClassPreString.Text = Setting.Default.AutoEntity_ClassPre;
            txtClassEndString.Text = Setting.Default.AutoEntity_ClassEnd;
            txbPropertyPre.Text = Setting.Default.AutoEntity_PropertyPre;

            //�������ݿ����ӿؼ�
            _IDBConfigSet = ContainerContext.Container.Resolve<IDBConfigSet>();
            DataTable dtConn = _IDBConfigSet.QueryDbConfig(_dicQuery).SafeGetDictionaryTable();
            uC_DbConnection1.SetDbConnComboBoxSource(dtConn);
            uC_DbConnection1.IsDbNameNotNull = false;
            //����ģ���ַ���Ϣ
            rtbParamList.AppendText(File.ReadAllText(GetSystemFullPath(DBTGlobalValue.AutoEntity.Param),System.Text.Encoding.Default));
            rtbTable.AppendText(File.ReadAllText(GetSystemFullPath(DBTGlobalValue.AutoEntity.Table), System.Text.Encoding.Default));
            rtbColumnProp.AppendText(File.ReadAllText(GetSystemFullPath(DBTGlobalValue.AutoEntity.ColumnProp), System.Text.Encoding.Default));
            rtbColumnStr.AppendText(File.ReadAllText(GetSystemFullPath(DBTGlobalValue.AutoEntity.ColumnStr), System.Text.Encoding.Default));
            //����Ϣ����
            tvDataBaseInfo.Font = new Font("������", 10f, FontStyle.Bold);
            dgvTableInfo.ReadOnly = true;
            dgvTableInfo.AutoGenerateColumns = false;
            dgvTableInfo.AllowUserToAddRows = false;
            dgvTableInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEntityInfo.ReadOnly = true;
            dgvEntityInfo.AutoGenerateColumns = false;
            dgvEntityInfo.AllowUserToAddRows = false;
            FlexGridColumnDefinition fdc = new FlexGridColumnDefinition();//����Tag
            fdc.AddColumn(
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.TableName).Name(DBColumnEntity.SqlString.TableName).Caption("����").Type().Align().Width(150).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.NameCN).Name(DBColumnEntity.SqlString.NameCN).Caption("������").Type().Align().Width(150).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.Name).Name(DBColumnEntity.SqlString.Name).Caption("����").Type().Align().Width(150).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.DataTypeFull).Name(DBColumnEntity.SqlString.DataTypeFull).Caption("���ͺʹ�С").Type().Align().Width(200).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.DataPrecision).Name(DBColumnEntity.SqlString.DataPrecision).Caption("����").Type().Align().Width(80).Visible().Build(), 
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.DataScale).Name(DBColumnEntity.SqlString.DataScale).Caption("С��λ��").Type().Align().Width(80).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.Default).Name(DBColumnEntity.SqlString.Default).Caption("Ĭ��ֵ").Type().Align().Width(80).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.KeyType).Name(DBColumnEntity.SqlString.KeyType).Caption("����").Type().Align().Width(80).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.NotNull).Name(DBColumnEntity.SqlString.NotNull).Caption("�ǿ�").Type().Align().Width(80).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.Extra).Name(DBColumnEntity.SqlString.Extra).Caption("��ע").Type().Align().Width(200).Visible().Build()
            );
            dgvTableInfo.Tag = fdc.GetGridTagString();
            //������������
            _lstCol = new List<string>() { AutoEntityString.Set.Col_Name, AutoEntityString.Set.Col_Notnull, AutoEntityString.Set.Col_Null };
            _miniXml = new MiniXmlCommon(DBTGlobalValue.AutoEntity.Xml_Column, _lstCol, AutoEntityString.Set.Col_Name, AutoEntityString.Set.RootItemName, AutoEntityString.Set.ChildItemName);
            _dtSet = _miniXml.LoadXMLFile();
            fdc = new FlexGridColumnDefinition();//����Tag
            fdc.AddColumn(
            new FlexGridColumn.Builder().DBName(AutoEntityString.Set.Col_Name).Name(AutoEntityString.Set.Col_Name).Caption("����").Type().Align().Width(200).Edit().Visible().MaxLen(100).Build(),
            new FlexGridColumn.Builder().DBName(AutoEntityString.Set.Col_Notnull).Name(AutoEntityString.Set.Col_Notnull).Caption("�ǿ�ʱ").Type().Align().Width(400).Edit().Visible().MaxLen(100).Build(),
            new FlexGridColumn.Builder().DBName(AutoEntityString.Set.Col_Null).Name(AutoEntityString.Set.Col_Null).Caption("��ֵʱ").Type().Align().Width(300).Edit().Visible().MaxLen(100).Build()
            );
            dgvParamSet.Tag = fdc.GetGridTagString();
            dgvParamSet.BindDataGridView(_dtSet, true);
            dgvParamSet.AllowUserToAddRows = true;
            //����ʵ������
            _dtGen = new DataTable();
            _dtGen.Columns.Add(new DataColumn(sGenPath));
            fdc = new FlexGridColumnDefinition();
            fdc.AddColumn(
            new FlexGridColumn.Builder().DBName(sGenPath).Name(sGenPath).Caption("����·��").Type().Align().Width(500).Visible().Build()
            );
            dgvEntityInfo.Tag = fdc.GetGridTagString();
            dgvEntityInfo.BindDataGridView(_dtGen, true);

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
                _dataAccess = AutoSQLExecutor.Common.AutoSQLExecutors.Connect(_dbServer);
                //�������ݿ�
                LoadTreeView(_dbServer);

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
                Setting.Default.AutoEntity_Path = dialog.SelectedPath;
                Setting.Default.Save();
            }
        } 
        #endregion


        #region ����������
        private void LoadTreeView(DbServerInfo server)
        {
            try
            {
                //�õ��ܹ�
                DataTable schema = _dataAccess.GetDataBases();
                tvDataBaseInfo.Nodes.Clear();
                TreeNode ndServer = new TreeNode();
                ndServer.ToolTipText = sTip;
                ndServer.Text = server.ServerName;
                ndServer.Tag = server;

                for (int i = 0; i < schema.Rows.Count; i++)
                {
                    TreeNode ndDB = new TreeNode();
                    string sDBName = schema.Rows[i][DBDataBaseEntity.SqlString.Name].ToString();
                    string sSchema = sDBName;
                    if (server.DatabaseType == DataBaseType.SqlServer)
                    {
                        server.Database = sDBName;
                        _dataAccess.ModifyConnectString(server);
                        sSchema = "";
                    }
                    //�õ����嵥
                    DataTable dtTables = _dataAccess.GetSqlSchemaTables(null, sSchema);
                    DataView dv = dtTables.DefaultView;
                    dtTables.DefaultView.Sort = "TABLE_NAME ASC";
                    for (int j = 0; j < dv.Count; j++)
                    {
                        TreeNode ndTable = new TreeNode();
                        ndTable.ToolTipText = sTip;
                        ndTable.Text = dv[j][DBTableEntity.SqlString.Name].ToString();
                        //���浱ǰ������
                        DataTable[] arrTables = new DataTable[2];
                        DataTable dtTable = DBTableEntity.GetTableStruct();
                        dtTable.ImportRow(dv[j].Row);
                        arrTables[0] = dtTable;
                        ndTable.Tag = arrTables;

                        ndDB.Nodes.Add(ndTable);
                    }
                    ndDB.ToolTipText = sTip;
                    ndDB.Text = sDBName;
                    ndDB.Tag = dtTables;//����������
                    ndServer.Nodes.Add(ndDB);
                }
                tvDataBaseInfo.Nodes.Add(ndServer);
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
                BuildTemplateEntity();
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
                BuildTemplateEntity();
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
                _selectedNode = e.Node;
            }
        } 
        #endregion

        #region ��������¼�
        private void tvDataBaseInfo_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (_selectedNode == null) return;
                if ((_selectedNode.Level == 1) || (_selectedNode.Level == 2))
                {
                    if (_selectedNode.IsSelected && (_selectedNode.Level == 1))
                    {
                        if (_selectedNode.Nodes.Count > 0)
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
            catch (Exception exception)
            {
                ShowErr(exception.Message);
            }
        } 
        #endregion

        #region ����굥���¼�
        private void tvDataBaseInfo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Level == 2)
                {
                    DataTable[] dtArr = e.Node.Tag as DataTable[];
                    if (dtArr[1] == null)
                    {
                        _dbServer.Database = e.Node.Parent.Text;
                        _dataAccess.ModifyConnectString(_dbServer);
                        dtArr[1] = _dataAccess.GetSqlSchemaTableColumns(e.Node.Text);
                        dgvTableInfo.BindDataGridView(dtArr[1]);
                    }
                    else
                    {
                        dgvTableInfo.BindDataGridView(dtArr[1]);
                    }
                }
            }
            catch (Exception exception)
            {
                ShowErr(exception.Message);
            }
        }
        #endregion

        #region ����ģ��ʵ��
        public void BuildTemplateEntity()
        {
            _dtGen.Rows.Clear();
            if (string.IsNullOrEmpty(txtPath.Text.Trim()))
            {
                ShowInfo("�������ô��·��!");
                tabControl1.SelectedTab = tpgEntity;
                return;
            }

            if (_selectedNode == null) return;
            DataTable dtTable = DBTableEntity.GetTableStruct();

            if (_selectedNode.Level == 1) 
            {
                dtTable.Merge((DataTable)_selectedNode.Tag);
            }
            else if(_selectedNode.Level == 2)
            {
                dtTable.Merge(((DataTable[])_selectedNode.Tag)[0]);
            }

            List<EntityInfo>  list = new List<EntityInfo>();
            
            foreach (DataRow drT in dtTable.Rows)
            {
                string sTableName = drT[DBTableEntity.SqlString.Name].ToString();
                string eEntityName = txtClassPreString.Text.Trim() + sTableName + txtClassEndString.Text.Trim();
                string sTableNameCN = drT[DBTableEntity.SqlString.NameCN].ToString();
                string sTableRemark = drT[DBTableEntity.SqlString.Comments].ToString();

                DataTable dtColumn = _dataAccess.GetSqlSchemaTableColumns(sTableName);
                StringBuilder sbEntity = new StringBuilder();
                StringBuilder sbString = new StringBuilder();
                StringBuilder sbColumnProp = new StringBuilder();
                StringBuilder sbColumnStr = new StringBuilder();
                string sFiter = ",";

                for (int j = 0; j < dtColumn.Rows.Count; j++)
                {
                    DataRow dr = dtColumn.Rows[j];
                    DBColumnEntity ent = DBColumnEntity.GetEntity(dr);
                    if (j == dtColumn.Rows.Count - 1)
                    {
                        sFiter = "";
                    }
                    
                    sbEntity.Append(txbPropertyPre.Text.Trim() + ent.Name + sFiter);
                    sbString.Append("\"" + ent.Name + "\""+ sFiter);
                    sbColumnStr.Append(rtbColumnStr.Text.Replace("#COLUMN_NAME#", ent.Name));
                    sbColumnProp.Append(GetColPropertyString(dr));
                }

                string sFile = rtbTable.Text
                    .Replace("#TABLE_NAME#", sTableName)
                    .Replace("#TABLE_NAME_AFFIX#", eEntityName)
                    .Replace("#TABLE_NAME_CN#", sTableNameCN)
                    .Replace("#TABLE_COMMENT#", sTableRemark)
                    .Replace("#COLUMN_LIST_VAR#", sbEntity.ToString())
                    .Replace("#COLUMN_LIST_STR#", sbString.ToString())
                    .Replace("#COLUMN_PROPERTY_VAR#", sbColumnProp.ToString())
                    .Replace("#COLUMN_STATIC_STR#", sbColumnStr.ToString());

                using (TextWriter writer = new StreamWriter(new BufferedStream(new FileStream(txtPath.Text.Trim() + @"\" + eEntityName + ".cs", FileMode.Create, FileAccess.Write)), Encoding.GetEncoding("gbk")))
                {
                    writer.Write(sFile);
                    DataRow dr = _dtGen.NewRow();
                    dr[sGenPath] = txtPath.Text.Trim() + @"\" + eEntityName + ".cs";
                    _dtGen.Rows.Add(dr);
                }
            }
            //��������
            Setting.Default.AutoEntity_ClassPre = txtClassPreString.Text.Trim();
            Setting.Default.AutoEntity_ClassEnd = txtClassEndString.Text.Trim();
            Setting.Default.AutoEntity_PropertyPre = txbPropertyPre.Text.Trim();
            Setting.Default.Save();
            ShowInfo("ʵ��ɹ����ɣ�");
        }
        #endregion

        public string GetColPropertyString(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(rtbColumnProp.Text.Replace("#COLUMN_NAME_VAR#", txbPropertyPre.Text.Trim() + dr[DBColumnEntity.SqlString.Name].ToString()));
            SetColProperty(dr, sb, DBColumnEntity.SqlString.Name);
            SetColProperty(dr, sb, DBColumnEntity.SqlString.DataType);
            SetColProperty(dr, sb, DBColumnEntity.SqlString.DataTypeFull);
            SetColProperty(dr, sb, DBColumnEntity.SqlString.DataPrecision);
            SetColProperty(dr, sb, DBColumnEntity.SqlString.DataScale);
            SetColProperty(dr, sb, DBColumnEntity.SqlString.NotNull);
            SetColProperty(dr, sb, DBColumnEntity.SqlString.KeyType);
            SetColProperty(dr, sb, DBColumnEntity.SqlString.DataLength);
            SetColProperty(dr, sb, DBColumnEntity.SqlString.NameCN);
            return sb.ToString() + ";\n";//���ǵü��Ϸֺ�
        }

        private void SetColProperty(DataRow dr, StringBuilder sb,string sProperty)
        {
            DataRow[] arrDR = _dtSet.Select(AutoEntityString.Set.Col_Name + "='" + sProperty + "'");
            if (arrDR.Length == 0) return;
            string sNotNull = arrDR[0][AutoEntityString.Set.Col_Notnull].ToString();
            string sNull = arrDR[0][AutoEntityString.Set.Col_Null].ToString();

            if (!string.IsNullOrEmpty(dr[sProperty].ToString()))
            {
                sb.Append(sNotNull.Replace("#V#", dr[sProperty].ToString()));
            }
            else if (!string.IsNullOrEmpty(sNull.Replace("#V#", dr[sProperty].ToString())))
            {
                sb.Append(sNull);
            }
        }

        private void TsbSave_Click(object sender, EventArgs e)
        {
            groupBox1.Focus();
            BindingSource bs = (BindingSource)dgvParamSet.DataSource;
            bs.EndEdit();
            DataTable dt = (DataTable)bs.DataSource;
            DataTable dtSave = dt.GetChanges();
            if (dtSave == null || dtSave.Rows.Count == 0)
            {
                ShowInfo("û���޸����ݣ����ñ��棡");
                return;
            }
            _miniXml.SaveXMLFile(dtSave, _miniXml.GetFileName());
            ShowInfo("����ɹ���");
        }
    }
}