using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Breezee.Core.Interface.KeyValuePairString;

namespace Breezee.Core.Interface
{
    /// <summary>
    /// �������ƣ���С����Jsonͨ����
    /// �������ߣ��ƹ���
    /// �������ڣ�2023-11-08
    /// ˵�����Թ̶���ʽ��JSON�ļ�ά���ࡣ
    /// </summary>
    public class MiniJsonConfig
    {
        private string _sDirectory; //Ŀ¼��
        private string _sFileName; //�ļ���
        private string _sFullFileName; //�ļ�ȫ·��

        private XmlConfigSaveType _saveType;
        private IList<string> _lstCol;
        private string _sPK;
        private DataTable _dtData;
        private JArray jsonArray;

        public DataTable Data { get => _dtData;  }
        public string DirName { get => _sDirectory;  }
        public string FileName { get => _sFileName; }
        public string FullFileName { get => _sFullFileName; }
        public IList<string> Columns { get => _lstCol; }
        public XmlConfigSaveType SaveType { get => _saveType; }
        /// <summary>
        /// ��������
        /// </summary>
        public string PK { get => _sPK; }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="sDirectory">XML�������ڵ�Ŀ¼</param>
        /// <param name="sFileName">XML�����ļ���</param>
        /// <param name="column">������������������</param>
        /// <param name="sPK">����</param>
        public MiniJsonConfig(string sDirectory, string sFileName,IList<string> lstCol, string sPK="ID")
        {
            if(string.IsNullOrEmpty(sDirectory) || string.IsNullOrEmpty(sDirectory) || lstCol == null || lstCol.Count==0)
            {
                throw new Exception("���δ���Ŀ¼���ļ��������Լ��ϲ���Ϊ�գ�");
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
            //������Ŀ¼ʱ����
            if (!Directory.Exists(sDirectory))
            {
                if (!Directory.Exists(_sDirectory))
                {
                    Directory.CreateDirectory(_sDirectory);
                }
                //�����ڣ��ʹ���
                string sNewArr = "[]";
                File.WriteAllText(_sFullFileName, sNewArr);
                jsonArray = new JArray(sNewArr);
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

            string sFileContent = File.ReadAllText(_sFullFileName);
            jsonArray = JsonConvert.DeserializeObject<JArray>(sFileContent);

            foreach (JObject obj in jsonArray)
            {
                DataRow row = _dtData.NewRow();
                foreach (string s in _lstCol)
                {
                    row[s] = obj[s];
                }
                _dtData.Rows.Add(row);
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

            jsonArray = new JArray();
            foreach (DataRow row in dtItems.Rows)
            {
                JObject jo = new JObject();
                foreach (DataColumn col in dtItems.Columns)
                {
                    jo.Add(new JProperty(col.ColumnName, row[col]));
                }
                jsonArray.Add(jo);
            }

            string sResult = jsonArray.ToString().Trim();
            File.WriteAllText(_sFullFileName, sResult);
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
