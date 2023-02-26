using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using Breezee.Core.Entity;
using Breezee.Framework.Mini.Entity;
using Breezee.Core.WinFormUI;

namespace Breezee.Framework.Mini.StartUp
{
    /// <summary>
    /// �������ƣ�DLLά��
    /// �������ߣ��ƹ���
    /// �������ڣ�2013-10-19
    /// ˵�����Թ̶���ʽ��XML�ļ�ά���ࡣ
    /// </summary>
    public class DLLSet
    {
        private string _strRoot = "xml"; //���ڵ�
        private string _strElement = "Dll"; //�ӽڵ�
        //�ڵ������
        private string _strGuid = "Guid"; //����
        private string _strName = "Name"; //��
        private string _strCode = "Code"; //ֵ
        private string _strFileName = Path.Combine(GlobalValue.StartupPath, MiniStaticString.ConfigDataPath, MiniStaticString.DllFileName);

        /// <summary>
        /// ���캯��
        /// </summary>
        public DLLSet()
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
                    row[_strGuid] = (string)ele.Attribute(_strGuid);
                    row[_strName] = (string)ele.Attribute(_strName);
                    row[_strCode] = (string)ele.Attribute(_strCode);

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
            dt.Columns.Add(_strGuid, typeof(string));
            dt.Columns.Add(_strName, typeof(string));
            dt.Columns.Add(_strCode, typeof(string));
            dt.PrimaryKey = new DataColumn[] { dt.Columns[_strGuid], dt.Columns[_strCode] };
            dt.Columns[_strGuid].DefaultValue = Guid.NewGuid().ToString();
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
                        new XAttribute(_strGuid, row[_strGuid].ToString()),
                        new XAttribute(_strName, row[_strName].ToString()),
                        new XAttribute(_strCode, row[_strCode].ToString())
                    )
                );
            }

            XElement ele = new XElement(_strRoot, keyItems);
            ele.Save(strFileName);
        }
    }
}
