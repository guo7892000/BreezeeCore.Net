using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.IO;
using Breezee.WorkHelper.DBTool.Entity;
using Breezee.WorkHelper.DBTool.Entity.ExcelTableSQL;

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
        /// <param name="columnList"></param>
        public static void Generate(TabControl tabControl, DataTable tableList, DataTable columnList, bool useDataTypeFull, bool useLYTemplate)
        {
            HtmlString = GenerateHtmlString(tableList, columnList, useDataTypeFull, useLYTemplate);

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
        private static string GenerateHtmlString(DataTable tableList, DataTable columnList,bool useDataTypeFull, bool useLYTemplate)
        {
            string htmlTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Html);
            string tableTemplate;
            string columnsTemplate;
            if (useLYTemplate)
            {
                tableTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Table_LY);
                columnsTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Column_LY);
            }
            else
            {
                tableTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Table);
                columnsTemplate = LoadTemplate(DBTGlobalValue.TableSQL.Html_Column);
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
                    string sDataType = useDataTypeFull ? row[ColCommon.ExcelCol.DataTypeFullNew].ToString() : row[ColCommon.ExcelCol.DataTypeNew].ToString();

                    string columnString = columnsTemplate.Replace("${ColumnName}", row[ColCommon.ExcelCol.Name].ToString())
                        .Replace("${ColumnCode}", row[ColCommon.ExcelCol.Code].ToString())
                        .Replace("${ColumnType}", sDataType)
                        .Replace("${ColumnWidth}", row[ColCommon.ExcelCol.DataLength].ToString())
                        .Replace("${DecimalPlace}", row[ColCommon.ExcelCol.DataDotLength].ToString())
                        .Replace("${PrimaryKey}", row[ColCommon.ExcelCol.KeyType].ToString())
                        .Replace("${DefaultValue}", row[ColCommon.ExcelCol.Default].ToString())
                        .Replace("${Rule}", row[ColCommon.ExcelCol.NotNull].ToString())
                        .Replace("${Remark}", row[ColCommon.ExcelCol.Remark].ToString())
                        .Replace("$(No)", index.ToString())
                        .Replace("${ChangeType}", strColumnChangeType);
                    columnBuilder.Append(columnString);
                    index++;
                }

                string tableString = tableTemplate.Replace("$$(ColumnsHolder)", columnBuilder.ToString())
                    .Replace("${tableName}", rowTable[EntTable.ExcelTable.Name].ToString())
                    .Replace("${tableCode}", rowTable[EntTable.ExcelTable.Code].ToString())
                    .Replace("${changeType}", rowTable[EntTable.ExcelTable.ChangeType].ToString())
                    .Replace("${tableRemark}", rowTable[EntTable.ExcelTable.Remark].ToString());
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
