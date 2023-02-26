using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using Breezee.WorkHelper.DBTool.Entity;
using Breezee.AutoSQLExecutor.Core;
using Breezee.Core.Interface;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ƣ���ֵά������
    /// �������ߣ��ƹ���
    /// �������ڣ�2013-10-19
    /// ˵�����Թ̶���ʽ��XML�ļ�ά���ࡣ��лˮ���֧�֣�
    /// </summary>
    public class KeyValue
    {
        private string _OrlFileName = DBTGlobalValue.Base.DefaultValue_Oracle; //Oracle�����޸ĵ�Ĭ��ֵ����
        private string _OrlExcludeFileName = DBTGlobalValue.Base.Exclude_Oracle; //Oracle�����޸Ĳ�ѯ���ų��ֶ�����
        private string _SQLFileName = DBTGlobalValue.Base.DefaultValue_SqlServer; //SqlServer�����޸ĵ�Ĭ��ֵ����
        private string _SQLExcludeFileName = DBTGlobalValue.Base.Exclude_SqlServer; //SqlServer�����޸Ĳ�ѯ���ų��ֶ�����
        private string _strRoot = "Items"; //���ڵ�
        private string _strElement = "Item"; //�ӽڵ�
        private string _strType = "TYPE"; //����
        private string _strKey = "KEY"; //��
        private string _strValue = "VALUE"; //ֵ
        private string _strFileName;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="strDBType">���ݿ�����</param>
        /// <param name="strDealType">��������</param>
        public KeyValue(DataBaseType dbt, AutoSqlColumnSetType ascst)
        {
            if (dbt == DataBaseType.Oracle)
            {
                if (ascst == AutoSqlColumnSetType.Default)
                {
                    _strFileName = _OrlFileName;
                }
                else
                {
                    _strFileName = _OrlExcludeFileName;
                }
            }
            else
            {
                if (ascst == AutoSqlColumnSetType.Default)
                {
                    _strFileName = _SQLFileName;
                }
                else
                {
                    _strFileName = _SQLExcludeFileName;
                }
            }
        }

        /// <summary>
        /// ��ȡ�����ļ�����
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return _strFileName;
        }
        
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <returns></returns>
        public DataTable LoadXMLFile()
        {
            DataTable dt = GenerateDataStruct();

            if (File.Exists(_strFileName))
            {
                //2014-3-24 ȥ���ļ��ĵ�ֻ������
                FileInfo f = new FileInfo(_strFileName);
                f.Attributes = FileAttributes.Normal;

                XDocument doc = XDocument.Load(_strFileName);
                foreach (XElement ele in doc.Root.Elements(_strElement))
                {
                    DataRow row = dt.NewRow();
                    row[_strType] = (string)ele.Element(_strType);
                    row[_strKey] = (string)ele.Element(_strKey);
                    row[_strValue] = (string)ele.Element(_strValue);

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        /// <summary>
        /// ���ɱ�ṹ
        /// </summary>
        /// <returns></returns>
        public DataTable GenerateDataStruct()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(_strType, typeof(string));
            dt.Columns.Add(_strKey, typeof(string));
            dt.Columns.Add(_strValue, typeof(string));
            dt.PrimaryKey = new DataColumn[] { dt.Columns[_strType], dt.Columns[_strKey] };
            return dt;
        }

        /// <summary>
        /// ����XML�ļ�����
        /// </summary>
        /// <param name="dtItems"></param>
        public void SaveXMLFile(DataTable dtItems, string strFileName)
        {
            if (dtItems == null)
            {
                return;
            }
            List<XElement> keyItems = new List<XElement>(dtItems.Rows.Count);
            foreach (DataRow row in dtItems.Rows)
            {
                keyItems.Add
                (
                    new XElement
                    (
                        _strElement,
                        new XElement(_strType, row[_strType].ToString()),
                        new XElement(_strKey, row[_strKey].ToString()),
                        new XElement(_strValue, row[_strValue].ToString())
                    )
                );
            }

            XElement ele = new XElement(_strRoot, keyItems);
            ele.Save(strFileName);
        }
    }
}
