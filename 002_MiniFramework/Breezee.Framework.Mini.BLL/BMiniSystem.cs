﻿using Breezee.Core.Tool;
using Breezee.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Breezee.Core.IOC;
using Breezee.Framework.Mini.IDAL;
using Breezee.Framework.Mini.IBLL;

namespace Breezee.Framework.Mini.BLL
{
    /// <summary>
    /// 数据库配置设置
    /// </summary>
    public class BMiniSystem : IMiniSystem
    {
        #region 初始化数据库配置设置
        /// <summary>
        /// 查询数据库配置设置
        /// </summary>
        /// <param name="dicQuery"></param>
        /// <returns></returns>
        public override IDictionary<string, object> InitDB(IDictionary<string, string> dicQuery)
        {
            IDictionary<string, object> dicRet = new Dictionary<string, object>();
            try
            {
                //var dal = ContainerContext.Container.Resolve<IDMiniDBInitializer>();
                dicRet = ExecuteResultHelper.QuerySuccess();
                DBInitializer.InitDataBase();
            }
            catch (Exception ex)
            {
                dicRet = ExecuteResultHelper.FailException(ex);
            }
            return dicRet;
        }
        #endregion

        #region 保存数据库配置
        /// <summary>
        /// 保存数据库配置
        /// </summary>
        /// <author>pansq</author>
        /// <status>已完成</status>
        /// <param name="dicQuery"></param>
        /// <returns></returns>
        //public override IDictionary<string, object> SaveDbConfig(IDictionary<string, object> dicIn)
        //{
        //    IDictionary<string, object> dicRet;
        //    DbTransaction DbTran = null;
        //    IDataAccess dataAccess = DBToolBaseDAC.Dac;
        //    try
        //    {
        //        //1.保存前判断是否存在
        //        DataTable dtSaveSupply = dicIn[IDBConfigSet.SaveDbConfig_InDicKey.DT_TABLE] as DataTable;

        //        IDictionary<string, string> dicQuery = new Dictionary<string, string>();
        //        dicQuery[DT_DBT_BD_DB_CONFIG.DB_CONFIG_CODE] = dtSaveSupply.Rows[0][DT_DBT_BD_DB_CONFIG.DB_CONFIG_CODE].ToString();
        //        //修改判断需要排除当前记录
        //        if (dtSaveSupply.Rows[0].RowState == DataRowState.Modified)
        //        {
        //            dicQuery[DT_DBT_BD_DB_CONFIG.DB_CONFIG_ID] = dtSaveSupply.Rows[0][DT_DBT_BD_DB_CONFIG.DB_CONFIG_ID].ToString();
        //        }

        //        var dal = ContainerContext.Container.Resolve<IDDBConfigSet>();
        //        DataTable dtExists = dal.QueryDbConfigExist(dicQuery);
        //        if (dtExists != null && dtExists.Rows.Count > 0)
        //        {
        //            return Fail("保存失败：该配置已存在！");
        //        }

        //        //2.保存

        //        DbConnection con = dataAccess.GetCurrentConnection();

        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        DbTran = con.BeginTransaction();
        //        //保存
        //        dtSaveSupply = dataAccess.SaveTable(dtSaveSupply, con, DbTran);

        //        DbTran.Commit();
        //        return Success();
        //    }
        //    catch (Exception ex)
        //    {
        //        dicRet = FailException(ex);
        //        if (DbTran != null)
        //        {
        //            DbTran.Rollback();
        //        }
        //    }
        //    return dicRet;
        //}
        #endregion
    }
}
