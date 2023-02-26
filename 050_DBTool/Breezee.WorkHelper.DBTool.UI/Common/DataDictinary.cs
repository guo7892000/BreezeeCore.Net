using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using Breezee.Core.Entity;
using Breezee.Core.Interface;
using Breezee.AutoSQLExecutor.Core;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ƣ������ֵ�ά������
    /// �������ߣ��ƹ���
    /// �������ڣ�2014-3-28
    /// ˵�����Թ̶���ʽ���ֵ�XML�ļ�ά����
    /// </summary>
    public class DataDictinary
    {
        private string _OralceDataDictinaryFileName = "./SaveFile/Oracle�����ֵ�.xml"; //Oracle�����ֵ�
        private string _SqlServerDataDictinaryFileName = "./SaveFile/Sql Server�����ֵ�.xml"; //Sql Server�����ֵ�
        private string _strRoot = "Items"; //���ڵ�
        private string _strElement = "Item"; //�ӽڵ�
        //�ڵ������
        private string _strColumnName = "������"; //����
        private string _strColumnCode = "�б���"; //��
        private string _strColumnType = "����"; //ֵ
        private string _strColumnLength = "����"; //ֵ
        private string _strFileName;

        /// <summary>
        /// ���캯��
        /// </summary>
        public DataDictinary()
        {

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
        public DataTable LoadXMLFile(DataBaseType dbt)
        {
            DataTable dt = GenerateDataStruct();
            if (dbt == DataBaseType.Oracle)
            {
                _strFileName = _OralceDataDictinaryFileName;
            }
            else
            {
                _strFileName = _SqlServerDataDictinaryFileName;
            }
            if (File.Exists(_strFileName))
            {
                //2014-3-24 ȥ���ļ��ĵ�ֻ������
                FileInfo f = new FileInfo(_strFileName);
                f.Attributes = FileAttributes.Normal;

                XDocument doc = XDocument.Load(_strFileName);
                foreach (XElement ele in doc.Root.Elements(_strElement))
                {
                    DataRow row = dt.NewRow();
                    row[_strColumnName] = (string)ele.Element(_strColumnName);
                    row[_strColumnCode] = (string)ele.Element(_strColumnCode);
                    row[_strColumnType] = (string)ele.Element(_strColumnType);
                    row[_strColumnLength] = (string)ele.Element(_strColumnLength);
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
            dt.Columns.Add(_strColumnName, typeof(string));
            dt.Columns.Add(_strColumnCode, typeof(string));
            dt.Columns.Add(_strColumnType, typeof(string));
            dt.Columns.Add(_strColumnLength, typeof(string));
            //������ΪΨһ����
            dt.PrimaryKey = new DataColumn[] { dt.Columns[_strColumnCode] };
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
                        new XElement(_strColumnName, row[_strColumnName].ToString()),
                        new XElement(_strColumnCode, row[_strColumnCode].ToString()),
                        new XElement(_strColumnType, row[_strColumnType].ToString()),
                        new XElement(_strColumnLength, row[_strColumnLength].ToString())
                    )
                );
            }

            XElement ele = new XElement(_strRoot, keyItems);
            ele.Save(strFileName);
        }
    }
}
