using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.IO;
using Breezee.WorkHelper.DBTool.Entity;
using Breezee.WorkHelper.DBTool.Entity.ExcelTableSQL;
using org.breezee.MyPeachNet;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ƣ����ɱ�ṹ��html�ļ�
    /// �������ߣ���ˮ��
    /// �������ڣ�2012-11-06
    /// ˵�������ɱ�ṹ��html�ļ������㸴�Ƶ�Excel
    /// </summary>
    internal class TableStructGenerator
    {
        #region ����
        public static readonly string TABKEY_TABLE_STRUCT = "Tab_Key_Table_Struct";

        private static string htmlString = string.Empty;

        private static string HtmlString
        {
            get { return htmlString; }
            set { htmlString = value; }
        } 
        #endregion

        /// <summary>
        /// �Ƴ���ṹTabҳ
        /// </summary>
        /// <param name="tabControl"></param>
        public static void RemoveTab(TabControl tabControl)
        {
            if (tabControl.TabPages.ContainsKey(TABKEY_TABLE_STRUCT))
            {
                tabControl.TabPages.RemoveByKey(TABKEY_TABLE_STRUCT);
            }
        }

        /// <summary>
        /// ���ɱ�ṹ����
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="tableList"></param>
        /// <param name="columnList">���嵥</param>
        /// <param name="useDataTypeFull">ȫ����</param>
        /// <param name="useLYTemplate">�Ƿ�ʹ��LYģ��</param>
        /// <param name="useNameRemark">�Ƿ�ʹ�ð��������ı�ע</param>
        public static void Generate(TabControl tabControl, DataTable tableList, DataTable columnList, TableStructGeneratorParamEntity docEntity)
        {
            HtmlString = GenerateHtmlString(tableList, columnList, docEntity);

            if (!tabControl.TabPages.ContainsKey(TABKEY_TABLE_STRUCT))
            {
                tabControl.TabPages.Add(TABKEY_TABLE_STRUCT, "��ṹ���");                
            }

            TabPage tabStruct = tabControl.TabPages[TABKEY_TABLE_STRUCT];
            tabStruct.Controls.Clear();

            WebBrowser browser = new WebBrowser();
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);

            tabStruct.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            browser.Navigate("about:blank");           
        }

        /// <summary>
        /// ������ĵ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;            
            browser.Document.Write(HtmlString);
        }

        /// <summary>
        /// ����Html�ַ�
        /// </summary>
        /// <param name="tableList">���嵥</param>
        /// <param name="columnList">���嵥</param>
        /// <returns></returns>
        private static string GenerateHtmlString(DataTable tableList, DataTable columnList, TableStructGeneratorParamEntity docEntity)
        {
            string htmlTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Html);
            string tableTemplate;
            string columnsTemplate;
            if (docEntity.useOldColumnCode)
            {
                tableTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Table_LY);
                columnsTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Column_Move);
            }
            else
            {
                if (docEntity.useLYTemplate)
                {
                    tableTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Table_LY);
                    columnsTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Column_LY);
                }
                else
                {
                    tableTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Table);
                    columnsTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Column);
                }
            }            

            StringBuilder tableBuilder = new StringBuilder();
            foreach(DataRow rowTable in tableList.Rows)
            {
                string tableCode = rowTable[EntTable.ExcelTable.Code].ToString();
                string commonColumn = rowTable[EntTable.ExcelTable.CommonColumnTableCode].ToString().Trim();
                DataRow[] rowColumns;

                if (string.IsNullOrEmpty(commonColumn))
                {
                    rowColumns = columnList.Select(EntTable.ExcelTable.Code + "='" + tableCode + "'");
                }
                else
                {
                    rowColumns = columnList.Select(EntTable.ExcelTable.Code + "='" + tableCode + "'");
                    DataRow[] rowCommonColumns = columnList.Select(EntTable.ExcelTable.Code + "='" + commonColumn + "'");
                    DataRow[] rowArr = new DataRow[rowColumns.Length + rowCommonColumns.Length];
                    rowColumns.CopyTo(rowArr,0);
                    rowCommonColumns.CopyTo(rowArr, rowColumns.Length);
                    rowColumns = rowArr;
                }

                int index = 1;
                StringBuilder columnBuilder = new StringBuilder();
                foreach (DataRow row in rowColumns)
                {
                    string strColumnChangeType = row[EntTable.ExcelTable.ChangeType].ToString().Trim() == "" ? "����" : row[EntTable.ExcelTable.ChangeType].ToString().Trim();
                    if (strColumnChangeType == "��ɾ����")
                    {
                        strColumnChangeType = "�޸�";
                    }

                    //�Ƿ�ʹ��ȫ�����滻
                    string sDataType = docEntity.useDataTypeFull ? row[ColCommon.ExcelCol.DataTypeFullNew].ToString() : row[ColCommon.ExcelCol.DataTypeNew].ToString();
                    string sColName = row[ColCommon.ExcelCol.Name].ToString();
                    string sSourceRemark = row[ColCommon.ExcelCol.Remark].ToString();
                    string sColRemark;
                    string sColNameAndRemark = sColName + "��" + sSourceRemark;
                    if (docEntity.useNameSameWithRemark)
                    {
                        //��������ͬ��ע����ô�����ͱ�ע����ʹ�á������ƣ���˵����
                        sColName = sColNameAndRemark;
                        sColRemark = sColNameAndRemark;
                    }
                    else
                    {
                        if (docEntity.useRemarkContainsName)
                        {
                            sColRemark = string.IsNullOrEmpty(sSourceRemark) ? sColName : sColNameAndRemark;
                        }
                        else
                        {
                            sColRemark = sSourceRemark;
                        }
                    }

                    //��Web�ַ�
                    string columnString = columnsTemplate.Replace("${ColumnName}", sColName)
                        .Replace("${ColumnCode}", row[ColCommon.ExcelCol.Code].ToString())
                        .Replace("${ColumnType}", sDataType)
                        .Replace("${ColumnWidth}", row[ColCommon.ExcelCol.DataLength].ToString())
                        .Replace("${DecimalPlace}", row[ColCommon.ExcelCol.DataDotLength].ToString())
                        .Replace("${PrimaryKey}", row[ColCommon.ExcelCol.KeyType].ToString())
                        .Replace("${DefaultValue}", row[ColCommon.ExcelCol.Default].ToString())
                        .Replace("${Rule}", row[ColCommon.ExcelCol.NotNull].ToString())
                        .Replace("${Remark}", sColRemark)
                        .Replace("$(No)", index.ToString())
                        .Replace("${ChangeType}", strColumnChangeType);
                    columnBuilder.Append(columnString);
                    index++;
                }

                //�����
                string sTableChangeType = rowTable[EntTable.ExcelTable.ChangeType].ToString();
                if (docEntity.useLYTemplate)
                {
                    sTableChangeType = sTableChangeType.Replace("����","����") +"��";
                }
                string sTableName = rowTable[EntTable.ExcelTable.Name].ToString();
                if(string.IsNullOrEmpty(sTableName))
                {
                    sTableName = docEntity.defaultColumnOrTableName + "��";
                }
                string sTableSoureRemark = rowTable[EntTable.ExcelTable.Remark].ToString();
                string sTableRemark;
                if (docEntity.useRemarkContainsName)
                {
                    sTableRemark = string.IsNullOrEmpty(sTableSoureRemark) ? sTableName : sTableName + "��" + sTableSoureRemark;
                }
                else
                {
                    sTableRemark = sTableSoureRemark;
                }

                string tableString = tableTemplate.Replace("$$(ColumnsHolder)", columnBuilder.ToString())
                    .Replace("${tableName}", sTableName)
                    .Replace("${tableCode}", rowTable[EntTable.ExcelTable.Code].ToString())
                    .Replace("${changeType}", sTableChangeType)
                    .Replace("${tableRemark}", sTableRemark);
                tableBuilder.Append(tableString);
            }

            string html = htmlTemplate.Replace("$$(TableHolder)", tableBuilder.ToString());
            return html;
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <returns></returns>
        private static string LoadTemplate(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
