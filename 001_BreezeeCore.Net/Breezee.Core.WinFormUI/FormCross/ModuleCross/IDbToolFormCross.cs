﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breezee.Core.WinFormUI
{
    /// <summary>
    /// 交叉引用通用接口_数据库工具
    /// 注：如果本接口方法已满足需求，但目标窗体直接继承该接口即可。否则需要新增接口
    /// </summary>
    [DllName(DllNameConstants.DLL_DB_TOOL)]
    public interface IDbToolFormCross : IFormCross
    {

    }
}
