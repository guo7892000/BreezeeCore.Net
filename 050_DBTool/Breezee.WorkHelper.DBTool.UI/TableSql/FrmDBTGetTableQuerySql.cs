﻿using Breezee.WorkHelper.DBTool.IBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Breezee.WorkHelper.DBTool.Entity;
using Breezee.Core.Interface;
using Breezee.Core.WinFormUI;
using Breezee.AutoSQLExecutor.Core;
using Breezee.Core.IOC;
using Breezee.AutoSQLExecutor.Common;
using Breezee.Core.Tool;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// 获取表的增删改SQL
    /// </summary>
    public partial class FrmDBTGetTableQuerySql : BaseForm
    {
        #region 变量
        private readonly string _strTableName = "变更表清单";
        private readonly string _strColName = "变更列清单";

        private readonly string _sGridColumnCondition = "IsCondition";
        private readonly string _sGridColumnSelect = "IsSelect";
        private readonly string _sGridColumnDynamic = "IsDynamic";
        private bool _allSelect = false;//默认全选，这里取反
        private bool _allCondition = true;//默认全不选，这里取反
        private bool _allDynamic = false;//默认全选，这里取反
        //常量
        private static string strTableAlias = "A"; //查询和修改中的表别名
        private static string strTableAliasAndDot = "";
        private static readonly string _strUpdateCtrolColumnCode = "UPDATE_CONTROL_ID";
        private string _strAutoSqlSuccess = "生成成功，并已复制到了粘贴板。详细见“生成的SQL”页签！";
        private string _strImportSuccess = "导入成功！可点“生成SQL”按钮得到本次导入的变更SQL。";
        //数据集
        private IDBConfigSet _IDBConfigSet;
        private DbServerInfo _dbServer;
        private IDataAccess _dataAccess;
        private IDBDefaultValue _IDBDefaultValue;
        private DataTable _dtDefault = null;
        DBSqlEntity sqlEntity;
        #endregion

        #region 构造函数
        public FrmDBTGetTableQuerySql()
        {
            InitializeComponent();
        }
        #endregion

        #region 加载事件
        private void FrmGetOracleSql_Load(object sender, EventArgs e)
        {
            _IDBDefaultValue = ContainerContext.Container.Resolve<IDBDefaultValue>();

            #region 绑定下拉框
            _dicString.Add("1", "新增");
            _dicString.Add("2", "修改");
            _dicString.Add("3", "查询");
            _dicString.Add("4", "删除");
            cmbType.BindTypeValueDropDownList(_dicString.GetTextValueTable(false), false, true);
            //
            _dicString.Clear();
            _dicString.Add("1", "左右#号");
            _dicString.Add("2", "SQL参数化");
            _dicString.Add("3", "MyBatis参数");
            _dicString.Add("4", "MyBatis动态参数");
            _dicString.Add("10", "自定义");
            
            cbbParaType.BindTypeValueDropDownList(_dicString.GetTextValueTable(false), false, true);


            IDictionary<string, string> dic_List = new Dictionary<string, string>
            {
                { "0", "无" },
                { "1", "驼峰式" },
                { "2", "首字母大写" }
            };
            cbbWordConvert.BindTypeValueDropDownList(dic_List.GetTextValueTable(false), false, true);
            #endregion

            #region 设置数据库连接控件
            _IDBConfigSet = ContainerContext.Container.Resolve<IDBConfigSet>();
            DataTable dtConn = _IDBConfigSet.QueryDbConfig(_dicQuery).SafeGetDictionaryTable();
            uC_DbConnection1.SetDbConnComboBoxSource(dtConn);
            uC_DbConnection1.IsDbNameNotNull = true;
            uC_DbConnection1.DBType_SelectedIndexChanged += cbbDatabaseType_SelectedIndexChanged;//数据库类型下拉框变化事件
            #endregion

            //设置下拉框查找数据源
            cbbTableName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbbTableName.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        #endregion

        #region 数据库类型下拉框变化事件
        private void cbbDatabaseType_SelectedIndexChanged(object sender, DBTypeSelectedChangeEventArgs e)
        {
            switch (e.SelectDBType)
            {
                case DataBaseType.PostgreSql:
                case DataBaseType.Oracle:
                    txbParamPre.Text = ":";
                    break;
                case DataBaseType.SQLite:
                case DataBaseType.SqlServer:
                case DataBaseType.MySql:
                    txbParamPre.Text = "@";
                    break;
                default:
                    txbParamPre.Text = "@";
                    break;
            }
        }
        #endregion

        #region 连接数据库事件
        private void tsbImport_Click(object sender, EventArgs e)
        {
            _dbServer = uC_DbConnection1.GetDbServerInfo();
            string sTableName = cbbTableName.Text.Trim();
            if (_dbServer == null || sTableName.IsNullOrEmpty())
            {
                return;
            }

            _dataAccess = AutoSQLExecutors.Connect(_dbServer);
            DataTable dtTable = DBTableEntity.GetTableStruct();

            DataRow[] drArr;
            string sFilter = DBTableEntity.SqlString.Name + "='" + sTableName + "'";
            if (uC_DbConnection1.UserTableList == null || uC_DbConnection1.UserTableList.Rows.Count == 0)
            {
                drArr = _dataAccess.GetSchemaTables().Select(sFilter);
            }
            else
            {
                drArr = uC_DbConnection1.UserTableList.Select(sFilter);
            }
            if (drArr.Count() == 0)
            {
                return;
            }
            else
            {
                DataRow dr = dtTable.NewRow();
                dr[DBTableEntity.SqlString.Owner] = drArr[0][DBTableEntity.SqlString.Owner].ToString();
                dr[DBTableEntity.SqlString.Name] = drArr[0][DBTableEntity.SqlString.Name].ToString();
                dr[DBTableEntity.SqlString.Comments] = drArr[0][DBTableEntity.SqlString.Comments].ToString();
                dtTable.Rows.Add(dr);
            }
            dtTable.TableName = _strTableName;
            //设置Tag
            SetTableTag(dtTable);
            SetColTag();
            //查询全局的默认值配置
            _dicQuery[DT_DBT_BD_COLUMN_DEFAULT.SqlString.IS_ENABLED] = "1";
            _dtDefault = _IDBDefaultValue.QueryDefaultValue(_dicQuery).SafeGetDictionaryTable(); //获取默认值、排除列配置信息
            //导入成功后处理
            tsbAutoSQL.Enabled = true;
            tabControl1.SelectedTab = tpImport;
            SetDefaultValue(null);
            //导入成功提示
            lblInfo.Text = _strImportSuccess;
        }
        #endregion

        private void SetTableTag(DataTable dt)
        {
            //查询结果
            FlexGridColumnDefinition fdc = new FlexGridColumnDefinition();
            fdc.AddColumn(
                FlexGridColumn.NewRowNoCol(),
                new FlexGridColumn.Builder().Name(DBTableEntity.SqlString.Name).Caption("表名").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBTableEntity.SqlString.NameCN).Caption("表名中文").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBTableEntity.SqlString.Schema).Caption("架构").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBTableEntity.SqlString.Owner).Caption("拥有者").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBTableEntity.SqlString.DBName).Caption("所属数据库").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBTableEntity.SqlString.Comments).Caption("备注").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(200).Edit(false).Visible().Build()
            );
            dgvTableList.Tag = fdc.GetGridTagString();
            dgvTableList.BindDataGridView(dt, true);
        }

        #region 设置Tag方法
        private void SetColTag()
        {
            DataTable dtCols = _dataAccess.GetSqlSchemaTableColumns(cbbTableName.Text.Trim());
            //增加条件列
            DataColumn dcCondiction = new DataColumn(_sGridColumnCondition);
            dcCondiction.DefaultValue = "0";
            dtCols.Columns.Add(dcCondiction);
            //增加选择列
            DataColumn dcSelected = new DataColumn(_sGridColumnSelect);
            dcSelected.DefaultValue = "1";
            dtCols.Columns.Add(dcSelected);
            //增加MyBatis动态列
            DataColumn dcDynamic = new DataColumn(_sGridColumnDynamic);
            dcDynamic.DefaultValue = "1";
            dtCols.Columns.Add(dcDynamic);
            dtCols.TableName = _strColName;
            //查询结果
            FlexGridColumnDefinition fdc = new FlexGridColumnDefinition();
            fdc.AddColumn(
                FlexGridColumn.NewRowNoCol(),
                new FlexGridColumn.Builder().Name(_sGridColumnSelect).Caption("选择").Type(DataGridViewColumnTypeEnum.CheckBox).Align(DataGridViewContentAlignment.MiddleCenter).Width(40).Edit().Visible().Build(),
                new FlexGridColumn.Builder().Name(_sGridColumnCondition).Caption("条件").Type(DataGridViewColumnTypeEnum.CheckBox).Align(DataGridViewContentAlignment.MiddleCenter).Width(40).Edit().Visible().Build(),
                new FlexGridColumn.Builder().Name(_sGridColumnDynamic).Caption("MyBatis动态列").Type(DataGridViewColumnTypeEnum.CheckBox).Align(DataGridViewContentAlignment.MiddleCenter).Width(80).Edit().Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.Default).Caption("固定值").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit().Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.Name).Caption("列名").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.DataTypeFull).Caption("类型").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.SortNum).Caption("排序号").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(40).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.KeyType).Caption("主键").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.NotNull).Caption("非空").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.DataType).Caption("数据类型").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.DataLength).Caption("字符长度").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.DataPrecision).Caption("精度").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.DataScale).Caption("尺度").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(100).Edit(false).Visible().Build(),
                new FlexGridColumn.Builder().Name(DBColumnEntity.SqlString.Comments).Caption("备注").Type(DataGridViewColumnTypeEnum.TextBox).Align(DataGridViewContentAlignment.MiddleLeft).Width(200).Edit(false).Visible().Build(),
                FlexGridColumn.NewHideCol(DBColumnEntity.SqlString.NameCN),
                FlexGridColumn.NewHideCol(DBColumnEntity.SqlString.Extra)
            );
            dgvColList.Tag = fdc.GetGridTagString();
            dgvColList.BindDataGridView(dtCols, true);
            //dgvColList.AllowUserToAddRows = true;//设置网格样式
        }
        #endregion

        #region 生成SQL按钮事件
        private void tsbAutoSQL_Click(object sender, EventArgs e)
        {
            #region 判断并取得数据
            txbTableShortName.Focus();
            //取得数据源
            DataTable dtMain = dgvTableList.GetBindingTable();
            DataTable dtSec = dgvColList.GetBindingTable();
            if (dtMain == null) return;
            //移除空行
            dtMain.DeleteNullRow();
            //得到变更后数据
            dtMain.AcceptChanges();
            dtSec.AcceptChanges();
            if (dtMain.Rows.Count == 0 || dtSec.Rows.Count == 0)
            {
                ShowInfo("请先查询！");
                return;
            }
            #endregion

            #region 生成增删改查SQL

            #region 变量
            string sWordConvert = cbbWordConvert.SelectedValue.ToString();
            string sBegin = "BEGIN_";
            string sEnd = "END_";
            sqlEntity = new DBSqlEntity();
            sqlEntity.NewLine = ckbNewLine.Checked ? DataBaseCommon.NewLine : "";
            sqlEntity.Tab = ckbNewLine.Checked ? DataBaseCommon.Tab : "";
            sqlEntity.IsHasRemark = ckbUseRemark.Checked;
            sqlEntity.IsUseGlobal = ckbUseDefaultConfig.Checked;
            if ("1".Equals(sWordConvert))
            {
                sqlEntity.WordUpperType = WordUpperType.FirstUpperSecond;
                sBegin = "begin";
                sEnd = "end";
            }
            else if ("2".Equals(sWordConvert))
            {
                sqlEntity.WordUpperType = WordUpperType.FirstUpperAll;
                sBegin = "Begin";
                sEnd = "End";
            }
            else
            {
                sqlEntity.WordUpperType = WordUpperType.None;
            }


            StringBuilder sbAllSql = new StringBuilder();
            StringBuilder sbWhereSql = new StringBuilder();
            string strWhereFirst = ckbNewLine.Checked ? "WHERE 1=1 " + sqlEntity.NewLine : " WHERE 1=1 " + sqlEntity.NewLine;
            string strWhereNoFirst = ckbNewLine.Checked ? "WHERE " : " WHERE ";


            string strAnd = " AND ";


            switch (cbbParaType.SelectedValue.ToString())
            {
                case "1":
                    sqlEntity.ParamType = SqlParamFormatType.BeginEndHash;
                    break;
                case "2":
                    sqlEntity.ParamType = SqlParamFormatType.SqlParm;
                    break;
                case "3":
                    sqlEntity.ParamType = SqlParamFormatType.MyBatis;
                    break;
                case "4":
                    sqlEntity.ParamType = SqlParamFormatType.MyBatisDynamicColoumn;
                    break;
                case "10":
                    sqlEntity.ParamType = SqlParamFormatType.UserDefine;
                    break;
                default:
                    throw new Exception("不支持的SqlParamType枚举类型：" + sqlEntity.ParamType.ToString());
            }

            string sParamPre = txbParamPre.Text.Trim();
            if (sqlEntity.ParamType == SqlParamFormatType.SqlParm && string.IsNullOrEmpty(sParamPre))
            {
                ShowInfo("当选择参数化时，其后的参数化字符不能为空！");
                txbParamPre.Focus();
                return;
            }

            string sDefineFormat = txbDefineFormart.Text.Trim();
            if (sqlEntity.ParamType == SqlParamFormatType.UserDefine)
            {
                if (string.IsNullOrEmpty(sParamPre) || string.IsNullOrEmpty(sDefineFormat))
                {
                    ShowInfo("当选择【自定义】时，【列名替代符】和【自定义格式】都不能为空！");
                    txbParamPre.Focus();
                    return;
                }
            }


            sqlEntity.TableName = cbbTableName.Text.Trim();
            sqlEntity.SqlType = GetSqlType();
            string strTSName = txbTableShortName.Text.Trim().Replace(".", "").Replace("'", "");
            sqlEntity.TableAlias = string.IsNullOrEmpty(strTSName) ? " A" : " " + strTSName;//查询和修改中的别名:注前面的空格为必须
            strTableAliasAndDot = sqlEntity.TableAlias + ".";

            string sColumnWherePre = strTableAliasAndDot;   //where条件中的列前缀

            //if (sqlEntity.SqlType == SqlType.Insert || sqlEntity.SqlType == SqlType.Update)
            if (sqlEntity.SqlType == SqlType.Insert)
            {
                sqlEntity.TableAlias = "";
                strTableAliasAndDot = "";
                sColumnWherePre = "";
            }
            #endregion

            #region 得到有效的数据
            DataTable dtColumnSelect = dtSec.Clone();
            DataTable dtColumnCondition = dtSec.Clone();
            DataTable dtColumnDynamic = dtSec.Clone();

            //得到【选择】选中的列
            string sFiter = string.Format("{0}='1'", _sGridColumnSelect);
            foreach (DataRow dr in dtSec.Select(sFiter))
            {
                dtColumnSelect.ImportRow(dr); //对非修改，不是排除列就导入
            }

            //得到【条件】选中的列
            sFiter = string.Format("{0}='1'", _sGridColumnCondition);
            foreach (DataRow dr in dtSec.Select(sFiter))
            {
                dtColumnCondition.ImportRow(dr);//对非修改，不是排除列就导入
            }

            //得到【MyBatis动态列】选中的列
            sFiter = string.Format("{0}='1'", _sGridColumnDynamic);
            foreach (DataRow dr in dtSec.Select(sFiter))
            {
                dtColumnDynamic.ImportRow(dr);
            }
            #endregion

            #region 得到条件
            string strNowAnd;
            if (sqlEntity.SqlType == SqlType.Delete || sqlEntity.SqlType == SqlType.Update)
            {
                sbWhereSql.Append(strWhereNoFirst); //删除和更新去掉1=1条件，是为了防止空条件时更新或删除全部数据；如空条件则会提示SQL不正确而导致执行失败。
                strNowAnd = "";
            }
            else
            {
                sbWhereSql.Append(strWhereFirst);
                strNowAnd = strAnd;
            }
            for (int i = 0; i < dtColumnCondition.Rows.Count; i++)
            {
                //变量声明
                string strColCode = dtColumnCondition.Rows[i][DBColumnEntity.SqlString.Name].ToString().Trim().ToUpper();
                string strColType = dtColumnCondition.Rows[i][DBColumnEntity.SqlString.DataType].ToString().Trim().ToUpper();
                string strColFixedValue = dtColumnCondition.Rows[i][DBColumnEntity.SqlString.Default].ToString().Trim();//固定值
                string strColDynamic = dtColumnCondition.Rows[i][_sGridColumnDynamic].ToString().Trim();//动态列复选框
                string strColComments = "";
                if (sqlEntity.IsHasRemark)
                {
                    strColComments = dtColumnCondition.Rows[i][DBColumnEntity.SqlString.Comments].ToString().Trim();//列说明                                                                                                                    
                    strColComments = DataBaseCommon.GetColumnComment(strColCode, strColComments);//列空注释的处理
                }

                //转换列的编码
                string strColCodeParm = strColCode;
                if(sqlEntity.WordUpperType == WordUpperType.FirstUpperSecond)
                {
                    strColCodeParm =  FirstLetterUpper(strColCode,false);
                }
                else if(sqlEntity.WordUpperType == WordUpperType.FirstUpperAll)
                {
                    strColCodeParm = FirstLetterUpper(strColCode);
                }

                bool bDynamicCol = false;

                switch (sqlEntity.ParamType)
                {
                    case SqlParamFormatType.BeginEndHash:
                        strColCodeParm = "#" + strColCodeParm + "#"; //加上#号的列编码
                        break;
                    case SqlParamFormatType.SqlParm:
                        strColCodeParm = sParamPre + strColCodeParm;
                        break;
                    case SqlParamFormatType.MyBatis:
                        strColCodeParm = sDefineFormat.Replace(sParamPre, strColCodeParm);//strColCodeParm = "#{" + strColCodeParm + "}";
                        break;
                    case SqlParamFormatType.MyBatisDynamicColoumn:
                        //strColCodeParm = "#{" + sParamPre + "." + strColCodeParm + "}";
                        if (dtColumnDynamic.Select(DBColumnEntity.SqlString.Name + "='" + strColCode + "'").Length > 0)
                        {
                            bDynamicCol = true;
                        }
                        break;
                    case SqlParamFormatType.UserDefine://自定义
                        strColCodeParm = sDefineFormat.Replace(sParamPre, strColCodeParm);
                        break;
                }

                string sConditionColumn = strNowAnd + strTableAliasAndDot + strColCode;


                if (sqlEntity.SqlType == SqlType.Query && (strColType == "DATE" || strColType == "DATETIME" || strColType.Contains("TIMESTAMP"))) //列为日期时间类型
                {
                    #region 查询的日期时间段处理
                    string strQueryWhereDateRange;

                    string strBeginDateParm = "#" + sBegin + strColCode + "#";
                    string strEndDateParm = "#" + sEnd + strColCode + "#";

                    switch (sqlEntity.ParamType)
                    {
                        case SqlParamFormatType.BeginEndHash:
                            strBeginDateParm = sParamPre + sBegin + strColCode;
                            strEndDateParm = sParamPre + sEnd + strColCode;
                            break;
                        case SqlParamFormatType.SqlParm:
                            strBeginDateParm = sParamPre + sBegin + strColCode;
                            strEndDateParm = sParamPre + sEnd + strColCode;
                            break;
                        case SqlParamFormatType.MyBatis:
                            strBeginDateParm = "#{" + sBegin + strColCode + "}";
                            strEndDateParm = "#{" + sEnd + strColCode + "}";
                            break;
                        case SqlParamFormatType.MyBatisDynamicColoumn:
                            strBeginDateParm = sBegin + strColCode;
                            strEndDateParm = sEnd + strColCode;

                            break;
                        case SqlParamFormatType.UserDefine://自定义
                            strBeginDateParm = sDefineFormat.Replace(sParamPre, sBegin + strColCode);
                            strEndDateParm = sDefineFormat.Replace(sParamPre, sEnd + strColCode);
                            break;
                    }

                    if (_dbServer.DatabaseType == DataBaseType.SqlServer)//SQL Server的时间范围
                    {
                        strQueryWhereDateRange = sConditionColumn + " >='" + strBeginDateParm + "' " + sqlEntity.NewLine + sConditionColumn + " < '" + strBeginDateParm + "' " + sqlEntity.NewLine; //结束日期：注要传入界面结束时间的+1天。
                    }
                    else if (_dbServer.DatabaseType == DataBaseType.SqlServer)//Oracle的时间范围
                    {
                        strQueryWhereDateRange = sConditionColumn + " >= TO_DATE('" + strBeginDateParm + "','YYYY-MM-DD') " + sqlEntity.NewLine + sConditionColumn + " < TO_DATE('" + strEndDateParm + "','YYYY-MM-DD') + 1 " + sqlEntity.NewLine; //结束日期：注要传入界面结束时间的+1天
                    }
                    else
                    {
                        strQueryWhereDateRange = sConditionColumn + " >= '" + strBeginDateParm + "' " + sqlEntity.NewLine + sConditionColumn + " < '" + strEndDateParm + "' " + sqlEntity.NewLine; //结束日期：注要传入界面结束时间的+1天
                    }

                    if (bDynamicCol)
                    {
                        string sColBeginDynamic = sqlEntity.Tab + string.Format("<if test=\"{0} !=null and {0} !=''\">{4}{1}>=#{2}{0}{3} </if>", sDefineFormat + strBeginDateParm, strTableAliasAndDot + strColCodeParm, "{", "}", strNowAnd) + sqlEntity.NewLine;
                        string sColEndDynamic = sqlEntity.Tab + string.Format("<if test=\"{0} !=null and {0} !=''\">AND {1}< #{2}{0}{3} </if>", sDefineFormat + strEndDateParm, strTableAliasAndDot + strColCodeParm, "{", "}") + sqlEntity.NewLine;
                        strQueryWhereDateRange = sColBeginDynamic + sColEndDynamic;
                    }
                    sbWhereSql.Append(strQueryWhereDateRange); //使用范围查询条件
                    #endregion
                }
                else
                {
                    if (bDynamicCol)
                    {
                        if ("1".Equals(strColDynamic))
                        {
                            string sColValueDynamic = sqlEntity.Tab + string.Format("<if test=\"{0} !=null and {0} !=''\">{4}{1}=#{2}{0}{3}</if>", sDefineFormat + strColCodeParm, strTableAliasAndDot + strColCodeParm, "{", "}", strNowAnd) + sqlEntity.NewLine;
                            sbWhereSql.Append(sColValueDynamic);
                        }
                        else
                        {
                            string sColValueDynamic = sqlEntity.Tab + string.Format("{4}{1}=#{2}{0}{3}", sDefineFormat + strColCodeParm, strTableAliasAndDot + strColCodeParm, "{", "}", strNowAnd) + sqlEntity.NewLine;
                            sbWhereSql.Append(sColValueDynamic);
                        }
                    }
                    else
                    {
                        string sColParam = sqlEntity.ParamType == SqlParamFormatType.BeginEndHash ? "'" + strColCodeParm + "'" : strColCodeParm;
                        sbWhereSql.Append(strNowAnd + MakeConditionColumnComment(strColCode, sColParam, "", sqlEntity.ParamType, sColumnWherePre, strColCodeParm));
                    }
                }
                strNowAnd = strAnd;
            }
            #endregion

            for (int i = 0; i < dtMain.Rows.Count; i++)//针对表清单循环
            {
                DataRow drTable = dtMain.Rows[i];
                #region 变量声明
                string strDataTableName = drTable[DBTableEntity.SqlString.Name].ToString().Trim();
                string strDataTableComment = drTable[DBTableEntity.SqlString.Comments].ToString().Trim();

                StringBuilder sbSelect = new StringBuilder();
                StringBuilder sbInsertColums = new StringBuilder();
                StringBuilder sbInsertVale = new StringBuilder();
                StringBuilder sbUpdate = new StringBuilder();

                string strOneSql = "";
                #endregion

                #region 生成SQL
                int iSelectLastNumber = dtColumnSelect.Rows.Count - 1; //选择列的最后一行数值
                for (int j = 0; j < dtColumnSelect.Rows.Count; j++)//针对列清单循环：因为只有一个表，所以第二个网格是该表的全部列
                {
                    DataRow drCol = dtColumnSelect.Rows[j];
                    #region 变量
                    string strColCode = drCol[DBColumnEntity.SqlString.Name].ToString().Trim().ToUpper();
                    string strColType = drCol[DBColumnEntity.SqlString.DataType].ToString().Trim().ToUpper();
                    string strColFixedValue = drCol[DBColumnEntity.SqlString.Default].ToString().Trim();//固定值
                    string strColComments = "";//列说明
                    bool bDynamicCol = false;


                    if (sqlEntity.IsHasRemark)
                    {
                        strColComments = drCol[DBColumnEntity.SqlString.Comments].ToString().Trim();//列说明                                                                                                            
                        strColComments = DataBaseCommon.GetColumnComment(strColCode, strColComments);//默认字段注释处理
                    }
                    string strColValue = ""; //列值  
                    string strNowComma = ","; //当前使用的逗号，最后一列的新增和修改是不用加逗号的，该值将会改为空值
                    #endregion

                    //转换列的编码
                    string strColCodeParm = strColCode;
                    if (sqlEntity.WordUpperType == WordUpperType.FirstUpperSecond)
                    {
                        strColCodeParm = FirstLetterUpper(strColCode, false);
                    }
                    else if (sqlEntity.WordUpperType == WordUpperType.FirstUpperAll)
                    {
                        strColCodeParm = FirstLetterUpper(strColCode);
                    }

                    if (string.IsNullOrEmpty(strColFixedValue)) //没有输入固定值
                    {
                        switch (sqlEntity.ParamType)
                        {
                            case SqlParamFormatType.BeginEndHash:
                                strColCodeParm = "#" + strColCodeParm + "#"; //加上#号的列编码
                                break;
                            case SqlParamFormatType.SqlParm:
                                strColCodeParm = sParamPre + strColCodeParm;
                                break;
                            case SqlParamFormatType.MyBatis:
                                strColCodeParm = sDefineFormat.Replace(sParamPre, strColCodeParm);//strColCodeParm = "#{" + strColCodeParm + "}";
                                break;
                            case SqlParamFormatType.MyBatisDynamicColoumn:
                                //strColCodeParm = "#{" + sParamPre + "." + strColCodeParm + "}";
                                if (dtColumnDynamic.Select(DBColumnEntity.SqlString.Name + "='" + strColCode + "'").Length > 0)
                                {
                                    bDynamicCol = true;
                                }
                                break;
                            case SqlParamFormatType.UserDefine://自定义
                                strColCodeParm = sDefineFormat.Replace(sParamPre, strColCodeParm);
                                break;
                        }
                    }
                    else //网格输入了固定值
                    {
                        strColValue = strColFixedValue;
                    }

                    //生成SQL
                    if (sqlEntity.SqlType == SqlType.Insert)
                    {
                        #region 新增(只能首尾分开拼接，没有条件)
                        string sValues = ckbNewLine.Checked ? "VALUES" : " VALUES";
                        sbWhereSql.Clear();
                        strTableAlias = "";
                        if ((j == 0 && j == iSelectLastNumber) || j == iSelectLastNumber)//只有一列
                        {
                            strNowComma = "";
                        }
                        string sColInsert = strTableAliasAndDot + strColCode + strNowComma + sqlEntity.NewLine;
                        string sColInsertDynamic = string.Format("<if test=\"{0} != null and {0} != ''\">{1},</if>", sDefineFormat + strColCodeParm, strTableAliasAndDot + strColCodeParm) + sqlEntity.NewLine;

                        string sColValueComment = MakeColumnValueComment(sqlEntity.SqlType, strNowComma, strColCode, strColValue, strColComments, strColType, sqlEntity.ParamType, strColCodeParm);
                        string sColValueDynamic = sqlEntity.Tab + string.Format("<if test=\"{0} != null and {0} != ''\">#{1}{0}{2},</if>", sDefineFormat + strColCodeParm, "{","}");

                        if (j == 0) //首行
                        {
                            if (j == iSelectLastNumber)//只有一列
                            {
                                sbInsertColums.Append("INSERT INTO " + MakeTableComment(strDataTableName + DataBaseCommon.AddRightBand(strTableAlias), strDataTableComment)
                                        + "(" + sqlEntity.NewLine + sqlEntity.Tab + sColInsert + ")" + sqlEntity.NewLine);
                                sbInsertVale.Append(sValues + sqlEntity.NewLine + "(" + sqlEntity.NewLine + sColValueComment + sqlEntity.NewLine + ")" + sqlEntity.NewLine);
                            }
                            else
                            {
                                if(bDynamicCol)
                                {
                                    sColInsert = sColInsertDynamic;
                                    sColValueComment = sColValueDynamic;
                                }
                                sbInsertColums.Append("INSERT INTO " + MakeTableComment(strDataTableName + DataBaseCommon.AddRightBand(strTableAlias), strDataTableComment)
                                        + "(" + sqlEntity.NewLine + "" + sqlEntity.Tab + sColInsert);
                                sbInsertVale.Append(sValues + sqlEntity.NewLine + "(" + sqlEntity.NewLine + "" + sColValueComment + sqlEntity.NewLine);
                            }
                        }
                        else if (j != iSelectLastNumber) //中间行
                        {
                            if (bDynamicCol)
                            {
                                sColInsert = sColInsertDynamic;
                                sColValueComment = sColValueDynamic;
                            }
                            sbInsertColums.Append(sqlEntity.Tab + sColInsert);
                            sbInsertVale.Append(sColValueComment + sqlEntity.NewLine);
                        }
                        else //尾行
                        {
                            sbInsertColums.Append(sqlEntity.Tab + strTableAliasAndDot + strColCode + "" + sqlEntity.NewLine + ")" + sqlEntity.NewLine);
                            //最后一行不用加逗号
                            sbInsertVale.Append(sColValueComment + sqlEntity.NewLine + ")" + sqlEntity.NewLine);
                        }
                        #endregion
                    }
                    else if (sqlEntity.SqlType == SqlType.Update)
                    {
                        #region 修改（直接拼接，条件为独立拼接）
                        if ((j == 0 && j == iSelectLastNumber) || j == iSelectLastNumber)//只有一列
                        {
                            strNowComma = "";
                        }
                        strTableAlias = sqlEntity.TableAlias;
                        string sColValueComment = MakeColumnValueComment(sqlEntity.SqlType, strNowComma, strColCode, strColValue, strColComments, strColType, sqlEntity.ParamType, strColCodeParm) + sqlEntity.NewLine;
                        string sColValueDynamic = sqlEntity.Tab + string.Format("<if test=\"{0} != null and {0} !=''\">{1}=#{2}{0}{3},</if>", sDefineFormat + strColCodeParm, strTableAliasAndDot + strColCodeParm, "{", "}") + sqlEntity.NewLine;
                        string sSet = ckbNewLine.Checked ? "SET " : " SET ";

                        if (j == 0) //首行
                        {
                            sbUpdate.Append("UPDATE " + MakeTableComment(strDataTableName + DataBaseCommon.AddRightBand(strTableAlias), strDataTableComment) + sSet + sColValueComment);
                        }
                        else if (j != iSelectLastNumber) //中间行
                        {
                            if (bDynamicCol)
                            {
                                sColValueComment = sColValueDynamic;
                            }
                            sbUpdate.Append(sColValueComment);
                        }
                        else //尾行
                        {
                            sbUpdate.Append(sColValueComment);
                        }
                        #endregion
                    }
                    else if (sqlEntity.SqlType == SqlType.Query)
                    {
                        #region 查询（直接拼接，条件为独立拼接）
                        string sFrom = ckbNewLine.Checked ? "FROM " : " FROM ";
                        if ((j == 0 && j == iSelectLastNumber) || j == iSelectLastNumber)//只有一列
                        {
                            strNowComma = "";
                        }
                        string sColValueComment = MakeQueryColumnComment(strNowComma, strTableAliasAndDot + strColCode, strColComments) + sqlEntity.NewLine;
                        strTableAlias = sqlEntity.TableAlias;
                        if (j == 0) //首行
                        {
                            if (j == iSelectLastNumber)//只有一列
                            {
                                sbSelect.Append("SELECT " + sColValueComment + sFrom + MakeTableComment(strDataTableName + DataBaseCommon.AddRightBand(strTableAlias), strDataTableComment));
                            }
                            else
                            {
                                sbSelect.Append("SELECT " + sColValueComment);
                            }
                        }
                        else if (j != iSelectLastNumber) //中间行
                        {
                            sbSelect.Append(sColValueComment);
                        }
                        else //尾行
                        {
                            sbSelect.Append(sColValueComment + sFrom + MakeTableComment(strDataTableName + DataBaseCommon.AddRightBand(strTableAlias), strDataTableComment));
                        }
                        #endregion
                    }
                }

                if (sqlEntity.SqlType == SqlType.Insert)
                {
                    strOneSql = sbInsertColums.ToString() + sbInsertVale.ToString();
                }
                else if (sqlEntity.SqlType == SqlType.Delete)
                {
                    strTableAlias = sqlEntity.TableAlias;
                    //最简单：使用表名和独立的条件拼接即可
                    strOneSql = "DELETE FROM " + strDataTableName + DataBaseCommon.AddRightBand(strTableAlias) + sqlEntity.NewLine + sbWhereSql.ToString();
                }
                else if (sqlEntity.SqlType == SqlType.Update)
                {
                    strOneSql = sbUpdate.ToString() + sbWhereSql.ToString();
                }
                else if (sqlEntity.SqlType == SqlType.Query)
                {
                    strOneSql = sbSelect.ToString() + sbWhereSql.ToString();
                }
                else //SqlType.Parameter
                {
                }
                #endregion

                sbAllSql.Append(strOneSql);
                i++;//下一个表
            }
            rtbResult.Clear();
            rtbResult.AppendText(sbAllSql.ToString() + sqlEntity.NewLine);
            Clipboard.SetData(DataFormats.UnicodeText, sbAllSql.ToString());
            tabControl1.SelectedTab = tpAutoSQL;
            //生成SQL成功后提示
            ShowInfo(_strAutoSqlSuccess);
            return;
            #endregion
        }

        private static string FirstLetterUpper(string strColCode,bool isFirstWorldUpper=true)
        {
            strColCode = strColCode.ToLower();
            string[] firstUpper = strColCode.Split('_');
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var s in firstUpper)
            {
                if(i==0 && !isFirstWorldUpper)
                {
                    sb.Append(s);
                }
                else
                {
                    sb.Append(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s));
                }
                i++;
            }
            strColCode = sb.ToString();
            return strColCode;
        }
        #endregion

        #region 帮助按钮事件
        private void tsbHelp_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 退出按钮事件
        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region 获取表清单复选框变化事件
        private void ckbGetTableList_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbGetTableList.Checked)
            {
                _dbServer = uC_DbConnection1.GetDbServerInfo();
                if (_dbServer == null)
                {
                    return;
                }
                //绑定下拉框
                cbbTableName.BindDropDownList(uC_DbConnection1.UserTableList.Sort("TABLE_NAME"), "TABLE_NAME", "TABLE_NAME", false);
                //查找自动完成数据源
                cbbTableName.AutoCompleteCustomSource.AddRange(uC_DbConnection1.UserTableList.AsEnumerable().Select(x => x.Field<string>("TABLE_NAME")).ToArray());
            }
            else
            {
                cbbTableName.DataSource = null;
            }
        }
        #endregion

        private void CkbUseDefaultConfig_CheckedChanged(object sender, EventArgs e)
        {
            SetDefaultValue(null);
        }

        private void SetDefaultValue(DataTable dtSec)
        {
            if (dtSec == null)
            {
                if (!ckbUseDefaultConfig.Checked || _dbServer == null || _dtDefault == null || _dtDefault.Rows.Count == 0)
                {
                    return;
                }
                txbTableShortName.Focus();
                dtSec = dgvColList.GetBindingTable();
                if (dtSec.Rows.Count == 0)
                {
                    return;
                }
            }

            string sDefaultColName;
            switch (_dbServer.DatabaseType)
            {
                case DataBaseType.SqlServer:
                    sDefaultColName = DT_DBT_BD_COLUMN_DEFAULT.SqlString.DEFAULT_SQLSERVER;
                    break;
                case DataBaseType.Oracle:
                    sDefaultColName = DT_DBT_BD_COLUMN_DEFAULT.SqlString.DEFAULT_ORACLE;
                    break;
                case DataBaseType.MySql:
                    sDefaultColName = DT_DBT_BD_COLUMN_DEFAULT.SqlString.DEFAULT_MYSQL;
                    break;
                case DataBaseType.SQLite:
                    sDefaultColName = DT_DBT_BD_COLUMN_DEFAULT.SqlString.DEFAULT_SQLITE;
                    break;
                case DataBaseType.PostgreSql:
                    sDefaultColName = DT_DBT_BD_COLUMN_DEFAULT.SqlString.DEFAULT_POSTGRESQL;
                    break;
                default:
                    throw new Exception("暂不支持该数据库类型！！");
            }

            string sConditionColName = "";
            SqlType sqlTypeNow = GetSqlType();
            switch (sqlTypeNow)
            {
                case SqlType.Insert:
                    break;
                case SqlType.Update:
                    sConditionColName = DT_DBT_BD_COLUMN_DEFAULT.SqlString.IS_CONDITION_UPDATE;
                    break;
                case SqlType.Query:
                    sConditionColName = DT_DBT_BD_COLUMN_DEFAULT.SqlString.IS_CONDITION_QUERY;
                    break;
                case SqlType.Delete:
                    sConditionColName = DT_DBT_BD_COLUMN_DEFAULT.SqlString.IS_CONDITION_DELETE;
                    break;
                case SqlType.Parameter:
                    break;
                default:
                    break;
            }

            foreach (DataRow drD in _dtDefault.Rows)//一般全局配置的数据比较少，以它循环速度快
            {
                string sColCode = drD[DT_DBT_BD_COLUMN_DEFAULT.SqlString.COLUMN_NAME].ToString().Trim().ToUpper();
                string sFiter = string.Format("{0}='{1}'", DBColumnEntity.SqlString.Name, sColCode);
                var drArr = dtSec.Select(sFiter);
                if (drArr.Length == 0) return;

                drArr[0][DBColumnEntity.SqlString.Default] = drD[sDefaultColName];//使用全局配置中的默认值
                if (string.IsNullOrEmpty(sConditionColName)) return;
                drArr[0][_sGridColumnCondition] = drD[sConditionColName];//选择为条件

            }
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlType sqlTypeNow = GetSqlType();
            if (sqlTypeNow == SqlType.Insert || sqlTypeNow == SqlType.Parameter)
            {
                return;
            }

            DataTable dtSec = dgvColList.GetBindingTable();
            if (_dbServer == null || dtSec == null || dtSec.Rows.Count == 0)
            {
                return;
            }

            if (sqlTypeNow == SqlType.Update)//只针对更新，其条件要加上并发控制ID
            {
                DataRow[] drUpdateControlColumn = dtSec.Select(DBColumnEntity.SqlString.Name + "='" + _strUpdateCtrolColumnCode + "'");//得到并发ID行
                if (drUpdateControlColumn.Length == 0)
                {
                    return;
                }
                if (drUpdateControlColumn[0][_sGridColumnCondition].ToString().Equals("0"))//
                {
                    drUpdateControlColumn[0][_sGridColumnCondition] = "1";
                }
            }

            if (ckbUseDefaultConfig.Checked)
            {
                SetDefaultValue(dtSec);
            }
        }

        private SqlType GetSqlType()
        {
            SqlType sqlTypeNow;
            switch (cmbType.SelectedValue.ToString())
            {
                case "1":
                    sqlTypeNow = SqlType.Insert;
                    break;
                case "2":
                    sqlTypeNow = SqlType.Update;
                    break;
                case "3":
                    sqlTypeNow = SqlType.Query;
                    break;
                case "4":
                    sqlTypeNow = SqlType.Delete;
                    break;
                default:
                    throw new Exception("暂不支持该SqlType枚举");
            }
            return sqlTypeNow;
        }

        private void CbbParaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbParaType.SelectedValue == null) return;
            string sType = cbbParaType.SelectedValue.ToString();
            switch (sType)
            {
                case "1":
                    lblParam.Visible = false;
                    txbParamPre.Visible = false;
                    lblDefineFormat.Visible = false;
                    txbDefineFormart.Visible = false;
                    break;
                case "2":
                    txbParamPre.Visible = true;
                    lblParam.Visible = true;
                    lblParam.Text = "参数前缀：";
                    lblDefineFormat.Visible = false;
                    txbDefineFormart.Visible = false;
                    break;
                case "3":
                    lblParam.Visible = false;
                    txbParamPre.Visible = false;
                    lblDefineFormat.Text = "参数格式：";
                    lblDefineFormat.Visible = true;
                    txbDefineFormart.Visible = true;
                    txbDefineFormart.Text = "#{param.@}";
                    break;
                case "4":
                    lblParam.Visible = true;
                    lblParam.Text = "列名替代符：";
                    txbParamPre.Visible = true;
                    lblDefineFormat.Text = "字典参数前缀：";
                    lblDefineFormat.Visible = true;
                    txbDefineFormart.Visible = true;
                    txbDefineFormart.Text = "param.";
                    break;
                case "10":
                    lblParam.Visible = true;
                    lblParam.Text = "列名替代符：";
                    txbParamPre.Visible = true;
                    lblDefineFormat.Text = "自定义格式：";
                    lblDefineFormat.Visible = true;
                    txbDefineFormart.Visible = true;
                    txbDefineFormart.Text = "";
                    break;
            }
        }

        #region 生成增删改查SQL方法
        /// <summary>
        /// 设置表说明
        /// </summary>
        /// <param name="strTableCode"></param>
        /// <param name="strColComments"></param>
        /// <returns></returns>
        public string MakeTableComment(string strTableCode, string strColComments)
        {
            if (!string.IsNullOrEmpty(strColComments))
            {
                return DataBaseCommon.AddLeftBand(strTableCode) + sqlEntity.Tab + "/*" + strColComments + "*/" + sqlEntity.NewLine;
            }
            return DataBaseCommon.AddLeftBand(strTableCode) + sqlEntity.Tab + sqlEntity.NewLine;
        }

        /// <summary>
        /// 设置查询列说明
        /// </summary>
        /// <param name="strComma">为逗号或空</param>
        /// <param name="strColCode">列编码</param>
        /// <param name="strColComments">列说明</param>
        /// <returns></returns>
        public string MakeQueryColumnComment(string strComma, string strColCode, string strColComments)
        {
            if (!string.IsNullOrEmpty(strColComments))
            {
                return sqlEntity.Tab + strColCode + strComma + sqlEntity.Tab + "/*" + strColComments + "*/";
            }
            return sqlEntity.Tab + strColCode + strComma;
        }

        /// <summary>
        /// 设置列值说明方法
        /// </summary>
        /// <param name="strComma">为逗号或空</param>
        /// <param name="strColCode">列编码</param>
        /// <param name="strColValue">列值</param>
        /// <param name="strColComments">列说明</param>
        /// <param name="strColType">列数据类型</param>
        /// <returns></returns>
        private string MakeColumnValueComment(SqlType sqlTypeNow, string strComma, string strColCode, string strColValue, string strColComments, string strColType, SqlParamFormatType paramType, string colParam)
        {
            string strColRemark = "";

            if (!string.IsNullOrEmpty(strColComments))
            {
                if (sqlTypeNow == SqlType.Insert)
                {
                    strColRemark = "/*" + strColCode + ":" + strColComments + "*/";//新增显示列名和备注
                }
                else if (sqlTypeNow == SqlType.Update)
                {
                    strColRemark = "/*" + strColComments + "*/"; //修改不显示列名，只显示备注
                }
            }

            string strColRelValue = "";
            if (string.IsNullOrEmpty(strColValue)) //列没有默认值则加引号
            {
                //列值为空时
                if (strColType == "DATE")
                {
                    //如果是Oracle的时间类型DATE，则需要将字符转为时间格式，要不会报“文字与字符串格式不匹配”错误
                    strColRelValue = "TO_DATE(" + DataBaseCommon.QuotationMark + colParam + DataBaseCommon.QuotationMark + ",'YYYY-MM-DD')";
                    if (paramType == SqlParamFormatType.SqlParm)
                    {
                        strColRelValue = "TO_DATE(" + colParam + ",'YYYY-MM-DD')";
                    }
                }
                else
                {
                    strColRelValue = colParam;
                    if (paramType == SqlParamFormatType.BeginEndHash)
                    {
                        strColRelValue = DataBaseCommon.QuotationMark + colParam + DataBaseCommon.QuotationMark;
                    }
                }
            }
            else //列有默认值则不加引号
            {
                strColRelValue = strColValue;
            }

            if (sqlTypeNow == SqlType.Insert)
            {
                return sqlEntity.Tab + strColRelValue + strComma + sqlEntity.Tab + strColRemark;
            }
            else //sqlTypeNow == SqlType.Update
            {
                return sqlEntity.Tab + strTableAliasAndDot + strColCode + "=" + strColRelValue + strComma + sqlEntity.Tab + strColRemark;
            }
        }


        /// <summary>
        /// 设置条件列说明
        /// </summary>
        /// <param name="strColCode">列编码</param>
        /// <param name="strColValue">列值</param>
        /// <param name="strColComments">列说明</param>
        /// <returns></returns>
        public string MakeConditionColumnComment(string strColCode, string strColValue, string strColComments, SqlParamFormatType paramType, string sTableAliasAndDot, string colParam)
        {
            string strRemark = sqlEntity.NewLine;
            if (!string.IsNullOrEmpty(strColComments))
            {
                strRemark = "/*" + strColComments + "*/" + sqlEntity.NewLine + "";
            }
            if (string.IsNullOrEmpty(strColValue))
            {
                if (paramType == SqlParamFormatType.SqlParm)
                {
                    return sTableAliasAndDot + strColCode + " = " + colParam + sqlEntity.Tab + strRemark;
                }
                //列值为空时，设置为：'#列编码#'
                return sTableAliasAndDot + strColCode + "=" + DataBaseCommon.QuotationMark + colParam + DataBaseCommon.QuotationMark + sqlEntity.Tab + strRemark;
            }
            else
            {
                //有固定值时
                return sTableAliasAndDot + strColCode + "=" + strColValue + sqlEntity.Tab + strRemark;
            }
        }

        #endregion

        private void cbbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbTableName.Text.Trim())) return;
            tsbImport.PerformClick();
        }

        private void dgvColList_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == dgvColList.Columns[_sGridColumnSelect].Index)
            {
                dgvColList.AllChecked(_sGridColumnSelect,ref _allSelect);
            }
            else if (e.ColumnIndex == dgvColList.Columns[_sGridColumnCondition].Index)
            {
                dgvColList.AllChecked(_sGridColumnCondition, ref _allCondition);
            }
            else if (e.ColumnIndex == dgvColList.Columns[_sGridColumnDynamic].Index)
            {
                dgvColList.AllChecked(_sGridColumnDynamic, ref _allDynamic);
            }
            
        }
    }

    public class DBSqlEntity
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表别名
        /// </summary>
        public string TableAlias { get; set; }
        /// <summary>
        /// SQL类型
        /// </summary>
        public SqlType SqlType { get; set; }

        /// <summary>
        /// SQL参数格式类型
        /// </summary>
        public SqlParamFormatType ParamType { get; set; }
        public string ParamCol { get; set; }

        public bool IsHasRemark { get; set; }
        public string NewLine { get; set; }
        public string Tab { get; set; }
        public bool IsUseGlobal { get; set; }
        public WordUpperType WordUpperType { get; set; }


    }

    public enum WordUpperType
    {
        /// <summary>
        /// 所有单词首字母转换大写，如SqlType
        /// </summary>
        FirstUpperAll,
        /// <summary>
        /// 首字母大写（除了第一个）
        /// </summary>
        FirstUpperSecond,
        /// <summary>
        /// 不做转换
        /// </summary>
        None,
    }

}
