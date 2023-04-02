﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Breezee.WorkHelper.DBTool.Entity;

namespace Breezee.WorkHelper.DBTool.IBLL
{
    public abstract class IDBConfigSet : IDBToolModule
    {
        #region 查询数据库配置设置
        /// <summary>
        /// 查询数据库配置设置
        /// </summary>
        /// <param name="dicQuery"></param>
        /// <returns></returns>
        public abstract IDictionary<string, object> QueryDbConfig(IDictionary<string, string> dicQuery);
        #endregion

        #region 保存数据库配置设置
        /// <summary>
        /// 保存数据库配置设置
        /// </summary>
        /// <param name="dicQuery"></param>
        /// <returns></returns>
        public abstract IDictionary<string, object> SaveDbConfig(IDictionary<string, object> dicQuery);
        #endregion

        public abstract IDictionary<string, object> DeleteDbConfig(IDictionary<string, string> dicQuery);

        public static class QueryDbConfig_InDicKey
        {
            public static readonly string DB_CONFIG_ID = "DB_CONFIG_ID";
            public static readonly string DB_CONFIG_CODE = "DB_CONFIG_CODE";
            public static readonly string DB_TYPE = "DB_TYPE";
            public static readonly string DB_NAME = "DB_NAME";
            //public static readonly string ORG_ID = "ORG_ID";
        }

        public static class SaveDbConfig_InDicKey
        {
            public static readonly string DT_TABLE = "DT_TABLE_DB_CONFIG";
        }

        public static class DeleteDbConfig_InDicKey
        {
            public static readonly string DB_CONFIG_ID = "DB_CONFIG_ID";
        }
    }

    
}
