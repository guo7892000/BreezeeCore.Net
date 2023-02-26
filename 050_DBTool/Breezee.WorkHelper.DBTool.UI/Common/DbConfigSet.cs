using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using Breezee.Core.Entity;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ƣ����ݿ�����ά������
    /// �������ߣ��ƹ���
    /// �������ڣ�2013-10-19
    /// ˵�����Թ̶���ʽ��XML�ļ�ά���ࡣ
    /// </summary>
    public class DbConfigSet
    {
        private string _OrlFileName = "./Config/DBTool/Data/DBConfig.xml"; //�����޸ĵ�Ĭ��ֵ����
        private string _strRoot = "Items"; //���ڵ�
        private string _strElement = "Item"; //�ӽڵ�
        //�ڵ������
        private string _strID = "ID";
        private string _strDbConfigName = "DbConfigName";
        private string _strDbType = "DbType"; //DB����
        private string _strServerIP = "ServerIP"; //IP��TnsName
        private string _strDbName = "DbName"; //���ݿ�����
        private string _strUserName = "UserName"; //��
        private string _strPassword = "Password"; //��
        private string _strFileName;
        //
        private DataTable dt;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="strDBType">���ݿ�����</param>
        /// <param name="strDealType">��������</param>
        public DbConfigSet()
        {
            _strFileName = _OrlFileName;
        }
        
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <returns></returns>
        public DataTable LoadXMLFile()
        {
            dt = GenerateDataStruct();

            if (File.Exists(_strFileName))
            {
                //2014-3-24 ȥ���ļ��ĵ�ֻ������
                FileInfo f = new FileInfo(_strFileName);
                f.Attributes = FileAttributes.Normal;

                XDocument doc = XDocument.Load(_strFileName);
                foreach (XElement ele in doc.Root.Elements(_strElement))
                {
                    DataRow row = dt.NewRow();
                    row[_strID] = (string)ele.Element(_strID);
                    row[_strDbConfigName] = (string)ele.Element(_strDbConfigName);
                    row[_strDbType] = (string)ele.Element(_strDbType);
                    row[_strServerIP] = (string)ele.Element(_strServerIP);
                    row[_strDbName] = (string)ele.Element(_strDbName);
                    row[_strUserName] = (string)ele.Element(_strUserName);
                    row[_strPassword] = (string)ele.Element(_strPassword);
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
            dt.Columns.Add(_strID, typeof(string));
            dt.Columns.Add(_strDbConfigName, typeof(string));
            dt.Columns.Add(_strDbType, typeof(string));
            dt.Columns.Add(_strServerIP, typeof(string));
            dt.Columns.Add(_strDbName, typeof(string));
            dt.Columns.Add(_strUserName, typeof(string));
            dt.Columns.Add(_strPassword, typeof(string));
            dt.PrimaryKey = new DataColumn[] { dt.Columns[_strID] };
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
                        new XElement(_strID, row[_strID].ToString()),
                        new XElement(_strDbConfigName, row[_strDbConfigName].ToString()),
                        new XElement(_strDbType, row[_strDbType].ToString()),
                        new XElement(_strServerIP, row[_strServerIP].ToString()),
                        new XElement(_strDbName, row[_strDbName].ToString()),
                        new XElement(_strUserName, row[_strUserName].ToString()),
                        new XElement(_strPassword, row[_strPassword].ToString())
                    )
                );
            }

            XElement ele = new XElement(_strRoot, keyItems);
            ele.Save(strFileName);
        }
    }
}
