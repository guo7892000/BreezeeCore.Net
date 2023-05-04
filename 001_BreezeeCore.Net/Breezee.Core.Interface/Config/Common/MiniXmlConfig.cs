using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;

namespace Breezee.Core.Interface
{
    /// <summary>
    /// �������ƣ���С����XMLͨ����
    /// �������ߣ��ƹ���
    /// �������ڣ�2021-08-30
    /// ˵�����Թ̶���ʽ��XML�ļ�ά���ࡣ
    ///     ��лˮ���֧�֣�
    ///     ע�����������ܺ��ո񣬷���ᱨ�����Ʋ����ԡ� ���ַ�(ʮ������ֵ 0x20)��ͷ��
    /// </summary>
    public class MiniXmlConfig
    {
        private string _sDirectory; //Ŀ¼��
        private string _sFileName; //�ļ���
        private string _sFullFileName; //�ļ�ȫ·��
        private string _sRoot; //���ڵ�
        private string _sElement; //�ӽڵ�
        private XmlConfigSaveType _saveType;
        private List<string> _lstCol;
        private string _sPK;
        private DataTable _dtData;

        public DataTable Data { get => _dtData;  }
        public string DirName { get => _sDirectory;  }
        public string FileName { get => _sFileName; }
        public string FullFileName { get => _sFullFileName; }
        public List<string> Columns { get => _lstCol; }
        public XmlConfigSaveType SaveType { get => _saveType; }
        /// <summary>
        /// ��������
        /// </summary>
        public string PK { get => _sPK; }
        /// <summary>
        /// ���ڵ�����
        /// </summary>
        public string Root { get => _sRoot; }
        /// <summary>
        /// �ӽڵ�����
        /// </summary>
        public string Element { get => _sElement; }


        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="sDirectory">XML�������ڵ�Ŀ¼</param>
        /// <param name="sFileName">XML�����ļ���</param>
        /// <param name="column">������������������</param>
        /// <param name="sPK">����</param>
        /// <param name="sRoot">��Ŀ¼��</param>
        /// <param name="sChild">��Ŀ¼��</param>
        /// <param name="saveType">XML���ñ������ͣ����Ի�������</param>
        public MiniXmlConfig(string sDirectory, string sFileName,List<string> lstCol, string sPK, string sRoot="items",string sChild="item", XmlConfigSaveType saveType = XmlConfigSaveType.Attribute)
        {
            if(string.IsNullOrEmpty(sDirectory) || string.IsNullOrEmpty(sDirectory) || lstCol == null || lstCol.Count==0)
            {
                throw new Exception("���δ���Ŀ¼���ļ������м��ϲ���Ϊ�գ�");
            }
            _sDirectory = sDirectory;
            _sFileName = sFileName;
            _sFullFileName = Path.Combine(_sDirectory, sFileName);
            _lstCol = lstCol;
            if (!_lstCol.Contains(sPK))
            {
                _lstCol.Add(sPK);
            }
            _sPK = sPK;
            _sRoot = sRoot;
            _sElement = sChild;
            _saveType = saveType;
            //������Ŀ¼ʱ����
            if (!Directory.Exists(sDirectory))
            {
                if (!Directory.Exists(_sDirectory))
                {
                    Directory.CreateDirectory(_sDirectory);
                }
                //����һ����ʵ��
                XElement ele = new XElement(_sRoot);
                ele.Save(_sFullFileName);
            }
        }

        /// <summary>
        /// �����ļ�����
        /// </summary>
        /// <returns></returns>
        public DataTable Load()
        {
            _dtData = GenerateDataStruct();
            if (!File.Exists(_sFullFileName))
            {
                return _dtData;
            }
            FileInfo f = new FileInfo(_sFullFileName);
            f.Attributes = FileAttributes.Normal;//ȥ���ļ��ĵ�ֻ������

            XDocument doc = XDocument.Load(_sFullFileName);
            if (_saveType == XmlConfigSaveType.Element)
            {
                foreach (XElement ele in doc.Root.Elements(_sElement))
                {
                    DataRow row = _dtData.NewRow();
                    foreach (string s in _lstCol)
                    {
                        row[s] = (string)ele.Element(s);
                    }
                    _dtData.Rows.Add(row);
                }
            }
            else
            {
                foreach (XElement ele in doc.Root.Elements(_sElement))
                {
                    DataRow row = _dtData.NewRow();
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
                    _dtData.Rows.Add(row);
                }
            }
            return _dtData;
        }

        /// <summary>
        /// ����XML�ļ�����
        /// </summary>
        /// <param name="dtItems"></param>
        public void Save(DataTable dtItems = null)
        {
            if (dtItems == null)
            {
                dtItems = _dtData == null ? GenerateDataStruct() : _dtData;
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
            if (!Directory.Exists(_sDirectory))
            {
                Directory.CreateDirectory(_sDirectory);
            }
            ele.Save(_sFullFileName);
        }

        /// <summary>
        /// ���ɱ�ṹ
        /// </summary>
        /// <returns></returns>
        private DataTable GenerateDataStruct()
        {
            _dtData = new DataTable();
            foreach (string s in _lstCol)
            {
                _dtData.Columns.Add(s, typeof(string));
            }
            _dtData.PrimaryKey = new DataColumn[] { _dtData.Columns[_sPK] };
            _dtData.Columns[_sPK].DefaultValue = Guid.NewGuid().ToString();
            return _dtData;
        }
    }
}
