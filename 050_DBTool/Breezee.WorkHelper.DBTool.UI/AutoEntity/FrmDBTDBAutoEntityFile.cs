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
using System.Text.RegularExpressions;
using org.breezee.MyPeachNet;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ƣ�����ʵ��
    /// �������ߣ��ƹ���
    /// �������ڣ�2016-10-20
    /// �޸���ʷ:
    ///   2023-09-07 BreezeeHui ģ�������ļ���׺
    /// </summary>
	public partial class FrmDBTDBAutoEntityFile : BaseForm
	{
        #region ����
        private DbServerInfo _dbServer;
        private TreeNode _selectedNode;
        private List<EntityInfo> list = new List<EntityInfo>();
        private IDBConfigSet _IDBConfigSet;
        private BindingSource bsTable = new BindingSource();
        //
        private IDataAccess _dataAccess;
        string sTip = "�������һ���ڵ���ٰ�����Ҽ�ѡ������";
        string sGenPath = "GenPath";
        DataTable _dtGen;
        //
        MiniXmlConfig _miniXml;
        List<string> _lstCol;
        DataTable _dtSet;

        DataSet _dsExcel;
        BindingSource _bsFileList;
        #endregion

        #region ���캯��
        public FrmDBTDBAutoEntityFile()
        {
            InitializeComponent();
        } 
        #endregion

        #region �����¼�
        private void FrmAutoEntity_Load(object sender, EventArgs e)
        {
            Text = "����ʵ���ļ�";
            ctxmBuilt.Visible = false;
            //�����û�ƫ��ֵ
            txtPath.Text = WinFormContext.UserLoveSettings.Get(DBTUserLoveConfig.AutoEntity_Path, "").Value;
            //txtPath.Text = Setting.Default.AutoEntity_Path;

            //�������ݿ����ӿؼ�
            _IDBConfigSet = ContainerContext.Container.Resolve<IDBConfigSet>();
            DataTable dtConn = _IDBConfigSet.QueryDbConfig(_dicQuery).SafeGetDictionaryTable();
            uC_DbConnection1.ShowGlobalMsg += ShowGlobalMsg_Click;
            uC_DbConnection1.SetDbConnComboBoxSource(dtConn);
            uC_DbConnection1.IsDbNameNotNull = false;
            uC_DbConnection1.DBConnName_SelectedIndexChanged += DBConnNameSelectedIndexChanged;

            //����Ϣ����
            tvDataBaseInfo.Font = new Font("������", 10f, FontStyle.Bold);
            dgvTableInfo.ReadOnly = true;
            dgvTableInfo.AutoGenerateColumns = false;
            dgvTableInfo.AllowUserToAddRows = false;
            dgvTableInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            FlexGridColumnDefinition fdc = new FlexGridColumnDefinition();//����Tag
            fdc.AddColumn(
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.TableName).Name(DBColumnEntity.SqlString.TableName).Caption("����").Type().Align().Width(100).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.NameCN).Name(DBColumnEntity.SqlString.NameCN).Caption("������").Type().Align().Width(100).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.Name).Name(DBColumnEntity.SqlString.Name).Caption("����").Type().Align().Width(100).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.DataTypeFull).Name(DBColumnEntity.SqlString.DataTypeFull).Caption("���ͺʹ�С").Type().Align().Width(100).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.DataPrecision).Name(DBColumnEntity.SqlString.DataPrecision).Caption("����").Type().Align().Width(40).Visible().Build(), 
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.DataScale).Name(DBColumnEntity.SqlString.DataScale).Caption("С��λ��").Type().Align().Width(60).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.Default).Name(DBColumnEntity.SqlString.Default).Caption("Ĭ��ֵ").Type().Align().Width(60).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.KeyType).Name(DBColumnEntity.SqlString.KeyType).Caption("����").Type().Align().Width(40).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.NotNull).Name(DBColumnEntity.SqlString.NotNull).Caption("�ǿ�").Type().Align().Width(40).Visible().Build(),
            new FlexGridColumn.Builder().DBName(DBColumnEntity.SqlString.Extra).Name(DBColumnEntity.SqlString.Extra).Caption("��ע").Type().Align().Width(200).Visible().Build()
            );
            dgvTableInfo.Tag = fdc.GetGridTagString();

            lblTableData.Text = "����ģ��ġ�ϵͳ��������ϵͳ���õĲ�������ֱ����ģ�����������ã�Ҳ�����ڡ��Զ�������������á�����˵����ģ������ͷ����ע��";
            lblColumnInfo.Text = "����ģ��ġ��Զ�������������Զ�������ϵͳ������Ȼ����ģ�����������á�������ת���������ݿ�����ת��Ϊ�����е����͡�";
            lblTemplateInfo.Text = "�ؼ�����Ⲣ�޸ķ���ʵ����Ŀ��ģ�壬���ṩ����Ŀ��Աʹ�ã�";
        }

        #region ��ʾȫ����ʾ��Ϣ�¼�
        private void ShowGlobalMsg_Click(object sender, string msg)
        {
            ShowDestopTipMsg(msg);
        }
        #endregion
        private void DBConnNameSelectedIndexChanged(object sender, EventArgs e)
        {
            btnLinkServer.PerformClick();
        }
        #endregion

        #region ����ģ��ʵ��
        public void BuildTemplateEntity()
        {
            if (string.IsNullOrEmpty(txtPath.Text.Trim()))
            {
                ShowInfo("�������ô��·��!");
                return;
            }

            if (string.IsNullOrEmpty(rtbEntityTemplate.Text.Trim()))
            {
                ShowInfo("���ȵ���ģ��!");
                return;
            }

            if (_selectedNode == null) return;
            DataTable dtTable = DBTableEntity.GetTableStruct();

            if (_selectedNode.Level == 1)
            {
                dtTable.Merge((DataTable)_selectedNode.Tag);
            }
            else if (_selectedNode.Level == 2)
            {
                dtTable.Merge(((DataTable[])_selectedNode.Tag)[0]);
            }

            List<EntityInfo> list = new List<EntityInfo>();
            _dicString = new Dictionary<string, string>();
            DataTable dtConvert = dgvTypeConvert.GetBindingTable();
            DataTable dtSysParam = dgvSysParam.GetBindingTable();
            //��ǰʱ�䴦��
            _dicString[AutoImportModuleString.AutoFileSysParam.DateNow] = DateTime.Now.ToString("yyyy-MM-dd");

            foreach (DataRow drT in dtTable.Rows)
            {
                //ϵͳ�����й̶�ֵ����
                string sTableName = drT[DBTableEntity.SqlString.Name].ToString();
                _dicString[AutoImportModuleString.AutoFileSysParam.TableDbName] = drT[DBTableEntity.SqlString.Name].ToString();
                _dicString[AutoImportModuleString.AutoFileSysParam.EntNameCn] = drT[DBTableEntity.SqlString.NameCN].ToString();
                _dicString[AutoImportModuleString.AutoFileSysParam.EntExt] = drT[DBTableEntity.SqlString.Comments].ToString();

                if (!string.IsNullOrEmpty(txbRemoveTablePre.Text.Trim()))
                {
                    string eEntityName = sTableName.Replace(txbRemoveTablePre.Text.Trim(), "");
                    _dicString[AutoImportModuleString.AutoFileSysParam.EntName] = eEntityName.FirstLetterUpper();
                    _dicString[AutoImportModuleString.AutoFileSysParam.EntNameLcc] = eEntityName.FirstLetterUpper(false);
                }
                else
                {
                    _dicString[AutoImportModuleString.AutoFileSysParam.EntName] = sTableName.FirstLetterUpper();
                    _dicString[AutoImportModuleString.AutoFileSysParam.EntNameLcc] = sTableName.FirstLetterUpper(false);
                }

                //�Զ�������еĹ̶�ֵ����
                DataTable dtMyDefine = dgvMyDefine.GetBindingTable();
                AutoFileUtil.DealMyFixParam(_dicString, dtMyDefine);

                //���²�ѯ����Ϣ�����ܴ�������ȡ����Ϊ���ڶ������
                DataTable dtColumn = _dataAccess.GetSqlSchemaTableColumns(sTableName, _dbServer.SchemaName);

                StringBuilder sbAllCol = new StringBuilder();
                StringBuilder sbEntity = new StringBuilder();

                //ѭ��������嵥
                foreach (DataRow dr in dtColumn.Rows)
                {
                    sbAllCol.Append(dr[DBColumnEntity.SqlString.Name].ToString() + ",");
                    //MyBatis��ʵ�嶨�壺�����������滻
                    DataRow[] drArr = dtSysParam.Select(AutoImportModuleString.ColumnNameSysParam.ParamName + "='" + AutoImportModuleString.AutoFileSysParam.ColEntNote + "'");
                    DataRow[] drArrType = dtConvert.Select(AutoImportModuleString.ColumnNameTypeConvert.DbType + "='" + dr[DBColumnEntity.SqlString.DataType].ToString() + "'");

                    if (drArr.Length > 0 && !string.IsNullOrEmpty(drArr[0][AutoImportModuleString.ColumnNameSysParam.ParamContent].ToString()))
                    {
                        //�滻�б���
                        string sColApi = AutoFileUtil.GetFinalString(drArr[0][AutoImportModuleString.ColumnNameSysParam.ParamContent].ToString(), dr, dtSysParam, _dicString, dtConvert);
                        //�滻����ת��
                        if (drArrType.Length > 0)
                        {
                            sColApi = sColApi.Replace(AutoImportModuleString.ColumnNameSysParam.ChangeType, drArrType[0][AutoImportModuleString.ColumnNameTypeConvert.DevLangType].ToString());
                        }
                        //�����ַ��滻
                        if ("PK".Equals(dr[DBColumnEntity.SqlString.KeyType].ToString()))
                        {
                            sbEntity.Append(sColApi.Replace("@TableField", "@TableId"));
                            sbEntity.Append(Environment.NewLine);
                        }
                        else
                        {
                            sbEntity.Append(sColApi);
                            sbEntity.Append(Environment.NewLine);
                        }
                    }

                }

                //�õ�����ƴ�ӵĶ�̬�ַ�
                //_dicString[AutoImportModuleString.AutoFileSysParam.ColDbNameAll] = sbAllCol.ToString().Substring(0, sbAllCol.ToString().Length - 1);
                _dicString[AutoImportModuleString.AutoFileSysParam.ColEntNote] = sbEntity.ToString();

                //�Զ�������ж�ֵ̬�Ĵ���
                DataTable dtMyDefineDynamic = dgvMyDefine.GetBindingTable();
                AutoFileUtil.FixMyDynamicParam(_dicString, dtMyDefineDynamic, dtColumn, dtSysParam, false);

                //�滻ģ������
                string sFileContent = rtbEntityTemplate.Text;
                string sFilePath = Path.Combine(txtPath.Text.Trim(), "AutoEntityFiles");
                if (!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);
                }
                //ȷ���ļ���׺��
                string sFileSuffix = txbModuleFileSuffix.Text.trim();
                if (!sFileSuffix.startsWith("."))
                {
                    sFileSuffix = "." + sFileSuffix;
                }
                //�����ļ�·���������ļ�
                sFilePath = Path.Combine(sFilePath, _dicString[AutoImportModuleString.AutoFileSysParam.EntNameClass] + sFileSuffix);
                foreach (string sKey in _dicString.Keys)
                {
                    sFileContent = sFileContent.Replace(sKey, _dicString[sKey]);
                }
                //����ļ�
                using (TextWriter writer = new StreamWriter(new BufferedStream(new FileStream(sFilePath, FileMode.Create, FileAccess.Write)), Encoding.GetEncoding("utf-8")))
                {
                    writer.Write(sFileContent);
                }
            }
            //�����û�ƫ��ֵ
            WinFormContext.UserLoveSettings.Set("AutoEntity_Path", txtPath.Text, "ʵ�屣��·��");
            WinFormContext.UserLoveSettings.Save();
            //Setting.Default.AutoEntity_Path = txtPath.Text;
            //Setting.Default.Save();
            ShowInfo("ʵ��ɹ����ɣ�");
        }
        #endregion

        /// <summary>
        /// ���밴ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            _dicString.Clear();
            _dicString[AutoImportModuleString.SheetName.EntityModule] = AutoImportModuleString.SheetName.EntityModule;
            _dicString[AutoImportModuleString.SheetName.MyParam] = AutoImportModuleString.SheetName.MyParam;
            _dicString[AutoImportModuleString.SheetName.DbTypeConvert] = AutoImportModuleString.SheetName.DbTypeConvert;
            _dicString[AutoImportModuleString.SheetName.SysParam] = AutoImportModuleString.SheetName.SysParam;
            _dsExcel = ExportHelper.GetExcelDataSet(_dicString);//����ģ��
            if (_dsExcel != null)
            {
                DataTable dt = _dsExcel.Tables[AutoImportModuleString.SheetName.EntityModule];
                if (dt == null)
                {
                    MsgHelper.ShowInfo("����ģ����������µ��룡");
                    return;
                }
                //��������
                cbbModuleList.BindDropDownList(dt, AutoImportModuleString.ColumnNameModule.ModuleCode, AutoImportModuleString.ColumnNameModule.ModuleCode, true);

                //���Զ����������
                FlexGridColumnDefinition fdc = new FlexGridColumnDefinition();
                fdc.AddColumn(
                    FlexGridColumn.NewRowNoCol(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameMyParam.ParamName).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameMyParam.ParamContent).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameMyParam.ConcatString).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameMyParam.ParamValueInfo).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build()
                );
                dgvMyDefine.Tag = fdc.GetGridTagString();
                dgvMyDefine.BindDataGridView(_dsExcel.Tables[AutoImportModuleString.SheetName.MyParam], true);
                //��ϵͳ��������
                fdc = new FlexGridColumnDefinition();
                fdc.AddColumn(
                    FlexGridColumn.NewRowNoCol(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameSysParam.ParamName).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameSysParam.ParamContent).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameSysParam.ParamValueInfo).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameSysParam.Example).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build()
                );
                dgvSysParam.Tag = fdc.GetGridTagString();
                dgvSysParam.BindDataGridView(_dsExcel.Tables[AutoImportModuleString.SheetName.SysParam], true);
                //������ת������
                fdc = new FlexGridColumnDefinition();
                fdc.AddColumn(
                    FlexGridColumn.NewRowNoCol(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameTypeConvert.DbType).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                    new FlexGridColumn.Builder().Name(AutoImportModuleString.ColumnNameTypeConvert.DevLangType).Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build()
                );
                dgvTypeConvert.Tag = fdc.GetGridTagString();
                dgvTypeConvert.BindDataGridView(_dsExcel.Tables[AutoImportModuleString.SheetName.DbTypeConvert], true);

                ShowInfo("����ɹ���");
            }
        }

        #region �������Ӱ�ť�¼�
        private async void btnLinkServer_Click(object sender, EventArgs e)
        {
            try
            {
                _dbServer = await uC_DbConnection1.GetDbServerInfo();
                if (_dbServer == null)
                {
                    return;
                }
                _dataAccess = AutoSQLExecutor.Common.AutoSQLExecutors.Connect(_dbServer);
                //�������ݿ�
                LoadTreeView(_dbServer);
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
                //�����û�ƫ��ֵ
                WinFormContext.UserLoveSettings.Set(DBTUserLoveConfig.AutoEntity_Path, dialog.SelectedPath, "��ʵ�����ɡ�����·��");
                WinFormContext.UserLoveSettings.Save();
                //Setting.Default.AutoEntity_Path1 = dialog.SelectedPath;
                //Setting.Default.Save();
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
                        dtArr[1] = _dataAccess.GetSqlSchemaTableColumns(e.Node.Text,_dbServer.SchemaName);
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
       
        /// <summary>
        /// ģ��������仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbModuleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbModuleList.Text.Trim().Length > 0 && _dsExcel != null)
            {
                DataTable dt = _dsExcel.Tables[AutoImportModuleString.SheetName.EntityModule];
                DataRow[] drArr = dt.Select(AutoImportModuleString.ColumnNameModule.ModuleCode + "='"+cbbModuleList.Text+"'");
                if(drArr.Length> 0)
                {
                    rtbEntityTemplate.Clear();
                    rtbEntityTemplate.AppendText(drArr[0][AutoImportModuleString.ColumnNameModule.ModuleContent].ToString()); //ģ������
                    txbModuleFileSuffix.Text = drArr[0][AutoImportModuleString.ColumnNameModule.ModuleFileSuffix].ToString(); //��׺
                }
            }
        }

        /// <summary>
        /// ģ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDownloadModel_Click(object sender, EventArgs e)
        {
            DBToolUIHelper.DownloadFile(DBTGlobalValue.AutoEntity.Excel_Code, "ģ��_���ɴ����ļ�", true);
        }
    }
}