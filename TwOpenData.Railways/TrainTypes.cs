﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 列車類型
    /// </summary>
    public enum TrainTypes {
        /// <summary>
        /// 常態
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 臨時
        /// </summary>
        Temporary = 1,
        /// <summary>
        /// 團體
        /// </summary>
        Organization = 2,
        /// <summary>
        /// 春節加開
        /// </summary>
        CNYExtra = 3
    }
}
