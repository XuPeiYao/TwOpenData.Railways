using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 列車等級
    /// </summary>
    public enum TrainLevels {
        /// <summary>
        /// 自強
        /// </summary>
        TzeChiang = 00,
        /// <summary>
        /// 太魯閣
        /// </summary>
        TzeChiang_Tarko = 01,
        /// <summary>
        /// 普悠瑪
        /// </summary>
        TzeChiang_Puyuma = 02,
        /// <summary>
        /// 莒光
        /// </summary>
        ChuKuang = 10,
        /// <summary>
        /// 復興
        /// </summary>
        FuHsing = 20,
        /// <summary>
        /// 區間
        /// </summary>
        Local = 30,
        /// <summary>
        /// 區間快車
        /// </summary>
        FastLocal = 31,
        /// <summary>
        /// 普快車
        /// </summary>
        Ordinary = 32
    }
}
