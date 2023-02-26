using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Data.OleDb;

/***************************************
 * �������ƣ��ַ���������
 * ��������ַ���
 * �������ߣ��ƹ���
 * �������ڣ�2014-7-25
 * ����˵�������ִ�����
 * �޸���ʷ��
 *      V1.0 �½� hgh 2014-7-25
 * ************************************/
namespace Breezee.Core.Tool
{
    /// <summary>
    /// �ַ���������
    /// </summary>
    public class StringHelper
    {
        #region ����
        private static readonly string _strBlank = " "; //�ո�
        private static readonly string _strQuotationMark = "'"; 
        #endregion

        #region ת��ΪSQL��ѯ����
        /// <summary>
        /// ת��ΪSQL��ѯ����
        /// </summary>
        /// <returns></returns>
        public static string ConvertToSQLQueryParam(string strInParam)
        {
            return "#" + strInParam + "#";
        } 
        #endregion

        #region ����GUID�ķ���(�����ӷ�)
        /// <summary>
        /// ����GUID�ķ���(�����ӷ�)������32λ��д�ַ�
        /// </summary>
        /// <returns></returns>
        public static string GetGUID()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        } 
        #endregion

        #region �ж��Ƿ�Ϊ˫���ȸ���
        /// <summary>
        /// �ж��Ƿ�Ϊ˫���ȸ���
        /// </summary>
        /// <param name="str">����֤���ַ���</param>
        /// <returns></returns>
        public static bool IsDouble(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    double i = double.Parse(str.Trim());
                    return true;
                }
            }
            catch
            {
            }
            return false;
        } 
        #endregion

        #region �ַ�ת��������
        /// <summary>
        /// �ַ�ת��������(С��λ��������)
        /// </summary>
        /// <param name="str">����֤���ַ���</param>
        /// <returns></returns>
        public static int ToInt(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    str = str.Trim();
                    if (str.Contains("."))//�������С����
                    {
                        int left = int.Parse(str.Split('.')[0]);
                        int right = int.Parse(str.Split('.')[1].Substring(0, 1));
                        if ((double)(right * 1.0 / 10) >= 0.5)
                            return left + 1;
                        else
                            return left;
                    }
                    else
                    {
                        int i = int.Parse(str);
                        return i;
                    }
                }
            }
            catch
            {
            }
            return 0;
        } 
        #endregion

        #region ���ַ�ת������������
        /// <summary>
        /// ���ַ�ת������������
        /// </summary>
        /// <param name="str">��ת�����ַ���</param>
        /// <returns></returns>
        public static DateTime ToDateTime(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    DateTime i = DateTime.Parse(str.Trim());
                    return i;
                }
            }
            catch
            {
            }
            return DateTime.Now.AddYears(-10); //���򷵻�10��ǰ
        } 
        #endregion

        #region ���ַ�ת����˫���ȸ���С��
        /// <summary>
        /// ���ַ�ת����˫���ȸ���С��
        /// </summary>
        /// <param name="str">��ת�����ַ���</param>
        /// <returns></returns>
        public static double ToDouble(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    double i = double.Parse(str.Trim());
                    return i;
                }
            }
            catch
            {
            }
            return 0; //���򷵻�
        } 
        #endregion

        #region ת����decimal����
        /// <summary>
        /// ת����decimal����
        /// </summary>
        /// <param name="str">��ת�����ַ���</param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(string str)
        {
            if (!string.IsNullOrEmpty(str.Trim()))
            {
                decimal value = 0;
                decimal.TryParse(str.Trim(), out value);
                return value;
            }
            return 0;
        } 
        #endregion

        #region ����ʱ��ıȽ�
        /// <summary>
        /// ����ʱ��ıȽ�(��ʼʱ�����ֹʱ��Ƚ�)
        /// </summary>
        /// <param name="beginTime"> ��ʼʱ��</param>
        /// <param name="endTime">��ֹʱ��</param>
        /// <returns></returns>
        public static bool CompareDateTime(string beginTime, string endTime)
        {
            try
            {
                if (!string.IsNullOrEmpty(beginTime.Trim()) && !string.IsNullOrEmpty(endTime.Trim()))
                {
                    DateTime dtBeginTime = DateTime.Parse(beginTime.Trim());
                    DateTime dtEndTime = DateTime.Parse(endTime.Trim());
                    if (dtBeginTime < dtEndTime || dtBeginTime == dtEndTime)
                        return true;
                }
            }
            catch
            { }
            return false;
        } 
        #endregion

        #region ����ʱ��ıȽ�
        /// <summary>
        /// ����ʱ��ıȽ�(�͵�ǰʱ����бȽϣ��Ƿ���ڵ�ǰʱ�䣩
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <param name="isLargerCurrent">�Ƿ���ڵ�ǰʱ��</param>
        /// <returns></returns>
        public static bool CompareCurrentTime(string time, bool isLargerCurrent)
        {
            try
            {
                if (!string.IsNullOrEmpty(time.Trim()))
                {
                    DateTime dtBeginTime = DateTime.Parse(time.Trim());
                    if (dtBeginTime < DateTime.Now && isLargerCurrent == false)  //С�ڵ�ǰʱ��
                        return true;
                    if (dtBeginTime >= DateTime.Now && isLargerCurrent)  //���ڵ�ǰʱ��
                        return true;
                }
            }
            catch
            { }
            return false;
        } 
        #endregion

        #region ��������ַ���
        /// <summary>
        /// ��������ַ���
        /// </summary>
        /// <param name="num">�ַ�����λ��</param>
        /// <returns></returns>
        public static string GetCode(int num)
        {
            string[] source = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string code = "";

            Random rd = new Random();
            for (int i = 0; i < num; i++)
            {
                code += source[rd.Next(0, source.Length)];
            }
            return code;
        } 
        #endregion

        #region �ж�һ�������Ƿ�Ϊ��
        /// <summary>
        /// �ж�һ�������Ƿ�Ϊ�գ�true��ʾ��
        /// </summary>
        /// <param name="val">����Ķ���</param>
        /// <returns>trueΪ��</returns>
        public static bool IsNullOrEmpty(object val)
        {
            if (val == null || val == DBNull.Value)
            {
                return true;
            }

            string s = val.ToString();
            if (string.IsNullOrWhiteSpace(s))
            {
                return true;
            }

            return false;
        } 
        #endregion

        #region ��ֹSQLע��
        /// <summary>
        /// ��ֹSQLע��
        /// </summary>
        /// <param name="strInParm"></param>
        /// <returns></returns>
        public static string PreventSQLInfuse(string strInParm)
        {
            #region ������ʽ����
            Regex reg0 = new Regex("'", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg1 = new Regex("<", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg2 = new Regex(">", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg3 = new Regex("select", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg4 = new Regex("execute", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg5 = new Regex("where", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg6 = new Regex("insert", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg7 = new Regex("delete", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg8 = new Regex("update", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg9 = new Regex("drop", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg10 = new Regex("create", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Regex reg11 = new Regex("truncate", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            #endregion
            // ������ʽ�滻 ��ע�봦��
            string s = reg0.Replace(strInParm, "''"); //�滻 '
            s = reg1.Replace(s, "&lt;"); // �滻 <
            s = reg2.Replace(s, "&gt;"); // �滻 >
            s = reg3.Replace(s, "sel&#101;ct"); // �滻 select
            s = reg4.Replace(s, "&#101xecute"); // �滻 execute
            s = reg5.Replace(s, "wh&#101;re"); // �滻 where
            s = reg6.Replace(s, "ins&#101;rt"); // �滻 insert
            s = reg7.Replace(s, "del&#101;te"); // �滻 delete
            s = reg8.Replace(s, "up&#100;ate"); // �滻 update
            s = reg9.Replace(s, "dro&#112;"); // �滻 drop
            s = reg10.Replace(s, "cr&#101;ate"); // �滻 create
            s = reg11.Replace(s, "trunc&#097;te"); // �滻 truncate

            return s;
        }
        #endregion

        #region ת��Ϊ����
        public static int ToInt(string strName, string input)
        {
            try
            {
                return Convert.ToInt32(input);
            }
            catch
            {
                throw new Exception(strName + "����Ϊ����!");
            }
        }
        #endregion

        #region ת��ΪС��
        public static Decimal ToDecimal(string strName, string input, int iNumber)
        {
            try
            {
                int iInt;
                Decimal dReturn;
                if (int.TryParse(input, out iInt))
                {
                    dReturn = Convert.ToDecimal(iInt * 1.00);
                }
                else
                {
                    dReturn = Convert.ToDecimal(input);
                }

                return Decimal.Round(dReturn, iNumber);
            }
            catch
            {
                throw new Exception(strName + "����ΪС��!");
            }
        } 
        #endregion

        #region ��ȡ�ַ�����(��Լ���������)
        /// <summary>
        /// ��ȡ�ַ�����(��Լ���������)����\nΪ�м��㣬\tΪ��ͬ��
        /// </summary>
        /// <param name="pasteText"></param>
        /// <returns></returns>
        public static Object[,] GetStringArray(ref string pasteText, ref int iRow, ref int iColumn)
        {
            string strReplace = pasteText.Trim();
            int tnum = 0;
            int nnum = 0;
            //��õ�ǰ���������ݵ�����
            for (int i = 0; i < pasteText.Length; i++)
            {
                if (pasteText.Substring(i, 1) == "\n")
                {
                    nnum++;
                }
            }
            //��õ�ǰ���������ݵ��������Ե�һ��Ϊ׼ȷ������
            string strCols = strReplace.Substring(0, strReplace.IndexOf("\n"));
            for (int i = 0; i < strCols.Length - 1; i++)
            {
                if (strCols.Substring(i, 1) == "\t")
                {
                    tnum++;
                }
            }
            Object[,] data;
            //ճ�����ϵ�����������EXCELʱ��ÿ��ĩ����\n����DATAGRIDVIEW�ڸ���ʱ�����һ��ĩû��\n
            if (pasteText.Substring(pasteText.Length - 1, 1) == "\n")
            {
                nnum = nnum - 1;
            }

            iRow = nnum + 1;
            iColumn = tnum + 1;
            data = new object[nnum + 1, tnum + 1];//����һ����ά����

            String rowstr;
            rowstr = "";
            //�����鸳ֵ
            for (int i = 0; i < iRow; i++)
            {
                for (int colIndex = 0; colIndex < iColumn; colIndex++)
                {
                    //һ���е����һ��
                    if (colIndex == tnum && pasteText.IndexOf("\r") != -1)
                    {
                        rowstr = pasteText.Substring(0, pasteText.IndexOf("\r"));
                    }
                    //���һ�е����һ��
                    if (colIndex == tnum && pasteText.IndexOf("\r") == -1)
                    {
                        rowstr = pasteText.Substring(0);
                    }
                    //��������
                    if (colIndex != tnum)
                    {
                        if (pasteText.IndexOf("\t") == -1)
                        {
                            rowstr = pasteText;
                            pasteText = "";
                        }
                        else
                        {
                            rowstr = pasteText.Substring(0, pasteText.IndexOf("\t"));
                            pasteText = pasteText.Substring(pasteText.IndexOf("\t") + 1);
                        }
                    }
                    data[i, colIndex] = rowstr;
                }
                //��ȡ��һ������
                pasteText = pasteText.Substring(pasteText.IndexOf("\n") + 1);

            }
            return data;
        }
        #endregion

        #region ������߿ո񷽷�
        public static string AddLeftBand(string strColCode)
        {
            return _strBlank + strColCode;
        }
        #endregion

        #region �����ұ߿ո񷽷�
        public static string AddRightBand(string strColCode)
        {
            return strColCode + _strBlank;
        }
        #endregion

        #region �������ұ߿ո񷽷�
        /// <summary>
        /// �������ұ߿ո񷽷�
        /// </summary>
        /// <param name="strColCode"></param>
        /// <returns></returns>
        public static string AddLeftRightBand(string strColCode)
        {
            return _strBlank + strColCode + _strBlank;
        }
        #endregion

        #region �����������ŷ���
        public static string AddLeftRightKuoHao(string strColCode)
        {
            return "(" + strColCode + ")";
        }
        #endregion

        #region �������ҵ����ŷ���
        public static string ChangeIntoSqlString(string strColCode)
        {
            return _strBlank + _strQuotationMark + strColCode + _strQuotationMark + _strBlank;
        }
        #endregion
    }
}
