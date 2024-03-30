using Breezee.Core;
using Breezee.Core.Interface;
using Breezee.Core.WinFormUI;
using Breezee.Framework.Mini.Entity;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Breezee.Framework.Mini.StartUp
{
    internal static class Program
    {
        #region Ӧ�ó��������ڵ�
        /// <summary>
        /// Ӧ�ó��������ڵ�
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //����Ӧ������
            WinFormContext.Instance.LoadAppConfig();
            //�򿪵�¼����
            FrmMiniLogin frmLogin = new FrmMiniLogin();
            if (frmLogin.ShowDialog() != DialogResult.OK) return;
            //ɾ���ɰ汾�߼������ϴθ���·��������Ҫɾ���ɰ汾
            WinFormConfig winConfig = WinFormContext.Instance.WinFormConfig;
            string sPrePath = winConfig.Get(GlobalKey.Upgrade_PreVersionPath, "");
            if (!string.IsNullOrEmpty(sPrePath) && "1".Equals(winConfig.Get(GlobalKey.Upgrade_IsDeleteOldVersion, "0")))
            {
                if ("1".Equals(winConfig.Get(GlobalKey.Upgrade_IsDeleteOldVersionNeedConfirm, "1")))
                {
                    if (MessageBox.Show("�ɰ汾·����" + sPrePath + "��ȷ��ɾ����", "ɾ���ɰ汾��ʾ", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Directory.Delete(sPrePath, true);
                    }
                    else
                    {
                        winConfig.Set(GlobalKey.Upgrade_PreVersionPath, "", "����ϸ��汾�ļ���");
                        winConfig.Save();
                    }
                }
                else
                {
                    Directory.Delete(sPrePath, true);
                }
            }
            //����������
            FrmMiniMainMDI frmMain = new FrmMiniMainMDI();
            WinFormContext.Instance.SetMdiParent(frmMain);
            //ȫ��Ӧ����
            FormApp app = new MiniApp();
            app.SetMain();
            app.LoginForm = frmLogin;
            app.MainForm = frmMain;
            app.Init();
            //����Ӧ��
            Application.Run(frmMain);
        }  
        #endregion

        #region Ӧ�ó�������쳣
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            //Log.FatalError(str);

            //AP�����쳣ʱ��ʾ
            if (e.Exception.StackTrace.Contains("SoapHttpClientProtocol"))
            {
                str = "��Զ�̷������޷����ӣ�����ϵ����Ա��";
            }

            MessageBox.Show(str, "ϵͳ����", MessageBoxButtons.OK);
            Clipboard.SetText(str);
        }
        #endregion

        #region �����Զ����쳣��Ϣ
        /// <summary>
        /// �����Զ����쳣��Ϣ
        /// </summary>
        /// <param name="ex">�쳣����</param>
        /// <param name="backStr">�����쳣��Ϣ����exΪnullʱ��Ч</param>
        /// <returns>�쳣�ַ����ı�</returns>
        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************�쳣�ı�****************************");
            sb.AppendLine("������ʱ�䡿��" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("���쳣���͡���" + ex.GetType().Name);
                sb.AppendLine("���쳣��Ϣ����" + ex.Message);
                sb.AppendLine("����ջ���á���" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("��δ�����쳣����" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
        #endregion
    }
}