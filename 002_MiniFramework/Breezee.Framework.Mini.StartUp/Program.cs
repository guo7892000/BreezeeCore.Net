using Breezee.Core;
using Breezee.Core.WinFormUI;
using Breezee.Framework.Mini.Entity;
using System.Text;

namespace Breezee.Framework.Mini.StartUp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            FrmMiniLogin frmLogin = new FrmMiniLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                frmLogin.Dispose();
                FrmMiniMainMDI frmMain = new FrmMiniMainMDI();
                WinFormContext.Instance.SetMdiParent(frmMain);

                FormApp app = new MiniApp();
                app.SetMain();
                app.LoginForm = frmLogin;
                app.MainForm = frmMain;
                app.Init();

                Application.Run(frmMain);
            }
        }

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