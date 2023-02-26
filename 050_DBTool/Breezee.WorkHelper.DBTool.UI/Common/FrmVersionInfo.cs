using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Breezee.Core.WinFormUI;

namespace Breezee.WorkHelper.DBTool.UI
{
    /// <summary>
    /// �������ߣ��ƹ���
    /// �������ڣ�2013-06-03
    /// ����˵�����鿴��ά��˵������
    /// </summary>
    public partial class FrmVersionInfo : BaseForm
    {
        //���������
        private string _strTitle; //����
        private string _strContentText; //����
        private string _strFilePath; //�ļ�·��
        private bool _isReadOnly = true; //�Ƿ�ֻ����Ĭ��Ϊ��

        public FrmVersionInfo()
        {

        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="strTitle">����</param>
        /// <param name="strRemark">����</param>
        public FrmVersionInfo(string strTitle, string strContentText)
        {
            _strTitle = strTitle;
            _strContentText = strContentText;
            _strFilePath = "";
            _isReadOnly = true;
            InitializeComponent();
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="strTitle">����</param>
        /// <param name="strFilePath">�ļ�·��</param>
        /// <param name="isReadOnly">�Ƿ�ֻ����true�ǣ�false��</param>
        public FrmVersionInfo(string strTitle, string strFilePath, bool isReadOnly)
        {
            _strTitle = strTitle;
            _strFilePath = strFilePath;
            _isReadOnly = isReadOnly;
            InitializeComponent();
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmVersionInfo_Load(object sender, EventArgs e)
        {
            Text = _strTitle;
            if (_isReadOnly)
            {
                rtbModifyHistory.AppendText(_strContentText);
                tsbSave.Visible = false;
            }
            else
            {
                tsbSave.Visible = true;
                if (string.IsNullOrEmpty(_strFilePath))
                {
                    return;
                }

                //2014-3-24 ȥ���ļ��ĵ�ֻ������
                FileInfo f = new FileInfo(_strFilePath);
                f.Attributes = FileAttributes.Normal;

                //�ļ���
                FileStream fs = new FileStream(_strFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                //�ַ���
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                _strContentText = sr.ReadToEnd();
                sr.Close();
                rtbModifyHistory.AppendText(_strContentText);
            }
            rtbModifyHistory.SelectionStart = 0;
        }

        /// <summary>
        /// ���水ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (!_isReadOnly)
            {
                if (string.IsNullOrEmpty(_strFilePath))
                {
                    return;
                }

                //2014-3-24 ȥ���ļ��ĵ�ֻ������
                FileInfo f = new FileInfo(_strFilePath);
                f.Attributes = FileAttributes.Normal;

                StreamWriter sw = new StreamWriter(_strFilePath,false,Encoding.Default);
                sw.Write(rtbModifyHistory.Text);
                sw.Close();
                ShowInfo("����ɹ���");
            }
            else
            {
                ShowInfo("���ܱ��棡"); 
            }
        }

        /// <summary>
        /// �˳���ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}