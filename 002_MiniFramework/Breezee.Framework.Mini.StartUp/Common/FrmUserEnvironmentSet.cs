﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Breezee.Core.Entity;
using Breezee.Core.Interface;
using Breezee.Core.Tool;
using Breezee.Framework.Mini.Entity;
using Breezee.Core.WinFormUI;
using System.IO;
using Breezee.Core.Tool.Helper;
using Breezee.AutoSQLExecutor.Core;

namespace Breezee.Framework.Mini.StartUp
{
    /// <summary>
    /// 用户环境设置
    /// </summary>
    public partial class FrmUserEnvironmentSet : BaseForm
    {
        #region 全局变量
        //表变量
        DataTable _dtColorNum;//数值颜色类型表
        DataTable _dtColorName;//名称颜色类型表
        private WinFormConfig _WinFormConfig;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmUserEnvironmentSet()
        {
            InitializeComponent();
        }
        #endregion

        #region 加载事件
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUserSkinSet_Load(object sender, EventArgs e)
        {
            _WinFormConfig = WinFormContext.Instance.WinFormConfig;
            DataTable dtFormSksy = MiniKeyValue.GetValue(MiniKeyEnum.FORM_SKIN_TYPE);
            DataTable dtSaveTip = MiniKeyValue.GetValue(MiniKeyEnum.SAVE_TIP);
            _dtColorNum = MiniKeyValue.GetValue(MiniKeyEnum.RBG_VALUE);
            _dtColorName = MiniKeyValue.GetValue(MiniKeyEnum.RBG_NAME);
            //网格头和奇数行颜色
            DataTable dtGridHeaderColor = MiniKeyValue.GetValue(MiniKeyEnum.GRID_HEADER_COLOR);
            DataTable dtGridOddRowColor = MiniKeyValue.GetValue(MiniKeyEnum.GRID_ODD_ROW_COLOR);

            //绑定下拉框
            cbbSkinTypeMain.BindXmlTypeValueDropDownList(dtFormSksy, false, true);
            cbbSkinTypeCommon.BindXmlTypeValueDropDownList(dtFormSksy, false, true);
            cbbMsgType.BindXmlTypeValueDropDownList(dtSaveTip, false, true);
            cbbGridHeaderColor.BindXmlTypeValueDropDownList(dtGridHeaderColor, false, true);
            cbbOddNumberRowColor.BindXmlTypeValueDropDownList(dtGridOddRowColor, false, true);

            _dicString["1"] = "插入到开始位置";
            _dicString["2"] = "追加到末层";
            DataTable dtAppType = _dicString.GetTextValueTable(false);
            cbbAppendType.BindTypeValueDropDownList(dtAppType, false, true);
            cbbOkAppendType.BindTypeValueDropDownList(dtAppType.Copy(), false, true);
            cbbErrAppendType.BindTypeValueDropDownList(dtAppType.Copy(), false, true);

            cbbMsgType.SelectedValue = _WinFormConfig.Get(GlobalKey.SavePromptType, "2");
            //主窗体皮肤类型
            string sMainSkinType = _WinFormConfig.Get(GlobalKey.MainSkinType, "0");
            cbbSkinTypeMain.SelectedValue = sMainSkinType;
            if (sMainSkinType.Equals("0") || sMainSkinType.Equals("1"))
            {
                cbbColorMain.SelectedValue = _WinFormConfig.Get(GlobalKey.MainSkinValue, "0");
            }
            else
            {
                txbSkinValueMain.Text = _WinFormConfig.Get(GlobalKey.MainSkinValue, "0");
            }

            //子窗体
            string sCommonSkinType = _WinFormConfig.Get(GlobalKey.CommonSkinType, "0");
            cbbSkinTypeCommon.SelectedValue = sCommonSkinType;
            if (sCommonSkinType.Equals("0") || sCommonSkinType.Equals("1"))
            {
                cbbColorCommon.SelectedValue = _WinFormConfig.Get(GlobalKey.CommonSkinValue, "0");
            }
            else
            {
                txbSkinValueCommon.Text = _WinFormConfig.Get(GlobalKey.CommonSkinValue, "0");
            }
            //用户自定义的配置路径
            txbMyLoveSettingPath.Text = GlobalContext.AppRootPath;
            lblDgvStyleInfo.Text = "功能退出时会保存网格样式，以便下次加载！如要清除，请先退出所有功能，再点“清除所有网格样式”按钮！";
            txbGirdStyleTempPath.Text = WinFormContext.Instance.DataGridTagHistoryPath;
            txbGirdStyleTempPath.ReadOnly= true;
            //升级配置
            ckbAutoCheckVersion.Checked = _WinFormConfig.Get(GlobalKey.Upgrade_IsAutoCheckVersion, "1").Equals("1") ? true : false;
            ckbUpgradeSuccessDelOldVerion.Checked = _WinFormConfig.Get(GlobalKey.Upgrade_IsDeleteOldVersion, "1").Equals("1") ? true : false; //默认删除旧版本
            ckbDelOldNeedConfirm.Checked = _WinFormConfig.Get(GlobalKey.Upgrade_IsDeleteOldVersionNeedConfirm, "1").Equals("1") ? true : false; //默认删除旧版本需要确认
            ckbUpgradeDelNewZipFile.Checked = _WinFormConfig.Get(GlobalKey.Upgrade_IsDeleteNewVerZipFile, "1").Equals("1") ? true : false; //默认升级新版本成功后删除新版本的压缩包
            toolTip1.SetToolTip(ckbUpgradeSuccessDelOldVerion, "如选中本项，升级完成后会自动删除旧版本，一些杀毒软件会误报为病毒，所以我们可以不选中本项，升级完成后自行删除旧版本！");
            txbUpgradeTempDir.Text = _WinFormConfig.Get(GlobalKey.Upgrade_TempPath, GlobalContext.PathTemp());
            ckbUpgradeAutoAjustDesktopQuickLink.Checked = _WinFormConfig.Get(GlobalKey.Upgrade_IsAutoCorrectDesktopQuickLink, "1").Equals("1") ? true : false;
            //日志配置
            ckbEnableLog.Checked = _WinFormConfig.Get(GlobalKey.GlobalLog_IsEnableLog, "0").Equals("1") ? true : false;
            txbLogPath.Text = _WinFormConfig.Get(GlobalKey.GlobalLog_LogPath, @"\Log");
            nudKeepDays.Value = int.Parse(_WinFormConfig.Get(GlobalKey.GlobalLog_KeepDays, "0"));
            cbbAppendType.SelectedValue = _WinFormConfig.Get(GlobalKey.GlobalLog_AppendType, "1");
            //正常SQL日志配置
            ckbOkEnableLog.Checked = _WinFormConfig.Get(GlobalKey.OkSqlLog_IsEnableLog, "0").Equals("1") ? true : false;
            txbOkLogPath.Text = _WinFormConfig.Get(GlobalKey.OkSqlLog_LogPath, @"\SqlLog\ok");
            nudOkKeepDays.Value = int.Parse(_WinFormConfig.Get(GlobalKey.OkSqlLog_KeepDays, "0"));
            cbbOkAppendType.SelectedValue = _WinFormConfig.Get(GlobalKey.OkSqlLog_AppendType, "1");
            //异常SQL日志配置
            ckbErrEnableLog.Checked = _WinFormConfig.Get(GlobalKey.ErrSqlLog_IsEnableLog, "0").Equals("1") ? true : false;
            txbErrLogPath.Text = _WinFormConfig.Get(GlobalKey.ErrSqlLog_LogPath, @"\SqlLog\err");
            nudErrKeepDays.Value = int.Parse(_WinFormConfig.Get(GlobalKey.ErrSqlLog_KeepDays, "0"));
            cbbErrAppendType.SelectedValue = _WinFormConfig.Get(GlobalKey.ErrSqlLog_AppendType, "1");

            //显示最大窗体数
            nudMaxOpenForm.Value = int.Parse(_WinFormConfig.Get(GlobalKey.MaxOpenFormNum, "15"));
            //网格头配置
            ckbIsDefineGridHeader.Checked = _WinFormConfig.Get(GlobalKey.IsUsedMyDefineGridHeaderStyle, "1").Equals("1") ? true : false;
            nudGridHeaderHight.Value = decimal.Parse(_WinFormConfig.Get(GlobalKey.GridHeaderHeight, "30"));
            cbbGridHeaderColor.SelectedValue = _WinFormConfig.Get(GlobalKey.GridHeaderBackColor, "LightBlue");
            cbbOddNumberRowColor.SelectedValue = _WinFormConfig.Get(GlobalKey.GridOddRowBackColor, "LightYellow");
        }
        #endregion

        #region 保存按钮事件
        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            //主窗体皮肤设置
            if (cbbSkinTypeMain.SelectedValue != null)
            {
                string sSkinTypeMain = cbbSkinTypeMain.SelectedValue.ToString();
                _WinFormConfig.Set(GlobalKey.MainSkinType, sSkinTypeMain, "主窗体皮肤设置");
                #region 主窗体皮肤设置保存
                switch (sSkinTypeMain)
                {
                    case "0": //默认
                        WinFormContext.UserEnvConfig.MainFormSkin.SkinType = FormSkinTypeEnum.Default;
                        WinFormContext.UserEnvConfig.MainFormSkin.ColorRBGOrImagePath = cbbColorMain.SelectedValue.ToString();
                        _WinFormConfig.Set(GlobalKey.MainSkinValue, cbbColorMain.SelectedValue.ToString(), "主窗体皮肤设置：" + cbbColorMain.Text);
                        break;
                    case "1": //选择颜色
                        WinFormContext.UserEnvConfig.MainFormSkin.SkinType = FormSkinTypeEnum.ColorList;
                        WinFormContext.UserEnvConfig.MainFormSkin.ColorRBGOrImagePath = cbbColorMain.SelectedValue.ToString();
                        _WinFormConfig.Set(GlobalKey.MainSkinValue, cbbColorMain.SelectedValue.ToString(), "主窗体皮肤设置：选择颜色");
                        break;
                    case "2": //自定义颜色
                        string strRBG = txbSkinValueMain.Text.Trim().ToString();
                        if (string.IsNullOrEmpty(strRBG))
                        {
                            ShowInfo("请选择一个自定义颜色！");
                            return;
                        }
                        WinFormContext.UserEnvConfig.MainFormSkin.SkinType = FormSkinTypeEnum.ColorDefine;
                        WinFormContext.UserEnvConfig.MainFormSkin.ColorRBGOrImagePath = strRBG;
                        _WinFormConfig.Set(GlobalKey.MainSkinValue, strRBG, "主窗体皮肤设置：自定义颜色");
                        break;
                    case "3": //选择图片
                        string strPicPath = txbSkinValueMain.Text.Trim().ToString();
                        if (string.IsNullOrEmpty(strPicPath))
                        {
                            ShowInfo("请选择一个图片！");
                            return;
                        }
                        WinFormContext.UserEnvConfig.MainFormSkin.SkinType = FormSkinTypeEnum.Picture;
                        WinFormContext.UserEnvConfig.MainFormSkin.ColorRBGOrImagePath = strPicPath;
                        _WinFormConfig.Set(GlobalKey.MainSkinValue, strPicPath, "主窗体皮肤设置：选择图片");
                        break;
                    default:
                        break;
                }
                #endregion
            }
            if (cbbSkinTypeCommon.SelectedValue != null)
            {
                string sSkinTypeCommon = cbbSkinTypeCommon.SelectedValue.ToString();
                _WinFormConfig.Set(GlobalKey.CommonSkinType, sSkinTypeCommon, "子窗体皮肤设置");
                #region 子窗体皮肤设置保存
                switch (sSkinTypeCommon)
                {
                    case "0": //默认
                        WinFormContext.UserEnvConfig.MainFormSkin.SkinType = FormSkinTypeEnum.Default;
                        WinFormContext.UserEnvConfig.MainFormSkin.ColorRBGOrImagePath = cbbColorCommon.SelectedValue.ToString();
                        _WinFormConfig.Set(GlobalKey.CommonSkinValue, cbbColorCommon.SelectedValue.ToString(), "主窗体皮肤设置：" + cbbColorCommon.Text);
                        break;
                    case "1": //选择颜色
                        WinFormContext.UserEnvConfig.MainFormSkin.SkinType = FormSkinTypeEnum.ColorList;
                        WinFormContext.UserEnvConfig.MainFormSkin.ColorRBGOrImagePath = cbbColorCommon.SelectedValue.ToString();
                        _WinFormConfig.Set(GlobalKey.CommonSkinValue, cbbColorCommon.SelectedValue.ToString(), "主窗体皮肤设置：选择颜色");
                        break;
                    case "2": //自定义颜色
                        string strRBG = txbSkinValueCommon.Text.Trim().ToString();
                        if (string.IsNullOrEmpty(strRBG))
                        {
                            ShowInfo("请选择一个自定义颜色！");
                            return;
                        }
                        WinFormContext.UserEnvConfig.MainFormSkin.SkinType = FormSkinTypeEnum.ColorDefine;
                        WinFormContext.UserEnvConfig.MainFormSkin.ColorRBGOrImagePath = strRBG;
                        _WinFormConfig.Set(GlobalKey.CommonSkinValue, strRBG, "主窗体皮肤设置：自定义颜色");
                        break;
                    case "3": //选择图片
                        string strPicPath = txbSkinValueCommon.Text.Trim().ToString();
                        if (string.IsNullOrEmpty(strPicPath))
                        {
                            ShowInfo("请选择一个图片！");
                            return;
                        }
                        WinFormContext.UserEnvConfig.MainFormSkin.SkinType = FormSkinTypeEnum.Picture;
                        WinFormContext.UserEnvConfig.MainFormSkin.ColorRBGOrImagePath = strPicPath;
                        _WinFormConfig.Set(GlobalKey.CommonSkinValue, strPicPath, "主窗体皮肤设置：选择图片");
                        break;
                    default:
                        break;
                }
                #endregion
            }

            if (cbbMsgType.SelectedValue != null)
            {
                #region 保存提示方式
                string strSavePromptType = cbbMsgType.SelectedValue.ToString();
                _WinFormConfig.Set(GlobalKey.SavePromptType, strSavePromptType, "保存提示方式");
                if (strSavePromptType == "2")//仅显示，不弹出
                {
                    WinFormContext.UserEnvConfig.SaveMsgPrompt = SaveMsgPromptTypeEnum.OnlyPromptNotPopup;
                }
                else //弹出提示
                {
                    WinFormContext.UserEnvConfig.SaveMsgPrompt = SaveMsgPromptTypeEnum.PopupPrompt;
                }
                #endregion
            }

            //网格头样式
            WinFormContext.UserEnvConfig.IsUsedMyDefineGridHeaderStyle = ckbIsDefineGridHeader.Checked;
            WinFormContext.UserEnvConfig.GridHeaderHeight = int.Parse(nudGridHeaderHight.Value.ToString());
            WinFormContext.UserEnvConfig.GridHeaderBackColor = Color.FromName(cbbGridHeaderColor.SelectedValue.ToString());
            WinFormContext.UserEnvConfig.OddRowBackColor = Color.FromName(cbbOddNumberRowColor.SelectedValue.ToString());
            _WinFormConfig.Set(GlobalKey.IsUsedMyDefineGridHeaderStyle, ckbIsDefineGridHeader.Checked ? "1" : "0", "是否自定义网格头信息");
            _WinFormConfig.Set(GlobalKey.GridHeaderHeight, nudGridHeaderHight.Value.ToString(), "网格头高度");
            _WinFormConfig.Set(GlobalKey.GridHeaderBackColor, cbbGridHeaderColor.SelectedValue.ToString(), "网格头颜色");
            _WinFormConfig.Set(GlobalKey.GridOddRowBackColor, cbbOddNumberRowColor.SelectedValue.ToString(), "奇数行颜色");
            
            //升级配置
            _WinFormConfig.Set(GlobalKey.Upgrade_IsAutoCheckVersion, ckbAutoCheckVersion.Checked ? "1" : "0", "是否自动检测新版本");
            _WinFormConfig.Set(GlobalKey.Upgrade_IsDeleteOldVersion, ckbUpgradeSuccessDelOldVerion.Checked ? "1" : "0", "是否在新版本升级成功后删除旧版本");
            _WinFormConfig.Set(GlobalKey.Upgrade_IsDeleteOldVersionNeedConfirm, ckbDelOldNeedConfirm.Checked ? "1" : "0", "是否在新版本升级成功后需要确认才删除旧版本");
            _WinFormConfig.Set(GlobalKey.Upgrade_IsDeleteNewVerZipFile, ckbUpgradeDelNewZipFile.Checked ? "1" : "0", "是否升级新版本成功后删除新版本的压缩包");
            _WinFormConfig.Set(GlobalKey.Upgrade_IsAutoCorrectDesktopQuickLink, ckbUpgradeAutoAjustDesktopQuickLink.Checked ? "1" : "0", "是否升级新版本成功后自动修正桌面快捷方式");
            _WinFormConfig.Set(GlobalKey.Upgrade_TempPath, txbUpgradeTempDir.Text.Trim(),"临时升级文件保存路径");
            _WinFormConfig.Set(GlobalKey.MaxOpenFormNum, nudMaxOpenForm.Value.ToString(), "打开窗体的最大数");
            WinFormContext.Instance.MaxOpenFormNum = int.Parse(nudMaxOpenForm.Value.ToString());//重新给全局变量赋值

            //日志配置
            _WinFormConfig.Set(GlobalKey.GlobalLog_IsEnableLog, ckbEnableLog.Checked ? "1" : "0", "是否启用全局日志");
            _WinFormConfig.Set(GlobalKey.GlobalLog_LogPath, txbLogPath.Text.Trim(), "全局日志路径");
            _WinFormConfig.Set(GlobalKey.GlobalLog_KeepDays, nudKeepDays.Value.ToString(), "全局日志保留天数");
            _WinFormConfig.Set(GlobalKey.GlobalLog_AppendType, cbbAppendType.SelectedValue.ToString(), "全局日志追加方式");
            //正常SQL日志配置
            _WinFormConfig.Set(GlobalKey.OkSqlLog_IsEnableLog, ckbOkEnableLog.Checked ? "1" : "0", "是否启用正常SQL日志");
            _WinFormConfig.Set(GlobalKey.OkSqlLog_LogPath, txbOkLogPath.Text.Trim(), "正常SQL日志路径");
            _WinFormConfig.Set(GlobalKey.OkSqlLog_KeepDays, nudOkKeepDays.Value.ToString(), "正常SQL日志保留天数");
            _WinFormConfig.Set(GlobalKey.OkSqlLog_AppendType, cbbOkAppendType.SelectedValue.ToString(), "正常SQL日志追加方式");
            //异常SQL日志配置
            _WinFormConfig.Set(GlobalKey.ErrSqlLog_IsEnableLog, ckbErrEnableLog.Checked ? "1" : "0", "是否启用异常SQL日志");
            _WinFormConfig.Set(GlobalKey.ErrSqlLog_LogPath, txbErrLogPath.Text.Trim(), "异常SQL日志路径");
            _WinFormConfig.Set(GlobalKey.ErrSqlLog_KeepDays, nudErrKeepDays.Value.ToString(), "异常SQL日志保留天数");
            _WinFormConfig.Set(GlobalKey.ErrSqlLog_AppendType, cbbErrAppendType.SelectedValue.ToString(), "异常SQL日志追加方式");
            //SQL日志配置静态类的变量赋值
            //正常日志
            SqlLogConfig.IsEnableRigthSqlLog = ckbOkEnableLog.Checked;
            SqlLogConfig.RigthSqlLogPath = txbOkLogPath.Text.Trim();
            SqlLogConfig.RightSqlLogKeepDays = int.Parse(nudOkKeepDays.Value.ToString());
            SqlLogConfig.RightSqlLogAddType = "1".Equals(cbbOkAppendType.SelectedValue.ToString())? SqlLogAddType.InsertBegin: SqlLogAddType.AppendEnd;
            //异常日志
            SqlLogConfig.IsEnableErrorSqlLog = ckbErrEnableLog.Checked;
            SqlLogConfig.ErrorSqlLogPath = txbErrLogPath.Text.Trim();
            SqlLogConfig.ErrorSqlLogKeepDays = int.Parse(nudErrKeepDays.Value.ToString());
            SqlLogConfig.ErrorSqlLogAddType = "1".Equals(cbbErrAppendType.SelectedValue.ToString()) ? SqlLogAddType.InsertBegin : SqlLogAddType.AppendEnd;

            _WinFormConfig.Save();
            ShowInfo("【用户环境设置】保存成功！");
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region 关闭按钮事件
        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region 主窗体皮肤类型下拉框选择变化事件
        /// <summary>
        /// 主窗体皮肤类型下拉框选择变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbSkinTypeMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbSkinTypeMain.SelectedValue == null)
            {
                return;
            }

            switch (cbbSkinTypeMain.SelectedValue.ToString())
            {
                case "0": //无
                    cbbColorMain.Enabled = true;
                    txbSkinValueMain.Text = "";
                    cbbColorMain.BindXmlTypeValueDropDownList(_dtColorNum, false, true);
                    break;
                case "1": //选择颜色
                    cbbColorMain.Enabled = true;
                    txbSkinValueMain.Text = "";
                    cbbColorMain.BindXmlTypeValueDropDownList(_dtColorName, false, true);
                    break;
                case "2": //自定义颜色
                    cbbColorMain.Enabled = false;
                    //txbSkinValueMain.Text = "";
                    break;
                case "3": //选择图片
                    cbbColorMain.Enabled = false;
                    //txbSkinValueMain.Text = "";
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 子窗体皮肤类型下拉框选择变化事件
        /// <summary>
        /// 子窗体皮肤类型下拉框选择变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbSkinTypeCommon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbSkinTypeCommon.SelectedValue == null)
            {
                return;
            }
            switch (cbbSkinTypeCommon.SelectedValue.ToString())
            {
                case "0": //无
                    cbbColorCommon.Enabled = true;
                    txbSkinValueCommon.Text = "";
                    cbbColorCommon.BindXmlTypeValueDropDownList(_dtColorNum, false, true);
                    break;
                case "1": //选择颜色
                    cbbColorCommon.Enabled = true;
                    txbSkinValueCommon.Text = "";
                    cbbColorCommon.BindXmlTypeValueDropDownList(_dtColorName, false, true);
                    break;
                case "2": //自定义颜色
                    cbbColorCommon.Enabled = false;
                    break;
                case "3": //选择图片
                    cbbColorCommon.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 主窗体选择自定义颜色或图片
        /// <summary>
        /// 主窗体选择自定义颜色或图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectPicMain_Click(object sender, EventArgs e)
        {
            if (cbbSkinTypeMain.SelectedValue == null)
            {
                return;
            }
            switch (cbbSkinTypeMain.SelectedValue.ToString())
            {
                case "0": //无
                    break;
                case "1": //选择颜色
                    break;
                case "2": //自定义颜色
                    if (this.cdlSelectColor.ShowDialog() == DialogResult.OK)
                    {
                        this.txbSkinValueMain.Text = cdlSelectColor.Color.ToArgb().ToString();
                    }
                    break;
                case "3": //选择图片
                    opfSelectPic.Filter = "(*.jpg,*.gif,*.bmp,*.png,*.jpeg)|*.JPG;*.GIF;*.BMP;*.PNG;*.JPEG";
                    if (this.opfSelectPic.ShowDialog() == DialogResult.OK)
                    {
                        this.txbSkinValueMain.Text = opfSelectPic.FileName;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 子窗体选择自定义颜色或图片
        /// <summary>
        /// 子窗体选择自定义颜色或图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectPicCommon_Click(object sender, EventArgs e)
        {
            if (cbbSkinTypeCommon.SelectedValue == null)
            {
                return;
            }
            switch (cbbSkinTypeCommon.SelectedValue.ToString())
            {
                case "0": //无
                    break;
                case "1": //选择颜色
                    break;
                case "2": //自定义颜色
                    if (this.cdlSelectColor.ShowDialog() == DialogResult.OK)
                    {
                        this.txbSkinValueCommon.Text = cdlSelectColor.Color.ToArgb().ToString();
                    }
                    break;
                case "3": //选择图片
                    opfSelectPic.Filter = "(*.jpg,*.gif,*.bmp,*.png,*.jpeg)|*.JPG;*.GIF;*.BMP;*.PNG;*.JPEG";
                    if (this.opfSelectPic.ShowDialog() == DialogResult.OK)
                    {
                        this.txbSkinValueCommon.Text = opfSelectPic.FileName;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 选择我的配置路径按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectMySettingPath_Click(object sender, EventArgs e)
        {
            if (fbdSelectPath.ShowDialog() != DialogResult.OK) return;
            string sDirName = fbdSelectPath.SelectedPath;
            if (Directory.GetFiles(sDirName, "*.*", SearchOption.AllDirectories).Length == 0)
            {
                DialogResult result = MsgHelper.ShowYesNo("确定修改默认配置文件路径？");
                if (result == DialogResult.Yes)
                {
                    //注：这里要取用户喜好目录下的所有文件
                    FileDirHelper.CopyFilesToDirKeepSrcDirName(GlobalContext.PathConfig(), sDirName, true);
                    WinFormContext.Instance.LoadAppConfig(sDirName);//重新加载应用配置
                    txbMyLoveSettingPath.Text = sDirName;
                }
            }
            else
            {
                MsgHelper.ShowErr("请选择一个空目录！");
            }
        }

        private void btnClearDGVStyle_Click(object sender, EventArgs e)
        {
            if (ShowYesNo("请确保除其他功能已关闭，确定要清空所有网格样式？") != DialogResult.Yes) return;
            Directory.Delete(WinFormContext.Instance.DataGridTagHistoryPath, true);
            ShowInfo("成功清空所有网格样式！");
        }

        private void btnSelectUpgradeTmpPath_Click(object sender, EventArgs e)
        {
            if (fbdSelectPath.ShowDialog() != DialogResult.OK) return;
            txbUpgradeTempDir.Text = fbdSelectPath.SelectedPath;
        }

        #region 选择日志路径相关
        private void btnSelectLogPath_Click(object sender, EventArgs e)
        {
            if (fbdSelectPath.ShowDialog() != DialogResult.OK) return;
            string sDirName = fbdSelectPath.SelectedPath;
            if (Directory.GetFiles(sDirName, "*.*", SearchOption.AllDirectories).Length == 0)
            {
                txbLogPath.Text = sDirName;
            }
            else
            {
                MsgHelper.ShowErr("请选择一个空目录！");
            }
        }

        private void btnSelectOkLogPath_Click(object sender, EventArgs e)
        {
            if (fbdSelectPath.ShowDialog() != DialogResult.OK) return;
            string sDirName = fbdSelectPath.SelectedPath;
            if (Directory.GetFiles(sDirName, "*.*", SearchOption.AllDirectories).Length == 0)
            {
                txbOkLogPath.Text = sDirName;
            }
            else
            {
                MsgHelper.ShowErr("请选择一个空目录！");
            }
        }

        private void btnSelectErrLogPath_Click(object sender, EventArgs e)
        {
            if (fbdSelectPath.ShowDialog() != DialogResult.OK) return;
            string sDirName = fbdSelectPath.SelectedPath;
            if (Directory.GetFiles(sDirName, "*.*", SearchOption.AllDirectories).Length == 0)
            {
                txbErrLogPath.Text = sDirName;
            }
            else
            {
                MsgHelper.ShowErr("请选择一个空目录！");
            }
        }
        #endregion

        #region 还原日志默认值
        private void btnLogReset_Click(object sender, EventArgs e)
        {
            //日志配置
            //ckbEnableLog.Checked = false;
            txbLogPath.Text = @"\Log";
            nudKeepDays.Value = 0;
            cbbAppendType.SelectedValue = "1";
        }

        private void btnOkLogReset_Click(object sender, EventArgs e)
        {
            //正常SQL日志配置
            //ckbOkEnableLog.Checked = false;
            txbOkLogPath.Text = @"\SqlLog\ok";
            nudOkKeepDays.Value = 0;
            cbbOkAppendType.SelectedValue = "1";
        }

        private void btnErrLogReset_Click(object sender, EventArgs e)
        {
            //异常SQL日志配置
            //ckbErrEnableLog.Checked = false;
            txbErrLogPath.Text = @"\SqlLog\err";
            nudErrKeepDays.Value = 0;
            cbbErrAppendType.SelectedValue = "1";
        } 
        #endregion
    }
}
