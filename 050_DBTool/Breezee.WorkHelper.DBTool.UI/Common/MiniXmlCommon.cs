using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using Breezee.Core.Entity;
using Breezee.Core.Interface;
using Breezee.WorkHelper.DBTool.Entity;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ƣ�����XMLͨ����
    /// �������ߣ��ƹ���
    /// �������ڣ�2021-08-30
    /// ˵�����Թ̶���ʽ��XML�ļ�ά���ࡣ
    ///     ��лˮ���֧�֣�
    /// </summary>
    public class MiniXmlCommon
    {
        private string _sFileName;
        private string _sRoot; //���ڵ�
        private string _sElement; //�ӽڵ�
        private XmlConfigSaveType _saveType;
        private List<string> _lstCol;
        private string _sPK;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="sPath">XML�ļ�·��</param>
        /// <param name="column">�����������������飺��һ����Ϊ����</param>
        /// <param name="sRoot">��Ŀ¼��</param>
        /// <param name="sChild">��Ŀ¼��</param>
        /// <param name="saveType">XML���ñ������ͣ����Ի�������</param>
        public MiniXmlCommon(string sPath,List<string> lstCol, string sPK, string sRoot="items",string sChild="item", XmlConfigSaveType saveType = XmlConfigSaveType.Attribute)
        {
            if(string.IsNullOrEmpty(sPath) || lstCol == null || lstCol.Count==0)
            {
                throw new Exception("���δ���·�����м��ϲ���Ϊ�գ�");
            }

            _sFileName = sPath;
            _lstCol = lstCol;
            if(!_lstCol.Contains(sPK))
            {
                _lstCol.Add(sPK);
            }
            _sPK = sPK;
            _sRoot = sRoot;
            _sElement = sChild;
            _saveType = saveType;
        }

        /// <summary>
        /// ��ȡ�����ļ�����
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return _sFileName;
        }
        
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <returns></returns>
        public DataTable LoadXMLFile()
        {
            DataTable dt = GenerateDataStruct();
            if (!File.Exists(_sFileName))
            {
                return dt;
            }
            FileInfo f = new FileInfo(_sFileName);
            f.Attributes = FileAttributes.Normal;//ȥ���ļ��ĵ�ֻ������

            XDocument doc = XDocument.Load(_sFileName);
            if (_saveType == XmlConfigSaveType.Element)
            {
                foreach (XElement ele in doc.Root.Elements(_sElement))
                {
                    DataRow row = dt.NewRow();
                    foreach (string s in _lstCol)
                    {
                        row[s] = (string)ele.Element(s);
                    }
                    dt.Rows.Add(row);
                }
            }
            else
            {
                foreach (XElement ele in doc.Root.Elements(_sElement))
                {
                    DataRow row = dt.NewRow();
                    foreach (XAttribute att in ele.Attributes())
                    {
                        foreach (string s in _lstCol)
                        {
                            if (att.Name.ToString().ToLower().Equals(s.ToLower()))
                            {
                                row[s] = att.Value.ToString();
                                break;
                            }
                        }

                    }
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
            foreach (string s in _lstCol)
            {
                dt.Columns.Add(s, typeof(string));
            }
            dt.PrimaryKey = new DataColumn[] { dt.Columns[_sPK] };
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
            if (_saveType == XmlConfigSaveType.Element)
            {
                foreach (DataRow row in dtItems.Rows)
                {
                    XElement elChile = new XElement(_sElement);
                    foreach (string s in _lstCol)
                    {
                        elChile.Add(new XElement(s, row[s].ToString()));
                    }
                    keyItems.Add(elChile);
                }
            }
            else
            {
                foreach (DataRow row in dtItems.Rows)
                {
                    XElement elChile = new XElement(_sElement);
                    foreach (string s in _lstCol)
                    {
                        elChile.SetAttributeValue(s, row[s].ToString());
                    }
                    keyItems.Add(elChile);
                }
            }

            XElement ele = new XElement(_sRoot, keyItems);
            ele.Save(strFileName);
        }
    }

    public enum XmlConfigSaveType
    {
        /// <summary>
        /// ���Է�ʽ����
        /// </summary>
        Attribute,
        /// <summary>
        /// �Ӷ���ʽ
        /// </summary>
        Element
    }
}
