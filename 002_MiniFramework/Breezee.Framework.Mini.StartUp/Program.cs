using Breezee.AutoSQLExecutor.Core;
using Breezee.Core;
using Breezee.Core.Interface;
using Breezee.Core.IOC;
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
            //IoC������DLLע����
            IoCDllRegister.Reg(new string[]
            {
                "Breezee.Core.Adapter.BLL",
                "Breezee.AutoSQLExecutor.Common",
                "Breezee.Framework.Mini.BLL",
                "Breezee.Framework.Mini.DAL",
                "Breezee.Framework.Mini.DAL.SQLite",
                "Breezee.WorkHelper.DBTool.BLL",
                "Breezee.WorkHelper.DBTool.DAL",
                "Breezee.WorkHelper.DBTool.DAL.SQLite"
            });
            //����Ӧ������
            WinFormContext.Instance.LoadAppConfig();
            //�򿪵�¼����
            FrmMiniLogin frmLogin = new FrmMiniLogin();
            if (frmLogin.ShowDialog() != DialogResult.OK) return;
            //ɾ���ɰ汾�ļ�
            DeleteOldVersionFile();
            //SQL��־����ȡֵ
            LoadSqlLogConfig();
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

        #region ɾ���ɰ汾�ļ�
        private static void DeleteOldVersionFile()
        {
            //ɾ���ɰ汾�߼������ϴθ���·��������Ҫɾ���ɰ汾
            WinFormConfig winConfig = WinFormContext.Instance.WinFormConfig;
            try
            {
                string sPrePath = winConfig.Get(GlobalKey.Upgrade_PreVersionPath, "");
                string sExePath = Path.GetFullPath(GlobalContext.AppEntryAssemblyPath);
                if (!string.IsNullOrEmpty(sPrePath) && "1".Equals(winConfig.Get(GlobalKey.Upgrade_IsDeleteOldVersion, "1")))
                {
                    string sPreVersionPath = Path.GetFullPath(sPrePath);
                    bool isSaveDir = sExePath.Equals(sPreVersionPath, StringComparison.OrdinalIgnoreCase);

                    if ("1".Equals(winConfig.Get(GlobalKey.Upgrade_IsDeleteOldVersionNeedConfirm, "1")))
                    {
                        if (Directory.Exists(sPrePath) && !isSaveDir)
                        {
                            if (MsgHelper.ShowOkCancel("�ɰ汾·����" + sPrePath + "��ȷ��ɾ����") == DialogResult.OK)
                            {
                                Directory.Delete(sPrePath, true);
                                winConfig.Set(GlobalKey.Upgrade_PreVersionPath, "", "����ϸ��汾�ļ���");
                                winConfig.Save();
                            }
                            else
                            {
                                winConfig.Set(GlobalKey.Upgrade_PreVersionPath, "", "����ϸ��汾�ļ���");
                                winConfig.Save();
                            }
                        }
                    }
                    else
                    {
                        if (Directory.Exists(sPrePath) && !isSaveDir)
                        {
                            Directory.Delete(sPrePath, true);
                            winConfig.Set(GlobalKey.Upgrade_PreVersionPath, "", "����ϸ��汾�ļ���");
                            winConfig.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                winConfig.Set(GlobalKey.Upgrade_PreVersionPath, "", "����ϸ��汾�ļ���");
                winConfig.Save();
            }
        }
        #endregion
        /// <summary>
        /// SQL��־���þ�̬��ı�����ֵ
        /// </summary>
        private static void LoadSqlLogConfig()
        {
            var _WinFormConfig = WinFormContext.Instance.WinFormConfig;
            //������־
            SqlLogConfig.IsEnableRigthSqlLog = _WinFormConfig.Get(GlobalKey.OkSqlLog_IsEnableLog, "0").Equals("1") ? true : false;
            SqlLogConfig.RigthSqlLogPath = _WinFormConfig.Get(GlobalKey.OkSqlLog_LogPath, @"\SqlLog\ok");
            SqlLogConfig.RightSqlLogKeepDays = int.Parse(_WinFormConfig.Get(GlobalKey.OkSqlLog_KeepDays, "0"));
            SqlLogConfig.RightSqlLogAddType = "1".Equals(_WinFormConfig.Get(GlobalKey.OkSqlLog_AppendType, "1")) ? SqlLogAddType.InsertBegin : SqlLogAddType.AppendEnd;
            //�쳣��־
            SqlLogConfig.IsEnableErrorSqlLog = _WinFormConfig.Get(GlobalKey.ErrSqlLog_IsEnableLog, "0").Equals("1") ? true : false;
            SqlLogConfig.ErrorSqlLogPath = _WinFormConfig.Get(GlobalKey.ErrSqlLog_LogPath, @"\SqlLog\err");
            SqlLogConfig.ErrorSqlLogKeepDays = int.Parse(_WinFormConfig.Get(GlobalKey.ErrSqlLog_KeepDays, "0"));
            SqlLogConfig.ErrorSqlLogAddType = "1".Equals(_WinFormConfig.Get(GlobalKey.ErrSqlLog_AppendType, "1")) ? SqlLogAddType.InsertBegin : SqlLogAddType.AppendEnd;
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